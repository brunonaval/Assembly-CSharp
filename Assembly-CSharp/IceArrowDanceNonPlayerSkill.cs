using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F8 RID: 1272
public class IceArrowDanceNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C64 RID: 7268 RVA: 0x0008F440 File Offset: 0x0008D640
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject selectedTarget = base.GetSelectedTarget(skillBaseConfig, true);
		if (selectedTarget == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, selectedTarget);
		base.StartCoroutine(skillBaseConfig, this.StartShooting(skillBaseConfig, selectedTarget));
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x0008F485 File Offset: 0x0008D685
	private IEnumerator StartShooting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.26f);
		Vector3 targetPosition = target.transform.position;
		int i = 0;
		while (i < skillBaseConfig.Skill.CastAmount && !(target == null))
		{
			this.ShootArrow(skillBaseConfig, target, targetPosition);
			yield return waitForDelay;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x0008F4A4 File Offset: 0x0008D6A4
	private void ShootArrow(SkillBaseConfig skillBaseConfig, GameObject target, Vector3 targetPosition)
	{
		Vector3 fromPosition = GlobalUtils.RandomCircle(targetPosition, 0.8f);
		Item ammo = this.CreateIceArrow();
		ShootConfig shootConfig = new ShootConfig(fromPosition, target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x0008F4DC File Offset: 0x0008D6DC
	private Item CreateIceArrow()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(8);
	}
}
