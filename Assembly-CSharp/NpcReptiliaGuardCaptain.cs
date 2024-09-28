using System;
using Mirror;
using UnityEngine;

// Token: 0x0200047F RID: 1151
public class NpcReptiliaGuardCaptain : MonoBehaviour
{
	// Token: 0x060019F3 RID: 6643 RVA: 0x00084DF8 File Offset: 0x00082FF8
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_reptilia_guard_captain_name", "npc_reptilia_guard_captain_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_reptilia_guard_captain_choice_bye", 99999),
			new NpcChoice("npc_chuck_choice_quests", 88888)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x04001664 RID: 5732
	private NpcModule npcModule;
}
