using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053F RID: 1343
public class PerfectShotSkill : SkillBase
{
	// Token: 0x06001D9C RID: 7580 RVA: 0x000945E4 File Offset: 0x000927E4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Expertise))
		{
			skillBaseConfig.Skill.MaxTargets = skillBaseConfig.Skill.MaxTargets * 2;
		}
		List<GameObject> targetsAtSameDirection = base.GetTargetsAtSameDirection(skillBaseConfig, true);
		if (targetsAtSameDirection.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.004f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, targetsAtSameDirection[0]);
		this.StartConditions(skillBaseConfig);
		this.ShootOnTargets(skillBaseConfig, targetsAtSameDirection);
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x00094660 File Offset: 0x00092860
	private void ShootOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		for (int i = 0; i < targets.Count; i++)
		{
			this.Shoot(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x0009468C File Offset: 0x0009288C
	private void StartConditions(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Precision, 2f, 2f, 50f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x000946E4 File Offset: 0x000928E4
	private void Shoot(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item item = equipmentModule.GetItem(SlotType.Ammo);
		Projectile projectile = item.Projectile;
		projectile.Knockback = true;
		item.Projectile = projectile;
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, item);
		base.Shoot(skillBaseConfig, shootConfig);
	}
}
