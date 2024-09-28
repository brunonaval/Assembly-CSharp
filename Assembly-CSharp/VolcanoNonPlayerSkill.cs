using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class VolcanoNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D19 RID: 7449 RVA: 0x00092338 File Offset: 0x00090538
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		VolcanoNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<VolcanoNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x00092378 File Offset: 0x00090578
	private void ShowRandomFireBlasts(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "FireBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x000923F4 File Offset: 0x000905F4
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int num = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, num, 2);
		float power = (float)num * 0.15f;
		Effect effect = new Effect("FireStrike", 0.2f, 0.3f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Burn, 8f, 1f, power, effect, 2, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(num, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(1f, condition)
		});
	}
}
