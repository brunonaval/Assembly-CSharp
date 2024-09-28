using System;
using UnityEngine;

// Token: 0x020004E6 RID: 1254
public class DarkScratchNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C13 RID: 7187 RVA: 0x0008E078 File Offset: 0x0008C278
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

	// Token: 0x06001C14 RID: 7188 RVA: 0x0008E0B8 File Offset: 0x0008C2B8
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 7);
		ConditionConfig conditionConfig = new ConditionConfig(0.05f, this.CreateStigmaCondition());
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001C15 RID: 7189 RVA: 0x0008E124 File Offset: 0x0008C324
	private Condition CreateStigmaCondition()
	{
		Effect effect = new Effect("DarkStrike", 0.5f, 0.25f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 4f, 1f, 0.1f, effect, 7, 0f, "");
	}
}
