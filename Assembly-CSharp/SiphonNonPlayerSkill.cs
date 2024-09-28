using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000513 RID: 1299
public class SiphonNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CD2 RID: 7378 RVA: 0x00090F58 File Offset: 0x0008F158
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

	// Token: 0x06001CD3 RID: 7379 RVA: 0x00090FD8 File Offset: 0x0008F1D8
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
		this.PullTarget(casterPosition, target, skillBaseConfig);
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x0009104C File Offset: 0x0008F24C
	private void PullTarget(Vector3 casterPosition, GameObject target, SkillBaseConfig skillBaseConfig)
	{
		if (target == null)
		{
			return;
		}
		MovementModule movementModule;
		target.TryGetComponent<MovementModule>(out movementModule);
		Vector3 position = casterPosition - target.transform.position;
		movementModule.KnockbackToPosition(position, 5f);
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x0009108C File Offset: 0x0008F28C
	private ConditionConfig CreateStunConditionConfig()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
		return new ConditionConfig(1f, condition);
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x000910E0 File Offset: 0x0008F2E0
	private ConditionConfig CreateProvokeConditionConfig()
	{
		Condition condition = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, 15f, 0.5f, 0f, default(Effect), 0, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
