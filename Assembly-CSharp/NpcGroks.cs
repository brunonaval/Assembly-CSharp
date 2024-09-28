using System;
using Mirror;
using UnityEngine;

// Token: 0x02000461 RID: 1121
public class NpcGroks : MonoBehaviour
{
	// Token: 0x0600190A RID: 6410 RVA: 0x0007F044 File Offset: 0x0007D244
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_groks_name", "npc_groks_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_default_choice_quests", 88888),
			new NpcChoice("npc_groks_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015F5 RID: 5621
	private NpcModule npcModule;
}
