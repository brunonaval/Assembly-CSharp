using System;
using UnityEngine;

// Token: 0x020004F0 RID: 1264
public class FireballNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C3C RID: 7228 RVA: 0x0008EC04 File Offset: 0x0008CE04
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootFireball(skillBaseConfig, target);
	}

	// Token: 0x06001C3D RID: 7229 RVA: 0x0008EC44 File Offset: 0x0008CE44
	private void ShootFireball(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateFireball();
		Condition condition = this.CreateBurnCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 0.3f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x0008EC81 File Offset: 0x0008CE81
	private Item CreateFireball()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(35);
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x0008EC90 File Offset: 0x0008CE90
	private Condition CreateBurnCondition()
	{
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 6f, 1f, 0.17f, effect, 2, 0f, "fireball");
	}
}
