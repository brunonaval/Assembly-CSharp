using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class TripleThrustNonPlayerSkill : NonPlayerSkillBase
{
	// Token: 0x06001D02 RID: 7426 RVA: 0x00091E68 File Offset: 0x00090068
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		base.StartCoroutine(skillBaseConfig, this.StartThrusting(skillBaseConfig, target));
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x00091E98 File Offset: 0x00090098
	private IEnumerator StartThrusting(SkillBaseConfig skillBaseConfig, GameObject target)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.2f);
		int num;
		for (int i = 0; i < 3; i = num + 1)
		{
			bool applyBleed = false;
			AnimationConfig animationConfig = new AnimationConfig(0.025f);
			base.RunCastingAnimation(skillBaseConfig, animationConfig, target);
			this.ApplyDamageOnTarget(skillBaseConfig, target, applyBleed);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x00091EB8 File Offset: 0x000900B8
	private void ApplyDamageOnTarget(SkillBaseConfig skillBaseConfig, GameObject target, bool applyBleed)
	{
		if (target == null)
		{
			return;
		}
		bool critical;
		bool blocked;
		int damage = base.CalculateDamageOnTarget(skillBaseConfig, out critical, out blocked, target);
		EffectConfig damageEffectConfig = base.BuildDamageEffectConfig(skillBaseConfig, critical, blocked, damage, 5);
		CombatModule combatModule;
		target.TryGetComponent<CombatModule>(out combatModule);
		if (applyBleed)
		{
			Condition condition = this.CreateBleedCondition(damage);
			combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, new ConditionConfig[]
			{
				new ConditionConfig(1f, condition)
			});
			return;
		}
		combatModule.ApplyDamage(damage, damageEffectConfig, skillBaseConfig.CasterObject, false, Array.Empty<ConditionConfig>());
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x00091F3C File Offset: 0x0009013C
	private Condition CreateBleedCondition(int damage)
	{
		Effect effect = new Effect("BloodPuff", 0.09375f, 0f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Bleed, 3f, 0.5f, 0.4f, effect, 3, 0f, "");
	}
}
