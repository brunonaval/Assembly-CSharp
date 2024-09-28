using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class RestrictionSkill : SkillBase
{
	// Token: 0x06001D73 RID: 7539 RVA: 0x00093BFC File Offset: 0x00091DFC
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, true);
		if (targets.Count == 0)
		{
			return;
		}
		base.StartCastingAnimation(skillBaseConfig, new AnimationConfig(1, 5f), null);
		Effect effect = new Effect("BlueStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Restricted, 5f, 0.5f, 0f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		for (int i = 0; i < targets.Count; i++)
		{
			ConditionModule conditionModule2;
			targets[i].TryGetComponent<ConditionModule>(out conditionModule2);
			conditionModule2.StartCondition(condition, skillBaseConfig.CasterObject, true);
		}
	}
}
