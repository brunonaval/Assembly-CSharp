using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004F3 RID: 1267
public class GodsPunishmentNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C48 RID: 7240 RVA: 0x0008EEA4 File Offset: 0x0008D0A4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, false);
		if (targets.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, targets[0]);
		Vector3 position = skillBaseConfig.CasterObject.transform.position;
		for (int i = 0; i < targets.Count; i++)
		{
			if (TargetFinder.HasValidWalkablePath(skillBaseConfig.CasterObject, targets[i]))
			{
				this.ApplyDamageOnTarget(skillBaseConfig, position, targets[i]);
			}
		}
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x0008EF24 File Offset: 0x0008D124
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
			this.CreateProvokeConditionConfig(),
			this.CreateStunConditionConfig()
		});
		this.KnockbackTarget(casterPosition, target, skillBaseConfig);
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x0008EF98 File Offset: 0x0008D198
	private void KnockbackTarget(Vector3 casterPosition, GameObject target, SkillBaseConfig skillBaseConfig)
	{
		if (target == null)
		{
			return;
		}
		MovementModule movementModule;
		target.TryGetComponent<MovementModule>(out movementModule);
		Direction direction = GlobalUtils.FindTargetDirection(skillBaseConfig.CasterObject.transform.position, target.transform.position);
		movementModule.Knockback(direction, 5f);
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x0008EFE8 File Offset: 0x0008D1E8
	private ConditionConfig CreateStunConditionConfig()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
		return new ConditionConfig(1f, condition);
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x0008F03C File Offset: 0x0008D23C
	private ConditionConfig CreateProvokeConditionConfig()
	{
		Condition condition = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, 15f, 0.5f, 0f, default(Effect), 0, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
