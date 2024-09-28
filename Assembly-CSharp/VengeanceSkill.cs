using System;

// Token: 0x02000555 RID: 1365
public class VengeanceSkill : SkillBase
{
	// Token: 0x06001E23 RID: 7715 RVA: 0x00096940 File Offset: 0x00094B40
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		SkillModule skillModule;
		skillBaseConfig.CasterObject.TryGetComponent<SkillModule>(out skillModule);
		if (skillModule.LastCastedNonDefaultSkillId == 0)
		{
			return;
		}
		base.StartCastingAnimation(skillBaseConfig, default(AnimationConfig), null);
		skillModule.SetLastUseTime(skillModule.LastCastedNonDefaultSkillId, 0.0);
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(this.CreateTargetEffect(skillBaseConfig));
	}

	// Token: 0x06001E24 RID: 7716 RVA: 0x000969A8 File Offset: 0x00094BA8
	private EffectConfig CreateTargetEffect(SkillBaseConfig skillBaseConfig)
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
