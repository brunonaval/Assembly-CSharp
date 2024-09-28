using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class ThunderstormSkill : SkillBase
{
	// Token: 0x06001B80 RID: 7040 RVA: 0x0008BAE8 File Offset: 0x00089CE8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		base.StartCoroutine(skillBaseConfig, this.StartAttackingTargets(skillBaseConfig, skillBaseConfig.CasterObject.transform.position));
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x0008BB29 File Offset: 0x00089D29
	private IEnumerator StartAttackingTargets(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition)
	{
		WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
		int totalCasts = skillBaseConfig.Skill.CastAmount;
		int num;
		for (int i = 0; i < totalCasts; i = num + 1)
		{
			this.ShowRandomThunders(skillBaseConfig, selectedTargetPosition);
			this.GetTargetsAndApplyDamageAsync(skillBaseConfig, selectedTargetPosition, i == totalCasts - 1);
			yield return waitForOneSecond;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001B82 RID: 7042 RVA: 0x0008BB48 File Offset: 0x00089D48
	private void GetTargetsAndApplyDamageAsync(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition, bool shouldStartBurnCondition)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, selectedTargetPosition, false);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i], shouldStartBurnCondition);
		}
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0008BB80 File Offset: 0x00089D80
	private void ShowRandomThunders(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "ThunderStrike",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "thunder",
			Position = selectedTargetPosition
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(21, skillBaseConfig.Skill.Range, 0.05f, effectConfig);
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x0008BBF8 File Offset: 0x00089DF8
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target, bool shouldStartBurnCondition)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 7);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		if (shouldStartBurnCondition)
		{
			ConditionConfig conditionConfig = ThunderstormSkill.CreateStigmaConditionConfig();
			combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
			{
				conditionConfig
			});
			return;
		}
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x0008BC70 File Offset: 0x00089E70
	private static ConditionConfig CreateStigmaConditionConfig()
	{
		Effect effect = new Effect("Thunder", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 10f, 1f, 0.17f, effect, 7, 0f, "eletric_strike");
		return new ConditionConfig(1f, condition);
	}
}
