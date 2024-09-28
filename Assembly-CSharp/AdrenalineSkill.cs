using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000539 RID: 1337
public class AdrenalineSkill : SkillBase
{
	// Token: 0x06001D7E RID: 7550 RVA: 0x00093F90 File Offset: 0x00092190
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		base.StartCoroutine(skillBaseConfig, this.Adrenaline(skillBaseConfig));
	}

	// Token: 0x06001D7F RID: 7551 RVA: 0x00093FC1 File Offset: 0x000921C1
	private IEnumerator Adrenaline(SkillBaseConfig skillBaseConfig)
	{
		WaitForSecondsRealtime delay = new WaitForSecondsRealtime(1f);
		AttributeModule casterAttributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out casterAttributeModule);
		EffectModule casterEffectModule;
		skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out casterEffectModule);
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			casterEffectModule.ShowEffects(this.CreateHealingEffect());
			casterAttributeModule.AddEndurance(casterAttributeModule.MaxEndurance);
			yield return delay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x00093FD8 File Offset: 0x000921D8
	private EffectConfig CreateHealingEffect()
	{
		return new EffectConfig
		{
			EffectName = "BlueStarBlast",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "heal_strike",
			TextColorId = 4
		};
	}
}
