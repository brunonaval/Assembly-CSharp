using System;
using UnityEngine;

// Token: 0x02000147 RID: 327
public struct ShootConfig
{
	// Token: 0x06000366 RID: 870 RVA: 0x000159B6 File Offset: 0x00013BB6
	public ShootConfig(Vector3 fromPosition, GameObject target, Skill skill, Item ammo, Condition condition, float conditionChance)
	{
		this.FromPosition = fromPosition;
		this.Target = target;
		this.Skill = skill;
		this.Ammo = ammo;
		this.Condition = condition;
		this.ConditionChance = conditionChance;
	}

	// Token: 0x06000367 RID: 871 RVA: 0x000159E5 File Offset: 0x00013BE5
	public ShootConfig(Vector3 fromPosition, GameObject target, Skill skill, Item ammo)
	{
		this.FromPosition = fromPosition;
		this.Target = target;
		this.Skill = skill;
		this.Ammo = ammo;
		this.Condition = default(Condition);
		this.ConditionChance = 0f;
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00015A1B File Offset: 0x00013C1B
	public ShootConfig(GameObject target, Skill skill, Item ammo)
	{
		this.FromPosition = Vector3.zero;
		this.Target = target;
		this.Skill = skill;
		this.Ammo = ammo;
		this.Condition = default(Condition);
		this.ConditionChance = 0f;
	}

	// Token: 0x040006AA RID: 1706
	public Vector3 FromPosition;

	// Token: 0x040006AB RID: 1707
	public GameObject Target;

	// Token: 0x040006AC RID: 1708
	public Skill Skill;

	// Token: 0x040006AD RID: 1709
	public Item Ammo;

	// Token: 0x040006AE RID: 1710
	public Condition Condition;

	// Token: 0x040006AF RID: 1711
	public float ConditionChance;
}
