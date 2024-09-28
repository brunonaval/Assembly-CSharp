using System;
using System.Collections;
using Mirror;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class MonsterSpawnBrushManager : MonoBehaviour
{
	// Token: 0x0600004F RID: 79 RVA: 0x00002A3C File Offset: 0x00000C3C
	private void Awake()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002F1C File Offset: 0x0000111C
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameObject.FindGameObjectWithTag("GameEnvironment") != null);
		if (NetworkServer.active && this.monsterId != 0)
		{
			MonsterSpawnBrushManager.<>c__DisplayClass5_0 CS$<>8__locals1 = new MonsterSpawnBrushManager.<>c__DisplayClass5_0();
			GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
			gameObject.TryGetComponent<ItemDatabaseModule>(out CS$<>8__locals1.itemDatabaseModule);
			gameObject.TryGetComponent<MonsterDatabaseModule>(out CS$<>8__locals1.monsterDatabaseModule);
			gameObject.TryGetComponent<BlueprintDatabaseModule>(out CS$<>8__locals1.blueprintDatabaseModule);
			yield return new WaitUntil(() => CS$<>8__locals1.itemDatabaseModule.IsLoaded);
			yield return new WaitUntil(() => CS$<>8__locals1.monsterDatabaseModule.IsLoaded);
			yield return new WaitUntil(() => CS$<>8__locals1.blueprintDatabaseModule.IsLoaded);
			if (CS$<>8__locals1.monsterDatabaseModule.GetMonster(this.monsterId).IsBoss)
			{
				BossManagementModule bossManagementModule;
				GameObject.Find("BossManager").TryGetComponent<BossManagementModule>(out bossManagementModule);
				bossManagementModule.AddBossConfig(this.monsterId, base.transform.position, this.miniBoss);
			}
			else
			{
				GlobalUtils.SpawnMonster(this.monsterPrefab, this.monsterId, base.transform.position, this.monsterRank, true, true);
			}
			CS$<>8__locals1 = null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400002C RID: 44
	[SerializeField]
	private int monsterId;

	// Token: 0x0400002D RID: 45
	[SerializeField]
	private Rank monsterRank = Rank.Normal;

	// Token: 0x0400002E RID: 46
	[SerializeField]
	private bool miniBoss;

	// Token: 0x0400002F RID: 47
	[SerializeField]
	private GameObject monsterPrefab;
}
