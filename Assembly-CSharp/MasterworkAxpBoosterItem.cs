using System;
using System.Collections.Generic;

// Token: 0x0200016D RID: 365
public class MasterworkAxpBoosterItem : ItemBase
{
	// Token: 0x060003F0 RID: 1008 RVA: 0x000179A8 File Offset: 0x00015BA8
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

	// Token: 0x060003F1 RID: 1009 RVA: 0x000179E8 File Offset: 0x00015BE8
	private void StartBoostCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		PlayerModule playerModule;
		itemBaseConfig.UserObject.TryGetComponent<PlayerModule>(out playerModule);
		float power = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
		float duration = 7200f;
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.AxpBonus, duration, 5f, power, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00017A54 File Offset: 0x00015C54
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
