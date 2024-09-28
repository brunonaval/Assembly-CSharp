using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E4 RID: 1252
public class ConfusingScreamNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C0A RID: 7178 RVA: 0x0008DE94 File Offset: 0x0008C094
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		if (targets.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, targets[0]);
		for (int i = 0; i < targets.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001C0B RID: 7179 RVA: 0x0008DEF0 File Offset: 0x0008C0F0
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
		ConditionConfig conditionConfig = new ConditionConfig(0.05f, this.CreateConfusionCondition());
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001C0C RID: 7180 RVA: 0x0008DF5C File Offset: 0x0008C15C
	private Condition CreateConfusionCondition()
	{
		Effect effect = new Effect("ConfusionRing", 0.25f, 0.075f);
		return new Condition(ConditionCategory.Confusion, ConditionType.Confusion, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
	}
}
