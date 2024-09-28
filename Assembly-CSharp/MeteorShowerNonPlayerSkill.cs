using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class MeteorShowerNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C9B RID: 7323 RVA: 0x0008FF78 File Offset: 0x0008E178
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		MeteorShowerNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<MeteorShowerNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001C9C RID: 7324 RVA: 0x0008FFB7 File Offset: 0x0008E1B7
	private IEnumerator StartAttackingTargets(SkillBaseConfig skillBaseConfig, Vector3 targetPosition)
	{
		int totalCasts = skillBaseConfig.Skill.CastAmount;
		WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
		int num;
		for (int i = 0; i < totalCasts; i = num + 1)
		{
			this.ShowRandomComets(skillBaseConfig, targetPosition);
			this.GetTargetsAndApplyDamage(skillBaseConfig, targetPosition);
			yield return waitForOneSecond;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001C9D RID: 7325 RVA: 0x0008FFD4 File Offset: 0x0008E1D4
	private void GetTargetsAndApplyDamage(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, selectedTargetPosition, false);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i]);
		}
	}

	// Token: 0x06001C9E RID: 7326 RVA: 0x0009000C File Offset: 0x0008E20C
	private void ShowRandomComets(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Comet",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "comet",
			Position = selectedTargetPosition
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(10, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001C9F RID: 7327 RVA: 0x00090084 File Offset: 0x0008E284
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 2);
		ConditionConfig conditionConfig = MeteorShowerNonPlayerSkill.CreateBurnConditionConfig();
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001CA0 RID: 7328 RVA: 0x000900E4 File Offset: 0x0008E2E4
	private static ConditionConfig CreateBurnConditionConfig()
	{
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 8f, 1f, 0.15f, effect, 2, 0f, "fireball");
		return new ConditionConfig(1f, condition);
	}
}
