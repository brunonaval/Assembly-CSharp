using System;

// Token: 0x02000170 RID: 368
public class WarehouseBoosterItem : ItemBase
{
	// Token: 0x060003FA RID: 1018 RVA: 0x00017CEC File Offset: 0x00015EEC
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		WarehouseModule component2 = itemBaseConfig.UserObject.GetComponent<WarehouseModule>();
		if (this.UserAlreadyHasTooManySlots(component2, component))
		{
			return false;
		}
		this.AddSlot(itemBaseConfig, component, component2);
		return true;
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00017D27 File Offset: 0x00015F27
	private void AddSlot(ItemBaseConfig itemBaseConfig, EffectModule effectModule, WarehouseModule warehouseModule)
	{
		warehouseModule.AddSlots(1, true);
		effectModule.ShowAnimatedText("+1 Slot", 0, true, itemBaseConfig.UserObject.transform.position);
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x00017D4E File Offset: 0x00015F4E
	private bool UserAlreadyHasTooManySlots(WarehouseModule warehouseModule, EffectModule effectModule)
	{
		if (warehouseModule.TotalSlots >= 450)
		{
			effectModule.ShowScreenMessage("item_too_many_warehouse_slots_message", 3, 3.5f, Array.Empty<string>());
			return true;
		}
		return false;
	}
}
