using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class InfernoNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C84 RID: 7300 RVA: 0x0008FB34 File Offset: 0x0008DD34
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

	// Token: 0x06001C85 RID: 7301 RVA: 0x0008FB79 File Offset: 0x0008DD79
	private IEnumerator StartShooting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.26f);
		Vector3 targetPosition = target.transform.position;
		int i = 0;
		while (i < skillBaseConfig.Skill.CastAmount && !(target == null))
		{
			this.ShootFireball(skillBaseConfig, target, targetPosition);
			yield return waitForDelay;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x0008FB98 File Offset: 0x0008DD98
	private void ShootFireball(SkillBaseConfig skillBaseConfig, GameObject target, Vector3 targetPosition)
	{
		Vector3 fromPosition = GlobalUtils.RandomCircle(targetPosition, 0.8f);
		Item ammo = this.CreateFireball();
		ShootConfig shootConfig = new ShootConfig(fromPosition, target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x0008EC81 File Offset: 0x0008CE81
	private Item CreateFireball()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(35);
	}
}
