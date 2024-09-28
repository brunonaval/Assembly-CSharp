using System;
using Mirror;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class NpcGarbac : MonoBehaviour
{
	// Token: 0x06001904 RID: 6404 RVA: 0x0007EEB4 File Offset: 0x0007D0B4
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_garbac_name", "npc_garbac_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_default_choice_quests", 88888),
			new NpcChoice("npc_garbac_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015F3 RID: 5619
	private NpcModule npcModule;
}
