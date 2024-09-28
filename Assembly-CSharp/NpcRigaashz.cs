using System;
using Mirror;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class NpcRigaashz : MonoBehaviour
{
	// Token: 0x060019F7 RID: 6647 RVA: 0x00084EC4 File Offset: 0x000830C4
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_rigaashz_name", "default_npc_warehouse_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_warehouse_choice_my_warehouse", 1),
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, delegate(GameObject player)
		{
			this.OpenWarehouse(player);
		}));
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x00084F50 File Offset: 0x00083150
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

	// Token: 0x04001666 RID: 5734
	private NpcModule npcModule;
}
