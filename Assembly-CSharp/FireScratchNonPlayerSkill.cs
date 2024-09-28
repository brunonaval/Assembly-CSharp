using System;
using UnityEngine;

// Token: 0x020004F1 RID: 1265
public class FireScratchNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C41 RID: 7233 RVA: 0x0008ECD8 File Offset: 0x0008CED8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x0008ED18 File Offset: 0x0008CF18
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
		ConditionConfig conditionConfig = new ConditionConfig(0.05f, this.CreateBurnCondition());
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x0008ED84 File Offset: 0x0008CF84
	private Condition CreateBurnCondition()
	{
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 8f, 1f, 0.1f, effect, 2, 0f, "fireball");
	}
}
