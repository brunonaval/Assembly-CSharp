using System;
using Mirror;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class NpcHaldine : MonoBehaviour
{
	// Token: 0x0600190C RID: 6412 RVA: 0x0007F0B4 File Offset: 0x0007D2B4
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_haldine_name", "npc_haldine_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_haldine_choice_quests", 88888),
			new NpcChoice("npc_haldine_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015F6 RID: 5622
	private NpcModule npcModule;
}
