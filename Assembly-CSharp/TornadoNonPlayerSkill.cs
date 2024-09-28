using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200051E RID: 1310
public class TornadoNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CFC RID: 7420 RVA: 0x00091BA4 File Offset: 0x0008FDA4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		TornadoNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<TornadoNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x00091BE4 File Offset: 0x0008FDE4
	private void ShowRandomWindBlasts(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "WindBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x00091C60 File Offset: 0x0008FE60
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 1);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}
}
