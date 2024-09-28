using System;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class CrushOfDoomSkill : SkillBase
{
	// Token: 0x06001DEF RID: 7663 RVA: 0x00095C94 File Offset: 0x00093E94
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.008f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001DF0 RID: 7664 RVA: 0x00095CD4 File Offset: 0x00093ED4
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
		Condition condition = this.CreateBleedCondition(damage);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(1f, condition)
		});
	}

	// Token: 0x06001DF1 RID: 7665 RVA: 0x00095D40 File Offset: 0x00093F40
	private Condition CreateBleedCondition(int damage)
	{
		Effect effect = new Effect("BloodPuff", 0.09375f, 0f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Bleed, 6f, 0.5f, 0.4f, effect, 3, 0f, "");
	}
}
