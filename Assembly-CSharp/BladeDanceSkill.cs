using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052B RID: 1323
public class BladeDanceSkill : SkillBase
{
	// Token: 0x06001D3E RID: 7486 RVA: 0x00092D94 File Offset: 0x00090F94
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		if (targets.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.025f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, targets[0]);
		this.StartCasterConditions(skillBaseConfig);
		for (int i = 0; i < targets.Count; i++)
		{
			base.StartCoroutine(skillBaseConfig, this.ApplyMultipleDamagesOnTarget(skillBaseConfig, targets[i]));
		}
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x00092DFC File Offset: 0x00090FFC
	private void StartCasterConditions(SkillBaseConfig skillBaseConfig)
	{
		float num = 0.72f;
		Condition condition = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, num, num, 0f, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x00092E50 File Offset: 0x00091050
	private IEnumerator ApplyMultipleDamagesOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(0.12f);
		int i = 0;
		while (i < skillBaseConfig.Skill.CastAmount && !(target == null))
		{
			CreatureModule creatureModule;
			target.TryGetComponent<CreatureModule>(out creatureModule);
			if (!creatureModule.IsAlive)
			{
				break;
			}
			bool critical;
			bool blocked;
			int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
			EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 7);
			if (i != 0)
			{
				damageEffectConfig.EffectName = null;
			}
			CombatModule combatModule;
			target.TryGetComponent<CombatModule>(out combatModule);
			combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
			yield return delay;
			int num = i;
			i = num + 1;
		}
		yield break;
	}
}
