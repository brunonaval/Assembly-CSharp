using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x020002CA RID: 714
public class BossManagementModule : MonoBehaviour
{
	// Token: 0x06000C6A RID: 3178 RVA: 0x00037D31 File Offset: 0x00035F31
	private void Awake()
	{
		this.bossSpawnConfig = new List<BossSpawnConfig>();
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00037D40 File Offset: 0x00035F40
	private void Start()
	{
		if (!NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		gameObject.TryGetComponent<MonsterDatabaseModule>(out this.monsterDatabaseModule);
		gameObject.TryGetComponent<NpcDatabaseModule>(out this.npcDatabaseModule);
		base.InvokeRepeating("FixedBossTimer", 0f, 60f);
		base.InvokeRepeating("MiniBossTimer", 0f, 60f);
		base.InvokeRepeating("TvtTimer", 0f, 60f);
		Debug.Log(string.Format("BOSS Management System Started. {0} BOSSES added.", this.bossSpawnConfig.Count));
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00037DE4 File Offset: 0x00035FE4
	private void TvtTimer()
	{
		if (Application.isEditor)
		{
			return;
		}
		if (this.lastTvtTime != 0f & Time.time - this.lastTvtTime < 3600f)
		{
			return;
		}
		if (!this.tvtSchedule.Contains(DateTime.UtcNow.Hour))
		{
			return;
		}
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("tvt_manager_spawn_point");
		NetworkServer.Spawn(GlobalUtils.SpawnNpc(this.npcPrefab, this.npcDatabaseModule, 38, locationFromSpawnPoint), null);
		NetworkServer.SendToReady<TvtEventMessage>(default(TvtEventMessage), 0);
		this.lastTvtTime = Time.time;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00037E7C File Offset: 0x0003607C
	private void FixedBossTimer()
	{
		if (Application.isEditor)
		{
			return;
		}
		if (this.lastFixedBossTime != 0f & Time.time - this.lastFixedBossTime < 3600f)
		{
			return;
		}
		if (!this.worldBossSchedule.Contains(DateTime.UtcNow.Hour))
		{
			return;
		}
		BossSpawnConfig[] array = (from b in this.bossSpawnConfig
		where DateTime.Now.Subtract(b.LastSpawnDate).TotalHours >= 24.0 & b.SpawnedObject == null & !b.MiniBoss
		select b).ToArray<BossSpawnConfig>();
		if (array.Length == 0)
		{
			return;
		}
		int num = UnityEngine.Random.Range(0, array.Length);
		BossSpawnConfig bossData = array[num];
		this.lastFixedBossTime = Time.time;
		base.StartCoroutine(this.SpawnBoss(bossData));
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00037F30 File Offset: 0x00036130
	private void MiniBossTimer()
	{
		if (Application.isEditor)
		{
			return;
		}
		if (this.lastMiniBossTime != 0f & Time.time - this.lastMiniBossTime < 3600f)
		{
			return;
		}
		if (!this.miniBossSchedule.Contains(DateTime.UtcNow.Hour))
		{
			return;
		}
		if (DateTime.UtcNow.Minute < 30)
		{
			return;
		}
		BossSpawnConfig[] array = (from b in this.bossSpawnConfig
		where b.SpawnedObject == null & b.MiniBoss
		select b).ToArray<BossSpawnConfig>();
		if (array.Length == 0)
		{
			return;
		}
		foreach (BossSpawnConfig bossData in array)
		{
			base.StartCoroutine(this.SpawnBoss(bossData));
		}
		this.lastMiniBossTime = Time.time;
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00037FFD File Offset: 0x000361FD
	private IEnumerator SpawnBoss(BossSpawnConfig bossData)
	{
		Monster monster = this.monsterDatabaseModule.GetMonster(bossData.MonsterId);
		this.gameEnvironmentModule.BroadcastScreenMessage("boss_spawn_warning_message", 2, 7f, new string[]
		{
			monster.Name
		});
		string bossName = monster.Name.Replace("monster_", "").Replace("_", " ");
		string format = "{0}{1}";
		object arg = char.ToUpper(bossName[0]);
		string text = bossName;
		bossName = string.Format(format, arg, text.Substring(1, text.Length - 1));
		BossEventMessage bossEventMessage = new BossEventMessage
		{
			BossLevel = monster.BaseLevel,
			BossPosition = bossData.SpawnPosition,
			BossName = bossName
		};
		NetworkServer.SendToReady<BossEventMessage>(bossEventMessage, 0);
		string message = string.Concat(new string[]
		{
			"A ",
			bossData.MiniBoss ? "MINI-" : "",
			"BOSS WILL RESPAWN IN 10 MINUTES: ",
			bossName.ToUpper(),
			"\r\nUM ",
			bossData.MiniBoss ? "MINI-" : "",
			"BOSS IRÁ REAPARECER EM 10 MINUTOS: ",
			bossName.ToUpper()
		});
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		yield return new WaitForSecondsRealtime(600f);
		message = string.Concat(new string[]
		{
			"A ",
			bossData.MiniBoss ? "MINI-" : "",
			"BOSS HAS RESPAWNED: ",
			bossName.ToUpper(),
			"\r\nUM ",
			bossData.MiniBoss ? "MINI-" : "",
			"BOSS REAPARECEU: ",
			bossName.ToUpper()
		});
		yield return DiscordWebhookManager.SendInGameEventsChannel(message);
		NetworkServer.SendToReady<BossEventMessage>(bossEventMessage, 0);
		bossData.SpawnedObject = GlobalUtils.SpawnMonster(this.monsterPrefab, bossData.MonsterId, bossData.SpawnPosition, Rank.Epic, false, false);
		bossData.LastSpawnDate = DateTime.Now;
		yield break;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00038014 File Offset: 0x00036214
	public void AddBossConfig(int monsterId, Vector3 position, bool miniBoss)
	{
		Debug.Log(string.Format("Added BOSS config: {0}", monsterId));
		if (this.bossSpawnConfig.Any((BossSpawnConfig b) => b.MonsterId == monsterId))
		{
			return;
		}
		BossSpawnConfig item = new BossSpawnConfig
		{
			LastSpawnDate = DateTime.MinValue,
			MonsterId = monsterId,
			SpawnedObject = null,
			SpawnPosition = position,
			MiniBoss = miniBoss
		};
		this.bossSpawnConfig.Add(item);
	}

	// Token: 0x04000D48 RID: 3400
	private float lastFixedBossTime;

	// Token: 0x04000D49 RID: 3401
	private float lastTvtTime;

	// Token: 0x04000D4A RID: 3402
	private float lastMiniBossTime;

	// Token: 0x04000D4B RID: 3403
	public List<BossSpawnConfig> bossSpawnConfig;

	// Token: 0x04000D4C RID: 3404
	private int[] tvtSchedule = new int[]
	{
		0,
		3,
		6,
		9,
		12,
		15,
		18,
		21
	};

	// Token: 0x04000D4D RID: 3405
	private int[] worldBossSchedule = new int[]
	{
		5,
		11,
		17,
		23
	};

	// Token: 0x04000D4E RID: 3406
	private int[] miniBossSchedule = new int[]
	{
		0,
		2,
		4,
		6,
		8,
		10,
		12,
		14,
		16,
		18,
		20,
		22
	};

	// Token: 0x04000D4F RID: 3407
	[SerializeField]
	private GameObject monsterPrefab;

	// Token: 0x04000D50 RID: 3408
	[SerializeField]
	private GameObject npcPrefab;

	// Token: 0x04000D51 RID: 3409
	[SerializeField]
	private GameEnvironmentModule gameEnvironmentModule;

	// Token: 0x04000D52 RID: 3410
	private MonsterDatabaseModule monsterDatabaseModule;

	// Token: 0x04000D53 RID: 3411
	private NpcDatabaseModule npcDatabaseModule;
}
