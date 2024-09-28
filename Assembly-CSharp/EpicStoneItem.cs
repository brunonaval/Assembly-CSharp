using System;

// Token: 0x02000196 RID: 406
public class EpicStoneItem : ItemBase
{
	// Token: 0x06000497 RID: 1175 RVA: 0x00019D5C File Offset: 0x00017F5C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		InventoryModule inventoryModule;
		itemBaseConfig.UserObject.TryGetComponent<InventoryModule>(out inventoryModule);
		EffectModule effectModule;
		itemBaseConfig.UserObject.TryGetComponent<EffectModule>(out effectModule);
		int itemId = 222;
		int num = 30;
		if (inventoryModule.GetAmount(itemId) < num)
		{
			effectModule.ShowScreenMessage("not_enough_epic_stone_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		inventoryModule.ConsumeItem(itemId, num);
		Item item = ItemBase.ItemDatabaseModule.GetItem(223);
		item.RequiredLevel = 1;
		inventoryModule.AddItem(item, -1, 1, true);
		this.ShowMagicEffect(effectModule);
		return false;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00019DE4 File Offset: 0x00017FE4
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
