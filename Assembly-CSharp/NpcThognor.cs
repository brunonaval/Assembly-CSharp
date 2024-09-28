using System;
using Mirror;
using UnityEngine;

// Token: 0x02000485 RID: 1157
public class NpcThognor : MonoBehaviour
{
	// Token: 0x06001A43 RID: 6723 RVA: 0x00086B40 File Offset: 0x00084D40
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_thognor_name", "npc_thognor_choice_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_thognor_choice_quests", 88888),
			new NpcChoice("npc_thognor_choice_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x0400166D RID: 5741
	private NpcModule npcModule;
}
