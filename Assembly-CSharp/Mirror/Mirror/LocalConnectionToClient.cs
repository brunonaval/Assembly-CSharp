using System;

namespace Mirror
{
	// Token: 0x0200001C RID: 28
	public class LocalConnectionToClient : NetworkConnectionToClient
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002D89 File Offset: 0x00000F89
		public LocalConnectionToClient() : base(0)
		{
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002D92 File Offset: 0x00000F92
		public override string address
		{
			get
			{
				return "localhost";
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002D9C File Offset: 0x00000F9C
		internal override void Send(ArraySegment<byte> segment, int channelId = 0)
		{
			NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
			networkWriterPooled.WriteBytes(segment.Array, segment.Offset, segment.Count);
			this.connectionToServer.queue.Enqueue(networkWriterPooled);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002DDB File Offset: 0x00000FDB
		internal override bool IsAlive(float timeout)
		{
			return true;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000024F9 File Offset: 0x000006F9
		protected override void UpdatePing()
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002DDE File Offset: 0x00000FDE
		internal void DisconnectInternal()
		{
			this.isReady = false;
			base.RemoveFromObservingsObservers();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002DED File Offset: 0x00000FED
		public override void Disconnect()
		{
			this.DisconnectInternal();
			this.connectionToServer.DisconnectInternal();
		}

		// Token: 0x04000025 RID: 37
		internal LocalConnectionToServer connectionToServer;
	}
}
