using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class ShootArrowSkill : SkillBase
{
	// Token: 0x06001DA1 RID: 7585 RVA: 0x00094738 File Offset: 0x00092938
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item item = equipmentModule.GetItem(SlotType.Ammo);
		if (!item.IsDefined)
		{
			EffectModule effectModule;
			skillBaseConfig.CasterObject.TryGetComponent<EffectModule>(out effectModule);
			effectModule.ShowScreenMessage("no_arrow_equipped_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
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
		this.ShootOnTargets(skillBaseConfig, targets, item);
		this.RandomlyStartPrecisionBlessing(skillBaseConfig);
	}

	// Token: 0x06001DA2 RID: 7586 RVA: 0x000947F4 File Offset: 0x000929F4
	private void ShootOnTargets(SkillBaseConfig skillBaseConfig, List<GameObject> targets, Item ammo)
	{
		for (int i = 0; i < targets.Count; i++)
		{
			this.Shoot(skillBaseConfig, targets[i], ammo);
		}
	}

	// Token: 0x06001DA3 RID: 7587 RVA: 0x00094824 File Offset: 0x00092A24
	private void Shoot(SkillBaseConfig skillBaseConfig, GameObject target, Item ammo)
	{
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x00094848 File Offset: 0x00092A48
	private void RandomlyStartPrecisionBlessing(SkillBaseConfig skillBaseConfig)
	{
		if (UnityEngine.Random.Range(0f, 1f) <= 0.05f)
		{
			Effect effect = new Effect("AirStrike", 0.5f, 0.25f);
			Condition condition = new Condition(ConditionCategory.Blessing, ConditionType.Precision, 6f, 1f, 0.1f, effect, 0, 0f, "");
			ConditionModule conditionModule;
			skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
			conditionModule.StartCondition(condition, skillBaseConfig.CasterObject, true);
		}
	}
}
