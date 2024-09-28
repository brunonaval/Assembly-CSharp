using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class TripleThrustSkill : SkillBase
{
	// Token: 0x06001E18 RID: 7704 RVA: 0x0009673C File Offset: 0x0009493C
	public override void Cast(SkillBaseConfig skillBaseConfig)
	{
		GameObject target = base.GetTarget(skillBaseConfig, true);
		if (target == null)
		{
			return;
		}
		base.StartCoroutine(skillBaseConfig, this.StartThrusting(skillBaseConfig, skillBaseConfig.Skill.CastAmount, target));
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x00096777 File Offset: 0x00094977
	private IEnumerator StartThrusting(SkillBaseConfig skillBaseConfig, int totalThrusts, GameObject target)
	{
		WaitForSeconds waitForDelay = new WaitForSeconds(0.2f);
		int num;
		for (int i = 0; i < totalThrusts; i = num + 1)
		{
			bool flag = i == 0;
			AnimationConfig animationConfig = new AnimationConfig(0.025f);
			base.StartCastingAnimation(skillBaseConfig, flag, animationConfig, target);
			this.ApplyDamageOnTarget(skillBaseConfig, target, flag);
			yield return waitForDelay;
			num = i;
		}
		yield break;
	}

	// Token: 0x06001E1A RID: 7706 RVA: 0x0009679C File Offset: 0x0009499C
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

	// Token: 0x06001E1B RID: 7707 RVA: 0x00096820 File Offset: 0x00094A20
	private Condition CreateBleedCondition(int damage)
	{
		Effect effect = new Effect("BloodPuff", 0.09375f, 0f);
		return new Condition(ConditionCategory.Degeneration, ConditionType.Bleed, 3f, 0.5f, 0.4f, effect, 3, 0f, "");
	}
}
