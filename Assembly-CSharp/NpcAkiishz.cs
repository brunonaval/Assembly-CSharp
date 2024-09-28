using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000450 RID: 1104
public class NpcAkiishz : MonoBehaviour
{
	// Token: 0x060018CC RID: 6348 RVA: 0x0007CE54 File Offset: 0x0007B054
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_akiishz_name", "npc_akiishz_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_akiishz_choice_buy_food", 1),
			new NpcChoice("npc_akiishz_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("npc_akiishz_choice_quests", 88888),
			new NpcChoice("npc_akiishz_choice_see_ya", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x060018CD RID: 6349 RVA: 0x0007CF50 File Offset: 0x0007B150
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
		NpcDialog npcDialog = new NpcDialog("npc_akiishz_food_title", "npc_akiishz_food_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_akiishz_choice_thanks", 99999),
			new NpcChoice("npc_akiishz_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018CE RID: 6350 RVA: 0x0007D08C File Offset: 0x0007B28C
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
		NpcDialog npcDialog = new NpcDialog("npc_akiishz_sell_store_title", "npc_akiishz_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_akiishz_choice_thanks", 99999),
			new NpcChoice("npc_akiishz_choice_buy_food", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x040015C3 RID: 5571
	private NpcModule npcModule;

	// Token: 0x040015C4 RID: 5572
	private ItemDatabaseModule itemDatabaseModule;
}
