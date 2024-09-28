using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000454 RID: 1108
public class NpcBlazar : MonoBehaviour
{
	// Token: 0x060018DD RID: 6365 RVA: 0x0007D964 File Offset: 0x0007BB64
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_blazar_name", "npc_blazar_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_blazar_choice_sell_items", 1),
			new NpcChoice("npc_blazar_choice_buy_potions", 2),
			new NpcChoice("npc_blazar_choice_warehouse", 3),
			new NpcChoice("default_npc_repurchase_items_choice", 99998),
			new NpcChoice("npc_default_choice_quests", 88888),
			new NpcChoice("npc_blazar_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenSellStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenPotionsStore)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.OpenWarehouse)));
	}

	// Token: 0x060018DE RID: 6366 RVA: 0x0007DA90 File Offset: 0x0007BC90
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
		NpcDialog npcDialog = new NpcDialog("npc_blazar_sell_store_title", "npc_blazar_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_blazar_choice_bye", 99999),
			new NpcChoice("npc_blazar_choice_buy_potions", 2)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018DF RID: 6367 RVA: 0x0007DB4C File Offset: 0x0007BD4C
	public void OpenPotionsStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		StoreItemConfig[] storeItems = new StoreItemConfig[]
		{
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(16), 600),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1317), 840),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1361)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1362))
		};
		PlayerModule component = player.GetComponent<PlayerModule>();
		NpcDialog npcDialog = new NpcDialog("npc_blazar_potions_store_title", "npc_blazar_potions_store_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_blazar_choice_bye", 99999),
			new NpcChoice("npc_blazar_choice_sell_items", 1)
		})
		{
			StoreAction = StoreAction.Buy
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018E0 RID: 6368 RVA: 0x0007DC64 File Offset: 0x0007BE64
	private void OpenWarehouse(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (GlobalUtils.IsClose(player.transform.position, base.transform.position, 0.96f))
		{
			component.TargetOpenWarehouse(component.connectionToClient, base.transform.position);
			component.TargetOpenInventory(component.connectionToClient);
		}
		this.npcModule.CloseDialog(player);
	}

	// Token: 0x040015CA RID: 5578
	private NpcModule npcModule;

	// Token: 0x040015CB RID: 5579
	private ItemDatabaseModule itemDatabaseModule;
}
