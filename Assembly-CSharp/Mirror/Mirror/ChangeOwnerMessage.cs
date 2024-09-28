using System;

namespace Mirror
{
	// Token: 0x02000027 RID: 39
	public struct ChangeOwnerMessage : NetworkMessage
	{
		// Token: 0x04000042 RID: 66
		public uint netId;

		// Token: 0x04000043 RID: 67
		public bool isOwner;

		// Token: 0x04000044 RID: 68
		public bool isLocalPlayer;
	}
}
