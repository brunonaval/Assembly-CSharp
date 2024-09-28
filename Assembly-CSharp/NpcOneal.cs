using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class NpcOneal : MonoBehaviour
{
	// Token: 0x060019DF RID: 6623 RVA: 0x000842D4 File Offset: 0x000824D4
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_oneal_name", "npc_oneal_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_oneal_choice_buy_items", 1),
			new NpcChoice("npc_oneal_choice_sell_items", 2),
			new NpcChoice("npc_oneal_choice_stay_in_sanctuary", 3),
			new NpcChoice("npc_oneal_choice_back_to_ebuin", 4),
			new NpcChoice("npc_oneal_choice_quests", 88888),
			new NpcChoice("npc_oneal_choice_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenBuyStore)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.OpenSellStore)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.StayInSanctuary)));
		this.npcModule.AddAction(new NpcAction(4, new NpcAction.NpcTask(this.BackToEbuin)));
		this.npcModule.AddAction(new NpcAction(5, new NpcAction.NpcTask(this.BackToEbuinConfirmed)));
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x00084434 File Offset: 0x00082634
	public void OpenBuyStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		AttributeModule component = player.GetComponent<AttributeModule>();
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		Item item = this.itemDatabaseModule.GetItem(967);
		list.Add(new StoreItemConfig(this.npcModule.NpcId, item));
		List<Item> list2 = (from i in this.itemDatabaseModule.Items
		where i.Quality == ItemQuality.Basic & (i.Category == ItemCategory.Armor | i.Category == ItemCategory.Weapon)
		select i).ToList<Item>();
		for (int j = 0; j < list2.Count; j++)
		{
			Item value = list2[j];
			value.RequiredLevel = component.BaseLevel;
			list2[j] = value;
		}
		list = list.Union(from s in list2
		select new StoreItemConfig(this.npcModule.NpcId, s)).ToList<StoreItemConfig>();
		NpcDialog npcDialog = new NpcDialog("npc_oneal_buy_store_title", "npc_oneal_buy_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_oneal_choice_bye", 99999),
			new NpcChoice("npc_oneal_choice_sell_items", 2)
		})
		{
			StoreAction = StoreAction.Buy
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x00084568 File Offset: 0x00082768
	public void OpenSellStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		Item[] uniqueSellableItems = player.GetComponent<InventoryModule>().GetUniqueSellableItems();
		List<StoreItemConfig> list = new List<StoreItemConfig>();
		foreach (Item item in uniqueSellableItems)
		{
			if (item.Amount > 0)
			{
				list.Add(new StoreItemConfig(this.npcModule.NpcId, item));
			}
		}
		NpcDialog npcDialog = new NpcDialog("npc_oneal_sell_store_title", "npc_oneal_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_oneal_choice_bye", 99999),
			new NpcChoice("npc_oneal_choice_buy_items", 1)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x00084624 File Offset: 0x00082824
	public void StayInSanctuary(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		EffectModule component = player.GetComponent<EffectModule>();
		WalletModule component2 = player.GetComponent<WalletModule>();
		MovementModule component3 = player.GetComponent<MovementModule>();
		if (component3.SpawnPoint == "dark_sanctuary")
		{
			component.ShowScreenMessage("spawn_point_already_defined_here_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (component2.GoldCoins < (long)this.defineSpawnPoint)
		{
			component.ShowScreenMessage("not_enough_money_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		component2.AddGoldCoins(-this.defineSpawnPoint);
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "Magic",
			EffectScaleModifier = 0.5f,
			EffectSpeedModifier = 0.25f,
			SoundEffectName = "spellbook"
		};
		component.ShowEffects(effectConfig);
		component3.SetSpawnPoint("dark_sanctuary", true);
	}

	// Token: 0x060019E3 RID: 6627 RVA: 0x000846F8 File Offset: 0x000828F8
	private void BackToEbuin(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		NpcDialog npcDialog = new NpcDialog("npc_oneal_name", "npc_oneal_back_to_ebuin_greet", new NpcChoice[]
		{
			new NpcChoice("npc_oneal_choice_yes", 5),
			new NpcChoice("npc_oneal_choice_no", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x060019E4 RID: 6628 RVA: 0x00084758 File Offset: 0x00082958
	private void BackToEbuinConfirmed(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		MovementModule component = player.GetComponent<MovementModule>();
		this.npcModule.CloseDialog(player);
		Effect teleportEffect = new Effect("TeleporterHit", 0.5f, 0.25f);
		Vector3 locationFromSpawnPoint = GlobalUtils.GetLocationFromSpawnPoint("ebuin_town");
		component.TargetTeleport(component.connectionToClient, locationFromSpawnPoint, teleportEffect);
	}

	// Token: 0x0400165C RID: 5724
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x0400165D RID: 5725
	private NpcModule npcModule;

	// Token: 0x0400165E RID: 5726
	private readonly int defineSpawnPoint = 80000;
}
