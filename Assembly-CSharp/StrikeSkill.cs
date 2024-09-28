using System;
using UnityEngine;

// Token: 0x02000537 RID: 1335
public class StrikeSkill : SkillBase
{
	// Token: 0x06001D75 RID: 7541 RVA: 0x00093CBC File Offset: 0x00091EBC
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		bool flag = Vector2.Distance(skillBaseConfig.CasterObject.transform.position, target.transform.position) > 0.64f;
		AnimationConfig animationConfig = new AnimationConfig(0.025f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		if (flag)
		{
			this.ShootIceForce(skillBaseConfig, target);
			return;
		}
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x00093D38 File Offset: 0x00091F38
	private void ShootIceForce(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item item = SkillBase.ItemDatabaseModule.GetItem(172);
		item.Attack = 0;
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, item);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x00093D7C File Offset: 0x00091F7C
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
		this.RandomlyStartToughnessBlessing(skillBaseConfig);
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x00093DD4 File Offset: 0x00091FD4
	private void RandomlyStartToughnessBlessing(SkillBaseConfig skillBaseConfig)
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
		{
			Effect effect = new Effect("IceBarrier", 0.5f, 0.25f);
			Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Toughness, 6f, 1f, 0.1f, effect, 0, 0f, "");
			ConditionModule conditionModule;
			skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
			conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		}
	}
}
