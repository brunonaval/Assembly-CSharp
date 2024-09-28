using System;
using UnityEngine;

// Token: 0x020004DA RID: 1242
public class LotusSkill : SkillBase
{
	// Token: 0x06001BD7 RID: 7127 RVA: 0x0008CF2C File Offset: 0x0008B12C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x0008CF6C File Offset: 0x0008B16C
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
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
		this.RandomlyStartHealing(skillBaseConfig);
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x0008CFC4 File Offset: 0x0008B1C4
	private void RandomlyStartHealing(SkillBaseConfig skillBaseConfig)
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
		{
			AttributeModule attributeModule;
			skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
			float power = (float)attributeModule.MaxHealth * 0.05f;
			Effect effect = new Effect("RecoveryStrike", 0.5f, 0.25f);
			Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 5f, 1f, power, effect, 0, 0f, "heal_strike");
			ConditionModule conditionModule;
			skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
			conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		}
	}
}
