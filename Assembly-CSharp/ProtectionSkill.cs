using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000531 RID: 1329
public class ProtectionSkill : SkillBase
{
	// Token: 0x06001D5F RID: 7519 RVA: 0x0009374C File Offset: 0x0009194C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		float duration = this.CalculateDuration(friendlyTargets.Count);
		foreach (GameObject gameObject in friendlyTargets)
		{
			for (int i = 0; i < friendlyTargets.Count; i++)
			{
				ConditionModule conditionModule;
				friendlyTargets[i].TryGetComponent<ConditionModule>(out conditionModule);
				Effect effect = new Effect("IceShield", 0.5f, 0.25f);
				Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, duration, 0.5f, 0f, effect, 0, 0f, "");
				conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
			}
		}
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x0009382C File Offset: 0x00091A2C
	private float CalculateDuration(int targetsCount)
	{
		if (targetsCount <= 2)
		{
			return 2f;
		}
		if (targetsCount <= 5)
		{
			return 3.5f;
		}
		if (targetsCount <= 7)
		{
			return 5f;
		}
		return 1f;
	}
}
