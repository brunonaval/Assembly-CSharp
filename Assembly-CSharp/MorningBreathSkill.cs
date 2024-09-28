using System;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public class MorningBreathSkill : SkillBase
{
	// Token: 0x06001B78 RID: 7032 RVA: 0x0008B8E0 File Offset: 0x00089AE0
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.HealCaster(skillBaseConfig);
		this.RemoveBadConditions(skillBaseConfig);
		this.StartInvisibilityCondition(skillBaseConfig);
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x0008B918 File Offset: 0x00089B18
	private void RemoveBadConditions(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.RemoveCondition(ConditionType.Burn);
		conditionModule.RemoveCondition(ConditionType.Poison);
		conditionModule.RemoveCondition(ConditionType.Stigma);
		conditionModule.RemoveCondition(ConditionType.Confusion);
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x0008B954 File Offset: 0x00089B54
	private void HealCaster(SkillBaseConfig skillBaseConfig)
	{
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int amount = Mathf.RoundToInt((float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower);
		attributeModule.AddHealth(skillBaseConfig.CasterObject, amount, true, this.CreateHealingEffect(skillBaseConfig));
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x0008B9A0 File Offset: 0x00089BA0
	private EffectConfig CreateHealingEffect(SkillBaseConfig skillBaseConfig)
	{
		return new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.TargetEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.TargetEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.TargetEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.TargetSoundEffectName,
			TextColorId = 1
		};
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x0008BA1C File Offset: 0x00089C1C
	private void StartInvisibilityCondition(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		Condition condition = new Condition(ConditionCategory.Invisibility, ConditionType.Invisible, 3f, 3f, 0f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
