using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000532 RID: 1330
public class PunishmentSkill : SkillBase
{
	// Token: 0x06001D62 RID: 7522 RVA: 0x00093854 File Offset: 0x00091A54
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		List<GameObject> targets = base.GetTargets(skillBaseConfig, true);
		if (targets.Count == 0)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.025f);
		base.StartCastingAnimation(skillBaseConfig, animationConfig, targets[0]);
		for (int i = 0; i < targets.Count; i++)
		{
			this.ShootIceball(skillBaseConfig, targets[i]);
		}
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x000938B0 File Offset: 0x00091AB0
	private void ShootIceball(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreateIceball();
		Condition condition = new Condition(ConditionCategory.Taunt, ConditionType.Provoke, 8f, 0.5f, 0f, default(Effect), 0, 0f, "");
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 1f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x00093914 File Offset: 0x00091B14
	private Item CreateIceball()
	{
		return SkillBase.ItemDatabaseModule.GetItem(1323);
	}
}
