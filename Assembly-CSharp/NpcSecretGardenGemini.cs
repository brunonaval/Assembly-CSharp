using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class NpcSecretGardenGemini : MonoBehaviour
{
	// Token: 0x06001A1E RID: 6686 RVA: 0x00085C88 File Offset: 0x00083E88
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_gemini_name", "npc_gemini_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_gemini_choice_buy_items", 1),
			new NpcChoice("npc_gemini_choice_sell_items", 2),
			new NpcChoice("npc_gemini_choice_teleport", 3),
			new NpcChoice("npc_gemini_choice_warehouse", 4),
			new NpcChoice("npc_gemini_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.OpenTeleportList)));
		this.npcModule.AddAction(new NpcAction(4, new NpcAction.NpcTask(this.OpenWarehouse)));
		this.npcModule.AddAction(new NpcAction(5, new NpcAction.NpcTask(this.ShowEbuinLocations)));
		this.npcModule.AddAction(new NpcAction(6, new NpcAction.NpcTask(this.ShowReptiliaLocations)));
		this.npcModule.AddAction(new NpcAction(100, delegate(GameObject player)
		{
			this.Teleport(player, "ebuin_town");
		}));
		this.npcModule.AddAction(new NpcAction(101, delegate(GameObject player)
		{
			this.Teleport(player, "ebuin_harbor");
		}));
		this.npcModule.AddAction(new NpcAction(102, delegate(GameObject player)
		{
			this.Teleport(player, "restless_forest");
		}));
		this.npcModule.AddAction(new NpcAction(103, delegate(GameObject player)
		{
			this.Teleport(player, "winterland");
		}));
		this.npcModule.AddAction(new NpcAction(104, delegate(GameObject player)
		{
			this.Teleport(player, "ebon_vertex");
		}));
		this.npcModule.AddAction(new NpcAction(105, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_west");
		}));
		this.npcModule.AddAction(new NpcAction(106, delegate(GameObject player)
		{
			this.Teleport(player, "chameleon_beach");
		}));
		this.npcModule.AddAction(new NpcAction(107, delegate(GameObject player)
		{
			this.Teleport(player, "swamp_of_silence");
		}));
		this.npcModule.AddAction(new NpcAction(108, delegate(GameObject player)
		{
			this.Teleport(player, "forest_of_shadows");
		}));
		this.npcModule.AddAction(new NpcAction(109, delegate(GameObject player)
		{
			this.Teleport(player, "forbidden_island");
		}));
		this.npcModule.AddAction(new NpcAction(110, delegate(GameObject player)
		{
			this.Teleport(player, "abandoned_mines");
		}));
		this.npcModule.AddAction(new NpcAction(111, delegate(GameObject player)
		{
			this.Teleport(player, "enchanted_forest");
		}));
		this.npcModule.AddAction(new NpcAction(112, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_south");
		}));
		this.npcModule.AddAction(new NpcAction(113, delegate(GameObject player)
		{
			this.Teleport(player, "shattered_woods");
		}));
		this.npcModule.AddAction(new NpcAction(114, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_east");
		}));
		this.npcModule.AddAction(new NpcAction(115, delegate(GameObject player)
		{
			this.Teleport(player, "sunset_forest");
		}));
		this.npcModule.AddAction(new NpcAction(116, delegate(GameObject player)
		{
			this.Teleport(player, "dark_castle");
		}));
		this.npcModule.AddAction(new NpcAction(117, delegate(GameObject player)
		{
			this.Teleport(player, "hunters_pass");
		}));
		this.npcModule.AddAction(new NpcAction(118, delegate(GameObject player)
		{
			this.Teleport(player, "dark_sanctuary");
		}));
		this.npcModule.AddAction(new NpcAction(119, delegate(GameObject player)
		{
			this.Teleport(player, "forbidden_island_west");
		}));
		this.npcModule.AddAction(new NpcAction(201, delegate(GameObject player)
		{
			this.Teleport(player, "calangia_village");
		}));
		this.npcModule.AddAction(new NpcAction(202, delegate(GameObject player)
		{
			this.Teleport(player, "reptilia_shore");
		}));
		this.npcModule.AddAction(new NpcAction(203, delegate(GameObject player)
		{
			this.Teleport(player, "lagartia_oasis");
		}));
		this.npcModule.AddAction(new NpcAction(204, delegate(GameObject player)
		{
			this.Teleport(player, "lagartia_catacombs");
		}));
		this.npcModule.AddAction(new NpcAction(205, delegate(GameObject player)
		{
			this.Teleport(player, "crocodilia_hot_springs");
		}));
		this.npcModule.AddAction(new NpcAction(206, delegate(GameObject player)
		{
			this.Teleport(player, "iguania_rocks");
		}));
		this.npcModule.AddAction(new NpcAction(207, delegate(GameObject player)
		{
			this.Teleport(player, "iguania_caves");
		}));
		this.npcModule.AddAction(new NpcAction(208, delegate(GameObject player)
		{
			this.Teleport(player, "crocodilia_caves");
		}));
	}

	// Token: 0x06001A1F RID: 6687 RVA: 0x00086150 File Offset: 0x00084350
	public void OpenBuyStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		StoreItemConfig[] storeItems = new StoreItemConfig[]
		{
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(279), 6750),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(284), 11250),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(193), 13500),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(301), 90000),
			new StoreItemConfig(this.npcModule.NpcId, this.itemDatabaseModule.GetItem(283), 675000),
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
		PlayerModule component = player.GetComponent<PlayerModule>();
		NpcDialog npcDialog = new NpcDialog("npc_gemini_buy_store_title", "npc_gemini_buy_store_greet", storeItems, new NpcChoice[]
		{
			new NpcChoice("npc_gemini_choice_thanks", 99999),
			new NpcChoice("npc_gemini_choice_sell_items", 2),
			new NpcChoice("npc_gemini_choice_teleport", 3)
		})
		{
			StoreAction = StoreAction.Buy
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001A20 RID: 6688 RVA: 0x00086448 File Offset: 0x00084648
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
		NpcDialog npcDialog = new NpcDialog("npc_gemini_sell_store_title", "npc_gemini_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_gemini_choice_bye", 99999),
			new NpcChoice("npc_gemini_choice_buy_items", 1),
			new NpcChoice("npc_gemini_choice_teleport", 3)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x00086518 File Offset: 0x00084718
	public void OpenTeleportList(GameObject player)
	{
		NpcDialog npcDialog = new NpcDialog("npc_gemini_name", "npc_gemini_teleport_greet", new NpcChoice[]
		{
			new NpcChoice("npc_gemini_ebuin_continent_choice", 5),
			new NpcChoice("npc_gemini_reptilia_continent_choice", 6),
			new NpcChoice("npc_gemini_stay_here_choice", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x00086584 File Offset: 0x00084784
	private void ShowReptiliaLocations(GameObject player)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_gemini_name", "npc_gemini_teleport_greet", new NpcChoice[]
		{
			new NpcChoice("teleport_calangia_village", "free", 201),
			new NpcChoice("teleport_reptilia_shore", "free", 202),
			new NpcChoice("teleport_lagartia_oasis", "free", 203),
			new NpcChoice("teleport_lagartia_catacombs", "free", 204),
			new NpcChoice("teleport_crocodilia_hot_springs", "free", 205),
			new NpcChoice("teleport_iguania_rocks", "free", 206),
			new NpcChoice("teleport_iguania_caves", "free", 207),
			new NpcChoice("teleport_crocodilia_caves", "free", 208),
			new NpcChoice("npc_gemini_stay_here_choice", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x000866A8 File Offset: 0x000848A8
	private void ShowEbuinLocations(GameObject player)
	{
		NpcDialog npcDialog = new NpcDialog("npc_gemini_name", "npc_gemini_teleport_greet", new NpcChoice[]
		{
			new NpcChoice("teleport_ebuin_town", "free", 100),
			new NpcChoice("teleport_ebuin_harbor", "free", 101),
			new NpcChoice("teleport_restless_forest", "free", 102),
			new NpcChoice("teleport_winterland", "free", 103),
			new NpcChoice("teleport_hunters_pass", "free", 117),
			new NpcChoice("teleport_ebon_vertex", "free", 104),
			new NpcChoice("teleport_niruth_valley_west", "free", 105),
			new NpcChoice("teleport_chameleon_beach", "free", 106),
			new NpcChoice("teleport_swamp_of_silence", "free", 107),
			new NpcChoice("teleport_forest_of_shadows", "free", 108),
			new NpcChoice("teleport_forbidden_island", "free", 109),
			new NpcChoice("teleport_abandoned_mines", "free", 110),
			new NpcChoice("teleport_enchanted_forest", "free", 111),
			new NpcChoice("teleport_niruth_valley_south", "free", 112),
			new NpcChoice("teleport_shattered_woods", "free", 113),
			new NpcChoice("teleport_niruth_valley_east", "free", 114),
			new NpcChoice("teleport_sunset_valley", "free", 115),
			new NpcChoice("teleport_dark_castle", "free", 116),
			new NpcChoice("teleport_dark_sanctuary", "free", 118),
			new NpcChoice("teleport_forbidden_island_west", "free", 119),
			new NpcChoice("npc_gemini_stay_here_choice", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x000868DC File Offset: 0x00084ADC
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

	// Token: 0x06001A25 RID: 6693 RVA: 0x00086954 File Offset: 0x00084B54
	private void Teleport(GameObject player, string locationName)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		WalletModule walletModule;
		player.TryGetComponent<WalletModule>(out walletModule);
		MovementModule movementModule;
		player.TryGetComponent<MovementModule>(out movementModule);
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		this.npcModule.CloseDialog(player);
		movementModule.TargetTeleport(movementModule.connectionToClient, GlobalUtils.GetLocationFromSpawnPoint(locationName), new Effect("TeleporterHit", 0.5f, 0.25f));
	}

	// Token: 0x0400166B RID: 5739
	private NpcModule npcModule;

	// Token: 0x0400166C RID: 5740
	private ItemDatabaseModule itemDatabaseModule;
}
