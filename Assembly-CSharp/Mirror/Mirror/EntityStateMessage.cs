using System;

namespace Mirror
{
	// Token: 0x0200002C RID: 44
	public struct EntityStateMessage : NetworkMessage
	{
		// Token: 0x04000047 RID: 71
		public uint netId;

		// Token: 0x04000048 RID: 72
		public ArraySegment<byte> payload;
	}
}
