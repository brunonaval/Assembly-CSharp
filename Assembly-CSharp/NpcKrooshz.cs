using System;
using Mirror;
using UnityEngine;

// Token: 0x02000470 RID: 1136
public class NpcKrooshz : MonoBehaviour
{
	// Token: 0x0600196C RID: 6508 RVA: 0x00081A08 File Offset: 0x0007FC08
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_krooshz_name", "npc_krooshz_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_krooshz_choice_villager", 1),
			new NpcChoice("npc_krooshz_choice_go_to_ebuin", 2),
			new NpcChoice("npc_krooshz_choice_teleport", 3),
			new NpcChoice("npc_krooshz_choice_quests", 88888),
			new NpcChoice("npc_krooshz_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.BecomeVillager)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.GoToEbuin)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.OpenTeleportList)));
		this.npcModule.AddAction(new NpcAction(4, new NpcAction.NpcTask(this.ShowEbuinLocations)));
		this.npcModule.AddAction(new NpcAction(5, new NpcAction.NpcTask(this.ShowReptiliaLocations)));
		this.npcModule.AddAction(new NpcAction(101, delegate(GameObject player)
		{
			this.Teleport(player, "ebuin_harbor", 200);
		}));
		this.npcModule.AddAction(new NpcAction(102, delegate(GameObject player)
		{
			this.Teleport(player, "restless_forest", 260);
		}));
		this.npcModule.AddAction(new NpcAction(103, delegate(GameObject player)
		{
			this.Teleport(player, "winterland", 300);
		}));
		this.npcModule.AddAction(new NpcAction(104, delegate(GameObject player)
		{
			this.Teleport(player, "ebon_vertex", 400);
		}));
		this.npcModule.AddAction(new NpcAction(105, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_west", 600);
		}));
		this.npcModule.AddAction(new NpcAction(106, delegate(GameObject player)
		{
			this.Teleport(player, "chameleon_beach", 1000);
		}));
		this.npcModule.AddAction(new NpcAction(107, delegate(GameObject player)
		{
			this.Teleport(player, "swamp_of_silence", 120);
		}));
		this.npcModule.AddAction(new NpcAction(108, delegate(GameObject player)
		{
			this.Teleport(player, "forest_of_shadows", 120);
		}));
		this.npcModule.AddAction(new NpcAction(109, delegate(GameObject player)
		{
			this.Teleport(player, "forbidden_island", 1120);
		}));
		this.npcModule.AddAction(new NpcAction(110, delegate(GameObject player)
		{
			this.Teleport(player, "abandoned_mines", 2000);
		}));
		this.npcModule.AddAction(new NpcAction(111, delegate(GameObject player)
		{
			this.Teleport(player, "enchanted_forest", 1720);
		}));
		this.npcModule.AddAction(new NpcAction(112, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_south", 1600);
		}));
		this.npcModule.AddAction(new NpcAction(113, delegate(GameObject player)
		{
			this.Teleport(player, "shattered_woods", 1800);
		}));
		this.npcModule.AddAction(new NpcAction(114, delegate(GameObject player)
		{
			this.Teleport(player, "niruth_valley_east", 1600);
		}));
		this.npcModule.AddAction(new NpcAction(115, delegate(GameObject player)
		{
			this.Teleport(player, "sunset_forest", 4800);
		}));
		this.npcModule.AddAction(new NpcAction(116, delegate(GameObject player)
		{
			this.Teleport(player, "dark_castle", 8000);
		}));
		this.npcModule.AddAction(new NpcAction(117, delegate(GameObject player)
		{
			this.Teleport(player, "hunters_pass", 1200);
		}));
		this.npcModule.AddAction(new NpcAction(118, delegate(GameObject player)
		{
			this.Teleport(player, "dark_sanctuary", 12000);
		}));
		this.npcModule.AddAction(new NpcAction(119, delegate(GameObject player)
		{
			this.Teleport(player, "forbidden_island_west", 10000);
		}));
		this.npcModule.AddAction(new NpcAction(202, delegate(GameObject player)
		{
			this.Teleport(player, "reptilia_shore", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(203, delegate(GameObject player)
		{
			this.Teleport(player, "lagartia_oasis", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(204, delegate(GameObject player)
		{
			this.Teleport(player, "lagartia_catacombs", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(205, delegate(GameObject player)
		{
			this.Teleport(player, "crocodilia_hot_springs", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(206, delegate(GameObject player)
		{
			this.Teleport(player, "iguania_rocks", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(207, delegate(GameObject player)
		{
			this.Teleport(player, "iguania_caves", 25000);
		}));
		this.npcModule.AddAction(new NpcAction(208, delegate(GameObject player)
		{
			this.Teleport(player, "crocodilia_caves", 25000);
		}));
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x00081E64 File Offset: 0x00080064
	private void BecomeVillager(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		EffectModule component = player.GetComponent<EffectModule>();
		WalletModule component2 = player.GetComponent<WalletModule>();
		MovementModule component3 = player.GetComponent<MovementModule>();
		if (component3.SpawnPoint == "calangia_village")
		{
			component.ShowScreenMessage("calangia_village_already_villager_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (component2.GoldCoins < (long)this.becomeVillagerPrice)
		{
			component.ShowScreenMessage("villager_calangia_village_gold_coins_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		component2.AddGoldCoins(-this.becomeVillagerPrice);
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		component.ShowEffects(effectConfig);
		component3.SetSpawnPoint("calangia_village", true);
		component.ShowScreenMessage("villager_calangia_village_message", 1, 3.5f, Array.Empty<string>());
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x00081F50 File Offset: 0x00080150
	private void GoToEbuin(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("ebuin_town_port");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x00081FA8 File Offset: 0x000801A8
	private void OpenTeleportList(GameObject player)
	{
		PlayerModule component = player.GetComponent<PlayerModule>();
		NpcDialog npcDialog = new NpcDialog("npc_krooshz_name", "npc_krooshz_teleport_greet", new NpcChoice[]
		{
			new NpcChoice("npc_krooshz_ebuin_continent_choice", 4),
			new NpcChoice("npc_krooshz_reptilia_continent_choice", 5),
			new NpcChoice("npc_krooshz_stay_here_choice", 99999)
		});
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x00082014 File Offset: 0x00080214
	private void ShowReptiliaLocations(GameObject player)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_krooshz_name", "npc_krooshz_teleport_greet", new NpcChoice[]
		{
			new NpcChoice("teleport_reptilia_shore", "25000", "gold_coins", 202),
			new NpcChoice("teleport_lagartia_oasis", "25000", "gold_coins", 203),
			new NpcChoice("teleport_lagartia_catacombs", "25000", "gold_coins", 204),
			new NpcChoice("teleport_crocodilia_hot_springs", "25000", "gold_coins", 205),
			new NpcChoice("teleport_iguania_rocks", "25000", "gold_coins", 206),
			new NpcChoice("teleport_iguania_caves", "25000", "gold_coins", 207),
			new NpcChoice("teleport_crocodilia_caves", "25000", "gold_coins", 208),
			new NpcChoice("npc_krooshz_stay_here_choice", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x00082140 File Offset: 0x00080340
	private void ShowEbuinLocations(GameObject player)
	{
		PlayerModule component = player.GetComponent<PlayerModule>();
		NpcDialog npcDialog = new NpcDialog("npc_krooshz_name", "npc_krooshz_teleport_greet", new NpcChoice[]
		{
			new NpcChoice("teleport_ebuin_harbor", "200", "gold_coins", 101),
			new NpcChoice("teleport_restless_forest", "260", "gold_coins", 102),
			new NpcChoice("teleport_winterland", "300", "gold_coins", 103),
			new NpcChoice("teleport_ebon_vertex", "400", "gold_coins", 104),
			new NpcChoice("teleport_niruth_valley_west", "600", "gold_coins", 105),
			new NpcChoice("teleport_chameleon_beach", "1000", "gold_coins", 106),
			new NpcChoice("teleport_swamp_of_silence", "120", "gold_coins", 107),
			new NpcChoice("teleport_forest_of_shadows", "120", "gold_coins", 108),
			new NpcChoice("teleport_forbidden_island", "1120", "gold_coins", 109),
			new NpcChoice("teleport_abandoned_mines", "2000", "gold_coins", 110),
			new NpcChoice("teleport_enchanted_forest", "1720", "gold_coins", 111),
			new NpcChoice("teleport_niruth_valley_south", "1600", "gold_coins", 112),
			new NpcChoice("teleport_shattered_woods", "1800", "gold_coins", 113),
			new NpcChoice("teleport_niruth_valley_east", "1600", "gold_coins", 114),
			new NpcChoice("teleport_sunset_valley", "4800", "gold_coins", 115),
			new NpcChoice("teleport_dark_castle", "8000", "gold_coins", 116),
			new NpcChoice("teleport_hunters_pass", "1200", "gold_coins", 117),
			new NpcChoice("teleport_dark_sanctuary", "12000", "gold_coins", 118),
			new NpcChoice("teleport_forbidden_island_west", "10000", "gold_coins", 119),
			new NpcChoice("npc_krooshz_stay_here_choice", 99999)
		});
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000823BC File Offset: 0x000805BC
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
			NpcDialog npcDialog = new NpcDialog("npc_krooshz_name", "default_npc_account_not_veteran", new NpcChoice[]
			{
				new NpcChoice("npc_krooshz_alright_choice", 99999)
			});
			playerModule.RenderNpcDialog(npcDialog);
			return;
		}
		if (playerModule.PremiumDays < 1)
		{
			if (walletModule.GoldCoins < (long)price)
			{
				NpcDialog npcDialog2 = new NpcDialog("npc_krooshz_name", "npc_krooshz_not_enough_money", new NpcChoice[]
				{
					new NpcChoice("npc_krooshz_alright_choice", 99999)
				});
				playerModule.RenderNpcDialog(npcDialog2);
				return;
			}
			walletModule.AddGoldCoins(-price);
		}
		this.npcModule.CloseDialog(player);
		movementModule.TargetTeleport(movementModule.connectionToClient, GlobalUtils.GetLocationFromSpawnPoint(locationName), new Effect("TeleporterHit", 0.5f, 0.25f));
	}

	// Token: 0x04001627 RID: 5671
	private NpcModule npcModule;

	// Token: 0x04001628 RID: 5672
	private readonly int becomeVillagerPrice = 300000;
}
