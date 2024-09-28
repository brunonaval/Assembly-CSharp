using System;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class SonicForceProjectile : ProjectileBase
{
	// Token: 0x06001B00 RID: 6912 RVA: 0x0008A124 File Offset: 0x00088324
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x0008A14C File Offset: 0x0008834C
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
		ConditionConfig projectileCondition = this.CreateStunConditionConfig();
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x0008A1A4 File Offset: 0x000883A4
	private ConditionConfig CreateStunConditionConfig()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 0.9f, 0.9f, 0f, effect, 5, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
