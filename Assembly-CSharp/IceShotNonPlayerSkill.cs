using System;
using UnityEngine;

// Token: 0x020004FB RID: 1275
public class IceShotNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C73 RID: 7283 RVA: 0x0008F644 File Offset: 0x0008D844
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

	// Token: 0x06001C74 RID: 7284 RVA: 0x0008F684 File Offset: 0x0008D884
	private void ShootArrow(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateIceArrow();
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C75 RID: 7285 RVA: 0x0008F4DC File Offset: 0x0008D6DC
	private Item CreateIceArrow()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(8);
	}
}
