using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020004FD RID: 1277
public class IcyTornadoNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C7B RID: 7291 RVA: 0x0008F7A4 File Offset: 0x0008D9A4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		IcyTornadoNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<IcyTornadoNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001C7C RID: 7292 RVA: 0x0008F7E4 File Offset: 0x0008D9E4
	private void ShowRandomIceWindBlasts(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "IceWindBlast",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001C7D RID: 7293 RVA: 0x0008F860 File Offset: 0x0008DA60
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 6);
		Effect effect = new Effect("IceSmoke", 0.25f, 0.2f);
		Condition condition = new Condition(ConditionCategory.Curse, ConditionType.Agility, 5f, 0.15f, 0.75f, effect, 6, 0f, "");
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(0.1f, condition)
		});
	}
}
