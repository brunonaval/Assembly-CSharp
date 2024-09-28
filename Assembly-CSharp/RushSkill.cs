using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class RushSkill : SkillBase
{
	// Token: 0x06001E09 RID: 7689 RVA: 0x00096228 File Offset: 0x00094428
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
		AnimationConfig animationConfig = new AnimationConfig(0.008f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.TeleportToTargetIfClose(skillBaseConfig, target);
		this.GetAndApplyDamageOnTargetsAsync(skillBaseConfig, target);
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x0009627C File Offset: 0x0009447C
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

	// Token: 0x06001E0B RID: 7691 RVA: 0x00096304 File Offset: 0x00094504
	private void GetAndApplyDamageOnTargetsAsync(SkillBaseConfig skillBaseConfig, GameObject selectedTarget)
	{
		List<GameObject> targetsFromPosition = base.GetTargetsFromPosition(skillBaseConfig, selectedTarget.transform.position, 1.3f, false);
		this.ShowEarthBlastEffects(skillBaseConfig);
		for (int i = 0; i < targetsFromPosition.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, targetsFromPosition[i]);
		}
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x00096350 File Offset: 0x00094550
	private void ShowEarthBlastEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "EarthBlast",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.5f,
			SoundEffectName = "earth_blast"
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x000963AC File Offset: 0x000945AC
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
		float duration = 5f;
		Effect effect = new Effect("DarkCurtain", 1f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Curse, ConditionType.Toughness, duration, 1f, 0.1f, effect, 0, 0f, "");
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			new ConditionConfig(1f, condition)
		});
	}
}
