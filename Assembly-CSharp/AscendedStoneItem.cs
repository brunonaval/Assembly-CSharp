using System;

// Token: 0x0200018F RID: 399
public class AscendedStoneItem : ItemBase
{
	// Token: 0x06000483 RID: 1155 RVA: 0x000196A0 File Offset: 0x000178A0
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		InventoryModule inventoryModule;
		itemBaseConfig.UserObject.TryGetComponent<InventoryModule>(out inventoryModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		int itemId = 221;
		int num = 5;
		if (inventoryModule.GetAmount(itemId) < num)
		{
			effectModule.ShowScreenMessage("not_enough_ascended_stone_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		inventoryModule.ConsumeItem(itemId, num);
		Item item = ItemBase.ItemDatabaseModule.GetItem(222);
		item.RequiredLevel = 1;
		inventoryModule.AddItem(item, -1, 1, true);
		this.ShowMagicEffect(effectModule);
		return false;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x00019728 File Offset: 0x00017928
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
