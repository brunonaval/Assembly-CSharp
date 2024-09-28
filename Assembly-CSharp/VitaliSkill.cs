using System;
using UnityEngine;

// Token: 0x020004E0 RID: 1248
public class VitaliSkill : SkillBase
{
	// Token: 0x06001BFD RID: 7165 RVA: 0x0008DA3C File Offset: 0x0008BC3C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x0008DA7C File Offset: 0x0008BC7C
	private void HealCasterHealth(SkillBaseConfig skillBaseConfig, int damage)
	{
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		attributeModule.AddHealth(skillBaseConfig.CasterObject, damage, true, this.BuildHealingEffects(damage));
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x0008DAAC File Offset: 0x0008BCAC
	private EffectConfig BuildHealingEffects(int damage)
	{
		return new EffectConfig
		{
			EffectName = "HealStrike",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "heal_strike",
			Text = damage.ToString(),
			TextColorId = 1
		};
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x0008DB08 File Offset: 0x0008BD08
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
		this.HealCasterHealth(skillBaseConfig, damage);
	}
}
