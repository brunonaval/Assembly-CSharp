using System;

// Token: 0x02000185 RID: 389
public class AppleItem : ItemBase
{
	// Token: 0x06000459 RID: 1113 RVA: 0x0001906C File Offset: 0x0001726C
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

	// Token: 0x0600045A RID: 1114 RVA: 0x000190A8 File Offset: 0x000172A8
	private void StartBlessingOnUser(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Food, ConditionType.FoodAgility, 600f, 5f, 0.05f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x00019045 File Offset: 0x00017245
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
