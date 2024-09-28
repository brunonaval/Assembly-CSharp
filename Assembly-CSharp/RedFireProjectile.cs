using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public class RedFireProjectile : ProjectileBase
{
	// Token: 0x06001AF9 RID: 6905 RVA: 0x00089FBC File Offset: 0x000881BC
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

	// Token: 0x06001AFA RID: 6906 RVA: 0x0008A008 File Offset: 0x00088208
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
		ConditionConfig projectileCondition = this.CreateBleedingCondition();
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001AFB RID: 6907 RVA: 0x0008A060 File Offset: 0x00088260
	private ConditionConfig CreateBleedingCondition()
	{
		Effect effect = new Effect("RedExplosion", 0.35f, 0.15f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Bleed, 6f, 1f, 0.17f, effect, 3, 0f, "curse");
		return new ConditionConfig(0.15f, condition);
	}
}
