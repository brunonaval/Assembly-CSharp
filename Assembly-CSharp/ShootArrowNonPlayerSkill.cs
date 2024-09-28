using System;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class ShootArrowNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CCE RID: 7374 RVA: 0x00090ED4 File Offset: 0x0008F0D4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.004f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootSeraphArrow(skillBaseConfig, target);
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x00090F14 File Offset: 0x0008F114
	private void ShootSeraphArrow(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateSeraphArrow();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x00090F44 File Offset: 0x0008F144
	private Item CreateSeraphArrow()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(609);
	}
}
