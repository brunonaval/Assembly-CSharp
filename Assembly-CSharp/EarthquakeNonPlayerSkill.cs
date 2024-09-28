using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020004E8 RID: 1256
public class EarthquakeNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C1B RID: 7195 RVA: 0x0008E1E8 File Offset: 0x0008C3E8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		EarthquakeNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<EarthquakeNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001C1C RID: 7196 RVA: 0x0008E228 File Offset: 0x0008C428
	private void ShowRandomEarthBlasts(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "EarthBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001C1D RID: 7197 RVA: 0x0008E2A4 File Offset: 0x0008C4A4
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int num = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, num, 5);
		float power = (float)num * 0.1f;
		Effect effect = new Effect("ConfusionRing", 0.25f, 0.075f);
		Condition condition = new Condition(ConditionCategory.Confusion, ConditionType.Confusion, 3.5f, 1f, power, effect, 5, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(num, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(0.1f, condition)
		});
	}
}
