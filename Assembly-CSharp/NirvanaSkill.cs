using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004DD RID: 1245
public class NirvanaSkill : SkillBase
{
	// Token: 0x06001BE9 RID: 7145 RVA: 0x0008D4D8 File Offset: 0x0008B6D8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		base.StartCoroutine(skillBaseConfig, this.StartAttackingTargets(skillBaseConfig, skillBaseConfig.CasterObject.transform.position));
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x0008D519 File Offset: 0x0008B719
	private IEnumerator StartAttackingTargets(SkillBaseConfig skillBaseConfig, Vector3 position)
	{
		WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
		int totalCasts = skillBaseConfig.Skill.CastAmount;
		int num;
		for (int i = 0; i < totalCasts; i = num + 1)
		{
			this.ShowRandomLightStrikes(skillBaseConfig, position);
			this.GetTargetsAndApplyDamage(skillBaseConfig, position);
			yield return waitForOneSecond;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x0008D538 File Offset: 0x0008B738
	private void GetTargetsAndApplyDamage(SkillBaseConfig skillBaseConfig, Vector3 position)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, position, false);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i]);
		}
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x0008D570 File Offset: 0x0008B770
	private void ShowRandomLightStrikes(SkillBaseConfig skillBaseConfig, Vector3 position)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "LightStrike",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "wind_blast",
			Position = position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(7, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0008D5E8 File Offset: 0x0008B7E8
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 6);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}
}
