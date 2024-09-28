using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004B2 RID: 1202
public class BigStoneProjectile : ProjectileBase
{
	// Token: 0x06001B0A RID: 6922 RVA: 0x0008A2D8 File Offset: 0x000884D8
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> targetsFromSkill = base.GetTargetsFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, null);
		for (int i = 0; i < targetsFromSkill.Count; i++)
		{
			this.ApplyDamageOnTarget(projectileBaseConfig, targetsFromSkill[i]);
		}
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x0008A314 File Offset: 0x00088514
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
