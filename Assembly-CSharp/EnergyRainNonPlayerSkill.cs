using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020004EC RID: 1260
public class EnergyRainNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C2B RID: 7211 RVA: 0x0008E68C File Offset: 0x0008C88C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		EnergyRainNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<EnergyRainNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x0008E6CC File Offset: 0x0008C8CC
	private void ShowRandomLightStrikes(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Thunder",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "eletric_strike",
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(40, skillBaseConfig.Skill.Range, 0.075f, effectConfig);
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x0008E754 File Offset: 0x0008C954
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
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Paralize, 2f, 1f, 0.1f, effect, 5, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(0.1f, condition)
		});
	}
}
