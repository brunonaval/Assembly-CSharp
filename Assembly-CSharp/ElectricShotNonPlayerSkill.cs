using System;
using UnityEngine;

// Token: 0x020004EB RID: 1259
public class ElectricShotNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C26 RID: 7206 RVA: 0x0008E5B8 File Offset: 0x0008C7B8
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

	// Token: 0x06001C27 RID: 7207 RVA: 0x0008E5F8 File Offset: 0x0008C7F8
	private void ShootBigStone(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateTheTeenBan();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, this.CreateStunCondition(), 0.05f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C28 RID: 7208 RVA: 0x0008E633 File Offset: 0x0008C833
	private Item CreateTheTeenBan()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(1257);
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x0008E644 File Offset: 0x0008C844
	private Condition CreateStunCondition()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		return new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
	}
}
