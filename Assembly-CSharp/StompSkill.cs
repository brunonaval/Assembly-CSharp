using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class StompSkill : SkillBase
{
	// Token: 0x06001E0F RID: 7695 RVA: 0x00096450 File Offset: 0x00094650
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.ShowEarthBlastEffect(skillBaseConfig);
		for (int i = 0; i < targets.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x000964A4 File Offset: 0x000946A4
	private void ShowEarthBlastEffect(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "EarthBlast",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x0009650C File Offset: 0x0009470C
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 5);
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 3.5f, 0.5f, 0f, effect, 5, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(1f, condition)
		});
	}
}
