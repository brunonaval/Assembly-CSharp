using System;
using Mirror;
using UnityEngine;

// Token: 0x0200029D RID: 669
public struct BossEventMessage : NetworkMessage
{
	// Token: 0x04000BFF RID: 3071
	public int BossLevel;

	// Token: 0x04000C00 RID: 3072
	public string BossName;

	// Token: 0x04000C01 RID: 3073
	public Vector2 BossPosition;
}
