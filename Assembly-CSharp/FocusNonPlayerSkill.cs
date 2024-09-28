using System;

// Token: 0x020004F2 RID: 1266
public class FocusNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C45 RID: 7237 RVA: 0x0008EDCC File Offset: 0x0008CFCC
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartConditions(skillBaseConfig);
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x0008EDF8 File Offset: 0x0008CFF8
	private void StartConditions(SkillBaseConfig skillBaseConfig)
	{
		Effect effect = new Effect("EarthBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 7f, 1f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 7f, 7f, skillBaseConfig.Skill.SkillPower, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
	}
}
