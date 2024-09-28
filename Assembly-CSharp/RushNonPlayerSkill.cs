using System;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class RushNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CC7 RID: 7367 RVA: 0x00090D14 File Offset: 0x0008EF14
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
		Vector3 position = target.transform.position - skillBaseConfig.CasterObject.transform.position;
		skillBaseConfig.CasterObject.GetComponent<MovementModule>().DashToPosition(position, 5f, false);
		this.ApplyDamageOnTarget(skillBaseConfig, skillBaseConfig.CasterObject.transform.position, target);
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x00090D94 File Offset: 0x0008EF94
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
			this.CreateStunConditionConfig()
		});
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x00090DF0 File Offset: 0x0008EFF0
	private ConditionConfig CreateStunConditionConfig()
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 2f, 0.5f, 0.1f, effect, 5, 0f, "");
		return new ConditionConfig(1f, condition);
	}
}
