using System;
using UnityEngine;

// Token: 0x020004EA RID: 1258
public class EarthShotNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C21 RID: 7201 RVA: 0x0008E4E4 File Offset: 0x0008C6E4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootBigStone(skillBaseConfig, target);
	}

	// Token: 0x06001C22 RID: 7202 RVA: 0x0008E524 File Offset: 0x0008C724
	private void ShootBigStone(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateBigStone();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, this.CreateStunCondition(), 0.05f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C23 RID: 7203 RVA: 0x0008E55F File Offset: 0x0008C75F
	private Item CreateBigStone()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(40);
	}

	// Token: 0x06001C24 RID: 7204 RVA: 0x0008E570 File Offset: 0x0008C770
	private Condition CreateStunCondition()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		return new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
	}
}
