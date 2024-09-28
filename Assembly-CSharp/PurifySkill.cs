using System;

// Token: 0x02000533 RID: 1331
public class PurifySkill : SkillBase
{
	// Token: 0x06001D66 RID: 7526 RVA: 0x00093928 File Offset: 0x00091B28
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartHealingOnCaster(skillBaseConfig);
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x00093954 File Offset: 0x00091B54
	private void StartHealingOnCaster(SkillBaseConfig skillBaseConfig)
	{
		float num = 1.5f;
		float duration = (float)skillBaseConfig.Skill.CastAmount * num;
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		float power = (float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower;
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, duration, num, power, skillBaseConfig.Skill.TargetEffect, 1, 0f, skillBaseConfig.Skill.TargetSoundEffectName);
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		PurifySkill.RemoveBadConditionsFromCaster(conditionModule);
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000939E7 File Offset: 0x00091BE7
	private static void RemoveBadConditionsFromCaster(ConditionModule casterConditionModule)
	{
		casterConditionModule.RemoveCondition(ConditionType.Burn);
		casterConditionModule.RemoveCondition(ConditionType.Poison);
		casterConditionModule.RemoveCondition(ConditionType.Stigma);
		casterConditionModule.RemoveCondition(ConditionType.Confusion);
		casterConditionModule.RemoveCondition(ConditionType.Provoke);
	}
}
