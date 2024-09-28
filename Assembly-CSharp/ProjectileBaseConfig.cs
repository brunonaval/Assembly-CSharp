using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
public struct ProjectileBaseConfig
{
	// Token: 0x06000347 RID: 839 RVA: 0x0001535A File Offset: 0x0001355A
	public ProjectileBaseConfig(Skill skill, Item ammo, GameObject projectileTarget, GameObject attacker, GameObject projectile, ConditionConfig skillCondition)
	{
		this.Ammo = ammo;
		this.Skill = skill;
		this.ProjectileTarget = projectileTarget;
		this.AttackerObject = attacker;
		this.ProjectileObject = projectile;
		this.SkillCondition = skillCondition;
		this.ProjectileCondition = default(ConditionConfig);
	}

	// Token: 0x0400064B RID: 1611
	public Item Ammo;

	// Token: 0x0400064C RID: 1612
	public Skill Skill;

	// Token: 0x0400064D RID: 1613
	public GameObject ProjectileTarget;

	// Token: 0x0400064E RID: 1614
	public GameObject AttackerObject;

	// Token: 0x0400064F RID: 1615
	public GameObject ProjectileObject;

	// Token: 0x04000650 RID: 1616
	public ConditionConfig ProjectileCondition;

	// Token: 0x04000651 RID: 1617
	public ConditionConfig SkillCondition;
}
