using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class RageSkill : SkillBase
{
	// Token: 0x06001DFD RID: 7677 RVA: 0x00095FB4 File Offset: 0x000941B4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.ShowRageEffects(skillBaseConfig);
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		base.StartCoroutine(skillBaseConfig, this.StartRageConditionsOnTargets(skillBaseConfig, friendlyTargets));
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x00095FF6 File Offset: 0x000941F6
	public IEnumerator StartRageConditionsOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.15f);
		int num;
		for (int i = 0; i < targets.Count; i = num + 1)
		{
			this.StartRageConditions(skillBaseConfig, targets[i]);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x00096014 File Offset: 0x00094214
	private float CalculatePowerBasedOnHealth(SkillBaseConfig skillBaseConfig)
	{
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int num = attributeModule.CurrentHealth / attributeModule.MaxHealth;
		if ((float)num >= 0.85f)
		{
			return 0.12f;
		}
		if ((float)num >= 0.5f)
		{
			return 0.27f;
		}
		return 0.37f;
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x00096060 File Offset: 0x00094260
	private void ShowRageEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		CreatureModule creatureModule;
		skillBaseConfig.CasterObject.TryGetComponent<CreatureModule>(out creatureModule);
		EffectConfig effectConfig = default(EffectConfig);
		if (creatureModule.Gender == CreatureGender.Male)
		{
			effectConfig.SoundEffectName = "male_rage";
			effectModule.ShowEffects(effectConfig);
			return;
		}
		if (creatureModule.Gender == CreatureGender.Female)
		{
			effectConfig.SoundEffectName = "female_rage";
			effectModule.ShowEffects(effectConfig);
		}
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000960CC File Offset: 0x000942CC
	private void StartRageConditions(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		float duration = 45f;
		float power = this.CalculatePowerBasedOnHealth(skillBaseConfig);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, duration, 3f, power, skillBaseConfig.Skill.TargetEffect, 0, 0f, "");
		Condition condition2 = new Condition(ConditionCategory.Blessing, ConditionType.Precision, duration, 3f, power, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
		AttributeModule attributeModule;
		target.TryGetComponent<AttributeModule>(out attributeModule);
		attributeModule.SetEnduranceToMax();
	}
}
