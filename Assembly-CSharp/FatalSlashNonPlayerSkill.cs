using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004EE RID: 1262
public class FatalSlashNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C31 RID: 7217 RVA: 0x0008E994 File Offset: 0x0008CB94
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.StartCasterConditions(skillBaseConfig);
		this.TeleportToTarget(skillBaseConfig, target);
		base.StartCoroutine(skillBaseConfig, this.ApplyMultipleDamagesOnTarget(skillBaseConfig, target));
	}

	// Token: 0x06001C32 RID: 7218 RVA: 0x0008E9E8 File Offset: 0x0008CBE8
	private void StartCasterConditions(SkillBaseConfig skillBaseConfig)
	{
		float num = 0.72f;
		Condition condition = new Condition(ConditionCategory.Invisibility, ConditionType.Vanish, num, num, 0f, default(Effect), 0, 0f, "");
		Condition condition2 = new Condition(ConditionCategory.Invincibility, ConditionType.Invincible, num, num, 0f, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		conditionModule.StartCondition(condition2, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001C33 RID: 7219 RVA: 0x0008EA71 File Offset: 0x0008CC71
	private IEnumerator ApplyMultipleDamagesOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		float seconds = 0.12f;
		WaitForSeconds waitForInterval = new WaitForSeconds(seconds);
		int i = 0;
		while (i < skillBaseConfig.Skill.CastAmount && !(target == null))
		{
			CreatureModule creatureModule;
			target.TryGetComponent<CreatureModule>(out creatureModule);
			if (!creatureModule.IsAlive)
			{
				break;
			}
			bool critical;
			bool blocked;
			int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
			EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 7);
			if (i != 0)
			{
				damageEffectConfig.EffectName = null;
			}
			CombatModule combatModule;
			target.TryGetComponent<CombatModule>(out combatModule);
			combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
			yield return waitForInterval;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x0008EA90 File Offset: 0x0008CC90
	private void TeleportToTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		MovementModule movementModule;
		skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
		movementModule.Teleport(target.transform.position, default(Effect));
	}
}
