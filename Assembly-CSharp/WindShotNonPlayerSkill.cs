using System;
using UnityEngine;

// Token: 0x02000529 RID: 1321
public class WindShotNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D29 RID: 7465 RVA: 0x000927B8 File Offset: 0x000909B8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootWindball(skillBaseConfig, target);
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x000927F8 File Offset: 0x000909F8
	private void ShootWindball(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateWindball();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x00092828 File Offset: 0x00090A28
	private Item CreateWindball()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(30);
	}
}
