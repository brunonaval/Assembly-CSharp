using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class AncientEnergyBrushManager : ActionBrushManager
{
	// Token: 0x06000043 RID: 67 RVA: 0x00002A4C File Offset: 0x00000C4C
	public override void ExecuteAction(GameObject player)
	{
		EffectModule component = player.GetComponent<EffectModule>();
		if (Time.time - this.lastActivation < this.rechargeTime)
		{
			component.ShowScreenMessage("object_recharging_message", 3, 3.5f, new string[]
			{
				Mathf.Round(this.rechargeTime - (Time.time - this.lastActivation)).ToString()
			});
			return;
		}
		this.lastActivation = Time.time;
		CombatModule combatModule;
		player.TryGetComponent<CombatModule>(out combatModule);
		List<GameObject> targetsFromPosition = combatModule.TargetFinder.GetTargetsFromPosition(this.maxTargers, this.range, true, false, false, false, base.transform.position, player, null);
		this.ShowWindVortexEffects(player);
		foreach (GameObject target in targetsFromPosition)
		{
			this.RemoveGoodConditionsAndStunTarget(player, target);
		}
		foreach (GameObject target2 in combatModule.TargetFinder.GetFriendlyTargetsFromPosition(10, this.range, false, false, false, false, base.transform.position, player, null))
		{
			this.RemoveBadConditionsAndHealTarget(player, target2);
		}
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002B98 File Offset: 0x00000D98
	private void ShowWindVortexEffects(GameObject player)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "WindVortex",
			EffectScaleModifier = 3f,
			EffectSpeedModifier = 0.4f,
			SoundEffectName = "vortex",
			Position = base.transform.position
		};
		player.GetComponent<EffectModule>().ShowEffects(effectConfig);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00002C00 File Offset: 0x00000E00
	private void RemoveBadConditionsAndHealTarget(GameObject player, GameObject target)
	{
		ConditionModule component = target.GetComponent<ConditionModule>();
		float power = (float)target.GetComponent<AttributeModule>().MaxHealth * 0.05f;
		Effect effect = new Effect("RecoveryStrike", 0.5f, 0.25f);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.Healing, 6f, 1f, power, effect, 0, 0f, "heal_strike");
		component.StartCondition(condition, player, true);
		foreach (Condition conditionToRemove in component.Conditions)
		{
			if (conditionToRemove.IsBadCondition)
			{
				component.RemoveCondition(conditionToRemove);
			}
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002CB8 File Offset: 0x00000EB8
	private void RemoveGoodConditionsAndStunTarget(GameObject player, GameObject target)
	{
		Effect effect = new Effect("YellowStars", 0.5f, 0.5f);
		Condition condition = new Condition(ConditionCategory.Paralyzing, ConditionType.Stun, 4f, 0.5f, 0f, effect, 5, 0f, "");
		ConditionModule component = target.GetComponent<ConditionModule>();
		component.StartCondition(condition, player, false);
		foreach (Condition conditionToRemove in component.Conditions)
		{
			if (conditionToRemove.IsGoodCondition)
			{
				component.RemoveCondition(conditionToRemove);
			}
		}
	}

	// Token: 0x04000023 RID: 35
	[SerializeField]
	private int maxTargers = 1;

	// Token: 0x04000024 RID: 36
	[SerializeField]
	private float range = 3f;

	// Token: 0x04000025 RID: 37
	[SerializeField]
	private float rechargeTime = 60f;

	// Token: 0x04000026 RID: 38
	private float lastActivation;
}
