using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
public class LapisSkill : SkillBase
{
	// Token: 0x06001BC3 RID: 7107 RVA: 0x0008CB0C File Offset: 0x0008AD0C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		float num = (float)skillBaseConfig.Skill.CastAmount * 1f;
		Vector3 position = skillBaseConfig.CasterObject.transform.position;
		effectModule.ShowVisualEffect("GreenRing", 4f, 0.5f, num, position);
		effectModule.PlaySoundEffect("magic_loop", 1f, num, position);
		base.StartCoroutine(skillBaseConfig, this.LifeSphereLoop(skillBaseConfig, position, num));
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x0008CB9A File Offset: 0x0008AD9A
	private IEnumerator LifeSphereLoop(SkillBaseConfig skillBaseConfig, Vector3 refugePosition, float refugeDuration)
	{
		float startTime = Time.time;
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(2f);
		do
		{
			List<GameObject> friendlyTargetsFromPosition = base.GetFriendlyTargetsFromPosition(skillBaseConfig, refugePosition, skillBaseConfig.Skill.Range, false);
			for (int i = 0; i < friendlyTargetsFromPosition.Count; i++)
			{
				if (!(friendlyTargetsFromPosition[i] == null))
				{
					AttributeModule attributeModule;
					friendlyTargetsFromPosition[i].TryGetComponent<AttributeModule>(out attributeModule);
					float power = (float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower;
					float duration = 2f;
					Effect effect = new Effect("HealWall", 0.5f, 0.3f);
					Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, duration, 1f, power, effect, 1, 0f, "heal_strike");
					ConditionModule conditionModule;
					friendlyTargetsFromPosition[i].TryGetComponent<ConditionModule>(out conditionModule);
					conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
				}
			}
			yield return delay;
		}
		while (Time.time - startTime <= refugeDuration);
		yield break;
	}
}
