using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000522 RID: 1314
public class ValkyrieWhipNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D0D RID: 7437 RVA: 0x0009204C File Offset: 0x0009024C
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

	// Token: 0x06001D0E RID: 7438 RVA: 0x000920CC File Offset: 0x000902CC
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

	// Token: 0x06001D0F RID: 7439 RVA: 0x00092134 File Offset: 0x00090334
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

	// Token: 0x06001D10 RID: 7440 RVA: 0x00092174 File Offset: 0x00090374
	private ConditionConfig CreateProvokeConditionConfig()
	{
		Condition condition = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, 15f, 0.5f, 0f, default(Effect), 0, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
