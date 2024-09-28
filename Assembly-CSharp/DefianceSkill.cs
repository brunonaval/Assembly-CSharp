using System;
using UnityEngine;

// Token: 0x0200054C RID: 1356
public class DefianceSkill : SkillBase
{
	// Token: 0x06001DF3 RID: 7667 RVA: 0x00095D88 File Offset: 0x00093F88
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		base.StartCastingAnimation(skillBaseConfig, default(AnimationConfig), null);
		this.HealCaster(skillBaseConfig);
		this.StartInvincibleCondition(skillBaseConfig);
	}

	// Token: 0x06001DF4 RID: 7668 RVA: 0x00095DB4 File Offset: 0x00093FB4
	private void StartInvincibleCondition(SkillBaseConfig skillBaseConfig)
	{
		Effect effect = new Effect("EarthStrike", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 3f, 0.5f, 0f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001DF5 RID: 7669 RVA: 0x00095E18 File Offset: 0x00094018
	private void HealCaster(SkillBaseConfig skillBaseConfig)
	{
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int amount = Mathf.RoundToInt((float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower);
		attributeModule.AddHealth(skillBaseConfig.CasterObject, amount, true, this.CreateHealingEffect(skillBaseConfig));
	}

	// Token: 0x06001DF6 RID: 7670 RVA: 0x00095E64 File Offset: 0x00094064
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
}
