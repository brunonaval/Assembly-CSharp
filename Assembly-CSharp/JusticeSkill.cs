using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public class JusticeSkill : SkillBase
{
	// Token: 0x06001D58 RID: 7512 RVA: 0x0009356C File Offset: 0x0009176C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		this.GetAndApplyDamageOnTargetsAsync(skillBaseConfig);
	}

	// Token: 0x06001D59 RID: 7513 RVA: 0x00093598 File Offset: 0x00091798
	private void GetAndApplyDamageOnTargetsAsync(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		this.ShowIceVortexEffects(skillBaseConfig);
		Vector3 position = skillBaseConfig.CasterObject.transform.position;
		for (int i = 0; i < targets.Count; i++)
		{
			this.ApplyDamageOnTarget(skillBaseConfig, position, targets[i]);
		}
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x000935E8 File Offset: 0x000917E8
	private void ShowIceVortexEffects(SkillBaseConfig skillBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "IceVortex",
			EffectScaleModifier = 3f,
			EffectSpeedModifier = 0.4f,
			SoundEffectName = "vortex"
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x00093644 File Offset: 0x00091844
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, Vector3 casterPosition, GameObject target)
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
		this.PullTarget(casterPosition, target, skillBaseConfig);
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x000936AC File Offset: 0x000918AC
	private ConditionConfig CreateProvokeConditionConfig()
	{
		Condition condition = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, 15f, 0.5f, 0f, default(Effect), 0, 0f, "");
		return new ConditionConfig(1f, condition);
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x000936F4 File Offset: 0x000918F4
	private void PullTarget(Vector3 casterPosition, GameObject target, SkillBaseConfig skillBaseConfig)
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
		Vector3 position = casterPosition - target.transform.position;
		movementModule.KnockbackToPosition(position, 2f);
	}
}
