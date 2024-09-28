using System;

// Token: 0x0200018A RID: 394
public class MeatElixirItem : ItemBase
{
	// Token: 0x0600046D RID: 1133 RVA: 0x0001935C File Offset: 0x0001755C
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

	// Token: 0x0600046E RID: 1134 RVA: 0x00019398 File Offset: 0x00017598
	private void StartBlessingOnUser(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Food, ConditionType.FoodPrecision, 1200f, 5f, 0.1f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x000193E1 File Offset: 0x000175E1
	private bool UserAlreadyHasBlessingActive(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.FoodPrecision))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
