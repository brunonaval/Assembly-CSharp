using System;

namespace kcp2k
{
	// Token: 0x02000009 RID: 9
	public enum KcpHeader : byte
	{
		// Token: 0x04000025 RID: 37
		Handshake = 1,
		// Token: 0x04000026 RID: 38
		Ping,
		// Token: 0x04000027 RID: 39
		Data,
		// Token: 0x04000028 RID: 40
		Disconnect
	}
}
