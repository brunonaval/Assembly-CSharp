using System;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class VenomScratchNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D15 RID: 7445 RVA: 0x0009224C File Offset: 0x0009044C
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

	// Token: 0x06001D16 RID: 7446 RVA: 0x0009228C File Offset: 0x0009048C
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		ConditionConfig conditionConfig = new ConditionConfig(0.05f, this.CreatePoisonCondition());
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 1);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000922F8 File Offset: 0x000904F8
	private Condition CreatePoisonCondition()
	{
		Effect effect = new Effect("VenomPuff", 0.1875f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Poison, 8f, 1f, 0.1f, effect, 1, 0f, "venom_puff");
	}
}
