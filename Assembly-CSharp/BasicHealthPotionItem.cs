using System;
using UnityEngine;

// Token: 0x020001C1 RID: 449
public class BasicHealthPotionItem : TvtItemBase
{
	// Token: 0x06000521 RID: 1313 RVA: 0x0001C290 File Offset: 0x0001A490
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		ConditionModule component2 = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		if (!base.ValidateTvtItemUsage(itemBaseConfig))
		{
			return false;
		}
		if (this.UserIsAlreadyHealing(component2, component))
		{
			return false;
		}
		this.StartHealingCondition(itemBaseConfig, component2);
		return true;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x0001C2D8 File Offset: 0x0001A4D8
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		float power = UnityEngine.Random.Range(15f, 25f);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 5f, 1f, power, new Effect("HealStrike", 0.5f, 0.25f), 1, 0f, "heal_strike");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x0001C023 File Offset: 0x0001A223
	private bool UserIsAlreadyHealing(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.HealthPotion))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
