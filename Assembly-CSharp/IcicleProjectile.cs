using System;
using UnityEngine;

// Token: 0x020004AA RID: 1194
public class IcicleProjectile : ProjectileBase
{
	// Token: 0x06001AEE RID: 6894 RVA: 0x00089DAC File Offset: 0x00087FAC
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x00089DD4 File Offset: 0x00087FD4
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

	// Token: 0x06001AF0 RID: 6896 RVA: 0x00089E2C File Offset: 0x0008802C
	private ConditionConfig CreateSlowConditionConfig()
	{
		Effect effect = new Effect("IceSmoke", 0.25f, 0.2f);
		Condition condition = new Condition(ConditionCategory.Curse, ConditionType.Agility, 4.5f, 0.15f, 0.66f, effect, 6, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
