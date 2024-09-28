using System;
using Mirror;
using UnityEngine;

// Token: 0x0200046D RID: 1133
public class NpcIzilda : MonoBehaviour
{
	// Token: 0x0600193F RID: 6463 RVA: 0x00080950 File Offset: 0x0007EB50
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_izilda_name", "npc_izilda_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_izilda_ebuin_continent_choice", 1),
			new NpcChoice("npc_izilda_reptilia_continent_choice", 2),
			new NpcChoice("npc_izilda_stay_here_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.ShowEbuinLocations)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.ShowReptiliaLocations)));
		this.npcModule.AddAction(new NpcAction(3, delegate(GameObject player)
		{
			this.Teleport(player, "ebuin_harbor", 200);
		}));
		this.npcModule.AddAction(new NpcAction(4, delegate(GameObject player)
		{
			this.Teleport(player, "restless_forest", 260);
		}));
		this.npcModule.AddAction(new NpcAction(5, delegate(GameObject player)
		{
			this.Teleport(player, "winterland", 300);
		}));
		this.npcModule.AddAction(new NpcAction(6, delegate(GameObject player)
		{
			this.Teleport(player, "ebon_vertex", 400);
		}));
		this.npcModule.AddAction(new NpcAction(7, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_west", 600);
		}));
		this.npcModule.AddAction(new NpcAction(8, delegate(GameObject player)
		{
			this.Teleport(player, "chameleon_beach", 1000);
		}));
		this.npcModule.AddAction(new NpcAction(9, delegate(GameObject player)
		{
			this.Teleport(player, "swamp_of_silence", 120);
		}));
		this.npcModule.AddAction(new NpcAction(10, delegate(GameObject player)
		{
			this.Teleport(player, "forest_of_shadows", 120);
		}));
		this.npcModule.AddAction(new NpcAction(11, delegate(GameObject player)
		{
			this.Teleport(player, "forbidden_island", 1120);
		}));
		this.npcModule.AddAction(new NpcAction(12, delegate(GameObject player)
		{
			this.Teleport(player, "abandoned_mines", 2000);
		}));
		this.npcModule.AddAction(new NpcAction(13, delegate(GameObject player)
		{
			this.Teleport(player, "enchanted_forest", 1720);
		}));
		this.npcModule.AddAction(new NpcAction(14, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_south", 1600);
		}));
		this.npcModule.AddAction(new NpcAction(15, delegate(GameObject player)
		{
			this.Teleport(player, "shattered_woods", 1800);
		}));
		this.npcModule.AddAction(new NpcAction(16, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_east", 2080);
		}));
		this.npcModule.AddAction(new NpcAction(17, delegate(GameObject player)
		{
			this.Teleport(player, "sunset_forest", 4800);
		}));
		this.npcModule.AddAction(new NpcAction(18, delegate(GameObject player)
		{
			this.Teleport(player, "dark_castle", 8000);
		}));
		this.npcModule.AddAction(new NpcAction(19, delegate(GameObject player)
		{
			this.Teleport(player, "hunters_pass", 1200);
		}));
		this.npcModule.AddAction(new NpcAction(20, delegate(GameObject player)
		{
			this.Teleport(player, "dark_sanctuary", 12000);
		}));
		this.npcModule.AddAction(new NpcAction(21, delegate(GameObject player)
		{
			this.Teleport(player, "forbidden_island_west", 10000);
		}));
		this.npcModule.AddAction(new NpcAction(22, delegate(GameObject player)
		{
			this.Teleport(player, "calangia_village", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(23, delegate(GameObject player)
		{
			this.Teleport(player, "reptilia_shore", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(24, delegate(GameObject player)
		{
			this.Teleport(player, "lagartia_oasis", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(25, delegate(GameObject player)
		{
			this.Teleport(player, "lagartia_catacombs", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(26, delegate(GameObject player)
		{
			this.Teleport(player, "crocodilia_hot_springs", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(27, delegate(GameObject player)
		{
			this.Teleport(player, "iguania_rocks", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(28, delegate(GameObject player)
		{
			this.Teleport(player, "iguania_caves", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(29, delegate(GameObject player)
		{
			this.Teleport(player, "crocodilia_caves", 25000);
		}));
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x00080D2C File Offset: 0x0007EF2C
	private void ShowEbuinLocations(GameObject player)
	{
		NpcDialog npcDialog = new NpcDialog("npc_izilda_name", "npc_izilda_handshake", new NpcChoice[]
		{
			new NpcChoice("teleport_ebuin_harbor", "200", "gold_coins", 3),
			new NpcChoice("teleport_restless_forest", "260", "gold_coins", 4),
			new NpcChoice("teleport_winterland", "300", "gold_coins", 5),
			new NpcChoice("teleport_hunters_pass", "1200", "gold_coins", 19),
			new NpcChoice("teleport_ebon_vertex", "400", "gold_coins", 6),
			new NpcChoice("teleport_niruth_valley_west", "600", "gold_coins", 7),
			new NpcChoice("teleport_chameleon_beach", "1000", "gold_coins", 8),
			new NpcChoice("teleport_swamp_of_silence", "120", "gold_coins", 9),
			new NpcChoice("teleport_forest_of_shadows", "120", "gold_coins", 10),
			new NpcChoice("teleport_forbidden_island", "1120", "gold_coins", 11),
			new NpcChoice("teleport_abandoned_mines", "2000", "gold_coins", 12),
			new NpcChoice("teleport_enchanted_forest", "1720", "gold_coins", 13),
			new NpcChoice("teleport_niruth_valley_south", "1600", "gold_coins", 14),
			new NpcChoice("teleport_shattered_woods", "1800", "gold_coins", 15),
			new NpcChoice("teleport_niruth_valley_east", "2080", "gold_coins", 16),
			new NpcChoice("teleport_sunset_valley", "4800", "gold_coins", 17),
			new NpcChoice("teleport_dark_castle", "8000", "gold_coins", 18),
			new NpcChoice("teleport_dark_sanctuary", "12000", "gold_coins", 20),
			new NpcChoice("teleport_forbidden_island_west", "10000", "gold_coins", 21),
			new NpcChoice("npc_izilda_stay_here_choice", 99999)
		});
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x00080FA4 File Offset: 0x0007F1A4
	private void ShowReptiliaLocations(GameObject player)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_izilda_name", "npc_izilda_handshake", new NpcChoice[]
		{
			new NpcChoice("teleport_calangia_village", "25000", "gold_coins", 22),
			new NpcChoice("teleport_reptilia_shore", "25000", "gold_coins", 23),
			new NpcChoice("teleport_lagartia_oasis", "25000", "gold_coins", 24),
			new NpcChoice("teleport_lagartia_catacombs", "25000", "gold_coins", 25),
			new NpcChoice("teleport_crocodilia_hot_springs", "25000", "gold_coins", 26),
			new NpcChoice("teleport_iguania_rocks", "25000", "gold_coins", 27),
			new NpcChoice("teleport_iguania_caves", "25000", "gold_coins", 28),
			new NpcChoice("teleport_crocodilia_caves", "25000", "gold_coins", 29),
			new NpcChoice("npc_izilda_stay_here_choice", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000810D8 File Offset: 0x0007F2D8
	private void Teleport(GameObject player, string locationName, int price)
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
		if (playerModule.PremiumDays == 0 & !GlobalUtils.PackageHasVeteranAccess(playerModule.PackageType))
		{
			NpcDialog npcDialog = new NpcDialog("npc_izilda_name", "npc_izilda_account_not_veteran", new NpcChoice[]
			{
				new NpcChoice("npc_izilda_alright_choice", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog);
			return;
		}
		if (playerModule.PremiumDays < 1)
		{
			if (walletModule.GoldCoins < (long)price)
			{
				NpcDialog npcDialog2 = new NpcDialog("npc_izilda_name", "npc_izilda_not_enough_money", new NpcChoice[]
				{
					new NpcChoice("npc_izilda_alright_choice", 99999)
				});
				playerModule.RenderNpcDialog(npcDialog2);
				return;
			}
			walletModule.AddGoldCoins(-price);
		}
		this.npcModule.CloseDialog(player);
		movementModule.TargetTeleport(movementModule.connectionToClient, GlobalUtils.GetLocationFromSpawnPoint(locationName), new Effect("TeleporterHit", 0.5f, 0.25f));
	}

	// Token: 0x04001623 RID: 5667
	private NpcModule npcModule;
}
