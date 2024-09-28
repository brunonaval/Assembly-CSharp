using System;
using UnityEngine;

// Token: 0x020001BF RID: 447
public class AwesomeHealthSerumItem : TvtItemBase
{
	// Token: 0x06000518 RID: 1304 RVA: 0x0001C050 File Offset: 0x0001A250
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

	// Token: 0x06000519 RID: 1305 RVA: 0x0001C09C File Offset: 0x0001A29C
	private void HealUser(ItemBaseConfig itemBaseConfig)
	{
		AttributeModule component = itemBaseConfig.UserObject.GetComponent<AttributeModule>();
		int amount = Mathf.RoundToInt((float)component.MaxHealth * 0.5f);
		component.AddHealth(itemBaseConfig.UserObject, amount, true, this.CreateHealingEffect());
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x0001C0DC File Offset: 0x0001A2DC
	private void StartHealingCondition(ItemBaseConfig itemBaseConfig, ConditionModule conditionModule)
	{
		Condition condition = new Condition(ConditionCategory.Regeneration, ConditionType.HealthPotion, 3f, 1f, 0f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001C124 File Offset: 0x0001A324
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

	// Token: 0x0600051C RID: 1308 RVA: 0x0001C023 File Offset: 0x0001A223
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
