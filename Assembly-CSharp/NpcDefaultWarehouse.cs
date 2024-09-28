using System;
using Mirror;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class NpcDefaultWarehouse : MonoBehaviour
{
	// Token: 0x060018C4 RID: 6340 RVA: 0x0007CB68 File Offset: 0x0007AD68
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		base.TryGetComponent<NpcModule>(out this.npcModule);
		base.TryGetComponent<CreatureModule>(out this.creatureModule);
		NpcDialog handshakeDialog = new NpcDialog(this.creatureModule.CreatureName, "default_npc_warehouse_handshake", new NpcChoice[]
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

	// Token: 0x060018C5 RID: 6341 RVA: 0x0007CC08 File Offset: 0x0007AE08
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

	// Token: 0x040015C0 RID: 5568
	private NpcModule npcModule;

	// Token: 0x040015C1 RID: 5569
	private CreatureModule creatureModule;
}
