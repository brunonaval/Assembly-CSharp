using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000544 RID: 1348
public class TripleShotSkill : SkillBase
{
	// Token: 0x06001DB0 RID: 7600 RVA: 0x00094BA0 File Offset: 0x00092DA0
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Expertise))
		{
			skillBaseConfig.Skill.MaxTargets = skillBaseConfig.Skill.MaxTargets * 2;
		}
		List<GameObject> targets = base.GetTargets(skillBaseConfig, true);
		if (targets.Count == 0)
		{
			return;
		}
		this.ShootOnTargets(skillBaseConfig, targets);
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x00094BF0 File Offset: 0x00092DF0
	private void ShootOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		for (int i = 0; i < targets.Count; i++)
		{
			base.StartCoroutine(skillBaseConfig, this.StartShooting(skillBaseConfig, targets[i]));
		}
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x00094C24 File Offset: 0x00092E24
	private IEnumerator StartShooting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item ammo = equipmentModule.GetItem(SlotType.Ammo);
		WaitForSeconds waitForDelay = new WaitForSeconds(0.13f);
		int num;
		for (int i = 0; i < skillBaseConfig.Skill.CastAmount; i = num + 1)
		{
			AnimationConfig animationConfig = new AnimationConfig(0.008250001f);
			base.StartCastingAnimation(skillBaseConfig, i == 0, animationConfig, target);
			this.Shoot(skillBaseConfig, target, ammo);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x00094C44 File Offset: 0x00092E44
	private void Shoot(SkillBaseConfig skillBaseConfig, GameObject target, Item ammo)
	{
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}
}
