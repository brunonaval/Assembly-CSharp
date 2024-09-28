using System;

// Token: 0x02000541 RID: 1345
public class StealthSkill : SkillBase
{
	// Token: 0x06001DA6 RID: 7590 RVA: 0x000948C4 File Offset: 0x00092AC4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartInvisibilityCondition(skillBaseConfig);
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x000948F0 File Offset: 0x00092AF0
	private void StartInvisibilityCondition(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		Condition condition = new Condition(ConditionCategory.Invisibility, ConditionType.Invisible, 5f, 5f, 0f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
