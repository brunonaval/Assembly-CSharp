using System;

// Token: 0x02000187 RID: 391
public class BananaItem : ItemBase
{
	// Token: 0x06000461 RID: 1121 RVA: 0x000191A0 File Offset: 0x000173A0
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

	// Token: 0x06000462 RID: 1122 RVA: 0x000191DC File Offset: 0x000173DC
	private void StartBlessingOnUser(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Food, ConditionType.FoodToughness, 600f, 5f, 0.05f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x00019179 File Offset: 0x00017379
	private bool UserAlreadyHasBlessingActive(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.FoodToughness))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
