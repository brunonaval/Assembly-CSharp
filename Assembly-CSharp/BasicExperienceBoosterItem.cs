using System;
using System.Collections.Generic;

// Token: 0x02000165 RID: 357
public class BasicExperienceBoosterItem : ItemBase
{
	// Token: 0x060003D5 RID: 981 RVA: 0x000172E0 File Offset: 0x000154E0
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

	// Token: 0x060003D6 RID: 982 RVA: 0x00017320 File Offset: 0x00015520
	private void StartBoostCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		PlayerModule playerModule;
		itemBaseConfig.UserObject.TryGetComponent<PlayerModule>(out playerModule);
		float power = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
		float duration = 1800f;
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.ExpBonus, duration, 5f, power, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001738C File Offset: 0x0001558C
	private bool UserAlreadyHasBoostActive(ConditionModule conditionModule, EffectModule effectModule)
	{
		using (IEnumerator<Condition> enumerator = conditionModule.GetConditions(ConditionType.ExpBonus).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Duration > 30f)
				{
					effectModule.ShowScreenMessage("item_active_exp_boost_message", 3, 3.5f, Array.Empty<string>());
					return true;
				}
			}
		}
		return false;
	}
}
