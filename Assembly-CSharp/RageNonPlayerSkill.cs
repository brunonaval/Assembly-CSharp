using System;

// Token: 0x0200050E RID: 1294
public class RageNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CC1 RID: 7361 RVA: 0x00090B40 File Offset: 0x0008ED40
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartBlessings(skillBaseConfig);
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x00090B6C File Offset: 0x0008ED6C
	private void StartBlessings(SkillBaseConfig skillBaseConfig)
	{
		Effect effect = new Effect("EarthBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 7f, 1f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Agility, 7f, 7f, skillBaseConfig.Skill.SkillPower, default(Effect), 0, 0f, "");
		Condition condition3 = new Condition(ConditionCategory.Blessing, ConditionType.Precision, 7f, 7f, skillBaseConfig.Skill.SkillPower, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition3, skillBaseConfig.CasterObject, true);
	}
}
