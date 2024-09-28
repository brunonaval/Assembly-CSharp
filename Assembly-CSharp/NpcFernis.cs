using System;
using Mirror;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class NpcFernis : MonoBehaviour
{
	// Token: 0x060018FA RID: 6394 RVA: 0x0007EB38 File Offset: 0x0007CD38
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_fernis_name", "npc_fernis_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_fernis_choice_quests", 88888),
			new NpcChoice("npc_fernis_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015EE RID: 5614
	private NpcModule npcModule;
}
