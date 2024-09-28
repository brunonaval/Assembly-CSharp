using System;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
public class IcicleSkill : SkillBase
{
	// Token: 0x06001B67 RID: 7015 RVA: 0x0008B5A4 File Offset: 0x000897A4
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.016f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootIcicle(skillBaseConfig, target);
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x0008B5E4 File Offset: 0x000897E4
	private void ShootIcicle(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateIcicle();
		ShootConfig shootConfig = new ShootConfig(target, skillBaseConfig.Skill, ammo);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x0008B60F File Offset: 0x0008980F
	private Item CreateIcicle()
	{
		return SkillBase.ItemDatabaseModule.GetItem(1277);
	}
}
