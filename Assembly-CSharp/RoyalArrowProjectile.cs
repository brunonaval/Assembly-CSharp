using System;
using UnityEngine;

// Token: 0x0200049D RID: 1181
public class RoyalArrowProjectile : ProjectileBase
{
	// Token: 0x06001AC5 RID: 6853 RVA: 0x000896B8 File Offset: 0x000878B8
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000896E0 File Offset: 0x000878E0
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
		base.ApplyDamageAndIncomingConditions(projectileBaseConfig, damage, damageEffectConfig, target);
	}
}
