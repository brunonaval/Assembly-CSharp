using System;
using UnityEngine;

// Token: 0x020001BE RID: 446
public class AwesomeHealthPotionItem : TvtItemBase
{
	// Token: 0x06000514 RID: 1300 RVA: 0x0001BF70 File Offset: 0x0001A170
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

	// Token: 0x06000515 RID: 1301 RVA: 0x0001BFB8 File Offset: 0x0001A1B8
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		itemBaseConfig.UserObject.GetComponent<AttributeModule>();
		float power = UnityEngine.Random.Range(120f, 150f);
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 5f, 1f, power, new Effect("HealStrike", 0.5f, 0.25f), 1, 0f, "heal_strike");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x0001C023 File Offset: 0x0001A223
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
