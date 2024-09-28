using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200003F RID: 63
	public class NetworkConnectionToClient : NetworkConnection
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006430 File Offset: 0x00004630
		public virtual string address
		{
			get
			{
				return Transport.active.ServerGetClientAddress(this.connectionId);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00006442 File Offset: 0x00004642
		[Obsolete(".clientOwnedObjects was renamed to .owned :)")]
		public HashSet<NetworkIdentity> clientOwnedObjects
		{
			get
			{
				return this.owned;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000644A File Offset: 0x0000464A
		private double bufferTime
		{
			get
			{
				return (double)NetworkServer.sendInterval * this.bufferTimeMultiplier;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006459 File Offset: 0x00004659
		public double rtt
		{
			get
			{
				return this._rtt.Value;
			}
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006468 File Offset: 0x00004668
		public NetworkConnectionToClient(int networkConnectionId) : base(networkConnectionId)
		{
			this.driftEma = new ExponentialMovingAverage(NetworkServer.sendRate * NetworkClient.snapshotSettings.driftEmaDuration);
			this.deliveryTimeEma = new ExponentialMovingAverage(NetworkServer.sendRate * NetworkClient.snapshotSettings.deliveryTimeEmaDuration);
			this.snapshotBufferSizeLimit = Mathf.Max((int)NetworkClient.snapshotSettings.bufferTimeMultiplier, this.snapshotBufferSizeLimit);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000652C File Offset: 0x0000472C
		public void OnTimeSnapshot(TimeSnapshot snapshot)
		{
			if (this.snapshots.Count >= this.snapshotBufferSizeLimit)
			{
				return;
			}
			if (NetworkClient.snapshotSettings.dynamicAdjustment)
			{
				this.bufferTimeMultiplier = SnapshotInterpolation.DynamicAdjustment((double)NetworkServer.sendInterval, this.deliveryTimeEma.StandardDeviation, (double)NetworkClient.snapshotSettings.dynamicAdjustmentTolerance);
			}
			SnapshotInterpolation.InsertAndAdjust<TimeSnapshot>(this.snapshots, NetworkClient.snapshotSettings.bufferLimit, snapshot, ref this.remoteTimeline, ref this.remoteTimescale, NetworkServer.sendInterval, this.bufferTime, NetworkClient.snapshotSettings.catchupSpeed, NetworkClient.snapshotSettings.slowdownSpeed, ref this.driftEma, NetworkClient.snapshotSettings.catchupNegativeThreshold, NetworkClient.snapshotSettings.catchupPositiveThreshold, ref this.deliveryTimeEma);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x000065E4 File Offset: 0x000047E4
		public void UpdateTimeInterpolation()
		{
			if (this.snapshots.Count > 0)
			{
				SnapshotInterpolation.StepTime((double)Time.unscaledDeltaTime, ref this.remoteTimeline, this.remoteTimescale);
				TimeSnapshot timeSnapshot;
				TimeSnapshot timeSnapshot2;
				double num;
				SnapshotInterpolation.StepInterpolation<TimeSnapshot>(this.snapshots, this.remoteTimeline, out timeSnapshot, out timeSnapshot2, out num);
			}
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000662D File Offset: 0x0000482D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void SendToTransport(ArraySegment<byte> segment, int channelId = 0)
		{
			Transport.active.ServerSend(this.connectionId, segment, channelId);
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006644 File Offset: 0x00004844
		protected virtual void UpdatePing()
		{
			if (NetworkTime.localTime >= this.lastPingTime + (double)NetworkTime.PingInterval)
			{
				NetworkPingMessage message = new NetworkPingMessage(NetworkTime.localTime);
				base.Send<NetworkPingMessage>(message, 1);
				this.lastPingTime = NetworkTime.localTime;
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006684 File Offset: 0x00004884
		internal override void Update()
		{
			this.UpdatePing();
			base.Update();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006692 File Offset: 0x00004892
		public override void Disconnect()
		{
			this.isReady = false;
			this.reliableRpcs.Position = 0;
			this.unreliableRpcs.Position = 0;
			Transport.active.ServerDisconnect(this.connectionId);
		}

		// Token: 0x06000137 RID: 311 RVA: 0x000066C3 File Offset: 0x000048C3
		internal void AddToObserving(NetworkIdentity netIdentity)
		{
			this.observing.Add(netIdentity);
			NetworkServer.ShowForConnection(netIdentity, this);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x000066D9 File Offset: 0x000048D9
		internal void RemoveFromObserving(NetworkIdentity netIdentity, bool isDestroyed)
		{
			this.observing.Remove(netIdentity);
			if (!isDestroyed)
			{
				NetworkServer.HideForConnection(netIdentity, this);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000066F4 File Offset: 0x000048F4
		internal void RemoveFromObservingsObservers()
		{
			foreach (NetworkIdentity networkIdentity in this.observing)
			{
				networkIdentity.RemoveObserver(this);
			}
			this.observing.Clear();
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006750 File Offset: 0x00004950
		internal void AddOwnedObject(NetworkIdentity obj)
		{
			this.owned.Add(obj);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000675F File Offset: 0x0000495F
		internal void RemoveOwnedObject(NetworkIdentity obj)
		{
			this.owned.Remove(obj);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00006770 File Offset: 0x00004970
		internal void DestroyOwnedObjects()
		{
			foreach (NetworkIdentity networkIdentity in new HashSet<NetworkIdentity>(this.owned))
			{
				if (networkIdentity != null)
				{
					if (networkIdentity.sceneId != 0UL)
					{
						NetworkServer.UnSpawn(networkIdentity.gameObject);
					}
					else
					{
						NetworkServer.Destroy(networkIdentity.gameObject);
					}
				}
			}
			this.owned.Clear();
		}

		// Token: 0x04000099 RID: 153
		private readonly NetworkWriter reliableRpcs = new NetworkWriter();

		// Token: 0x0400009A RID: 154
		private readonly NetworkWriter unreliableRpcs = new NetworkWriter();

		// Token: 0x0400009B RID: 155
		public readonly HashSet<NetworkIdentity> observing = new HashSet<NetworkIdentity>();

		// Token: 0x0400009C RID: 156
		public Unbatcher unbatcher = new Unbatcher();

		// Token: 0x0400009D RID: 157
		private ExponentialMovingAverage driftEma;

		// Token: 0x0400009E RID: 158
		private ExponentialMovingAverage deliveryTimeEma;

		// Token: 0x0400009F RID: 159
		public double remoteTimeline;

		// Token: 0x040000A0 RID: 160
		public double remoteTimescale;

		// Token: 0x040000A1 RID: 161
		private double bufferTimeMultiplier = 2.0;

		// Token: 0x040000A2 RID: 162
		private readonly SortedList<double, TimeSnapshot> snapshots = new SortedList<double, TimeSnapshot>();

		// Token: 0x040000A3 RID: 163
		public int snapshotBufferSizeLimit = 64;

		// Token: 0x040000A4 RID: 164
		private double lastPingTime;

		// Token: 0x040000A5 RID: 165
		internal ExponentialMovingAverage _rtt = new ExponentialMovingAverage(NetworkTime.PingWindowSize);
	}
}
