using System;
using System.Collections.Generic;

// Token: 0x0200016B RID: 363
public class FineExperienceBoosterItem : ItemBase
{
	// Token: 0x060003E8 RID: 1000 RVA: 0x00017808 File Offset: 0x00015A08
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

	// Token: 0x060003E9 RID: 1001 RVA: 0x00017848 File Offset: 0x00015A48
	private void StartBoostCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		PlayerModule playerModule;
		itemBaseConfig.UserObject.TryGetComponent<PlayerModule>(out playerModule);
		float power = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
		float duration = 3600f;
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.ExpBonus, duration, 5f, power, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x000178B4 File Offset: 0x00015AB4
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
