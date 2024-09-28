using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class FireballSkill : SkillBase
{
	// Token: 0x06001B59 RID: 7001 RVA: 0x0008B238 File Offset: 0x00089438
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootFireball(skillBaseConfig, target);
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x0008B278 File Offset: 0x00089478
	private void ShootFireball(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateFireball();
		Projectile projectile = ammo.Projectile;
		projectile.ExplosionEffect.ScaleModifier = 0.35f;
		ammo.Projectile = projectile;
		Condition condition = this.CreateBurnCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 0.1f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x0008B2D6 File Offset: 0x000894D6
	private Item CreateFireball()
	{
		return SkillBase.ItemDatabaseModule.GetItem(35);
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x0008B2E4 File Offset: 0x000894E4
	private Condition CreateBurnCondition()
	{
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 6f, 1f, 0.17f, effect, 2, 0f, "fireball");
	}
}
