using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049F RID: 1183
public class TheTeenBaanProjectile : ProjectileBase
{
	// Token: 0x06001ACB RID: 6859 RVA: 0x00089798 File Offset: 0x00087998
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> targetsFromSkill = base.GetTargetsFromSkill(projectileBaseConfig);
		if (targetsFromSkill.Count == 0)
		{
			return;
		}
		base.ShowExplosionEffects(projectileBaseConfig, targetsFromSkill[0]);
		for (int i = 0; i < targetsFromSkill.Count; i++)
		{
			this.ApplyDamageOnTarget(projectileBaseConfig, targetsFromSkill[i]);
		}
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000897E4 File Offset: 0x000879E4
	private void ApplyDamageOnTarget(ProjectileBaseConfig projectileBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(projectileBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildHitEffectConfig(projectileBaseConfig, critical, blocked, damage, projectileBaseConfig.Ammo.TextColorId);
		ConditionConfig projectileCondition = this.CreateStigmaCondition();
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x0008983C File Offset: 0x00087A3C
	private ConditionConfig CreateStigmaCondition()
	{
		Effect effect = new Effect("DarkStrike", 0.2f, 0.3f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 6f, 1f, 0.17f, effect, 7, 0f, "stigma");
		return new ConditionConfig(0.13f, condition);
	}
}
