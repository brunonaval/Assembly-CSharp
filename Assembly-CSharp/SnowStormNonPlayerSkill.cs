using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class SnowStormNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CDE RID: 7390 RVA: 0x00091248 File Offset: 0x0008F448
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		SnowStormNonPlayerSkill.<Cast>d__0 <Cast>d__;
		<Cast>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<Cast>d__.<>4__this = this;
		<Cast>d__.skillBaseConfig = skillBaseConfig;
		<Cast>d__.<>1__state = -1;
		<Cast>d__.<>t__builder.Start<SnowStormNonPlayerSkill.<Cast>d__0>(ref <Cast>d__);
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x00091288 File Offset: 0x0008F488
	private void ShowRandomIceWalls(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "IceWall",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			Position = skillBaseConfig.CasterObject.transform.position
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffectsRandomly(20, skillBaseConfig.Skill.Range, 0.15f, effectConfig);
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x00091304 File Offset: 0x0008F504
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
			this.CreateFreezeConditionConfig()
		});
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x00091360 File Offset: 0x0008F560
	private ConditionConfig CreateFreezeConditionConfig()
	{
		Effect effect = new Effect("IceWindStrike", 0.25f, 0.2f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Freeze, 2f, 0.15f, 0f, effect, 0, 0f, "");
		return new ConditionConfig(0.1f, condition);
	}
}
