using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004AB RID: 1195
public class LifeSphereProjectile : ProjectileBase
{
	// Token: 0x06001AF2 RID: 6898 RVA: 0x00089E80 File Offset: 0x00088080
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		List<GameObject> friendlyTargetsFromSkill = base.GetFriendlyTargetsFromSkill(projectileBaseConfig);
		this.ShowExplosionEffects(projectileBaseConfig);
		short attack = projectileBaseConfig.Ammo.Attack;
		for (int i = 0; i < friendlyTargetsFromSkill.Count; i++)
		{
			this.AddHealthToTarget(projectileBaseConfig, (int)attack, friendlyTargetsFromSkill[i]);
		}
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x00089ECC File Offset: 0x000880CC
	private void ShowExplosionEffects(ProjectileBaseConfig projectileBaseConfig)
	{
		EffectConfig effectConfig = base.BuildExplosionEffectConfig(projectileBaseConfig, null);
		projectileBaseConfig.AttackerObject.GetComponent<EffectModule>().ShowEffectsRandomly(5, projectileBaseConfig.Ammo.Projectile.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x00089F0C File Offset: 0x0008810C
	private void AddHealthToTarget(ProjectileBaseConfig projectileBaseConfig, int health, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		AttributeModule attributeModule;
		if (!target.TryGetComponent<AttributeModule>(out attributeModule))
		{
			return;
		}
		EffectConfig healEffect = base.BuildHitEffectConfig(projectileBaseConfig, false, false, health, 1);
		attributeModule.AddHealth(projectileBaseConfig.AttackerObject, health, true, healEffect);
	}
}
