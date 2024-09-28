using System;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public class ParalyzingStingNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CB0 RID: 7344 RVA: 0x000906A4 File Offset: 0x0008E8A4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootParalizeSting(skillBaseConfig, target);
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000906E4 File Offset: 0x0008E8E4
	private void ShootParalizeSting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateParalizeSting();
		Condition condition = this.CreateParalizeCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 0.05f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001CB2 RID: 7346 RVA: 0x00090721 File Offset: 0x0008E921
	private Item CreateParalizeSting()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(19);
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x00090730 File Offset: 0x0008E930
	private Condition CreateParalizeCondition()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		return new Condition(ConditionCategory.Paralyzing, ConditionType.Paralize, 2f, 1f, 0.1f, effect, 5, 0f, "");
	}
}
