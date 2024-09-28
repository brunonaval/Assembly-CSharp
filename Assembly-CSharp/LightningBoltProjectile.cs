using System;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class LightningBoltProjectile : ProjectileBase
{
	// Token: 0x06001AF6 RID: 6902 RVA: 0x00089F4C File Offset: 0x0008814C
	public override void Explode(ProjectileBaseConfig projectileBaseConfig)
	{
		GameObject targetFromSkill = base.GetTargetFromSkill(projectileBaseConfig);
		base.ShowExplosionEffects(projectileBaseConfig, targetFromSkill);
		this.ApplyDamageOnTarget(projectileBaseConfig, targetFromSkill);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x00089F74 File Offset: 0x00088174
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
