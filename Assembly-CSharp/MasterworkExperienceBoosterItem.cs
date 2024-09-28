using System;
using System.Collections.Generic;

// Token: 0x0200016F RID: 367
public class MasterworkExperienceBoosterItem : ItemBase
{
	// Token: 0x060003F6 RID: 1014 RVA: 0x00017BD4 File Offset: 0x00015DD4
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		ConditionModule component2 = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		if (this.UserAlreadyHasBoostActive(component2, component))
		{
			return false;
		}
		this.StartBoostCondition(itemBaseConfig, component2);
		return true;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00017C10 File Offset: 0x00015E10
	private void StartBoostCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		PlayerModule playerModule;
		itemBaseConfig.UserObject.TryGetComponent<PlayerModule>(out playerModule);
		float power = (playerModule.PremiumDays > 0) ? 0.25f : 0.5f;
		float duration = 7200f;
		Condition condition = new Condition(ConditionCategory.Boost, ConditionType.ExpBonus, duration, 5f, power, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00017C7C File Offset: 0x00015E7C
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
