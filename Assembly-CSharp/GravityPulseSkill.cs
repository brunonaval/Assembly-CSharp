using System;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
public class GravityPulseSkill : SkillBase
{
	// Token: 0x06001B5E RID: 7006 RVA: 0x0008B32C File Offset: 0x0008952C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootDarkSphere(skillBaseConfig, target);
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x0008B36C File Offset: 0x0008956C
	private void ShootDarkSphere(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateDarkSphere();
		Condition condition = GravityPulseSkill.CreateConfusionCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 1f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x0008B3A8 File Offset: 0x000895A8
	private Item CreateDarkSphere()
	{
		Item item = SkillBase.ItemDatabaseModule.GetItem(1330);
		Projectile projectile = item.Projectile;
		projectile.Pull = true;
		item.Projectile = projectile;
		return item;
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x0008B3E0 File Offset: 0x000895E0
	private static Condition CreateConfusionCondition()
	{
		Effect effect = new Effect("ConfusionRing", 0.25f, 0.075f);
		return new Condition(ConditionCategory.Confusion, ConditionType.Confusion, 2f, 0.5f, 0.15f, effect, 0, 0f, "");
	}
}
