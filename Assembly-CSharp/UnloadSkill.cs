using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000546 RID: 1350
public class UnloadSkill : SkillBase
{
	// Token: 0x06001DBB RID: 7611 RVA: 0x00094D74 File Offset: 0x00092F74
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		ConditionModule conditionModule;
		skillBaseConfig.CasterObject.TryGetComponent<ConditionModule>(out conditionModule);
		bool flag = conditionModule.HasActiveCondition(ConditionCategory.Expertise);
		if (flag)
		{
			skillBaseConfig.Skill.MaxTargets = skillBaseConfig.Skill.MaxTargets * 2;
		}
		List<GameObject> targets = base.GetTargets(skillBaseConfig, true);
		if (targets.Count == 0)
		{
			return;
		}
		this.ShootOnTargets(skillBaseConfig, flag, targets);
	}

	// Token: 0x06001DBC RID: 7612 RVA: 0x00094DC8 File Offset: 0x00092FC8
	private void ShootOnTargets(SkillBaseConfig skillBaseConfig, bool hasExpertise, List<GameObject> targets)
	{
		EquipmentModule equipmentModule;
		skillBaseConfig.CasterObject.TryGetComponent<EquipmentModule>(out equipmentModule);
		Item item = equipmentModule.GetItem(SlotType.Ammo);
		PvpModule pvpModule;
		skillBaseConfig.CasterObject.TryGetComponent<PvpModule>(out pvpModule);
		int num = skillBaseConfig.Skill.CastAmount;
		if (pvpModule.TvtTeam != TvtTeam.None)
		{
			num /= 2;
		}
		else
		{
			num = (hasExpertise ? (num / 2) : num);
		}
		for (int i = 0; i < targets.Count; i++)
		{
			base.StartCoroutine(skillBaseConfig, this.StartShooting(skillBaseConfig, targets[i], item, num));
		}
	}

	// Token: 0x06001DBD RID: 7613 RVA: 0x00094E4C File Offset: 0x0009304C
	private IEnumerator StartShooting(SkillBaseConfig skillBaseConfig, GameObject target, Item equippedArrow, int shootCount)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.13f);
		int num;
		for (int i = 0; i < shootCount; i = num + 1)
		{
			if (target != null)
			{
				CreatureModule creatureModule;
				target.TryGetComponent<CreatureModule>(out creatureModule);
				if (!creatureModule.IsAlive)
				{
					target = base.GetTarget(skillBaseConfig, true);
					if (target == null)
					{
						break;
					}
				}
			}
			Item ammo = this.CreateArrowRandomly(equippedArrow);
			AnimationConfig animationConfig = new AnimationConfig(0.008250001f);
			base.StartCastingAnimation(skillBaseConfig, i == 0, animationConfig, target);
			this.Shoot(skillBaseConfig, target, ammo);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001DBE RID: 7614 RVA: 0x00094E78 File Offset: 0x00093078
	private Item CreateArrowRandomly(Item equippedArrow)
	{
		int num = UnityEngine.Random.Range(0, UnloadSkill.arrowIds.Length + 1);
		int itemId = (num >= UnloadSkill.arrowIds.Length) ? equippedArrow.Id : UnloadSkill.arrowIds[num];
		Item item = SkillBase.ItemDatabaseModule.GetItem(itemId);
		equippedArrow.Projectile = item.Projectile;
		equippedArrow.ProjectileExplosionSoundEffectName = item.ProjectileExplosionSoundEffectName;
		equippedArrow.ProjectileShootSoundEffectName = item.ProjectileShootSoundEffectName;
		return equippedArrow;
	}

	// Token: 0x06001DBF RID: 7615 RVA: 0x00094EE8 File Offset: 0x000930E8
	private void Shoot(SkillBaseConfig skillBaseConfig, GameObject target, Item ammo)
	{
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x0400179D RID: 6045
	private static readonly int[] arrowIds = new int[]
	{
		609,
		33,
		34,
		8
	};
}
