using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018E RID: 398
public abstract class ItemBase
{
	// Token: 0x0600047D RID: 1149 RVA: 0x000195C1 File Offset: 0x000177C1
	static ItemBase()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		gameObject.TryGetComponent<ItemDatabaseModule>(out ItemBase.ItemDatabaseModule);
		gameObject.TryGetComponent<QuestDatabaseModule>(out ItemBase.QuestDatabaseModule);
		gameObject.TryGetComponent<BlueprintDatabaseModule>(out ItemBase.BlueprintDatabaseModule);
		gameObject.TryGetComponent<NpcDatabaseModule>(out ItemBase.NpcDatabaseModule);
	}

	// Token: 0x0600047E RID: 1150
	public abstract bool UseItem(ItemBaseConfig itemBaseConfig);

	// Token: 0x0600047F RID: 1151 RVA: 0x000195FC File Offset: 0x000177FC
	public void UseAndConsumeItem(ItemBaseConfig itemBaseConfig)
	{
		if (!this.UseItem(itemBaseConfig))
		{
			return;
		}
		if (itemBaseConfig.Item.Type == ItemType.Gold)
		{
			this.ConsumeItem(itemBaseConfig, itemBaseConfig.Item.Amount);
			return;
		}
		this.ConsumeItem(itemBaseConfig, 1);
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x00019634 File Offset: 0x00017834
	private void ConsumeItem(ItemBaseConfig itemBaseConfig, int amount)
	{
		InventoryModule inventoryModule;
		itemBaseConfig.UserObject.TryGetComponent<InventoryModule>(out inventoryModule);
		if (amount >= itemBaseConfig.Item.Amount)
		{
			inventoryModule.RemoveItem(itemBaseConfig.Item.SlotPosition);
			return;
		}
		inventoryModule.ConsumeItem(itemBaseConfig.Item, amount);
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0001967C File Offset: 0x0001787C
	protected Coroutine StartCoroutine(ItemBaseConfig itemBaseConfig, IEnumerator routine)
	{
		ItemModule itemModule;
		itemBaseConfig.UserObject.TryGetComponent<ItemModule>(out itemModule);
		return itemModule.StartCoroutine(routine);
	}

	// Token: 0x04000787 RID: 1927
	protected static readonly ItemDatabaseModule ItemDatabaseModule;

	// Token: 0x04000788 RID: 1928
	protected static readonly QuestDatabaseModule QuestDatabaseModule;

	// Token: 0x04000789 RID: 1929
	protected static readonly BlueprintDatabaseModule BlueprintDatabaseModule;

	// Token: 0x0400078A RID: 1930
	protected static readonly NpcDatabaseModule NpcDatabaseModule;
}
