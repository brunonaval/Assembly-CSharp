using System;

// Token: 0x020001BD RID: 445
public class AntidoteItem : ItemBase
{
	// Token: 0x0600050F RID: 1295 RVA: 0x0001BE39 File Offset: 0x0001A039
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		if (this.AlreadyHasAntidoteEffect(itemBaseConfig))
		{
			return false;
		}
		this.RemovePoisonCondition(itemBaseConfig);
		this.ShowEffects(itemBaseConfig);
		return true;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x0001BE58 File Offset: 0x0001A058
	private void RemovePoisonCondition(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		conditionModule.RemoveCondition(ConditionType.Poison);
		PvpModule pvpModule;
		itemBaseConfig.UserObject.TryGetComponent<PvpModule>(out pvpModule);
		if (pvpModule.PvpEnabled)
		{
			return;
		}
		conditionModule.StartCondition(new Condition(ConditionCategory.Boost, ConditionType.Antidote, 5f, 5f, 0f, default(Effect), 0, 0f, ""), itemBaseConfig.UserObject, true);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0001BECC File Offset: 0x0001A0CC
	private void ShowEffects(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "LightCast",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		component.ShowEffects(effectConfig);
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0001BF24 File Offset: 0x0001A124
	private bool AlreadyHasAntidoteEffect(ItemBaseConfig itemBaseConfig)
	{
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		if (conditionModule.HasActiveCondition(ConditionType.Antidote))
		{
			effectModule.ShowScreenMessage("item_cant_use_right_now_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
