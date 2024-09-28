using System;
using UnityEngine;

// Token: 0x020004F7 RID: 1271
public class HunterShotNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C5F RID: 7263 RVA: 0x0008F390 File Offset: 0x0008D590
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootArrow(skillBaseConfig, target);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x0008F3D0 File Offset: 0x0008D5D0
	private void ShootArrow(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateArrowRandomly();
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x0008F3FC File Offset: 0x0008D5FC
	private Item CreateArrowRandomly()
	{
		int num = UnityEngine.Random.Range(0, HunterShotNonPlayerSkill.arrowIds.Length);
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(HunterShotNonPlayerSkill.arrowIds[num]);
	}

	// Token: 0x0400171C RID: 5916
	private static readonly int[] arrowIds = new int[]
	{
		8,
		33,
		34
	};
}
