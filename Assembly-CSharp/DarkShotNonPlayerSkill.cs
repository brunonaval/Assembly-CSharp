using System;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class DarkShotNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C17 RID: 7191 RVA: 0x0008E16C File Offset: 0x0008C36C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootBlackSkull(skillBaseConfig, target);
	}

	// Token: 0x06001C18 RID: 7192 RVA: 0x0008E1AC File Offset: 0x0008C3AC
	private void ShootBlackSkull(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateBlackSkull();
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C19 RID: 7193 RVA: 0x0008E1D7 File Offset: 0x0008C3D7
	private Item CreateBlackSkull()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(26);
	}
}
