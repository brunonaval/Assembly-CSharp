using System;

// Token: 0x0200052D RID: 1325
public class DenialSkill : SkillBase
{
	// Token: 0x06001D48 RID: 7496 RVA: 0x00092FA8 File Offset: 0x000911A8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.ShowIceCurtainEffects(skillBaseConfig);
		this.StartCasterConditions(skillBaseConfig);
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x00092FD8 File Offset: 0x000911D8
	private void StartCasterConditions(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		Effect effect = new Effect("IceBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 10f, 1f, 0.25f, effect, 0, 0f, "");
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		Effect effect2 = new Effect("IceHit", 0.25f, 0.25f);
		Condition condition2 = new Condition(ConditionCategory.Reflecting, ConditionType.Reflect, 10f, 1f, skillBaseConfig.Skill.SkillPower, effect2, 6, 0f, "earth_blast");
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x0009308C File Offset: 0x0009128C
	private void ShowIceCurtainEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "IceCurtain",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.3f,
			SoundEffectName = "blessing"
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}
}
