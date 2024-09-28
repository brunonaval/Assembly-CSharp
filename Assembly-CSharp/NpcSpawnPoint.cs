using System;
using UnityEngine;

// Token: 0x02000125 RID: 293
public class NpcSpawnPoint
{
	// Token: 0x06000325 RID: 805 RVA: 0x0001467D File Offset: 0x0001287D
	public NpcSpawnPoint(int npcId, string npcName, Vector3 spawnPointPosition, GameObject npcObject)
	{
		this.NpcId = npcId;
		this.NpcName = npcName;
		this.SpawnPointPosition = spawnPointPosition;
		this.NpcObject = npcObject;
	}

	// Token: 0x040005EA RID: 1514
	public int NpcId;

	// Token: 0x040005EB RID: 1515
	public Vector3 SpawnPointPosition;

	// Token: 0x040005EC RID: 1516
	public string NpcName;

	// Token: 0x040005ED RID: 1517
	public GameObject NpcObject;
}
