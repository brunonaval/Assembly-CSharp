using System;
using UnityEngine;

// Token: 0x020004FC RID: 1276
public class IcyScratchNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001C77 RID: 7287 RVA: 0x0008F6B0 File Offset: 0x0008D8B0
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		AnimationConfig animationConfig = new AnimationConfig(0.1f);
		base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
		this.ApplyDamageOnTarget(skillBaseConfig, target);
	}

	// Token: 0x06001C78 RID: 7288 RVA: 0x0008F6F0 File Offset: 0x0008D8F0
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		ConditionConfig conditionConfig = new ConditionConfig(0.05f, this.CreateSlowCondition());
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 6);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
		{
			conditionConfig
		});
	}

	// Token: 0x06001C79 RID: 7289 RVA: 0x0008F75C File Offset: 0x0008D95C
	private Condition CreateSlowCondition()
	{
		Effect effect = new Effect("IceSmoke", 0.25f, 0.2f);
		return new Condition(ConditionCategory.Curse, ConditionType.Agility, 4.5f, 0.15f, 0.4f, effect, 6, 0f, "");
	}
}
