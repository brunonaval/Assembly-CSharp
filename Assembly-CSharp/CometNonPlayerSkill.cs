using System;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class CometNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C07 RID: 7175 RVA: 0x0008DE04 File Offset: 0x0008C004
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001C08 RID: 7176 RVA: 0x0008DE44 File Offset: 0x0008C044
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 2);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}
}
