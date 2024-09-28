using System;
using UnityEngine;

// Token: 0x0200050B RID: 1291
public class PoisonousStingNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001CB5 RID: 7349 RVA: 0x00090778 File Offset: 0x0008E978
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ShootPoisonousSting(skillBaseConfig, target);
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x000907B8 File Offset: 0x0008E9B8
	private void ShootPoisonousSting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		Item ammo = this.CreatePoisonousSting();
		Condition condition = this.CreatePoisonCondition();
		ShootConfig shootConfig = new ShootConfig(Vector3.zero, target, skillBaseConfig.Skill, ammo, condition, 0.3f);
		base.Shoot(skillBaseConfig, shootConfig);
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x00090721 File Offset: 0x0008E921
	private Item CreatePoisonousSting()
	{
		return NonPlayerSkillBase.ItemDatabaseModule.GetItem(19);
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x000907F8 File Offset: 0x0008E9F8
	private Condition CreatePoisonCondition()
	{
		Effect effect = new Effect("VenomPuff", 0.1875f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Poison, 8f, 1f, 0.1f, effect, 1, 0f, "venom_puff");
	}
}
