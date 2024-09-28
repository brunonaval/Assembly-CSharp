using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class JudgementSkill : SkillBase
{
	// Token: 0x06001D54 RID: 7508 RVA: 0x00093378 File Offset: 0x00091578
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		for (int i = 0; i < friendlyTargets.Count; i++)
		{
			this.StartBlessingsOnFriendlyTarget(skillBaseConfig, friendlyTargets[i]);
		}
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		for (int j = 0; j < targets.Count; j++)
		{
			this.StartCursesOnTarget(skillBaseConfig, targets[j]);
		}
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x000933F0 File Offset: 0x000915F0
	private void StartBlessingsOnFriendlyTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		float duration = 600f;
		Effect effect = new Effect("IceBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, duration, 3f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		EffectModule effectModule;
		target.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect("blessing", 1f, 0f, Vector3.zero);
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x00093484 File Offset: 0x00091684
	private void StartCursesOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		float duration = 15f;
		Effect effect = new Effect("DarkCurtain", 1f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Curse, ConditionType.Toughness, duration, 1f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		Condition condition2 = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, duration, 0.5f, 0f, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.SetTarget(skillBaseConfig.CasterObject, true);
		EffectModule effectModule;
		target.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect("curse", 1f, 0f, Vector3.zero);
	}
}
