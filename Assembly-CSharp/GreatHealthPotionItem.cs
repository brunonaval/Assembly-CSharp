using System;
using UnityEngine;

// Token: 0x020001C4 RID: 452
public class GreatHealthPotionItem : TvtItemBase
{
	// Token: 0x0600052E RID: 1326 RVA: 0x0001C530 File Offset: 0x0001A730
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

	// Token: 0x0600052F RID: 1327 RVA: 0x0001C578 File Offset: 0x0001A778
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		itemBaseConfig.UserObject.GetComponent<AttributeModule>();
		float power = UnityEngine.Random.Range(60f, 70f);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 5f, 1f, power, new Effect("HealStrike", 0.5f, 0.25f), 1, 0f, "heal_strike");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x0001C023 File Offset: 0x0001A223
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
