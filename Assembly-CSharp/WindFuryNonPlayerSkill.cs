using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public class WindFuryNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D1F RID: 7455 RVA: 0x00092634 File Offset: 0x00090834
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

	// Token: 0x06001D20 RID: 7456 RVA: 0x00092679 File Offset: 0x00090879
	private IEnumerator StartShooting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		WaitForSeconds delay = new WaitForSeconds(0.26f);
		Vector3 targetPosition = target.transform.position;
		int i = 0;
		while (i < skillBaseConfig.Skill.CastAmount && !(target == null))
		{
			this.ShootWindFury(skillBaseConfig, target, targetPosition);
			yield return delay;
			int num = i;
			i = num + 1;
		}
		yield break;
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x00092698 File Offset: 0x00090898
	private void ShootWindFury(SkillBaseConfig skillBaseConfig, GameObject target, Vector3 targetPosition)
	{
		Vector3 fromPosition = GlobalUtils.RandomCircle(targetPosition, 0.8f);
		Item item = NonPlayerSkillBase.ItemDatabaseModule.GetItem(31);
		ShootConfig shootConfig = new ShootConfig(fromPosition, target, skillBaseConfig.Skill, item);
		base.Shoot(skillBaseConfig, shootConfig);
	}
}
