using System;
using UnityEngine;

// Token: 0x020001C7 RID: 455
public class RefinedHealthSerumItem : TvtItemBase
{
	// Token: 0x0600053A RID: 1338 RVA: 0x0001C74C File Offset: 0x0001A94C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		ConditionModule component2 = itemBaseConfig.UserObject.GetComponent<ConditionModule>();
		if (!base.ValidateTvtItemUsage(itemBaseConfig))
		{
			return false;
		}
		if (this.UserIsAlreadyHealing(component2, component))
		{
			return false;
		}
		this.HealUser(itemBaseConfig);
		this.StartHealingCondition(itemBaseConfig, component2);
		return true;
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x0001C798 File Offset: 0x0001A998
	private void HealUser(ItemBaseConfig itemBaseConfig)
	{
		AttributeModule component = itemBaseConfig.UserObject.GetComponent<AttributeModule>();
		int amount = Mathf.RoundToInt((float)component.MaxHealth * 0.15f);
		component.AddHealth(itemBaseConfig.UserObject, amount, true, this.CreateHealingEffect());
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x0001C7D8 File Offset: 0x0001A9D8
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 3f, 1f, 0f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x0001C820 File Offset: 0x0001AA20
	private EffectConfig CreateHealingEffect()
	{
		return new EffectConfig
		{
			EffectName = "HealStrike",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.5f,
			SoundEffectName = "heal_strike",
			TextColorId = 1
		};
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x0001C023 File Offset: 0x0001A223
	private bool UserIsAlreadyHealing(ConditionModule conditionModule, EffectModule effectModule)
	{
		if (conditionModule.HasActiveCondition(ConditionType.HealthPotion))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
