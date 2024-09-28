using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x0200047D RID: 1149
public class NpcOutlawMercenary : MonoBehaviour
{
	// Token: 0x060019EA RID: 6634 RVA: 0x00084800 File Offset: 0x00082A00
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_outlaw_mercenary_name", "npc_outlaw_mercenary_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_outlaw_mercenary_choice_buy_items", 1),
			new NpcChoice("npc_outlaw_mercenary_choice_sell_items", 2),
			new NpcChoice("npc_outlaw_mercenary_choice_warehouse", 3),
			new NpcChoice("npc_outlaw_mercenary_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.OpenWarehouse)));
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x00084900 File Offset: 0x00082B00
	public void OpenBuyStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		StoreItemConfig[] storeItems = new StoreItemConfig[]
		{
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1361)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1362)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1363)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1364)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1365)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(16), 600),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1317), 840),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1275)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1305)),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(1306))
		};
		NpcDialog npcDialog = new NpcDialog("npc_outlaw_mercenary_buy_store_title", "npc_outlaw_mercenary_buy_store_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_outlaw_mercenary_choice_thanks", 99999),
			new NpcChoice("npc_outlaw_mercenary_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x00084B04 File Offset: 0x00082D04
	public void OpenSellStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		Item[] uniqueSellableItems = player.GetComponent<InventoryModule>().GetUniqueSellableItems();
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		foreach (Item item in uniqueSellableItems)
		{
			if (item.Amount > 0)
			{
				list.Add(new StoreItemConfig(this.npcModule.NpcId, item));
			}
		}
		NpcDialog npcDialog = new NpcDialog("npc_outlaw_mercenary_sell_store_title", "npc_outlaw_mercenary_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_outlaw_mercenary_choice_bye", 99999),
			new NpcChoice("npc_outlaw_mercenary_choice_buy_items", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x00084BC8 File Offset: 0x00082DC8
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

	// Token: 0x04001661 RID: 5729
	private NpcModule npcModule;

	// Token: 0x04001662 RID: 5730
	private ItemDatabaseModule itemDatabaseModule;
}
