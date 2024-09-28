using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
public class HunterArrowDanceNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C53 RID: 7251 RVA: 0x0008F1D0 File Offset: 0x0008D3D0
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

	// Token: 0x06001C54 RID: 7252 RVA: 0x0008F215 File Offset: 0x0008D415
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

	// Token: 0x06001C55 RID: 7253 RVA: 0x0008F234 File Offset: 0x0008D434
	private void ShootArrow(SkillBaseConfig skillBaseConfig, GameObject target, Vector3 targetPosition)
	{
		Vector3 fromPosition = GlobalUtils.RandomCircle(targetPosition, 0.8f);
		Item ammo = this.CreateArrowRandomly();
		ShootConfig shootConfig = new ShootConfig(fromPosition, target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x0008F26C File Offset: 0x0008D46C
	private Item CreateArrowRandomly()
	{
		int num = UnityEngine.Random.Range(0, HunterArrowDanceNonPlayerSkill.arrowIds.Length);
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(HunterArrowDanceNonPlayerSkill.arrowIds[num]);
	}

	// Token: 0x04001713 RID: 5907
	private static readonly int[] arrowIds = new int[]
	{
		8,
		33,
		34
	};
}
