using System;
using UnityEngine;

// Token: 0x020001C5 RID: 453
public class MasterHealthPotionItem : TvtItemBase
{
	// Token: 0x06000532 RID: 1330 RVA: 0x0001C5E4 File Offset: 0x0001A7E4
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

	// Token: 0x06000533 RID: 1331 RVA: 0x0001C62C File Offset: 0x0001A82C
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		itemBaseConfig.UserObject.GetComponent<AttributeModule>();
		float power = UnityEngine.Random.Range(80f, 100f);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 5f, 1f, power, new Effect("HealStrike", 0.5f, 0.25f), 1, 0f, "heal_strike");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x0001C023 File Offset: 0x0001A223
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
