using System;
using UnityEngine;

// Token: 0x0200049A RID: 1178
public class FlameArrowProjectile : ProjectileBase
{
	// Token: 0x06001ABA RID: 6842 RVA: 0x00089498 File Offset: 0x00087698
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000894C0 File Offset: 0x000876C0
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
		ConditionConfig projectileCondition = this.CreateBurnConditionConfig(damage);
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x00089518 File Offset: 0x00087718
	private ConditionConfig CreateBurnConditionConfig(int damage)
	{
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 6f, 1f, 0.13f, effect, 2, 0f, "fireball");
		return new ConditionConfig(0.13f, condition);
	}
}
