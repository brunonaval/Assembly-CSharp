using System;

// Token: 0x02000543 RID: 1347
public class TouchOfNatureSkill : SkillBase
{
	// Token: 0x06001DAD RID: 7597 RVA: 0x00094A6C File Offset: 0x00092C6C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartCasterConditions(skillBaseConfig);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x00094A98 File Offset: 0x00092C98
	private void StartCasterConditions(SkillBaseConfig skillBaseConfig)
	{
		float num = 1.5f;
		float duration = (float)skillBaseConfig.Skill.CastAmount * num;
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		float power = (float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower;
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, duration, num, power, skillBaseConfig.Skill.TargetEffect, 1, 0f, skillBaseConfig.Skill.TargetSoundEffectName);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		Condition condition2 = new Condition(ConditionCategory.Invincibility, ConditionType.Dash, 3f, 3f, 0f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		Condition condition3 = new Condition(ConditionCategory.Blessing, ConditionType.Agility, 5f, 5f, 3f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition3, skillBaseConfig.CasterObject, true);
	}
}
