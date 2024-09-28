using System;

// Token: 0x020001C8 RID: 456
public class RemedyItem : TvtItemBase
{
	// Token: 0x06000540 RID: 1344 RVA: 0x0001C86E File Offset: 0x0001AA6E
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		if (this.AlreadyHasRemedyEffect(itemBaseConfig))
		{
			return false;
		}
		if (!base.ValidateTvtItemUsage(itemBaseConfig))
		{
			return false;
		}
		this.RemoveConditions(itemBaseConfig);
		this.ShowEffects(itemBaseConfig);
		return true;
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x0001C898 File Offset: 0x0001AA98
	private void ShowEffects(ItemBaseConfig itemBaseConfig)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "LightCast",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		itemBaseConfig.UserObject.GetComponent<EffectModule>().ShowEffects(effectConfig);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x0001C8F0 File Offset: 0x0001AAF0
	private void RemoveConditions(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.RemoveCondition(ConditionType.Stun);
		conditionModule.RemoveCondition(ConditionType.Paralize);
		conditionModule.RemoveCondition(ConditionType.Confusion);
		conditionModule.RemoveCondition(ConditionType.Freeze);
		conditionModule.StartCondition(new Condition(ConditionCategory.Boost, ConditionType.Remedy, 5f, 5f, 0f, default(Effect), 0, 0f, ""), itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x0001C964 File Offset: 0x0001AB64
	private bool AlreadyHasRemedyEffect(ItemBaseConfig itemBaseConfig)
	{
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionType.Remedy))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
