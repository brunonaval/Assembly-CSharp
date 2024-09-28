using System;

namespace kcp2k
{
	// Token: 0x02000004 RID: 4
	public enum ErrorCode : byte
	{
		// Token: 0x04000004 RID: 4
		DnsResolve,
		// Token: 0x04000005 RID: 5
		Timeout,
		// Token: 0x04000006 RID: 6
		Congestion,
		// Token: 0x04000007 RID: 7
		InvalidReceive,
		// Token: 0x04000008 RID: 8
		InvalidSend,
		// Token: 0x04000009 RID: 9
		ConnectionClosed,
		// Token: 0x0400000A RID: 10
		Unexpected
	}
}
