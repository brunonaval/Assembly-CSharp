using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class NightmareNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CAA RID: 7338 RVA: 0x000903A4 File Offset: 0x0008E5A4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		NightmareNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<NightmareNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x000903E4 File Offset: 0x0008E5E4
	private void ShowRandomDarkWalls(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "DarkWall",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x00090460 File Offset: 0x0008E660
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int num = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, num, 7);
		float power = (float)num * 0.15f;
		Effect effect = new Effect("DarkStrike", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 8f, 1f, power, effect, 7, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(num, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(1f, condition)
		});
	}
}
