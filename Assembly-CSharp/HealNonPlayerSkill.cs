using System;
using UnityEngine;

// Token: 0x020004F4 RID: 1268
public class HealNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C4E RID: 7246 RVA: 0x0008F084 File Offset: 0x0008D284
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		this.HealCaster(skillBaseConfig);
		this.RemoveBadConditions(skillBaseConfig);
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x0008F0B4 File Offset: 0x0008D2B4
	private void RemoveBadConditions(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.RemoveCondition(ConditionType.Burn);
		conditionModule.RemoveCondition(ConditionType.Poison);
		conditionModule.RemoveCondition(ConditionType.Stigma);
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x0008F0E8 File Offset: 0x0008D2E8
	private void HealCaster(SkillBaseConfig skillBaseConfig)
	{
		NonPlayerAttributeModule nonPlayerAttributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<NonPlayerAttributeModule>(out nonPlayerAttributeModule);
		int num = Mathf.RoundToInt((float)nonPlayerAttributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower);
		nonPlayerAttributeModule.AddHealth(num);
		this.ShowHealingEffects(skillBaseConfig, num);
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x0008F12C File Offset: 0x0008D32C
	private void ShowHealingEffects(SkillBaseConfig skillBaseConfig, int healPower)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.TargetEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.TargetEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.TargetEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.TargetSoundEffectName,
			Text = string.Format("+{0}", healPower),
			TextColorId = 1
		};
		skillBaseConfig.CasterObject.GetComponent<EffectModule>().ShowEffects(effectConfig);
	}
}
