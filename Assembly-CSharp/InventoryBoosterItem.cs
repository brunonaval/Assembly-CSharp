using System;

// Token: 0x0200016C RID: 364
public class InventoryBoosterItem : ItemBase
{
	// Token: 0x060003EC RID: 1004 RVA: 0x00017924 File Offset: 0x00015B24
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		InventoryModule component2 = itemBaseConfig.UserObject.GetComponent<InventoryModule>();
		if (this.UserAlreadyHasTooManySlots(component2, component))
		{
			return false;
		}
		this.AddSlot(itemBaseConfig, component, component2);
		return true;
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00017437 File Offset: 0x00015637
	private void AddSlot(ItemBaseConfig itemBaseConfig, EffectModule effectModule, InventoryModule inventoryModule)
	{
		inventoryModule.AddSlots(1, true);
		effectModule.ShowAnimatedText("+1 Slot", 0, true, itemBaseConfig.UserObject.transform.position);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x00017960 File Offset: 0x00015B60
	private bool UserAlreadyHasTooManySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.TotalSlots >= 200)
		{
			effectModule.ShowScreenMessage("item_too_many_inventory_slots_message", 3, 3.5f, new string[]
			{
				inventoryModule.TotalSlots.ToString()
			});
			return true;
		}
		return false;
	}
}
