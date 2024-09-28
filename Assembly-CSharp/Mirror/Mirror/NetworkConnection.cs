using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200003E RID: 62
	public abstract class NetworkConnection
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006236 File Offset: 0x00004436
		// (set) Token: 0x0600011F RID: 287 RVA: 0x0000623E File Offset: 0x0000443E
		public NetworkIdentity identity { get; internal set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00006247 File Offset: 0x00004447
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000624F File Offset: 0x0000444F
		public double remoteTimeStamp { get; internal set; }

		// Token: 0x06000122 RID: 290 RVA: 0x00006258 File Offset: 0x00004458
		internal NetworkConnection()
		{
			this.lastMessageTime = Time.time;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006281 File Offset: 0x00004481
		internal NetworkConnection(int networkConnectionId) : this()
		{
			this.connectionId = networkConnectionId;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006290 File Offset: 0x00004490
		protected Batcher GetBatchForChannelId(int channelId)
		{
			Batcher batcher;
			if (!this.batches.TryGetValue(channelId, out batcher))
			{
				batcher = new Batcher(Transport.active.GetBatchThreshold(channelId));
				this.batches[channelId] = batcher;
			}
			return batcher;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000062CC File Offset: 0x000044CC
		public void Send<T>(T message, int channelId = 0) where T : struct, NetworkMessage
		{
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				NetworkMessages.Pack<T>(message, networkWriterPooled);
				int num = NetworkMessages.MaxMessageSize(channelId);
				if (networkWriterPooled.Position > num)
				{
					Debug.LogError(string.Format("NetworkConnection.Send: message of type {0} with a size of {1} bytes is larger than the max allowed message size in one batch: {2}.\nThe message was dropped, please make it smaller.", typeof(T), networkWriterPooled.Position, num));
				}
				else
				{
					NetworkDiagnostics.OnSend<T>(message, channelId, networkWriterPooled.Position, 1);
					this.Send(networkWriterPooled.ToArraySegment(), channelId);
				}
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000635C File Offset: 0x0000455C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal virtual void Send(ArraySegment<byte> segment, int channelId = 0)
		{
			this.GetBatchForChannelId(channelId).AddMessage(segment, NetworkTime.localTime);
		}

		// Token: 0x06000127 RID: 295
		protected abstract void SendToTransport(ArraySegment<byte> segment, int channelId = 0);

		// Token: 0x06000128 RID: 296 RVA: 0x00006370 File Offset: 0x00004570
		internal virtual void Update()
		{
			foreach (KeyValuePair<int, Batcher> keyValuePair in this.batches)
			{
				using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
				{
					while (keyValuePair.Value.GetBatch(networkWriterPooled))
					{
						ArraySegment<byte> segment = networkWriterPooled.ToArraySegment();
						this.SendToTransport(segment, keyValuePair.Key);
						networkWriterPooled.Position = 0;
					}
				}
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006408 File Offset: 0x00004608
		internal virtual bool IsAlive(float timeout)
		{
			return Time.time - this.lastMessageTime < timeout;
		}

		// Token: 0x0600012A RID: 298
		public abstract void Disconnect();

		// Token: 0x0600012B RID: 299 RVA: 0x00006419 File Offset: 0x00004619
		public override string ToString()
		{
			return string.Format("connection({0})", this.connectionId);
		}

		// Token: 0x0400008F RID: 143
		public const int LocalConnectionId = 0;

		// Token: 0x04000090 RID: 144
		public readonly int connectionId;

		// Token: 0x04000091 RID: 145
		public bool isAuthenticated;

		// Token: 0x04000092 RID: 146
		public object authenticationData;

		// Token: 0x04000093 RID: 147
		public bool isReady;

		// Token: 0x04000094 RID: 148
		public float lastMessageTime;

		// Token: 0x04000096 RID: 150
		public readonly HashSet<NetworkIdentity> owned = new HashSet<NetworkIdentity>();

		// Token: 0x04000097 RID: 151
		protected Dictionary<int, Batcher> batches = new Dictionary<int, Batcher>();
	}
}
