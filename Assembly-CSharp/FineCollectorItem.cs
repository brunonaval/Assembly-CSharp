using System;

// Token: 0x0200016A RID: 362
public class FineCollectorItem : ItemBase
{
	// Token: 0x060003E6 RID: 998 RVA: 0x000176F8 File Offset: 0x000158F8
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		ConditionModule conditionModule;
		itemBaseConfig.UserObject.TryGetComponent<ConditionModule>(out conditionModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		if (conditionModule.HasActiveCondition(ConditionCategory.Telekinesis))
		{
			conditionModule.RemoveCondition(ConditionType.Collect);
			effectModule.ShowEffects(new EffectConfig
			{
				EffectName = "RedStarBlast",
				EffectScaleModifier = 1.5f,
				EffectSpeedModifier = 0.2f,
				SoundEffectName = "telekinesis_off"
			});
			return false;
		}
		InventoryModule inventoryModule;
		itemBaseConfig.UserObject.TryGetComponent<InventoryModule>(out inventoryModule);
		inventoryModule.TargetShowCollectorConfigWindow();
		effectModule.ShowEffects(new EffectConfig
		{
			EffectName = "StarBlast",
			EffectScaleModifier = 1.5f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "telekinesis_on"
		});
		Condition condition = new Condition(ConditionCategory.Telekinesis, ConditionType.Collect, 86400f, 1.6f, 7f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
		return false;
	}
}
