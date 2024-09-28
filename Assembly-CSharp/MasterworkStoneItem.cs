using System;

// Token: 0x020001A2 RID: 418
public class MasterworkStoneItem : ItemBase
{
	// Token: 0x060004C5 RID: 1221 RVA: 0x0001A960 File Offset: 0x00018B60
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		InventoryModule inventoryModule;
		itemBaseConfig.UserObject.TryGetComponent<InventoryModule>(out inventoryModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		int itemId = 13;
		int num = 10;
		if (inventoryModule.GetAmount(itemId) < num)
		{
			effectModule.ShowScreenMessage("not_enough_masterwork_stone_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		inventoryModule.ConsumeItem(itemId, num);
		Item item = ItemBase.ItemDatabaseModule.GetItem(221);
		item.RequiredLevel = 1;
		inventoryModule.AddItem(item, -1, 1, true);
		this.ShowMagicEffect(effectModule);
		return false;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x0001A9E8 File Offset: 0x00018BE8
	private void ShowMagicEffect(EffectModule effectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		effectModule.ShowEffects(effectConfig);
	}
}
