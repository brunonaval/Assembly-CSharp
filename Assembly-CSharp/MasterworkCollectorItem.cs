using System;

// Token: 0x0200016E RID: 366
public class MasterworkCollectorItem : ItemBase
{
	// Token: 0x060003F4 RID: 1012 RVA: 0x00017AC4 File Offset: 0x00015CC4
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
		Condition condition = new Condition(ConditionCategory.Telekinesis, ConditionType.Collect, 86400f, 1f, 15f, default(Effect), 0, 0f, "");
		conditionModule.StartCondition(condition, itemBaseConfig.UserObject, true);
		return false;
	}
}
