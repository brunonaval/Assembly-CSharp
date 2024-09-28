using System;
using UnityEngine;

// Token: 0x0200053E RID: 1342
public class ExpertiseSkill : SkillBase
{
	// Token: 0x06001D98 RID: 7576 RVA: 0x000944AC File Offset: 0x000926AC
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		EffectModule casterEffectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out casterEffectModule);
		this.ShowWindStrikeEffect(casterEffectModule, "blessing");
		this.StartExpertiseCondition(skillBaseConfig, casterEffectModule);
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x000944F0 File Offset: 0x000926F0
	private void StartExpertiseCondition(SkillBaseConfig skillBaseConfig, EffectModule casterEffectModule)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		PvpModule pvpModule;
		skillBaseConfig.CasterObject.TryGetComponent<PvpModule>(out pvpModule);
		float duration = (pvpModule.TvtTeam == TvtTeam.None) ? 45f : 5f;
		Effect effect = new Effect("WindLeafAura", 1f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Expertise, ConditionType.Expertise, duration, 2f, 0f, effect, 0, 0f, "");
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		casterEffectModule.PlaySoundEffect(skillBaseConfig.Skill.TargetSoundEffectName, 1f, 0f, Vector3.zero);
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x00094598 File Offset: 0x00092798
	private void ShowWindStrikeEffect(EffectModule casterEffectModule, string soundEffectName)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "WindStrike",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.3f,
			SoundEffectName = soundEffectName
		};
		casterEffectModule.ShowEffects(effectConfig);
	}
}
