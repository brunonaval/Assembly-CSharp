using System;
using Mirror;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class NpcDerek : MonoBehaviour
{
	// Token: 0x060018F6 RID: 6390 RVA: 0x0007EA6C File Offset: 0x0007CC6C
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_derek_name", "npc_derek_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_derek_choice_quests", 88888),
			new NpcChoice("npc_derek_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015EC RID: 5612
	private NpcModule npcModule;
}
