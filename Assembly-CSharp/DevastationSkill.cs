using System;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class DevastationSkill : SkillBase
{
	// Token: 0x06001DF8 RID: 7672 RVA: 0x00095EE0 File Offset: 0x000940E0
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.008f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootBigStone(skillBaseConfig, target);
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x00095F20 File Offset: 0x00094120
	private void ShootBigStone(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateBigStone();
		Condition condition = this.CreateStunCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 1f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x00095F5D File Offset: 0x0009415D
	private Item CreateBigStone()
	{
		return SkillBase.ItemDatabaseModule.GetItem(40);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x00095F6C File Offset: 0x0009416C
	private Condition CreateStunCondition()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		return new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
	}
}
