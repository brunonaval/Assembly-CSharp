using System;
using Mirror;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class NpcEbuinSentinel : MonoBehaviour
{
	// Token: 0x060018F8 RID: 6392 RVA: 0x0007EADC File Offset: 0x0007CCDC
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_ebuin_sentinel_name", "npc_ebuin_sentinel_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_ebuin_sentinel_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x040015ED RID: 5613
	private NpcModule npcModule;
}
