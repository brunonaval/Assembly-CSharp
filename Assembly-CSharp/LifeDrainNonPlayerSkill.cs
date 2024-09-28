using System;
using UnityEngine;

// Token: 0x02000503 RID: 1283
public class LifeDrainNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C92 RID: 7314 RVA: 0x0008FD48 File Offset: 0x0008DF48
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

	// Token: 0x06001C93 RID: 7315 RVA: 0x0008FD88 File Offset: 0x0008DF88
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 7);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
		this.HealCasterHealth(skillBaseConfig, damage);
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x0008FDDE File Offset: 0x0008DFDE
	private void HealCasterHealth(SkillBaseConfig skillBaseConfig, int damage)
	{
		this.ShowHealingEffects(skillBaseConfig, damage);
		skillBaseConfig.CasterObject.GetComponent<NonPlayerAttributeModule>().AddHealth(damage);
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x0008FDFC File Offset: 0x0008DFFC
	private void ShowHealingEffects(SkillBaseConfig skillBaseConfig, int damage)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "HealStrike",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "heal_strike",
			Text = damage.ToString(),
			TextColorId = 1
		};
		EffectModule effectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
		effectModule.ShowEffects(effectConfig);
	}
}
