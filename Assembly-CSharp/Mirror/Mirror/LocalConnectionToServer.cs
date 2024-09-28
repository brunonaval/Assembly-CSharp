using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200001D RID: 29
	public class LocalConnectionToServer : NetworkConnectionToServer
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002D92 File Offset: 0x00000F92
		[Obsolete("Use LocalConnectionToClient.address instead.")]
		public string address
		{
			get
			{
				return "localhost";
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002E00 File Offset: 0x00001000
		internal void QueueConnectedEvent()
		{
			this.connectedEventPending = true;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002E09 File Offset: 0x00001009
		internal void QueueDisconnectedEvent()
		{
			this.disconnectedEventPending = true;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002E14 File Offset: 0x00001014
		internal override void Send(ArraySegment<byte> segment, int channelId = 0)
		{
			if (segment.Count == 0)
			{
				Debug.LogError("LocalConnection.SendBytes cannot send zero bytes");
				return;
			}
			Batcher batchForChannelId = base.GetBatchForChannelId(channelId);
			batchForChannelId.AddMessage(segment, NetworkTime.localTime);
			using (NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get())
			{
				if (batchForChannelId.GetBatch(networkWriterPooled))
				{
					NetworkServer.OnTransportData(this.connectionId, networkWriterPooled.ToArraySegment(), channelId);
				}
				else
				{
					Debug.LogError("Local connection failed to make batch. This should never happen.");
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002E94 File Offset: 0x00001094
		internal override void Update()
		{
			base.Update();
			if (this.connectedEventPending)
			{
				this.connectedEventPending = false;
				Action onConnectedEvent = NetworkClient.OnConnectedEvent;
				if (onConnectedEvent != null)
				{
					onConnectedEvent();
				}
			}
			while (this.queue.Count > 0)
			{
				NetworkWriterPooled networkWriterPooled = this.queue.Dequeue();
				ArraySegment<byte> message = networkWriterPooled.ToArraySegment();
				Batcher batchForChannelId = base.GetBatchForChannelId(0);
				batchForChannelId.AddMessage(message, NetworkTime.localTime);
				using (NetworkWriterPooled networkWriterPooled2 = NetworkWriterPool.Get())
				{
					if (batchForChannelId.GetBatch(networkWriterPooled2))
					{
						NetworkClient.OnTransportData(networkWriterPooled2.ToArraySegment(), 0);
					}
				}
				NetworkWriterPool.Return(networkWriterPooled);
			}
			if (this.disconnectedEventPending)
			{
				this.disconnectedEventPending = false;
				Action onDisconnectedEvent = NetworkClient.OnDisconnectedEvent;
				if (onDisconnectedEvent == null)
				{
					return;
				}
				onDisconnectedEvent();
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F58 File Offset: 0x00001158
		internal void DisconnectInternal()
		{
			this.isReady = false;
			NetworkClient.ready = false;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002F67 File Offset: 0x00001167
		public override void Disconnect()
		{
			this.connectionToClient.DisconnectInternal();
			this.DisconnectInternal();
			NetworkServer.RemoveLocalConnection();
			NetworkClient.OnTransportDisconnected();
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002DDB File Offset: 0x00000FDB
		internal override bool IsAlive(float timeout)
		{
			return true;
		}

		// Token: 0x04000026 RID: 38
		internal LocalConnectionToClient connectionToClient;

		// Token: 0x04000027 RID: 39
		internal readonly Queue<NetworkWriterPooled> queue = new Queue<NetworkWriterPooled>();

		// Token: 0x04000028 RID: 40
		private bool connectedEventPending;

		// Token: 0x04000029 RID: 41
		private bool disconnectedEventPending;
	}
}
