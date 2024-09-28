using System;
using System.Collections.Generic;

// Token: 0x0200011E RID: 286
public struct Monster
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000311 RID: 785 RVA: 0x0001413F File Offset: 0x0001233F
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) | this.Id > 0;
		}
	}

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000312 RID: 786 RVA: 0x0001415C File Offset: 0x0001235C
	public string PluralName
	{
		get
		{
			if (string.IsNullOrEmpty(this.Name))
			{
				return this.Name;
			}
			if (this.Name.ToLower().EndsWith("s") | this.Name.ToLower().EndsWith("ss") | this.Name.ToLower().EndsWith("ch") | this.Name.ToLower().EndsWith("sh") | this.Name.ToLower().EndsWith("x") | this.Name.ToLower().EndsWith("z") | this.Name.ToLower().EndsWith("o"))
			{
				return this.Name + "es";
			}
			if (this.Name.ToLower().EndsWith("y"))
			{
				return this.Name.Substring(0, this.Name.Length - 1) + "ies";
			}
			if (this.Name.ToLower().EndsWith("f"))
			{
				return this.Name.Substring(0, this.Name.Length - 1) + "ves";
			}
			if (this.Name.ToLower().EndsWith("fe"))
			{
				return this.Name.Substring(0, this.Name.Length - 2) + "ves";
			}
			return this.Name + "s";
		}
	}

	// Token: 0x06000313 RID: 787 RVA: 0x000142EC File Offset: 0x000124EC
	public Monster(int id, string name, int baseLevel, int powerLevel, int agilityLevel, int precisionLevel, int toughnessLevel, int vitalityLevel, int attack, int defense, string skinMetaName, int experience, float spawnDelay, float aggroRange, bool canMove, bool isPreemptive, bool isPassive, bool isBoss, float spawnRange, float findTargetInterval, float changeTargetChance, bool allowSizeScaling, bool allowRankNamePrefix, string respawnMessage, int bossEnergyItemId, float bossEnergyDropChance)
	{
		this.Id = id;
		this.Name = name;
		this.BaseLevel = baseLevel;
		this.PowerLevel = powerLevel;
		this.AgilityLevel = agilityLevel;
		this.PrecisionLevel = precisionLevel;
		this.ToughnessLevel = toughnessLevel;
		this.VitalityLevel = vitalityLevel;
		this.Attack = attack;
		this.Defense = defense;
		this.SkinMetaName = skinMetaName;
		this.Experience = experience;
		this.SpawnDelay = spawnDelay;
		this.AggroRange = aggroRange;
		this.CanMove = canMove;
		this.IsPreemptive = isPreemptive;
		this.IsPassive = isPassive;
		this.IsBoss = isBoss;
		this.SpawnRange = spawnRange;
		this.FindTargetInterval = findTargetInterval;
		this.ChangeTargetChance = changeTargetChance;
		this.AllowSizeScaling = allowSizeScaling;
		this.AllowRankNamePrefix = allowRankNamePrefix;
		this.RespawnMessage = respawnMessage;
		this.BossEnergyItemId = bossEnergyItemId;
		this.BossEnergyDropChance = bossEnergyDropChance;
		this.SkillConfigs = new List<SkillConfig>();
		this.LootConfigs = new List<LootConfig>();
	}

	// Token: 0x040005A9 RID: 1449
	public int Id;

	// Token: 0x040005AA RID: 1450
	public string Name;

	// Token: 0x040005AB RID: 1451
	public int BaseLevel;

	// Token: 0x040005AC RID: 1452
	public int PowerLevel;

	// Token: 0x040005AD RID: 1453
	public int AgilityLevel;

	// Token: 0x040005AE RID: 1454
	public int PrecisionLevel;

	// Token: 0x040005AF RID: 1455
	public int ToughnessLevel;

	// Token: 0x040005B0 RID: 1456
	public int VitalityLevel;

	// Token: 0x040005B1 RID: 1457
	public int Attack;

	// Token: 0x040005B2 RID: 1458
	public int Defense;

	// Token: 0x040005B3 RID: 1459
	public string SkinMetaName;

	// Token: 0x040005B4 RID: 1460
	public int Experience;

	// Token: 0x040005B5 RID: 1461
	public float SpawnDelay;

	// Token: 0x040005B6 RID: 1462
	public float AggroRange;

	// Token: 0x040005B7 RID: 1463
	public bool CanMove;

	// Token: 0x040005B8 RID: 1464
	public bool IsPreemptive;

	// Token: 0x040005B9 RID: 1465
	public bool IsPassive;

	// Token: 0x040005BA RID: 1466
	public bool IsBoss;

	// Token: 0x040005BB RID: 1467
	public float SpawnRange;

	// Token: 0x040005BC RID: 1468
	public float FindTargetInterval;

	// Token: 0x040005BD RID: 1469
	public float ChangeTargetChance;

	// Token: 0x040005BE RID: 1470
	public bool AllowSizeScaling;

	// Token: 0x040005BF RID: 1471
	public bool AllowRankNamePrefix;

	// Token: 0x040005C0 RID: 1472
	public string RespawnMessage;

	// Token: 0x040005C1 RID: 1473
	public int BossEnergyItemId;

	// Token: 0x040005C2 RID: 1474
	public float BossEnergyDropChance;

	// Token: 0x040005C3 RID: 1475
	public List<SkillConfig> SkillConfigs;

	// Token: 0x040005C4 RID: 1476
	public List<LootConfig> LootConfigs;
}
