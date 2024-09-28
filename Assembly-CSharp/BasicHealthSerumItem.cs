using System;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class BasicHealthSerumItem : TvtItemBase
{
	// Token: 0x06000525 RID: 1317 RVA: 0x0001C338 File Offset: 0x0001A538
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

	// Token: 0x06000526 RID: 1318 RVA: 0x0001C384 File Offset: 0x0001A584
	private void HealUser(ItemBaseConfig itemBaseConfig)
	{
		AttributeModule component = itemBaseConfig.UserObject.GetComponent<AttributeModule>();
		int amount = Mathf.RoundToInt((float)component.MaxHealth * 0.1f);
		component.AddHealth(itemBaseConfig.UserObject, amount, true, this.CreateHealingEffect());
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x0001C3C4 File Offset: 0x0001A5C4
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 3f, 1f, 0f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x0001C40C File Offset: 0x0001A60C
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

	// Token: 0x06000529 RID: 1321 RVA: 0x0001C023 File Offset: 0x0001A223
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
