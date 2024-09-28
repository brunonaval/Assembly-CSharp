using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public class PoisonStormNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CBA RID: 7354 RVA: 0x00090838 File Offset: 0x0008EA38
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		PoisonStormNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<PoisonStormNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x00090878 File Offset: 0x0008EA78
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
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			this.CreatePoisonConditionConfig()
		});
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x000908D4 File Offset: 0x0008EAD4
	private ConditionConfig CreatePoisonConditionConfig()
	{
		Effect effect = new Effect("VenomPuff", 0.1875f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Poison, 8f, 1f, 0.1f, effect, 1, 0f, "venom_puff");
		return new ConditionConfig(1f, condition);
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x00090924 File Offset: 0x0008EB24
	private void ShowRandomVenomBalls(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "VenomBall",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.15f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(40, skillBaseConfig.Skill.Range, 0.05f, effectConfig);
	}
}
