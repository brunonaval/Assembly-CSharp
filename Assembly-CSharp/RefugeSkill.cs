using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000534 RID: 1332
public class RefugeSkill : SkillBase
{
	// Token: 0x06001D6A RID: 7530 RVA: 0x00093A10 File Offset: 0x00091C10
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		float num = (float)skillBaseConfig.Skill.CastAmount * 1f;
		Vector3 position = skillBaseConfig.CasterObject.transform.position;
		effectModule.ShowVisualEffect("BlueRing", 4f, 0.5f, num, position);
		effectModule.PlaySoundEffect("magic_loop", 1f, num, position);
		base.StartCoroutine(skillBaseConfig, this.RefugeSphereLoop(skillBaseConfig, position, num));
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x00093A9E File Offset: 0x00091C9E
	private IEnumerator RefugeSphereLoop(SkillBaseConfig skillBaseConfig, Vector3 refugePosition, float refugeDuration)
	{
		float startTime = Time.time;
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(2f);
		do
		{
			List<GameObject> friendlyTargetsFromPosition = base.GetFriendlyTargetsFromPosition(skillBaseConfig, refugePosition, skillBaseConfig.Skill.Range, false);
			for (int i = 0; i < friendlyTargetsFromPosition.Count; i++)
			{
				float duration = 2f;
				Effect effect = new Effect("IceCurtain", 1f, 0.3f);
				Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, duration, 1f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
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
