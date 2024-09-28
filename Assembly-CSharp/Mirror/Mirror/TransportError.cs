using System;

namespace Mirror
{
	// Token: 0x0200008E RID: 142
	public enum TransportError : byte
	{
		// Token: 0x04000197 RID: 407
		DnsResolve,
		// Token: 0x04000198 RID: 408
		Refused,
		// Token: 0x04000199 RID: 409
		Timeout,
		// Token: 0x0400019A RID: 410
		Congestion,
		// Token: 0x0400019B RID: 411
		InvalidReceive,
		// Token: 0x0400019C RID: 412
		InvalidSend,
		// Token: 0x0400019D RID: 413
		ConnectionClosed,
		// Token: 0x0400019E RID: 414
		Unexpected
	}
}
