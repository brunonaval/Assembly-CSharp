using System;

// Token: 0x0200050F RID: 1295
public class RegenNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CC4 RID: 7364 RVA: 0x00090C5C File Offset: 0x0008EE5C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		this.StartRegeneration(skillBaseConfig);
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x00090C88 File Offset: 0x0008EE88
	private void StartRegeneration(SkillBaseConfig skillBaseConfig)
	{
		float num = 1.5f;
		float duration = (float)skillBaseConfig.Skill.CastAmount * num;
		NonPlayerAttributeModule nonPlayerAttributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<NonPlayerAttributeModule>(out nonPlayerAttributeModule);
		float power = (float)nonPlayerAttributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower;
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, duration, num, power, skillBaseConfig.Skill.TargetEffect, 1, 0f, skillBaseConfig.Skill.TargetSoundEffectName);
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
