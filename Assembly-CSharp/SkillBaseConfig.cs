using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
public struct SkillBaseConfig
{
	// Token: 0x0600038A RID: 906 RVA: 0x00016256 File Offset: 0x00014456
	public SkillBaseConfig(Skill skill, Vector3 shootPivot, GameObject casterObject)
	{
		this.Skill = skill;
		this.ShootPivot = shootPivot;
		this.CasterObject = casterObject;
	}

	// Token: 0x040006CE RID: 1742
	public Skill Skill;

	// Token: 0x040006CF RID: 1743
	public Vector3 ShootPivot;

	// Token: 0x040006D0 RID: 1744
	public GameObject CasterObject;
}
