using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000451 RID: 1105
public class NpcAlec : MonoBehaviour
{
	// Token: 0x060018D0 RID: 6352 RVA: 0x0007D158 File Offset: 0x0007B358
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_alec_name", "npc_alec_choice_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_alec_choice_buy_items", 1),
			new NpcChoice("npc_alec_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("npc_alec_choice_quests", 88888),
			new NpcChoice("npc_alec_choice_bye", 99999)
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

	// Token: 0x060018D1 RID: 6353 RVA: 0x0007D254 File Offset: 0x0007B454
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
		NpcDialog npcDialog = new NpcDialog("npc_alec_sell_store_title", "npc_alec_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_alec_choice_bye", 99999),
			new NpcChoice("npc_alec_choice_buy_items", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x0007D310 File Offset: 0x0007B510
	public void OpenBuyStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		StoreItemConfig[] storeItems = new StoreItemConfig[]
		{
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(301), 100000),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(228), 85000),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1275)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1305)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1306)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1361)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1362)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1363)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1364)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1365)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(16), 600),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1317), 840),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(279), 7500),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(284), 12500),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(193), 15000),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(283), 750000)
		};
		PlayerModule component = player.GetComponent<PlayerModule>();
		NpcDialog npcDialog = new NpcDialog("npc_alec_buy_store_title", "npc_alec_buy_store_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_alec_choice_thanks", 99999),
			new NpcChoice("npc_alec_choice_buy_spellbooks", 1),
			new NpcChoice("npc_alec_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x040015C5 RID: 5573
	private NpcModule npcModule;

	// Token: 0x040015C6 RID: 5574
	private ItemDatabaseModule itemDatabaseModule;
}
