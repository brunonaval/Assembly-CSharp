using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200051B RID: 1307
public class ThunderStormNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CED RID: 7405 RVA: 0x00091754 File Offset: 0x0008F954
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		ThunderStormNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<ThunderStormNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x00091793 File Offset: 0x0008F993
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

	// Token: 0x06001CEF RID: 7407 RVA: 0x000917B0 File Offset: 0x0008F9B0
	private void GetTargetsAndApplyDamageAsync(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition, bool shouldStartBurnCondition)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, selectedTargetPosition, false);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i], shouldStartBurnCondition);
		}
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x000917E8 File Offset: 0x0008F9E8
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
			ConditionConfig conditionConfig = ThunderStormNonPlayerSkill.CreateStigmaConditionConfig();
			combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
			{
				conditionConfig
			});
			return;
		}
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x00091860 File Offset: 0x0008FA60
	private static ConditionConfig CreateStigmaConditionConfig()
	{
		Effect effect = new Effect("Thunder", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 10f, 1f, 0.17f, effect, 7, 0f, "eletric_strike");
		return new ConditionConfig(1f, condition);
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x000918B4 File Offset: 0x0008FAB4
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
}
