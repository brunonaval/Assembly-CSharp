using System;
using Mirror;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public class NpcLaila : MonoBehaviour
{
	// Token: 0x0600198E RID: 6542 RVA: 0x000826BC File Offset: 0x000808BC
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_laila_name", "npc_laila_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_laila_sail_choice", 1),
			new NpcChoice("default_npc_quests_choice", 88888),
			new NpcChoice("npc_laila_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.ShowSailOptions)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.GoToWinterland)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.GoToWinterlandConfirmed)));
		this.npcModule.AddAction(new NpcAction(4, new NpcAction.NpcTask(this.BackToEbuin)));
		this.npcModule.AddAction(new NpcAction(5, new NpcAction.NpcTask(this.BackToEbuinConfirmed)));
		this.npcModule.AddAction(new NpcAction(6, new NpcAction.NpcTask(this.GoToReptilia)));
		this.npcModule.AddAction(new NpcAction(7, new NpcAction.NpcTask(this.GoToReptiliaConfirmed)));
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x0008280C File Offset: 0x00080A0C
	private void ShowSailOptions(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_laila_name", "npc_laila_sail_greet", new NpcChoice[]
		{
			new NpcChoice("npc_laila_ebuin_choice", 4),
			new NpcChoice("npc_laila_nowhere_choice", 99999)
		});
		if (player.GetComponent<AreaModule>().CurrentArea.ToLower().Contains("ebuin"))
		{
			npcDialog = new NpcDialog("npc_laila_name", "npc_laila_sail_greet", new NpcChoice[]
			{
				new NpcChoice("npc_laila_winterland_choice", 2),
				new NpcChoice("npc_laila_reptilia_choice", 6),
				new NpcChoice("npc_laila_nowhere_choice", 99999)
			});
		}
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000828DC File Offset: 0x00080ADC
	private void GoToReptilia(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_laila_name", "npc_laila_go_to_reptilia_greet", new NpcChoice[]
		{
			new NpcChoice("npc_laila_choice_yes", 7),
			new NpcChoice("npc_laila_choice_no", 99999)
		});
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x00082940 File Offset: 0x00080B40
	private void GoToReptiliaConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("reptilia_shore");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x00082998 File Offset: 0x00080B98
	private void GoToWinterland(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_laila_name", "npc_laila_go_to_winterland_greet", new NpcChoice[]
		{
			new NpcChoice("npc_laila_choice_yes", 3),
			new NpcChoice("npc_laila_choice_no", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000829F8 File Offset: 0x00080BF8
	private void GoToWinterlandConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("winterland");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x00082A50 File Offset: 0x00080C50
	private void BackToEbuin(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_laila_name", "npc_laila_back_to_ebuin_greet", new NpcChoice[]
		{
			new NpcChoice("npc_laila_choice_yes", 5),
			new NpcChoice("npc_laila_choice_no", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x00082AB0 File Offset: 0x00080CB0
	private void BackToEbuinConfirmed(GameObject player)
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

	// Token: 0x04001629 RID: 5673
	private NpcModule npcModule;
}
