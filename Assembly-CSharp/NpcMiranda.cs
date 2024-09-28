using System;
using Mirror;
using UnityEngine;

// Token: 0x02000472 RID: 1138
public class NpcMiranda : MonoBehaviour
{
	// Token: 0x06001997 RID: 6551 RVA: 0x00082B08 File Offset: 0x00080D08
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_miranda_name", "npc_miranda_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_miranda_choice_quests", 88888),
			new NpcChoice("npc_miranda_choice_see_you", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x0400162A RID: 5674
	private NpcModule npcModule;
}
