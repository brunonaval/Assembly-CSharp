using System;

// Token: 0x02000120 RID: 288
public struct Npc
{
	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000315 RID: 789 RVA: 0x00014401 File Offset: 0x00012601
	public bool IsDefined
	{
		get
		{
			return !string.IsNullOrEmpty(this.Name) | this.Id > 0;
		}
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0001441C File Offset: 0x0001261C
	public Npc(int id, string name, string title, CreatureGender gender, string script, int baseLevel, int powerLevel, int agilityLevel, int precisionLevel, int toughnessLevel, int vitalityLevel, int attack, int defense, string skinMetaName, bool canMove, bool isCombatant, float walkRange, float walkInterval, float walkChance)
	{
		this.Id = id;
		this.Name = name;
		this.Title = title;
		this.Gender = gender;
		this.Script = script;
		this.BaseLevel = baseLevel;
		this.PowerLevel = powerLevel;
		this.AgilityLevel = agilityLevel;
		this.PrecisionLevel = precisionLevel;
		this.ToughnessLevel = toughnessLevel;
		this.VitalityLevel = vitalityLevel;
		this.Attack = attack;
		this.Defense = defense;
		this.SkinMetaName = skinMetaName;
		this.CanMove = canMove;
		this.IsCombatant = isCombatant;
		this.WalkRange = walkRange;
		this.WalkInterval = walkInterval;
		this.WalkChance = walkChance;
		this.SkillConfigs = new SkillConfig[0];
	}

	// Token: 0x040005C9 RID: 1481
	public int Id;

	// Token: 0x040005CA RID: 1482
	public int Attack;

	// Token: 0x040005CB RID: 1483
	public int Defense;

	// Token: 0x040005CC RID: 1484
	public string Name;

	// Token: 0x040005CD RID: 1485
	public string Title;

	// Token: 0x040005CE RID: 1486
	public bool CanMove;

	// Token: 0x040005CF RID: 1487
	public CreatureGender Gender;

	// Token: 0x040005D0 RID: 1488
	public string Script;

	// Token: 0x040005D1 RID: 1489
	public int BaseLevel;

	// Token: 0x040005D2 RID: 1490
	public int PowerLevel;

	// Token: 0x040005D3 RID: 1491
	public string SkinMetaName;

	// Token: 0x040005D4 RID: 1492
	public float WalkRange;

	// Token: 0x040005D5 RID: 1493
	public int AgilityLevel;

	// Token: 0x040005D6 RID: 1494
	public bool IsCombatant;

	// Token: 0x040005D7 RID: 1495
	public float WalkChance;

	// Token: 0x040005D8 RID: 1496
	public int VitalityLevel;

	// Token: 0x040005D9 RID: 1497
	public int PrecisionLevel;

	// Token: 0x040005DA RID: 1498
	public int ToughnessLevel;

	// Token: 0x040005DB RID: 1499
	public float WalkInterval;

	// Token: 0x040005DC RID: 1500
	public SkillConfig[] SkillConfigs;
}
