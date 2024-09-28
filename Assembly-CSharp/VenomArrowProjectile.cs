using System;
using UnityEngine;

// Token: 0x020004A1 RID: 1185
public class VenomArrowProjectile : ProjectileBase
{
	// Token: 0x06001AD2 RID: 6866 RVA: 0x00089900 File Offset: 0x00087B00
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x00089928 File Offset: 0x00087B28
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
		ConditionConfig projectileCondition = VenomArrowProjectile.CreatePoisonConditionConfig(damage);
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x00089980 File Offset: 0x00087B80
	private static ConditionConfig CreatePoisonConditionConfig(int damage)
	{
		Effect effect = new Effect("VenomPuff", 0.1875f, 0f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Poison, 7.5f, 1f, 0.13f, effect, 1, 0f, "venom_puff");
		return new ConditionConfig(0.13f, condition);
	}
}
