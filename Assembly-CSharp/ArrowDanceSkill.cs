using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200053B RID: 1339
public class ArrowDanceSkill : SkillBase
{
	// Token: 0x06001D88 RID: 7560 RVA: 0x00094114 File Offset: 0x00092314
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, true);
		if (targets.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.05f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, null);
		for (int i = 0; i < targets.Count; i++)
		{
			base.StartCoroutine(skillBaseConfig, this.StartShooting(skillBaseConfig, targets[i]));
		}
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x0009416F File Offset: 0x0009236F
	private IEnumerator StartShooting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.26f);
		Vector3 targetPosition = target.transform.position;
		CreatureModule targetCreatureModule;
		target.TryGetComponent<CreatureModule>(out targetCreatureModule);
		int i = 0;
		while (i < skillBaseConfig.Skill.CastAmount && !(target == null) && targetCreatureModule.IsAlive)
		{
			this.Shoot(skillBaseConfig, targetPosition, targetCreatureModule, target);
			yield return waitForDelay;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x0009418C File Offset: 0x0009238C
	private void Shoot(SkillBaseConfig skillBaseConfig, Vector3 targetPosition, CreatureModule targetCreatureModule, GameObject target)
	{
		Vector3 fromPosition = GlobalUtils.RandomCircle(targetPosition, 1f);
		Item item = skillBaseConfig.CasterObject.GetComponent<EquipmentModule>().GetItem(SlotType.Ammo);
		ShootConfig shootConfig = new ShootConfig(fromPosition, target, skillBaseConfig.Skill, item);
		base.Shoot(skillBaseConfig, shootConfig);
	}
}
