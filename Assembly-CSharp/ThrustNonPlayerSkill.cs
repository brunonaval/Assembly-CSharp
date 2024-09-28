using System;
using UnityEngine;

// Token: 0x0200051A RID: 1306
public class ThrustNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CEA RID: 7402 RVA: 0x000916B8 File Offset: 0x0008F8B8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		if (skillBaseConfig.CasterObject == null)
		{
			return;
		}
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x00091704 File Offset: 0x0008F904
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
