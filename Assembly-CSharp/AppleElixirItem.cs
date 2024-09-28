using System;

// Token: 0x02000184 RID: 388
public class AppleElixirItem : ItemBase
{
	// Token: 0x06000455 RID: 1109 RVA: 0x00018FC0 File Offset: 0x000171C0
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		ConditionModule component2 = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		if (this.UserAlreadyHasBlessingActive(component2, component))
		{
			return false;
		}
		this.StartBlessingOnUser(itemBaseConfig, component2);
		return true;
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00018FFC File Offset: 0x000171FC
	private void StartBlessingOnUser(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Food, ConditionType.FoodAgility, 1200f, 5f, 0.1f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00019045 File Offset: 0x00017245
	private bool UserAlreadyHasBlessingActive(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.FoodAgility))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
