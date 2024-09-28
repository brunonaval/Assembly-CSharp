using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000319 RID: 793
public class DatabaseModule : MonoBehaviour
{
	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0004759F File Offset: 0x0004579F
	// (set) Token: 0x06000F0F RID: 3855 RVA: 0x000475A7 File Offset: 0x000457A7
	public bool IsLoaded { get; private set; }

	// Token: 0x06000F10 RID: 3856 RVA: 0x000475B0 File Offset: 0x000457B0
	private void Awake()
	{
		base.TryGetComponent<NpcDatabaseModule>(out this.npcDatabaseModule);
		base.TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<TitleDatabaseModule>(out this.titleDatabaseModule);
		base.TryGetComponent<MonsterDatabaseModule>(out this.monsterDatabaseModule);
		base.TryGetComponent<SkillDatabaseModule>(out this.skillDatabaseModule);
		base.TryGetComponent<QuestDatabaseModule>(out this.questDatabaseModule);
		base.TryGetComponent<DailyRewardDatabaseModule>(out this.dailyRewardDatabaseModule);
		base.TryGetComponent<BlueprintDatabaseModule>(out this.blueprintDatabaseModule);
		base.TryGetComponent<PlayerDatabaseModule>(out this.playerDatabaseModule);
	}

	// Token: 0x06000F11 RID: 3857 RVA: 0x00047632 File Offset: 0x00045832
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.titleDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.itemDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.monsterDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.npcDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.skillDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.questDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.dailyRewardDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.blueprintDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.playerDatabaseModule.IsLoaded);
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x04000EFB RID: 3835
	private NpcDatabaseModule npcDatabaseModule;

	// Token: 0x04000EFC RID: 3836
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x04000EFD RID: 3837
	private TitleDatabaseModule titleDatabaseModule;

	// Token: 0x04000EFE RID: 3838
	private MonsterDatabaseModule monsterDatabaseModule;

	// Token: 0x04000EFF RID: 3839
	private SkillDatabaseModule skillDatabaseModule;

	// Token: 0x04000F00 RID: 3840
	private QuestDatabaseModule questDatabaseModule;

	// Token: 0x04000F01 RID: 3841
	private DailyRewardDatabaseModule dailyRewardDatabaseModule;

	// Token: 0x04000F02 RID: 3842
	private BlueprintDatabaseModule blueprintDatabaseModule;

	// Token: 0x04000F03 RID: 3843
	private PlayerDatabaseModule playerDatabaseModule;
}
