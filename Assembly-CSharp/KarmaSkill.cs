using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class KarmaSkill : SkillBase
{
	// Token: 0x06001BBA RID: 7098 RVA: 0x0008C810 File Offset: 0x0008AA10
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.025f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.GetAndApplyDamageOnTargetsAsync(skillBaseConfig, target);
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x0008C850 File Offset: 0x0008AA50
	private void GetAndApplyDamageOnTargetsAsync(SkillBaseConfig skillBaseConfig, GameObject selectedTarget)
	{
		Vector3 position = selectedTarget.transform.position;
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, position, 1.5f, false);
		this.ShowBloodBlastEffects(skillBaseConfig, position);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i]);
		}
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x0008C8A0 File Offset: 0x0008AAA0
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 3);
		Condition randomCurse = this.GetRandomCurse();
		ConditionConfig conditionConfig = new ConditionConfig(1f, randomCurse);
		Condition condition = KarmaSkill.CreateStigmaCondition();
		ConditionConfig conditionConfig2 = new ConditionConfig(1f, condition);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig,
			conditionConfig2
		});
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x0008C92C File Offset: 0x0008AB2C
	private Condition GetRandomCurse()
	{
		int index = UnityEngine.Random.Range(0, KarmaSkill.randomCurses.Count);
		return KarmaSkill.randomCurses[index];
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x0008C958 File Offset: 0x0008AB58
	private static Condition CreateStigmaCondition()
	{
		return new Condition(ConditionCategory.Degeneration, ConditionType.Stigma, 4f, 1f, 0.17f, new Effect("DarkStrike", 0.5f, 0.25f), 7, 0f, "");
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x0008C99C File Offset: 0x0008AB9C
	private static List<Condition> GenerateRandomCurses()
	{
		return new List<Condition>
		{
			new Condition(ConditionCategory.Curse, ConditionType.Toughness, 4f, 1f, 0.3f, new Effect("BloodCurtain", 1f, 0.3f), 0, 0f, ""),
			new Condition(ConditionCategory.Curse, ConditionType.Power, 4f, 1f, 0.3f, new Effect("BloodCurtain", 1f, 0.3f), 0, 0f, ""),
			new Condition(ConditionCategory.Curse, ConditionType.Agility, 4f, 1f, 0.5f, new Effect("BloodCurtain", 1f, 0.3f), 0, 0f, ""),
			new Condition(ConditionCategory.Confusion, ConditionType.Confusion, 2f, 0.5f, 0.1f, new Effect("RedStars", 0.5f, 0.5f), 3, 0f, "")
		};
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x0008CA9C File Offset: 0x0008AC9C
	private void ShowBloodBlastEffects(SkillBaseConfig skillBaseConfig, Vector3 selectedTargetPosition)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "BloodBlast",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.3f,
			SoundEffectName = "curse",
			Position = selectedTargetPosition
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x040016E8 RID: 5864
	private static readonly List<Condition> randomCurses = KarmaSkill.GenerateRandomCurses();
}
