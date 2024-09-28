using System;
using Mirror;
using UnityEngine;

// Token: 0x02000483 RID: 1155
public class NpcSawana : MonoBehaviour
{
	// Token: 0x06001A1C RID: 6684 RVA: 0x00085C18 File Offset: 0x00083E18
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_sawana_name", "npc_sawana_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_sawana_choice_quests", 88888),
			new NpcChoice("npc_sawana_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x0400166A RID: 5738
	private NpcModule npcModule;
}
