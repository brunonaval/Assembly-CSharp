﻿using System;
using UnityEngine;

// Token: 0x020004B1 RID: 1201
public class WindFuryProjectile : ProjectileBase
{
	// Token: 0x06001B07 RID: 6919 RVA: 0x0008A268 File Offset: 0x00088468
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x0008A290 File Offset: 0x00088490
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
