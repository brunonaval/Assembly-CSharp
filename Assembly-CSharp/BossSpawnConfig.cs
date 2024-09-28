using System;
using UnityEngine;

// Token: 0x020002CE RID: 718
[Serializable]
public class BossSpawnConfig
{
	// Token: 0x04000D5E RID: 3422
	public int MonsterId;

	// Token: 0x04000D5F RID: 3423
	public Vector3 SpawnPosition;

	// Token: 0x04000D60 RID: 3424
	public DateTime LastSpawnDate;

	// Token: 0x04000D61 RID: 3425
	public GameObject SpawnedObject;

	// Token: 0x04000D62 RID: 3426
	public bool MiniBoss;
}
