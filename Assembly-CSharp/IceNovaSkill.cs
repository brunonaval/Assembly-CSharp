using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class IceNovaSkill : SkillBase
{
	// Token: 0x06001B63 RID: 7011 RVA: 0x0008B428 File Offset: 0x00089628
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.ShowRandomIceBlasts(skillBaseConfig);
		for (int i = 0; i < targets.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x0008B47C File Offset: 0x0008967C
	private void ShowRandomIceBlasts(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "IceBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "ice_strike",
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.05f, effectConfig);
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x0008B504 File Offset: 0x00089704
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
		Effect effect = new Effect("IceWindStrike", 0.25f, 0.2f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Freeze, 2f, 0.15f, 0f, effect, 0, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(1f, condition)
		});
	}
}
