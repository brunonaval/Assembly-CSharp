using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004DC RID: 1244
public class LyriumSkill : SkillBase
{
	// Token: 0x06001BE0 RID: 7136 RVA: 0x0008D194 File Offset: 0x0008B394
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.ShowHealBlastEffects(skillBaseConfig);
		this.HealTargets(skillBaseConfig, friendlyTargets);
		skillBaseConfig.CasterObject.GetComponent<AttributeModule>().SetEnduranceToMax();
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x0008D1E0 File Offset: 0x0008B3E0
	private void HealTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int healPower = Mathf.RoundToInt((float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower);
		for (int i = 0; i < targets.Count; i++)
		{
			this.HealTarget(skillBaseConfig, healPower, targets[i]);
		}
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x0008D234 File Offset: 0x0008B434
	private void ShowHealBlastEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "HealBlast",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.3f,
			SoundEffectName = "heal_blast"
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x0008D290 File Offset: 0x0008B490
	private void HealTarget(SkillBaseConfig skillBaseConfig, int healPower, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		AttributeModule attributeModule;
		target.TryGetComponent<AttributeModule>(out attributeModule);
		attributeModule.AddHealth(skillBaseConfig.CasterObject, healPower, true, this.CreateHealingEffect(skillBaseConfig));
		Condition randomBlessing = this.GetRandomBlessing();
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(randomBlessing, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x0008D2E8 File Offset: 0x0008B4E8
	private static List<Condition> GenerateRandomBlessings()
	{
		return new List<Condition>
		{
			new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 4f, 1f, 0.17f, new Effect("RecoveryStrike", 0.5f, 0.25f), 6, 0f, "heal_strike"),
			new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 4f, 1f, 0.3f, new Effect("IceCurtain", 1f, 0.3f), 0, 0f, ""),
			new Condition(ConditionCategory.Blessing, ConditionType.Agility, 4f, 1f, 0.5f, new Effect("IceCurtain", 1f, 0.3f), 0, 0f, ""),
			new Condition(ConditionCategory.Blessing, ConditionType.Power, 4f, 1f, 0.3f, new Effect("IceCurtain", 1f, 0.3f), 0, 0f, ""),
			new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, 2f, 0.5f, 0.1f, new Effect("EarthStrike", 0.5f, 0.25f), 0, 0f, "")
		};
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x0008D424 File Offset: 0x0008B624
	private Condition GetRandomBlessing()
	{
		int index = UnityEngine.Random.Range(0, LyriumSkill.randomBlessings.Count);
		return LyriumSkill.randomBlessings[index];
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x0008D450 File Offset: 0x0008B650
	private EffectConfig CreateHealingEffect(SkillBaseConfig skillBaseConfig)
	{
		return new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.TargetEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.TargetEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.TargetEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.TargetSoundEffectName,
			TextColorId = 1
		};
	}

	// Token: 0x040016F9 RID: 5881
	private static readonly List<Condition> randomBlessings = LyriumSkill.GenerateRandomBlessings();
}
