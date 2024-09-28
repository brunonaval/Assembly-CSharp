using System;
using Mirror;
using UnityEngine;

// Token: 0x0200046F RID: 1135
public class NpcJack : MonoBehaviour
{
	// Token: 0x06001965 RID: 6501 RVA: 0x000816DC File Offset: 0x0007F8DC
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_jack_name", "npc_jack_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_jack_sail_choice", 1),
			new NpcChoice("npc_jack_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.ShowSailOptions)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.GoToForbiddenIsland)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.GoToForbiddenIslandConfirmed)));
		this.npcModule.AddAction(new NpcAction(4, new NpcAction.NpcTask(this.BackToChameleonBeach)));
		this.npcModule.AddAction(new NpcAction(5, new NpcAction.NpcTask(this.BackToChameleonBeachConfirmed)));
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000817DC File Offset: 0x0007F9DC
	private void ShowSailOptions(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_jack_name", "npc_jack_sail_greet", new NpcChoice[]
		{
			new NpcChoice("npc_jack_chameleon_beach_choice", 4),
			new NpcChoice("npc_jack_nowhere_choice", 99999)
		});
		if (player.GetComponent<AreaModule>().CurrentArea.ToLower().Contains("chameleon"))
		{
			npcDialog = new NpcDialog("npc_jack_name", "npc_jack_sail_greet", new NpcChoice[]
			{
				new NpcChoice("npc_jack_forbidden_island", 2),
				new NpcChoice("npc_jack_nowhere_choice", 99999)
			});
		}
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x00081898 File Offset: 0x0007FA98
	private void GoToForbiddenIsland(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_jack_name", "npc_jack_go_to_forbidden_island_greet", new NpcChoice[]
		{
			new NpcChoice("npc_jack_choice_yes", 3),
			new NpcChoice("npc_jack_choice_no", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000818F8 File Offset: 0x0007FAF8
	private void GoToForbiddenIslandConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleportedHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("forbidden_island");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x00081950 File Offset: 0x0007FB50
	private void BackToChameleonBeach(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_jack_name", "npc_jack_back_to_chameleon_beach_greet", new NpcChoice[]
		{
			new NpcChoice("npc_jack_choice_yes", 5),
			new NpcChoice("npc_jack_choice_no", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000819B0 File Offset: 0x0007FBB0
	private void BackToChameleonBeachConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("chameleon_beach_sea");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x04001626 RID: 5670
	private NpcModule npcModule;
}
