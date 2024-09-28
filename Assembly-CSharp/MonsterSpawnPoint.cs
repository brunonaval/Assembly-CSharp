using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class MonsterSpawnPoint
{
	// Token: 0x06000314 RID: 788 RVA: 0x000143DC File Offset: 0x000125DC
	public MonsterSpawnPoint(int monsterId, Rank monsterRank, Vector3 spawnPointPosition, GameObject monsterObject)
	{
		this.MonsterId = monsterId;
		this.MonsterRank = monsterRank;
		this.SpawnPointPosition = spawnPointPosition;
		this.MonsterObject = monsterObject;
	}

	// Token: 0x040005C5 RID: 1477
	public int MonsterId;

	// Token: 0x040005C6 RID: 1478
	public Rank MonsterRank;

	// Token: 0x040005C7 RID: 1479
	public Vector3 SpawnPointPosition;

	// Token: 0x040005C8 RID: 1480
	public GameObject MonsterObject;
}
