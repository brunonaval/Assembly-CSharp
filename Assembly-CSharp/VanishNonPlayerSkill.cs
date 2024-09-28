using System;

// Token: 0x02000523 RID: 1315
public class VanishNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D12 RID: 7442 RVA: 0x000921BC File Offset: 0x000903BC
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartInvisibility(skillBaseConfig);
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000921E8 File Offset: 0x000903E8
	private void StartInvisibility(SkillBaseConfig skillBaseConfig)
	{
		Condition condition = new Condition(ConditionCategory.Invisibility, ConditionType.Invisible, skillBaseConfig.Skill.SkillPower, skillBaseConfig.Skill.SkillPower, 0f, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
