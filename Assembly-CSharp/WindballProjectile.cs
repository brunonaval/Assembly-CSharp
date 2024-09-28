using System;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class WindballProjectile : ProjectileBase
{
	// Token: 0x06001B04 RID: 6916 RVA: 0x0008A1F8 File Offset: 0x000883F8
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x0008A220 File Offset: 0x00088420
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
