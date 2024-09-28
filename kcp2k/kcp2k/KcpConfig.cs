using System;

namespace kcp2k
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public class KcpConfig
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000026EC File Offset: 0x000008EC
		public KcpConfig(bool DualMode = true, int RecvBufferSize = 7340032, int SendBufferSize = 7340032, int Mtu = 1200, bool NoDelay = true, uint Interval = 10U, int FastResend = 0, bool CongestionWindow = false, uint SendWindowSize = 32U, uint ReceiveWindowSize = 128U, int Timeout = 10000, uint MaxRetransmits = 20U)
		{
			this.DualMode = DualMode;
			this.RecvBufferSize = RecvBufferSize;
			this.SendBufferSize = SendBufferSize;
			this.Mtu = Mtu;
			this.NoDelay = NoDelay;
			this.Interval = Interval;
			this.FastResend = FastResend;
			this.CongestionWindow = CongestionWindow;
			this.SendWindowSize = SendWindowSize;
			this.ReceiveWindowSize = ReceiveWindowSize;
			this.Timeout = Timeout;
			this.MaxRetransmits = MaxRetransmits;
		}

		// Token: 0x04000018 RID: 24
		public bool DualMode;

		// Token: 0x04000019 RID: 25
		public int RecvBufferSize;

		// Token: 0x0400001A RID: 26
		public int SendBufferSize;

		// Token: 0x0400001B RID: 27
		public int Mtu;

		// Token: 0x0400001C RID: 28
		public bool NoDelay;

		// Token: 0x0400001D RID: 29
		public uint Interval;

		// Token: 0x0400001E RID: 30
		public int FastResend;

		// Token: 0x0400001F RID: 31
		public bool CongestionWindow;

		// Token: 0x04000020 RID: 32
		public uint SendWindowSize;

		// Token: 0x04000021 RID: 33
		public uint ReceiveWindowSize;

		// Token: 0x04000022 RID: 34
		public int Timeout;

		// Token: 0x04000023 RID: 35
		public uint MaxRetransmits;
	}
}
