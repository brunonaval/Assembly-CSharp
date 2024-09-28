using System;
using Mirror;
using UnityEngine;

// Token: 0x02000480 RID: 1152
public class NpcReptiliaMercenary : MonoBehaviour
{
	// Token: 0x060019F5 RID: 6645 RVA: 0x00084E68 File Offset: 0x00083068
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_reptilia_mercenary_name", "npc_reptilia_mercenary_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_reptilia_mercenary_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x04001665 RID: 5733
	private NpcModule npcModule;
}
