using System;
using UnityEngine;

// Token: 0x02000514 RID: 1300
public class SlashNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CD8 RID: 7384 RVA: 0x00091128 File Offset: 0x0008F328
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

	// Token: 0x06001CD9 RID: 7385 RVA: 0x00091168 File Offset: 0x0008F368
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
