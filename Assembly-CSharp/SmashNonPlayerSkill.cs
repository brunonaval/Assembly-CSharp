using System;
using UnityEngine;

// Token: 0x02000515 RID: 1301
public class SmashNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CDB RID: 7387 RVA: 0x000911B8 File Offset: 0x0008F3B8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x000911F8 File Offset: 0x0008F3F8
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
	}
}
