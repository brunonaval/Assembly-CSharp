using System;
using Mirror;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class NpcVikingGuard : MonoBehaviour
{
	// Token: 0x06001A45 RID: 6725 RVA: 0x00086BB0 File Offset: 0x00084DB0
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		this.creatureModule = base.GetComponent<CreatureModule>();
		NpcDialog handshakeDialog = new NpcDialog(this.creatureModule.CreatureName, "default_npc_guard_handshake", new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
	}

	// Token: 0x0400166E RID: 5742
	private NpcModule npcModule;

	// Token: 0x0400166F RID: 5743
	private CreatureModule creatureModule;
}
