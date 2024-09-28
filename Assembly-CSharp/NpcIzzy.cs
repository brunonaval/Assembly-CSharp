using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x0200046E RID: 1134
public class NpcIzzy : MonoBehaviour
{
	// Token: 0x0600195F RID: 6495 RVA: 0x000813D8 File Offset: 0x0007F5D8
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_izzy_name", "npc_izzy_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_izzy_choice_buy_food", 1),
			new NpcChoice("npc_izzy_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("npc_izzy_choice_quests", 88888),
			new NpcChoice("npc_izzy_choice_see_ya", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, delegate(GameObject player)
		{
			this.OpenBuyStore(player);
		}));
		this.npcModule.AddAction(new NpcAction(2, delegate(GameObject player)
		{
			this.OpenSellStore(player);
		}));
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x000814D4 File Offset: 0x0007F6D4
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
		PlayerModule component = player.GetComponent<PlayerModule>();
		NpcDialog npcDialog = new NpcDialog("npc_izzy_food_title", "npc_izzy_food_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_izzy_choice_thanks", 99999),
			new NpcChoice("npc_izzy_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x0008160C File Offset: 0x0007F80C
	public void OpenSellStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		Item[] uniqueSellableItems = player.GetComponent<InventoryModule>().GetUniqueSellableItems();
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		foreach (Item item in uniqueSellableItems)
		{
			if (item.Amount > 0)
			{
				list.Add(new StoreItemConfig(this.npcModule.NpcId, item));
			}
		}
		NpcDialog npcDialog = new NpcDialog("npc_izzy_sell_store_title", "npc_izzy_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_izzy_choice_thanks", 99999),
			new NpcChoice("npc_izzy_choice_buy_food", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x04001624 RID: 5668
	private NpcModule npcModule;

	// Token: 0x04001625 RID: 5669
	private ItemDatabaseModule itemDatabaseModule;
}
