using System;
using System.Collections.Generic;

// Token: 0x02000163 RID: 355
public class BasicAxpBoosterItem : ItemBase
{
	// Token: 0x060003CF RID: 975 RVA: 0x000170C0 File Offset: 0x000152C0
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (this.UserAlreadyHasBoostActive(conditionModule, effectModule))
		{
			return false;
		}
		this.StartBoostCondition(itemBaseConfig, conditionModule);
		return true;
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x00017100 File Offset: 0x00015300
	private void StartBoostCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		PlayerModule playerModule;
		itemBaseConfig.UserObject.TryGetComponent<PlayerModule>(out playerModule);
		float power = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
		float duration = 1800f;
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.AxpBonus, duration, 5f, power, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001716C File Offset: 0x0001536C
	private bool UserAlreadyHasBoostActive(ConditionModule conditionModule, EffectModule effectModule)
	{
		using (IEnumerator<Condition> enumerator = conditionModule.GetConditions(ConditionType.AxpBonus).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Duration > 30f)
				{
					effectModule.ShowScreenMessage("item_active_axp_boost_message", 3, 3.5f, Array.Empty<string>());
					return true;
				}
			}
		}
		return false;
	}
}
