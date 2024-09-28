using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004A7 RID: 1191
public class FireballProjectile : ProjectileBase
{
	// Token: 0x06001AE4 RID: 6884 RVA: 0x00089BE4 File Offset: 0x00087DE4
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> targetsFromSkill = base.GetTargetsFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, null);
		for (int i = 0; i < targetsFromSkill.Count; i++)
		{
			this.ApplyDamageOnTarget(projectileBaseConfig, targetsFromSkill[i]);
		}
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x00089C20 File Offset: 0x00087E20
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
