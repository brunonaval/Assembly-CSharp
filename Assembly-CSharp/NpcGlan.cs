using System;
using Mirror;
using UnityEngine;

// Token: 0x02000460 RID: 1120
public class NpcGlan : MonoBehaviour
{
	// Token: 0x06001906 RID: 6406 RVA: 0x0007EF24 File Offset: 0x0007D124
	public void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_glan_name", "npc_glan_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_glan_choice_my_warehouse", 1),
			new NpcChoice("npc_glan_choice_quests", 88888),
			new NpcChoice("npc_glan_choice_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, delegate(GameObject player)
		{
			this.OpenWarehouse(player);
		}));
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x0007EFC4 File Offset: 0x0007D1C4
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

	// Token: 0x040015F4 RID: 5620
	private NpcModule npcModule;
}
