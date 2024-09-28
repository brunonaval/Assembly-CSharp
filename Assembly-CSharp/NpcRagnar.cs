using System;
using Mirror;
using UnityEngine;

// Token: 0x0200047E RID: 1150
public class NpcRagnar : MonoBehaviour
{
	// Token: 0x060019EF RID: 6639 RVA: 0x00084C40 File Offset: 0x00082E40
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("Ragnar", "npc_ragnar_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		Collider2D collider2D = Physics2D.OverlapPoint(base.transform.position, 1 << LayerMask.NameToLayer("WorldArea"));
		if (collider2D != null)
		{
			WorldAreaModule component = collider2D.GetComponent<WorldAreaModule>();
			if (component.AreaName.Contains("artic"))
			{
				handshakeDialog.AddChoice(new NpcChoice("npc_ragnar_goto_fensalir", 1));
			}
			if (component.AreaName.Contains("fensalir"))
			{
				handshakeDialog.AddChoice(new NpcChoice("npc_ragnar_back_to_winterlands", 2));
			}
		}
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.TeleportToFensalir)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.TeleportBackToWinterlands)));
	}

	// Token: 0x060019F0 RID: 6640 RVA: 0x00084D50 File Offset: 0x00082F50
	private void TeleportBackToWinterlands(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule.CloseDialog(player);
		MovementModule component = player.GetComponent<MovementModule>();
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("winterland_north");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, new Effect("TeleporterHit", 0.5f, 0.25f));
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00084DA4 File Offset: 0x00082FA4
	private void TeleportToFensalir(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule.CloseDialog(player);
		MovementModule component = player.GetComponent<MovementModule>();
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("fensalir_harbor");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, new Effect("TeleporterHit", 0.5f, 0.25f));
	}

	// Token: 0x04001663 RID: 5731
	private NpcModule npcModule;
}
