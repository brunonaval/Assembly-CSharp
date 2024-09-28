using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000504 RID: 1284
public class MassParalyzingNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C97 RID: 7319 RVA: 0x0008FE70 File Offset: 0x0008E070
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		if (targets.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, null);
		for (int i = 0; i < targets.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x0008FEC4 File Offset: 0x0008E0C4
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		ConditionConfig conditionConfig = new ConditionConfig(0.5f, this.CreateParalizeCondition());
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 5);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x0008FF30 File Offset: 0x0008E130
	private Condition CreateParalizeCondition()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		return new Condition(ConditionCategory.Paralyzing, ConditionType.Paralize, 2f, 1f, 0.1f, effect, 5, 0f, "");
	}
}
