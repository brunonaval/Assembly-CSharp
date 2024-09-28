using System;

namespace Mirror
{
	// Token: 0x02000025 RID: 37
	public struct RpcMessage : NetworkMessage
	{
		// Token: 0x04000035 RID: 53
		public uint netId;

		// Token: 0x04000036 RID: 54
		public byte componentIndex;

		// Token: 0x04000037 RID: 55
		public ushort functionHash;

		// Token: 0x04000038 RID: 56
		public ArraySegment<byte> payload;
	}
}
