using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A6 RID: 1190
public class DarkSphereProjectile : ProjectileBase
{
	// Token: 0x06001AE1 RID: 6881 RVA: 0x00089B60 File Offset: 0x00087D60
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> targetsFromSkill = base.GetTargetsFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, null);
		for (int i = 0; i < targetsFromSkill.Count; i++)
		{
			this.ApplyDamageOnTarget(projectileBaseConfig, targetsFromSkill[i]);
		}
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x00089B9C File Offset: 0x00087D9C
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
