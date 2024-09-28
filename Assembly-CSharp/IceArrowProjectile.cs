using System;
using UnityEngine;

// Token: 0x0200049B RID: 1179
public class IceArrowProjectile : ProjectileBase
{
	// Token: 0x06001ABE RID: 6846 RVA: 0x00089574 File Offset: 0x00087774
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x0008959C File Offset: 0x0008779C
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
		ConditionConfig projectileCondition = this.CreateSlowConditionConfig();
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000895F4 File Offset: 0x000877F4
	private ConditionConfig CreateSlowConditionConfig()
	{
		Effect effect = new Effect("IceSmoke", 0.25f, 0.2f);
		Condition condition = new Condition(ConditionCategory.Curse, ConditionType.Agility, 4.5f, 0.15f, 0.66f, effect, 6, 0f, "");
		return new ConditionConfig(0.13f, condition);
	}
}
