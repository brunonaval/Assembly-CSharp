using System;
using UnityEngine;

// Token: 0x020004A8 RID: 1192
public class IceballProjectile : ProjectileBase
{
	// Token: 0x06001AE7 RID: 6887 RVA: 0x00089C68 File Offset: 0x00087E68
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x00089C90 File Offset: 0x00087E90
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
		ConditionConfig projectileCondition = this.CreateConfusionConditionConfig();
		projectileBaseConfig.ProjectileCondition = projectileCondition;
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x00089CE8 File Offset: 0x00087EE8
	private ConditionConfig CreateConfusionConditionConfig()
	{
		Effect effect = new Effect("BlueStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Confusion, ConditionType.Confusion, 2f, 1f, 0.2f, effect, 6, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
