using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public class TornadoSkill : SkillBase
{
	// Token: 0x06001B8D RID: 7053 RVA: 0x0008BDA8 File Offset: 0x00089FA8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		int castAmount = skillBaseConfig.Skill.CastAmount;
		float num = 1f;
		float duration = (float)(castAmount - 1) * num;
		ConditionModule casterConditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out casterConditionModule);
		this.StartAgilityCondition(skillBaseConfig, casterConditionModule, duration);
		this.StartTransformationCondition(skillBaseConfig, casterConditionModule, duration);
		base.StartCoroutine(skillBaseConfig, this.StartAttackingTargets(skillBaseConfig, castAmount, num));
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x0008BE18 File Offset: 0x0008A018
	private void StartAgilityCondition(SkillBaseConfig skillBaseConfig, ConditionModule casterConditionModule, float duration)
	{
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Agility, duration, 5f, 2f, default(Effect), 0, 0f, "");
		casterConditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x0008BE5C File Offset: 0x0008A05C
	private void StartTransformationCondition(SkillBaseConfig skillBaseConfig, ConditionModule casterConditionModule, float duration)
	{
		Effect effect = new Effect("SmokeBlast", 1f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Transformation, ConditionType.Mutate, duration, 0.75f, 0f, effect, 0, 0f, "");
		casterConditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x0008BEAF File Offset: 0x0008A0AF
	private IEnumerator StartAttackingTargets(SkillBaseConfig skillBaseConfig, int totalCasts, float damageInterval)
	{
		CreatureModule casterCreatureModule;
		skillBaseConfig.CasterObject.TryGetComponent<CreatureModule>(out casterCreatureModule);
		WaitForSeconds waitForDamageInterval = new WaitForSeconds(damageInterval);
		int num;
		for (int i = 0; i < totalCasts; i = num + 1)
		{
			if (!casterCreatureModule.IsAlive)
			{
				yield break;
			}
			this.ShowThunderEffects(skillBaseConfig);
			this.GetTargetsAndApplyDamage(skillBaseConfig);
			yield return waitForDamageInterval;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x0008BED4 File Offset: 0x0008A0D4
	private void GetTargetsAndApplyDamage(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		for (int i = 0; i < targets.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targets[i]);
			this.ShootLightningBoltOnTarget(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x0008BF18 File Offset: 0x0008A118
	private void ShowThunderEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Thunder",
			EffectScaleModifier = 1f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "thunder"
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x0008BF74 File Offset: 0x0008A174
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		if (!GlobalUtils.IsClose(skillBaseConfig.CasterObject.transform.position, target.transform.position, 0.8f))
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 10);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
		this.KnockbackTarget(skillBaseConfig, target);
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x0008C000 File Offset: 0x0008A200
	private void KnockbackTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		if (combatModule.IsPvpProtected(skillBaseConfig.CasterObject))
		{
			return;
		}
		MovementModule movementModule;
		target.TryGetComponent<MovementModule>(out movementModule);
		Direction direction = GlobalUtils.FindTargetDirection(skillBaseConfig.CasterObject.transform.position, target.transform.position);
		movementModule.Knockback(direction, 2f);
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x0008C068 File Offset: 0x0008A268
	private void ShootLightningBoltOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		if (GlobalUtils.IsClose(skillBaseConfig.CasterObject.transform.position, target.transform.position, 0.8f))
		{
			return;
		}
		Item ammo = this.CreateLightningBolt();
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x0008C0CF File Offset: 0x0008A2CF
	private Item CreateLightningBolt()
	{
		return SkillBase.ItemDatabaseModule.GetItem(1308);
	}
}
