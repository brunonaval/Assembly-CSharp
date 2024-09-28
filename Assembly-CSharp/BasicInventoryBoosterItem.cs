using System;

// Token: 0x02000166 RID: 358
public class BasicInventoryBoosterItem : ItemBase
{
	// Token: 0x060003D9 RID: 985 RVA: 0x000173FC File Offset: 0x000155FC
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

	// Token: 0x060003DA RID: 986 RVA: 0x00017437 File Offset: 0x00015637
	private void AddSlot(ItemBaseConfig itemBaseConfig, EffectModule effectModule, InventoryModule inventoryModule)
	{
		inventoryModule.AddSlots(1, true);
		effectModule.ShowAnimatedText("+1 Slot", 0, true, itemBaseConfig.UserObject.transform.position);
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00017460 File Offset: 0x00015660
	private bool UserAlreadyHasTooManySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.TotalSlots >= 25)
		{
			effectModule.ShowScreenMessage("item_too_many_basic_inventory_slots_message", 3, 3.5f, new string[]
			{
				inventoryModule.TotalSlots.ToString()
			});
			return true;
		}
		return false;
	}
}
