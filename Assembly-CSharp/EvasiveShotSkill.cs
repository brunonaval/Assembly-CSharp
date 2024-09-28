using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053D RID: 1341
public class EvasiveShotSkill : SkillBase
{
	// Token: 0x06001D92 RID: 7570 RVA: 0x000942D8 File Offset: 0x000924D8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Expertise))
		{
			skillBaseConfig.Skill.MaxTargets = skillBaseConfig.Skill.MaxTargets * 2;
		}
		List<GameObject> targets = base.GetTargets(skillBaseConfig, true);
		AnimationConfig animationConfig = new AnimationConfig(0.004f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, (targets.Count == 0) ? null : targets[0]);
		this.BecomeInvisible(skillBaseConfig);
		this.Evade(skillBaseConfig, (targets.Count == 0) ? null : targets[0]);
		this.ShootOnTargets(skillBaseConfig, targets);
	}

	// Token: 0x06001D93 RID: 7571 RVA: 0x00094368 File Offset: 0x00092568
	private void ShootOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets)
	{
		if (targets.Count == 0)
		{
			this.Shoot(skillBaseConfig, null);
			return;
		}
		for (int i = 0; i < targets.Count; i++)
		{
			this.Shoot(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001D94 RID: 7572 RVA: 0x000943A8 File Offset: 0x000925A8
	private void Evade(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		MovementModule movementModule;
		skillBaseConfig.CasterObject.TryGetComponent<MovementModule>(out movementModule);
		if (target != null)
		{
			Direction direction = GlobalUtils.FindTargetDirection(skillBaseConfig.CasterObject.transform.position, target.transform.position);
			movementModule.Knockback(GlobalUtils.InverseDirection(direction), 3f);
			return;
		}
		movementModule.Knockback(GlobalUtils.InverseDirection(movementModule.Direction), 3f);
	}

	// Token: 0x06001D95 RID: 7573 RVA: 0x00094418 File Offset: 0x00092618
	private void BecomeInvisible(SkillBaseConfig skillBaseConfig)
	{
		Condition condition = new Condition(ConditionCategory.Invisibility, ConditionType.Invisible, 1.5f, 1.5f, 0f, default(Effect), 0, 0f, "");
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x00094470 File Offset: 0x00092670
	private void Shoot(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item item = equipmentModule.GetItem(SlotType.Ammo);
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, item);
		base.Shoot(skillBaseConfig, shootConfig);
	}
}
