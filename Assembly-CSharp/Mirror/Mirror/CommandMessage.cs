using System;

namespace Mirror
{
	// Token: 0x02000024 RID: 36
	public struct CommandMessage : NetworkMessage
	{
		// Token: 0x04000031 RID: 49
		public uint netId;

		// Token: 0x04000032 RID: 50
		public byte componentIndex;

		// Token: 0x04000033 RID: 51
		public ushort functionHash;

		// Token: 0x04000034 RID: 52
		public ArraySegment<byte> payload;
	}
}
