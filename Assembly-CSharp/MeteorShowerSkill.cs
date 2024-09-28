using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class MeteorShowerSkill : SkillBase
{
	// Token: 0x06001B6B RID: 7019 RVA: 0x0008B620 File Offset: 0x00089820
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		base.StartCoroutine(skillBaseConfig, this.StartAttackingTargets(skillBaseConfig, skillBaseConfig.CasterObject.transform.position));
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x0008B661 File Offset: 0x00089861
	private IEnumerator StartAttackingTargets(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition)
	{
		WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
		int totalCasts = skillBaseConfig.Skill.CastAmount;
		int num;
		for (int i = 0; i < totalCasts; i = num + 1)
		{
			this.ShowRandomComets(skillBaseConfig, selectedTargetPosition);
			this.GetTargetsAndApplyDamage(skillBaseConfig, selectedTargetPosition, i == totalCasts - 1);
			yield return waitForOneSecond;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x0008B680 File Offset: 0x00089880
	private void GetTargetsAndApplyDamage(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition, bool shouldStartBurnCondition)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, selectedTargetPosition, false);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i], shouldStartBurnCondition);
		}
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x0008B6B8 File Offset: 0x000898B8
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
		effectModule.ShowEffectsRandomly(7, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x0008B730 File Offset: 0x00089930
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target, bool shouldStartBurnCondition)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 2);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		if (shouldStartBurnCondition)
		{
			ConditionConfig conditionConfig = MeteorShowerSkill.CreateBurnConditionConfig();
			combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
			{
				conditionConfig
			});
			return;
		}
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x0008B7A8 File Offset: 0x000899A8
	private static ConditionConfig CreateBurnConditionConfig()
	{
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 10f, 1f, 0.17f, effect, 2, 0f, "fireball");
		return new ConditionConfig(1f, condition);
	}
}
