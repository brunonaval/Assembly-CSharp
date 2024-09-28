using System;

// Token: 0x0200003E RID: 62
public abstract class CombatBase
{
	// Token: 0x060000A1 RID: 161 RVA: 0x000039C8 File Offset: 0x00001BC8
	protected EffectConfig BuildDamageEffectConfig(bool critical, bool blocked, string effectName, float effectScaleModifier, float effectSpeedModifier, string soundEffectName, int damage, int damageTextColorId)
	{
		EffectConfig result = new EffectConfig
		{
			EffectName = effectName,
			EffectScaleModifier = effectScaleModifier,
			EffectSpeedModifier = effectSpeedModifier,
			SoundEffectName = soundEffectName,
			Text = damage.ToString(),
			TextColorId = damageTextColorId
		};
		if (critical)
		{
			result.TextColorId = 3;
		}
		if (damage == 0)
		{
			result.EffectName = "Puff";
			result.EffectScaleModifier = 0.35f;
			result.EffectSpeedModifier = 0.75f;
			result.Text = string.Empty;
		}
		if (blocked)
		{
			result.EffectName = "OrangeSparks";
			result.EffectScaleModifier = 2f;
			result.EffectSpeedModifier = 1f;
			result.Text = "damage_blocked";
			result.TextColorId = 2;
			result.SoundEffectName = "hit_blocked";
		}
		return result;
	}
}
