using System;
using UnityEngine;

// Token: 0x020004E5 RID: 1253
public class DarkballNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C0E RID: 7182 RVA: 0x0008DFA4 File Offset: 0x0008C1A4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootDarkball(skillBaseConfig, target);
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x0008DFE4 File Offset: 0x0008C1E4
	private void ShootDarkball(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateDarkball();
		Condition condition = this.CreateStigmaCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 0.3f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C10 RID: 7184 RVA: 0x0008E021 File Offset: 0x0008C221
	private Item CreateDarkball()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(36);
	}

	// Token: 0x06001C11 RID: 7185 RVA: 0x0008E030 File Offset: 0x0008C230
	private Condition CreateStigmaCondition()
	{
		Effect effect = new Effect("DarkStrike", 0.2f, 0.3f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 6f, 1f, 0.17f, effect, 2, 0f, "");
	}
}
