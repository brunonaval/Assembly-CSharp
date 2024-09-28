using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000026 RID: 38
	public struct SpawnMessage : NetworkMessage
	{
		// Token: 0x04000039 RID: 57
		public uint netId;

		// Token: 0x0400003A RID: 58
		public bool isLocalPlayer;

		// Token: 0x0400003B RID: 59
		public bool isOwner;

		// Token: 0x0400003C RID: 60
		public ulong sceneId;

		// Token: 0x0400003D RID: 61
		public uint assetId;

		// Token: 0x0400003E RID: 62
		public Vector3 position;

		// Token: 0x0400003F RID: 63
		public Quaternion rotation;

		// Token: 0x04000040 RID: 64
		public Vector3 scale;

		// Token: 0x04000041 RID: 65
		public ArraySegment<byte> payload;
	}
}
