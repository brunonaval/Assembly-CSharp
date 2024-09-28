using System;
using Mirror;
using UnityEngine;

// Token: 0x02000455 RID: 1109
public class NpcBruce : MonoBehaviour
{
	// Token: 0x060018E2 RID: 6370 RVA: 0x0007DCDC File Offset: 0x0007BEDC
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_bruce_name", "npc_bruce_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_bruce_choice_quests", 88888),
			new NpcChoice("npc_bruce_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015CC RID: 5580
	private NpcModule npcModule;
}
