using System;
using UnityEngine;

// Token: 0x020004DB RID: 1243
public class LuminaireSkill : SkillBase
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x0008D058 File Offset: 0x0008B258
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject friendlyTargetAndAvoidCaster = base.GetFriendlyTargetAndAvoidCaster(skillBaseConfig, true);
		AnimationConfig animationConfig = new AnimationConfig(0.025f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, friendlyTargetAndAvoidCaster);
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int health = Mathf.CeilToInt((float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower);
		if (friendlyTargetAndAvoidCaster == null)
		{
			this.StartHealing(skillBaseConfig, attributeModule, true);
			return;
		}
		this.ShootLifeSphere(skillBaseConfig, attributeModule, health, friendlyTargetAndAvoidCaster);
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x0008D0C8 File Offset: 0x0008B2C8
	private void ShootLifeSphere(SkillBaseConfig skillBaseConfig, AttributeModule casterAttributeModule, int health, GameObject target)
	{
		Item ammo = this.CreateLifeSphere(skillBaseConfig);
		ammo.Attack = (short)health;
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
		this.StartHealing(skillBaseConfig, casterAttributeModule, false);
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x0008D107 File Offset: 0x0008B307
	private Item CreateLifeSphere(SkillBaseConfig skillBaseConfig)
	{
		return SkillBase.ItemDatabaseModule.GetItem(961);
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x0008D118 File Offset: 0x0008B318
	private void StartHealing(SkillBaseConfig skillBaseConfig, AttributeModule casterAttributeModule, bool doubleDuration)
	{
		float power = (float)casterAttributeModule.MaxHealth * 0.05f;
		Effect effect = new Effect("RecoveryStrike", 0.5f, 0.25f);
		float duration = doubleDuration ? 6f : 3f;
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, duration, 1f, power, effect, 0, 0f, "heal_strike");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}
}
