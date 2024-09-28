using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class EmpowerSkill : SkillBase
{
	// Token: 0x06001BA6 RID: 7078 RVA: 0x0008C430 File Offset: 0x0008A630
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		base.StartCoroutine(skillBaseConfig, this.StartBlessingOnTargets(skillBaseConfig, friendlyTargets));
	}

	// Token: 0x06001BA7 RID: 7079 RVA: 0x0008C46B File Offset: 0x0008A66B
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

	// Token: 0x06001BA8 RID: 7080 RVA: 0x0008C488 File Offset: 0x0008A688
	private void StartBlessingsOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		float duration = 600f;
		Effect effect = new Effect("DarkBarrier", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, duration, 3f, skillBaseConfig.Skill.SkillPower, effect, 0, 0f, "");
		Effect effect2 = new Effect("AirStrike", 0.5f, 0.25f);
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Precision, duration, 3f, skillBaseConfig.Skill.SkillPower, effect2, 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		EffectModule effectModule;
		target.TryGetComponent<EffectModule>(out effectModule);
		effectModule.PlaySoundEffect(skillBaseConfig.Skill.TargetSoundEffectName, 1f, 0f, Vector3.zero);
	}
}
