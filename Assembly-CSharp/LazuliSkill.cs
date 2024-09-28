using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class LazuliSkill : SkillBase
{
	// Token: 0x06001BCC RID: 7116 RVA: 0x0008CD2C File Offset: 0x0008AF2C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		List<GameObject> friendlyTargets = base.GetFriendlyTargets(skillBaseConfig, false);
		base.StartCoroutine(skillBaseConfig, this.HealAllTargets(skillBaseConfig, friendlyTargets));
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x0008CD67 File Offset: 0x0008AF67
	private IEnumerator HealAllTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		AttributeModule attributeModule;
		skillBaseConfig.CasterObject.TryGetComponent<AttributeModule>(out attributeModule);
		int healPower = Mathf.RoundToInt((float)attributeModule.MaxHealth * skillBaseConfig.Skill.SkillPower);
		WaitForSeconds waitForDelay = new WaitForSeconds(0.15f);
		int num;
		for (int i = 0; i < targets.Count; i = num + 1)
		{
			this.HealTarget(skillBaseConfig, healPower, targets[i]);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x0008CD84 File Offset: 0x0008AF84
	private void HealTarget(SkillBaseConfig skillBaseConfig, int healPower, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		AttributeModule attributeModule;
		target.TryGetComponent<AttributeModule>(out attributeModule);
		attributeModule.AddHealth(skillBaseConfig.CasterObject, healPower, true, this.CreateHealingEffect(skillBaseConfig));
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x0008CDBC File Offset: 0x0008AFBC
	private EffectConfig CreateHealingEffect(SkillBaseConfig skillBaseConfig)
	{
		return new EffectConfig
		{
			EffectName = skillBaseConfig.Skill.TargetEffect.Name,
			EffectScaleModifier = skillBaseConfig.Skill.TargetEffect.ScaleModifier,
			EffectSpeedModifier = skillBaseConfig.Skill.TargetEffect.SpeedModifier,
			SoundEffectName = skillBaseConfig.Skill.TargetSoundEffectName,
			TextColorId = 1
		};
	}
}
