using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class BattlefieldSkill : SkillBase
{
	// Token: 0x06001DE6 RID: 7654 RVA: 0x00095AA8 File Offset: 0x00093CA8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		float num = (float)skillBaseConfig.Skill.CastAmount * 1f;
		Vector3 position = skillBaseConfig.CasterObject.transform.position;
		effectModule.ShowVisualEffect("RedRing", 4f, 0.5f, num, position);
		effectModule.PlaySoundEffect("magic_loop", 1f, num, position);
		base.StartCoroutine(skillBaseConfig, this.BattlefieldSphereLoop(skillBaseConfig, position, num));
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x00095B36 File Offset: 0x00093D36
	private IEnumerator BattlefieldSphereLoop(SkillBaseConfig skillBaseConfig, Vector3 refugePosition, float refugeDuration)
	{
		float startTime = Time.time;
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(2f);
		do
		{
			List<GameObject> friendlyTargetsFromPosition = base.GetFriendlyTargetsFromPosition(skillBaseConfig, refugePosition, skillBaseConfig.Skill.Range, false);
			for (int i = 0; i < friendlyTargetsFromPosition.Count; i++)
			{
				float duration = 2f;
				Effect effect = new Effect("BloodCurtain", 1f, 0.3f);
				Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, duration, 1f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
				ConditionModule conditionModule;
				friendlyTargetsFromPosition[i].TryGetComponent<ConditionModule>(out conditionModule);
				conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
			}
			yield return delay;
		}
		while (Time.time - startTime <= refugeDuration);
		yield break;
	}
}
