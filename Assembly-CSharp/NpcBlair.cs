using System;
using Mirror;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class NpcBlair : MonoBehaviour
{
	// Token: 0x060018D9 RID: 6361 RVA: 0x0007D7E8 File Offset: 0x0007B9E8
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_blair_name", "npc_blair_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_blair_go_back_to_hazara_island_choice", 88889),
			new NpcChoice("npc_blair_choice_quests", 88888),
			new NpcChoice("npc_blair_choice_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(88889, new NpcAction.NpcTask(this.GoBackToHazaraIsland)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.GoBackToHazaraIslandConfirmed)));
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x0007D8AC File Offset: 0x0007BAAC
	private void GoBackToHazaraIsland(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_blair_name", "npc_blair_go_back_to_hazara_island_greet", new NpcChoice[]
		{
			new NpcChoice("npc_blair_choice_yes", 2),
			new NpcChoice("npc_blair_choice_no", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0007D90C File Offset: 0x0007BB0C
	private void GoBackToHazaraIslandConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleportedHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("wahmar_harbor");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x040015C9 RID: 5577
	private NpcModule npcModule;
}
