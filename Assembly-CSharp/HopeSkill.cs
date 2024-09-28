using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
public class HopeSkill : SkillBase
{
	// Token: 0x06001BB0 RID: 7088 RVA: 0x0008C62C File Offset: 0x0008A82C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		base.StartCoroutine(skillBaseConfig, this.StartBlessingOnTargets(skillBaseConfig, friendlyTargets));
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x0008C667 File Offset: 0x0008A867
	private IEnumerator StartBlessingOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.15f);
		int num;
		for (int i = 0; i < targets.Count; i = num + 1)
		{
			this.StartBlessingsOnTarget(skillBaseConfig, targets[i]);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x0008C684 File Offset: 0x0008A884
	private void StartBlessingsOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		float duration = 600f;
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Agility, duration, 3f, skillBaseConfig.Skill.SkillPower, default(Effect), 0, 0f, "");
		Effect effect = new Effect("IceCurtain", 1f, 0.3f);
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Vitality, duration, 3f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		EffectModule effectModule;
		target.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect(skillBaseConfig.Skill.TargetSoundEffectName, 1f, 0f, Vector3.zero);
	}
}
