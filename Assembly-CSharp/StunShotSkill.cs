using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000542 RID: 1346
public class StunShotSkill : SkillBase
{
	// Token: 0x06001DA9 RID: 7593 RVA: 0x00094948 File Offset: 0x00092B48
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
		AnimationConfig animationConfig = new AnimationConfig(0.004f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, targets[0]);
		this.ShootOnTargets(skillBaseConfig, targets);
	}

	// Token: 0x06001DAA RID: 7594 RVA: 0x000949BC File Offset: 0x00092BBC
	private void ShootOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		for (int i = 0; i < targets.Count; i++)
		{
			this.Shoot(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001DAB RID: 7595 RVA: 0x000949E8 File Offset: 0x00092BE8
	private void Shoot(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item item = equipmentModule.GetItem(SlotType.Ammo);
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 3f, 0.5f, 0f, effect, 5, 0f, "");
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, item, condition, 1f);
		base.Shoot(skillBaseConfig, shootConfig);
	}
}
