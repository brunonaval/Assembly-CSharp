using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class NpcDefaultFood : MonoBehaviour
{
	// Token: 0x060018C0 RID: 6336 RVA: 0x0007C868 File Offset: 0x0007AA68
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		NpcDialog handshakeDialog = new NpcDialog(this.creatureModule.CreatureName, "default_npc_food_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_choice_buy_food", 1),
			new NpcChoice("default_npc_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x060018C1 RID: 6337 RVA: 0x0007C960 File Offset: 0x0007AB60
	public void OpenBuyStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		StoreItemConfig[] storeItems = new StoreItemConfig[]
		{
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(934)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1302)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1303)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1304)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(948))
		};
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("default_npc_buy_store_title", "default_npc_buy_store_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999),
			new NpcChoice("default_npc_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x0007CA9C File Offset: 0x0007AC9C
	public void OpenSellStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		InventoryModule inventoryModule;
		player.TryGetComponent<InventoryModule>(out inventoryModule);
		Item[] uniqueSellableItems = inventoryModule.GetUniqueSellableItems();
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		foreach (Item item in uniqueSellableItems)
		{
			if (item.Amount > 0)
			{
				list.Add(new StoreItemConfig(this.npcModule.NpcId, item));
			}
		}
		NpcDialog npcDialog = new NpcDialog("default_npc_sell_store_title", "default_npc_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999),
			new NpcChoice("default_npc_choice_buy_food", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x040015BD RID: 5565
	private NpcModule npcModule;

	// Token: 0x040015BE RID: 5566
	private CreatureModule creatureModule;

	// Token: 0x040015BF RID: 5567
	private ItemDatabaseModule itemDatabaseModule;
}
