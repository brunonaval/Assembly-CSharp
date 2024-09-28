using System;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class ThrustSkill : SkillBase
{
	// Token: 0x06001E13 RID: 7699 RVA: 0x000965AC File Offset: 0x000947AC
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		bool flag = Vector2.Distance(skillBaseConfig.CasterObject.transform.position, target.transform.position) > 0.8f;
		AnimationConfig animationConfig = new AnimationConfig(0.008f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.RandomlyStartPowerBlessing(skillBaseConfig);
		if (flag)
		{
			this.ShootSpear(skillBaseConfig, target);
			return;
		}
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x0009662C File Offset: 0x0009482C
	private void ShootSpear(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item item = SkillBase.ItemDatabaseModule.GetItem(227);
		item.Attack = 0;
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, item);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00096670 File Offset: 0x00094870
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 5);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x000966C0 File Offset: 0x000948C0
	private void RandomlyStartPowerBlessing(SkillBaseConfig skillBaseConfig)
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
		{
			Effect effect = new Effect("BloodBarrier", 0.5f, 0.25f);
			Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Power, 6f, 1f, 0.1f, effect, 0, 0f, "");
			ConditionModule conditionModule;
			skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
			conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		}
	}
}
