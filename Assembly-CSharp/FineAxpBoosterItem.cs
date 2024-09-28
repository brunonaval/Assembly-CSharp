using System;
using System.Collections.Generic;

// Token: 0x02000169 RID: 361
public class FineAxpBoosterItem : ItemBase
{
	// Token: 0x060003E2 RID: 994 RVA: 0x000175DC File Offset: 0x000157DC
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

	// Token: 0x060003E3 RID: 995 RVA: 0x0001761C File Offset: 0x0001581C
	private void StartBoostCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		PlayerModule playerModule;
		itemBaseConfig.UserObject.TryGetComponent<PlayerModule>(out playerModule);
		float power = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
		float duration = 3600f;
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.AxpBonus, duration, 5f, power, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00017688 File Offset: 0x00015888
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
