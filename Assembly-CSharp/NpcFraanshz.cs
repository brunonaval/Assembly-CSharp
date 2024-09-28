using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x0200045D RID: 1117
public class NpcFraanshz : MonoBehaviour
{
	// Token: 0x060018FC RID: 6396 RVA: 0x0007EBA8 File Offset: 0x0007CDA8
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_fraanshz_name", "npc_fraanshz_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_fraanshz_choice_buy_items", 1),
			new NpcChoice("npc_fraanshz_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("npc_fraanshz_choice_quests", 88888),
			new NpcChoice("npc_fraanshz_choice_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x0007ECA4 File Offset: 0x0007CEA4
	public void OpenBuyStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		AttributeModule component = player.GetComponent<AttributeModule>();
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		Item item = this.itemDatabaseModule.GetItem(967);
		list.Add(new StoreItemConfig(this.npcModule.NpcId, item));
		List<Item> list2 = (from i in this.itemDatabaseModule.Items
		where i.Quality == ItemQuality.Basic & (i.Category == ItemCategory.Armor | i.Category == ItemCategory.Weapon | i.Category == ItemCategory.Projectile)
		select i).ToList<Item>();
		for (int j = 0; j < list2.Count; j++)
		{
			Item value = list2[j];
			value.RequiredLevel = component.BaseLevel;
			list2[j] = value;
		}
		list = list.Union(from s in list2
		select new StoreItemConfig(this.npcModule.NpcId, s)).ToList<StoreItemConfig>();
		NpcDialog npcDialog = new NpcDialog("npc_fraanshz_buy_store_title", "npc_fraanshz_buy_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_fraanshz_choice_bye", 99999),
			new NpcChoice("npc_fraanshz_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x0007EDD8 File Offset: 0x0007CFD8
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
		NpcDialog npcDialog = new NpcDialog("npc_fraanshz_sell_store_title", "npc_fraanshz_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_fraanshz_choice_bye", 99999),
			new NpcChoice("npc_fraanshz_choice_buy_items", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x040015EF RID: 5615
	private NpcModule npcModule;

	// Token: 0x040015F0 RID: 5616
	private ItemDatabaseModule itemDatabaseModule;
}
