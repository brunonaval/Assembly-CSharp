using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052E RID: 1326
public class InterventionSkill : SkillBase
{
	// Token: 0x06001D4C RID: 7500 RVA: 0x000930E8 File Offset: 0x000912E8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		if (!TargetFinder.HasValidWalkablePath(skillBaseConfig.CasterObject, target))
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.TeleportToTargetIfClose(skillBaseConfig, target);
		this.GetAndApplyDamageOnTargetsAsync(skillBaseConfig, target);
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x0009313C File Offset: 0x0009133C
	private void GetAndApplyDamageOnTargetsAsync(SkillBaseConfig skillBaseConfig, GameObject selectedTarget)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, selectedTarget.transform.position, 1.2f, false);
		this.ShowIcePulseEffects(skillBaseConfig);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i]);
		}
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x00093188 File Offset: 0x00091388
	private void ShowIcePulseEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "IcePulse",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.5f,
			SoundEffectName = "teleporter_hit"
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000931E4 File Offset: 0x000913E4
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
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			this.CreateProvokeConditionConfig()
		});
		this.StartBadConditionsOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x00093248 File Offset: 0x00091448
	private ConditionConfig CreateProvokeConditionConfig()
	{
		Condition condition = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, 5f, 0.5f, 0f, default(Effect), 0, 0f, "");
		return new ConditionConfig(1f, condition);
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x00093290 File Offset: 0x00091490
	private void StartBadConditionsOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Effect effect = new Effect("DarkCurtain", 1f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Curse, ConditionType.Toughness, 3f, 1f, 0.2f, effect, 0, 0f, "");
		ConditionModule conditionModule;
		target.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x000932F0 File Offset: 0x000914F0
	private void TeleportToTargetIfClose(SkillBaseConfig skillBaseConfig, GameObject selectedTarget)
	{
		if (!GlobalUtils.IsClose(skillBaseConfig.CasterObject.transform.position, selectedTarget.transform.position, 0.48f))
		{
			MovementModule movementModule;
			skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
			movementModule.Teleport(selectedTarget.transform.position, default(Effect));
			movementModule.TargetTeleport(movementModule.connectionToClient, selectedTarget.transform.position, default(Effect));
		}
	}
}
