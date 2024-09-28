using System;
using Mirror;
using UnityEngine;

// Token: 0x0200044F RID: 1103
public class NpcAika : MonoBehaviour
{
	// Token: 0x060018C8 RID: 6344 RVA: 0x0007CC88 File Offset: 0x0007AE88
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_aika_name", "npc_aika_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_aika_choice_leave_hazara", 1),
			new NpcChoice("npc_aika_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.BackToEbuin)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.BackToEbuinConfirmed)));
	}

	// Token: 0x060018C9 RID: 6345 RVA: 0x0007CD30 File Offset: 0x0007AF30
	private void BackToEbuin(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_aika_name", "npc_aika_are_you_sure_greet", new NpcChoice[]
		{
			new NpcChoice("npc_aika_choice_yes", 2),
			new NpcChoice("npc_aika_choice_not_yet", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060018CA RID: 6346 RVA: 0x0007CD90 File Offset: 0x0007AF90
	private void BackToEbuinConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		QuestModule component = player.GetComponent<QuestModule>();
		if (!component.IsQuestCompleted(51) && !component.CanCompleteQuest(51))
		{
			NpcDialog npcDialog = new NpcDialog("npc_aika_name", "npc_aika_need_finish_training_first_greet", new NpcChoice[]
			{
				new NpcChoice("npc_aika_choice_alright", 99999)
			});
			player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
			return;
		}
		this.npcModule.CloseDialog(player);
		MovementModule component2 = player.GetComponent<MovementModule>();
		if (component2.SpawnPoint == "old_training_area")
		{
			component2.SetSpawnPoint("ebuin_harbor", true);
		}
		component2.TargetTeleport(component2.connectionToClient, component2.SpawnPointLocation, new Effect("TeleporterHit", 0.5f, 0.25f));
	}

	// Token: 0x040015C2 RID: 5570
	private NpcModule npcModule;
}
