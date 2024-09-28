using System;
using Mirror;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class NpcYalerik : MonoBehaviour
{
	// Token: 0x06001A57 RID: 6743 RVA: 0x000871A4 File Offset: 0x000853A4
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_yalerik_name", "npc_yalerik_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_default_choice_quests", 88888),
			new NpcChoice("npc_yalerik_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x0400167C RID: 5756
	private NpcModule npcModule;
}
