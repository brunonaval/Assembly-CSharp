using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class NpcHendrick : MonoBehaviour
{
	// Token: 0x0600190E RID: 6414 RVA: 0x0007F124 File Offset: 0x0007D324
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_hendrick_name", "npc_hendrick_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_hendrick_choice_buy_items", 1),
			new NpcChoice("npc_hendrick_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("npc_hendrick_choice_quests", 88888),
			new NpcChoice("npc_hendrick_choice_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x0007F220 File Offset: 0x0007D420
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
			value.RequiredLevel = Mathf.Min(component.BaseLevel, 850);
			list2[j] = value;
		}
		list = list.Union(from s in list2
		select new StoreItemConfig(this.npcModule.NpcId, s)).ToList<StoreItemConfig>();
		NpcDialog npcDialog = new NpcDialog("npc_hendrick_buy_store_title", "npc_hendrick_buy_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_hendrick_choice_bye", 99999),
			new NpcChoice("npc_hendrick_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x0007F35C File Offset: 0x0007D55C
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
		NpcDialog npcDialog = new NpcDialog("npc_hendrick_sell_store_title", "npc_hendrick_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_hendrick_choice_bye", 99999),
			new NpcChoice("npc_hendrick_choice_buy_items", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x040015F7 RID: 5623
	private NpcModule npcModule;

	// Token: 0x040015F8 RID: 5624
	private ItemDatabaseModule itemDatabaseModule;
}
