using System;
using UnityEngine;

// Token: 0x020004FA RID: 1274
public class IceBlasterNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C6F RID: 7279 RVA: 0x0008F5C8 File Offset: 0x0008D7C8
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootIceForce(skillBaseConfig, target);
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x0008F608 File Offset: 0x0008D808
	private void ShootIceForce(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateIceForce();
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001C71 RID: 7281 RVA: 0x0008F633 File Offset: 0x0008D833
	private Item CreateIceForce()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(172);
	}
}
