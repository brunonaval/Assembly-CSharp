using System;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public class CosmosSkill : SkillBase
{
	// Token: 0x06001B9E RID: 7070 RVA: 0x0008C1B8 File Offset: 0x0008A3B8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject gameObject = base.GetFriendlySelectedTarget(skillBaseConfig, true) ?? skillBaseConfig.CasterObject;
		if (gameObject == null)
		{
			this.ShowDontHaveTargetMessage(skillBaseConfig);
			return;
		}
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int num = Mathf.CeilToInt(skillBaseConfig.Skill.SkillPower * (float)attributeModule.MaxHealth);
		if (attributeModule.CurrentHealth < num)
		{
			this.ShowTooLowHealthMessage(skillBaseConfig);
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, gameObject);
		this.RemoveHealthAndApplyConditions(skillBaseConfig, num, attributeModule, gameObject);
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x0008C240 File Offset: 0x0008A440
	private void RemoveHealthAndApplyConditions(SkillBaseConfig skillBaseConfig, int neededHealth, AttributeModule casterAttributeModule, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		this.RemoveHealthFromCaster(skillBaseConfig, neededHealth, casterAttributeModule);
		this.ShowTargetEffects(skillBaseConfig, target);
		this.ApplyResurrectConditionOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001BA0 RID: 7072 RVA: 0x0008C268 File Offset: 0x0008A468
	private void ApplyResurrectConditionOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Condition condition = new Condition(ConditionCategory.Resurrection, ConditionType.Resurrect, 1800f, 5f, 0f, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x0008C2BC File Offset: 0x0008A4BC
	private void ShowTargetEffects(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.TargetEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.TargetEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.TargetEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.TargetSoundEffectName
		};
		EffectModule effectModule;
		target.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x0008C340 File Offset: 0x0008A540
	private void RemoveHealthFromCaster(SkillBaseConfig skillBaseConfig, int neededHealth, AttributeModule casterAttributeModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "BloodStrike",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			Text = string.Format("{0}", neededHealth),
			TextColorId = 3
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
		casterAttributeModule.AddHealth(skillBaseConfig.CasterObject, -neededHealth, true, default(EffectConfig));
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x0008C3C8 File Offset: 0x0008A5C8
	private void ShowTooLowHealthMessage(SkillBaseConfig skillBaseConfig)
	{
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowScreenMessage("health_too_low_to_use_skill_message", 3, 3.5f, Array.Empty<string>());
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x0008C3FC File Offset: 0x0008A5FC
	private void ShowDontHaveTargetMessage(SkillBaseConfig skillBaseConfig)
	{
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowScreenMessage("skill_dont_have_target_message", 3, 3.5f, Array.Empty<string>());
	}
}
