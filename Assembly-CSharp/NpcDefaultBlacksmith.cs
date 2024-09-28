using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x0200044A RID: 1098
public class NpcDefaultBlacksmith : MonoBehaviour
{
	// Token: 0x060018B4 RID: 6324 RVA: 0x0007C390 File Offset: 0x0007A590
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		NpcDialog handshakeDialog = new NpcDialog(this.creatureModule.CreatureName, "default_npc_blacksmith_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_blacksmith_choice_craft", 1),
			new NpcChoice("default_npc_blacksmith_choice_sell_items", 2),
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenCraft)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x0007C472 File Offset: 0x0007A672
	private void OpenCraft(GameObject player)
	{
		player.GetComponent<PlayerModule>();
		CraftModule component = player.GetComponent<CraftModule>();
		component.TargetShowCraftWindow(component.connectionToClient);
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x0007C48C File Offset: 0x0007A68C
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

	// Token: 0x040015B5 RID: 5557
	private NpcModule npcModule;

	// Token: 0x040015B6 RID: 5558
	private CreatureModule creatureModule;

	// Token: 0x040015B7 RID: 5559
	private ItemDatabaseModule itemDatabaseModule;
}
