using System;
using Mirror;
using UnityEngine;

// Token: 0x02000452 RID: 1106
public class NpcBella : MonoBehaviour
{
	// Token: 0x060018D6 RID: 6358 RVA: 0x0007D648 File Offset: 0x0007B848
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_bella_name", "npc_bella_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_bella_choice_citizen", 1),
			new NpcChoice("npc_bella_choice_quests", 88888),
			new NpcChoice("npc_bella_choice_bye", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.BecomeCitizen)));
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x0007D6E8 File Offset: 0x0007B8E8
	private void BecomeCitizen(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		EffectModule component = player.GetComponent<EffectModule>();
		WalletModule component2 = player.GetComponent<WalletModule>();
		MovementModule component3 = player.GetComponent<MovementModule>();
		if (component3.SpawnPoint == "ebuin_town")
		{
			component.ShowScreenMessage("ebuin_town_already_citizen_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (component2.GoldCoins < (long)this.becomeCitizenPrice)
		{
			component.ShowScreenMessage("citizen_ebuin_town_gold_coins_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		component2.AddGoldCoins(-this.becomeCitizenPrice);
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		component.ShowEffects(effectConfig);
		component3.SetSpawnPoint("ebuin_town", true);
		component.ShowScreenMessage("citizen_ebuin_town_message", 1, 3.5f, Array.Empty<string>());
	}

	// Token: 0x040015C7 RID: 5575
	private NpcModule npcModule;

	// Token: 0x040015C8 RID: 5576
	private readonly int becomeCitizenPrice = 1000;
}
