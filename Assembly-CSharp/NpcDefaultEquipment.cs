using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x0200044B RID: 1099
public class NpcDefaultEquipment : MonoBehaviour
{
	// Token: 0x060018B8 RID: 6328 RVA: 0x0007C538 File Offset: 0x0007A738
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
		NpcDialog handshakeDialog = new NpcDialog(this.creatureModule.CreatureName, "default_npc_equipment_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_choice_buy_equipments", 1),
			new NpcChoice("default_npc_choice_sell_items", 2),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x0007C630 File Offset: 0x0007A830
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
		NpcDialog npcDialog = new NpcDialog("default_npc_buy_store_title", "default_npc_buy_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999),
			new NpcChoice("default_npc_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x0007C764 File Offset: 0x0007A964
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
		NpcDialog npcDialog = new NpcDialog("default_npc_sell_store_title", "default_npc_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999),
			new NpcChoice("default_npc_choice_buy_food", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x040015B8 RID: 5560
	private NpcModule npcModule;

	// Token: 0x040015B9 RID: 5561
	private CreatureModule creatureModule;

	// Token: 0x040015BA RID: 5562
	private ItemDatabaseModule itemDatabaseModule;
}
