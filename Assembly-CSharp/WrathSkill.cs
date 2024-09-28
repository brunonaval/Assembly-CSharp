using System;
using UnityEngine;

// Token: 0x02000538 RID: 1336
public class WrathSkill : SkillBase
{
	// Token: 0x06001D7A RID: 7546 RVA: 0x00093E50 File Offset: 0x00092050
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		EffectModule casterEffectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out casterEffectModule);
		if (conditionModule.HasActiveCondition(ConditionType.Wrath))
		{
			this.ShowDarkCurtainEffect("curse", casterEffectModule);
			conditionModule.RemoveCondition(ConditionType.Wrath);
			return;
		}
		this.ShowDarkCurtainEffect("blessing", casterEffectModule);
		this.StartWrathCondition(skillBaseConfig, conditionModule, casterEffectModule);
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x00093EC4 File Offset: 0x000920C4
	private void StartWrathCondition(SkillBaseConfig skillBaseConfig, ConditionModule casterConditionModule, EffectModule casterEffectModule)
	{
		Effect effect = new Effect("DarkLeafAura", 1f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Wrath, ConditionType.Wrath, 3600f, 2f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		casterConditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		casterEffectModule.PlaySoundEffect(skillBaseConfig.Skill.TargetSoundEffectName, 1f, 0f, Vector3.zero);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x00093F44 File Offset: 0x00092144
	private void ShowDarkCurtainEffect(string soundEffectName, EffectModule casterEffectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "DarkCurtain",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.3f,
			SoundEffectName = soundEffectName
		};
		casterEffectModule.ShowEffects(effectConfig);
	}
}
