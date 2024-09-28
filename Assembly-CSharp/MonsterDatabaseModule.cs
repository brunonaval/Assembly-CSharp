using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class MonsterDatabaseModule : MonoBehaviour
{
	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x060011B8 RID: 4536 RVA: 0x000543FE File Offset: 0x000525FE
	// (set) Token: 0x060011B9 RID: 4537 RVA: 0x00054406 File Offset: 0x00052606
	public bool IsLoaded { get; private set; }

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060011BA RID: 4538 RVA: 0x0005440F File Offset: 0x0005260F
	// (set) Token: 0x060011BB RID: 4539 RVA: 0x00054416 File Offset: 0x00052616
	public static Dictionary<int, List<PossibleLoot>> PossibleLoots { get; private set; }

	// Token: 0x060011BC RID: 4540 RVA: 0x0005441E File Offset: 0x0005261E
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
		this.itemDatabaseModule = base.GetComponent<ItemDatabaseModule>();
		this.skillDatabaseModule = base.GetComponent<SkillDatabaseModule>();
		this.blueprintDatabaseModule = base.GetComponent<BlueprintDatabaseModule>();
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x0005444A File Offset: 0x0005264A
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.itemDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.skillDatabaseModule.IsLoaded);
		yield return new WaitUntil(() => this.blueprintDatabaseModule.IsLoaded);
		yield return this.ProcessMonstersAsync().WaitAsCoroutine();
		this.ProcessPossibleLoots();
		this.IsLoaded = true;
		yield break;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x0005445C File Offset: 0x0005265C
	private void ProcessPossibleLoots()
	{
		MonsterDatabaseModule.PossibleLoots = new Dictionary<int, List<PossibleLoot>>();
		for (int i = 0; i < this.monsters.Count; i++)
		{
			Monster monster = this.monsters[i];
			List<PossibleLoot> list = new List<PossibleLoot>();
			for (int j = 0; j < this.itemDatabaseModule.DroppableItems.Length; j++)
			{
				Item item = this.itemDatabaseModule.DroppableItems[j];
				if (item.RequiredLevel <= monster.BaseLevel)
				{
					if (item.Type == ItemType.Blueprint)
					{
						this.AddBlueprintToPossibleLoots(list, monster, item);
					}
					else if (item.Equipable)
					{
						this.AddEquipmentToPossibleLoots(list, monster, item);
					}
					else
					{
						this.AddItemToPossibleLoots(list, item);
					}
				}
			}
			this.AddLootConfigToPossibleLoots(list, monster);
			MonsterDatabaseModule.PossibleLoots.Add(monster.Id, list);
		}
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x0005452C File Offset: 0x0005272C
	private void AddLootConfigToPossibleLoots(List<PossibleLoot> tempPossibleLoot, Monster monster)
	{
		for (int i = 0; i < monster.LootConfigs.Count; i++)
		{
			LootConfig lootConfig = monster.LootConfigs[i];
			Item item = this.itemDatabaseModule.GetItem(lootConfig.ItemId);
			item.RequiredLevel = lootConfig.RequiredLevel;
			item.Rarity = lootConfig.Rarity;
			PossibleLoot item2 = new PossibleLoot(item, lootConfig.MinAmount, lootConfig.MaxAmount, lootConfig.DropChance);
			tempPossibleLoot.Add(item2);
		}
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000545A8 File Offset: 0x000527A8
	private void AddItemToPossibleLoots(List<PossibleLoot> tempPossibleLoot, Item item)
	{
		if (item.Category == ItemCategory.Material)
		{
			item.RequiredLevel = 1;
		}
		PossibleLoot item2 = new PossibleLoot(item, 1, 1, item.DropByLevelChance);
		tempPossibleLoot.Add(item2);
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000545E0 File Offset: 0x000527E0
	private void AddEquipmentToPossibleLoots(List<PossibleLoot> tempPossibleLoot, Monster monster, Item item)
	{
		int maxQualityDropLevel = GlobalUtils.GetMaxQualityDropLevel(item.Quality);
		if (monster.BaseLevel > maxQualityDropLevel)
		{
			return;
		}
		Rarity[] array = new Rarity[]
		{
			Rarity.Common,
			Rarity.Uncommon,
			Rarity.Rare,
			Rarity.Exotic
		};
		array = (from r in array
		where r <= GlobalUtils.GetMaxRarityForQuality(item.Quality)
		select r).ToArray<Rarity>();
		int num = UnityEngine.Random.Range(0, array.Length);
		item.Rarity = array[num];
		item.RequiredLevel = Mathf.Min(850, monster.BaseLevel);
		float dropChance = GlobalUtils.EquipmentQualityToPercentDropChance(item.Quality);
		PossibleLoot item2 = new PossibleLoot(item, 1, 1, dropChance);
		tempPossibleLoot.Add(item2);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x000546A0 File Offset: 0x000528A0
	private void AddBlueprintToPossibleLoots(List<PossibleLoot> tempPossibleLoot, Monster monster, Item blueprintItem)
	{
		if (monster.BaseLevel < 10)
		{
			return;
		}
		if (monster.BaseLevel > blueprintItem.RequiredLevel * 2)
		{
			return;
		}
		float num = GlobalUtils.BlueprintPercentDropChance(blueprintItem.Quality);
		num *= this.blueprintDatabaseModule.GetBlueprintDropRate(blueprintItem.BlueprintId);
		PossibleLoot item = new PossibleLoot(blueprintItem, 1, 1, num);
		tempPossibleLoot.Add(item);
	}

	// Token: 0x060011C3 RID: 4547 RVA: 0x000546FC File Offset: 0x000528FC
	private Task ProcessMonstersAsync()
	{
		MonsterDatabaseModule.<ProcessMonstersAsync>d__19 <ProcessMonstersAsync>d__;
		<ProcessMonstersAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ProcessMonstersAsync>d__.<>4__this = this;
		<ProcessMonstersAsync>d__.<>1__state = -1;
		<ProcessMonstersAsync>d__.<>t__builder.Start<MonsterDatabaseModule.<ProcessMonstersAsync>d__19>(ref <ProcessMonstersAsync>d__);
		return <ProcessMonstersAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00054740 File Offset: 0x00052940
	private Task<Monster> ProcessMonsterLoots(Monster monster)
	{
		MonsterDatabaseModule.<ProcessMonsterLoots>d__20 <ProcessMonsterLoots>d__;
		<ProcessMonsterLoots>d__.<>t__builder = AsyncTaskMethodBuilder<Monster>.Create();
		<ProcessMonsterLoots>d__.<>4__this = this;
		<ProcessMonsterLoots>d__.monster = monster;
		<ProcessMonsterLoots>d__.<>1__state = -1;
		<ProcessMonsterLoots>d__.<>t__builder.Start<MonsterDatabaseModule.<ProcessMonsterLoots>d__20>(ref <ProcessMonsterLoots>d__);
		return <ProcessMonsterLoots>d__.<>t__builder.Task;
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x0005478C File Offset: 0x0005298C
	private Task<Monster> ProcessMonsterSkills(Monster monster)
	{
		MonsterDatabaseModule.<ProcessMonsterSkills>d__21 <ProcessMonsterSkills>d__;
		<ProcessMonsterSkills>d__.<>t__builder = AsyncTaskMethodBuilder<Monster>.Create();
		<ProcessMonsterSkills>d__.<>4__this = this;
		<ProcessMonsterSkills>d__.monster = monster;
		<ProcessMonsterSkills>d__.<>1__state = -1;
		<ProcessMonsterSkills>d__.<>t__builder.Start<MonsterDatabaseModule.<ProcessMonsterSkills>d__21>(ref <ProcessMonsterSkills>d__);
		return <ProcessMonsterSkills>d__.<>t__builder.Task;
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x000547D8 File Offset: 0x000529D8
	private void AddLootConfig(Monster monster, DataRow dbItem)
	{
		monster.LootConfigs.Add(new LootConfig((dbItem["ItemId"] as int?).GetValueOrDefault(), (dbItem["RequiredLevel"] as int?).GetValueOrDefault(), (Rarity)((dbItem["Rarity"] as int?) ?? 1), (dbItem["MinAmount"] as int?).GetValueOrDefault(), (dbItem["MaxAmount"] as int?).GetValueOrDefault(), (dbItem["DropChance"] as float?).GetValueOrDefault()));
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x000548B0 File Offset: 0x00052AB0
	private void AddSkillConfig(Monster monster, DataRow dbSkill)
	{
		int valueOrDefault = (dbSkill["SkillId"] as int?).GetValueOrDefault();
		float num = (dbSkill["SkillPower"] as float?).GetValueOrDefault();
		if (num == 0f)
		{
			num = this.skillDatabaseModule.CreateSkill(valueOrDefault).SkillPower;
		}
		monster.SkillConfigs.Add(new SkillConfig(valueOrDefault, (dbSkill["CastChance"] as float?).GetValueOrDefault(), (dbSkill["CastInterval"] as float?) ?? 99f, num));
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x00054970 File Offset: 0x00052B70
	private Monster BuildMonster(DataRow dbMonster)
	{
		return new Monster((dbMonster["Id"] as int?).GetValueOrDefault(), dbMonster["Name"].ToString(), (dbMonster["BaseLevel"] as int?) ?? 1, (dbMonster["PowerLevel"] as int?) ?? 1, (dbMonster["AgilityLevel"] as int?) ?? 1, (dbMonster["PrecisionLevel"] as int?) ?? 1, (dbMonster["ToughnessLevel"] as int?) ?? 1, (dbMonster["VitalityLevel"] as int?) ?? 1, (dbMonster["Attack"] as int?).GetValueOrDefault(), (dbMonster["Defense"] as int?).GetValueOrDefault(), dbMonster["SkinMetaName"].ToString(), (dbMonster["Experience"] as int?).GetValueOrDefault(), (dbMonster["SpawnDelay"] as float?) ?? 10f, (dbMonster["AggroRange"] as float?) ?? 3f, ((bool?)dbMonster["CanMove"]) ?? true, ((bool?)dbMonster["IsPreemptive"]).GetValueOrDefault(), ((bool?)dbMonster["IsPassive"]) ?? true, ((bool?)dbMonster["IsBoss"]).GetValueOrDefault(), (dbMonster["SpawnRange"] as float?) ?? 5f, (dbMonster["FindTargetInterval"] as float?) ?? 2f, (dbMonster["ChangeTargetChance"] as float?) ?? 0.02f, ((bool?)dbMonster["AllowSizeScaling"]) ?? true, ((bool?)dbMonster["AllowRankNamePrefix"]) ?? true, dbMonster["RespawnMessage"].ToString(), (dbMonster["BossEnergyItemId"] as int?).GetValueOrDefault(), (dbMonster["BossEnergyDropChance"] as float?).GetValueOrDefault());
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x00054CF8 File Offset: 0x00052EF8
	public void Add(Monster monster)
	{
		if (this.GetMonster(monster.Id).IsDefined)
		{
			Debug.LogErrorFormat("Can't add monster, id [{0}] already exists.", new object[]
			{
				monster.Id
			});
			return;
		}
		this.monsters.Add(monster);
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x00054D48 File Offset: 0x00052F48
	public Monster GetMonster(int monsterId)
	{
		for (int i = 0; i < this.monsters.Count; i++)
		{
			if (this.monsters[i].Id == monsterId)
			{
				return this.monsters[i];
			}
		}
		return default(Monster);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x00054D98 File Offset: 0x00052F98
	public Monster[] GetNonPassiveMonsters(int minLevel, int maxLevel)
	{
		return (from m in this.monsters
		where m.BaseLevel >= minLevel & m.BaseLevel <= maxLevel & !m.IsBoss & !m.IsPassive
		select m).ToArray<Monster>();
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x00054DD8 File Offset: 0x00052FD8
	public Monster[] GetNonPassiveBosses(int minLevel, int maxLevel)
	{
		return (from m in this.monsters
		where m.BaseLevel >= minLevel & m.BaseLevel <= maxLevel & m.IsBoss & !m.IsPassive
		select m).ToArray<Monster>();
	}

	// Token: 0x040010BE RID: 4286
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x040010BF RID: 4287
	private SkillDatabaseModule skillDatabaseModule;

	// Token: 0x040010C0 RID: 4288
	private BlueprintDatabaseModule blueprintDatabaseModule;

	// Token: 0x040010C1 RID: 4289
	private List<Monster> monsters = new List<Monster>();
}
