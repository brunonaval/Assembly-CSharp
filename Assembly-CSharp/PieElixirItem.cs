using System;

// Token: 0x0200018C RID: 396
public class PieElixirItem : ItemBase
{
	// Token: 0x06000475 RID: 1141 RVA: 0x00019490 File Offset: 0x00017690
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

	// Token: 0x06000476 RID: 1142 RVA: 0x000194CC File Offset: 0x000176CC
	private void StartBlessingOnUser(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Food, ConditionType.FoodPower, 1200f, 5f, 0.1f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x00019515 File Offset: 0x00017715
	private bool UserAlreadyHasBlessingActive(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.FoodPower))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
