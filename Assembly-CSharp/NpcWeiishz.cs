using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x0200048A RID: 1162
public class NpcWeiishz : MonoBehaviour
{
	// Token: 0x06001A53 RID: 6739 RVA: 0x00087028 File Offset: 0x00085228
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_weiishz_name", "default_npc_blacksmith_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_blacksmith_choice_craft", 1),
			new NpcChoice("default_npc_blacksmith_choice_sell_items", 2),
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenCraft)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x06001A54 RID: 6740 RVA: 0x0007C472 File Offset: 0x0007A672
	private void OpenCraft(GameObject player)
	{
		player.GetComponent<PlayerModule>();
		CraftModule component = player.GetComponent<CraftModule>();
		component.TargetShowCraftWindow(component.connectionToClient);
	}

	// Token: 0x06001A55 RID: 6741 RVA: 0x000870F8 File Offset: 0x000852F8
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
		NpcDialog npcDialog = new NpcDialog("default_npc_blacksmith_sell_store_title", "default_npc_blacksmith_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x0400167A RID: 5754
	private NpcModule npcModule;

	// Token: 0x0400167B RID: 5755
	private ItemDatabaseModule itemDatabaseModule;
}
