using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Mirror;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class NpcHeroldIronhead : MonoBehaviour
{
	// Token: 0x06001916 RID: 6422 RVA: 0x0007F438 File Offset: 0x0007D638
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("DatabaseManager");
		this.itemDatabaseModule = gameObject.GetComponent<ItemDatabaseModule>();
		this.blueprintDatabaseModule = gameObject.GetComponent<BlueprintDatabaseModule>();
		this.npcModule = base.GetComponent<NpcModule>();
		NpcDialog handshakeDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession", 1),
			new NpcChoice("npc_herold_ironhead_choice_craft", 9),
			new NpcChoice("npc_herold_ironhead_choice_basic_blueprints", 8),
			new NpcChoice("npc_herold_ironhead_choice_sell_items", 10),
			new NpcChoice("npc_herold_ironhead_choice_forget_profession", 11),
			new NpcChoice("npc_herold_ironhead_change_profession", 13),
			new NpcChoice("npc_herold_ironhead_nothing", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.Profession)));
		this.npcModule.AddAction(new NpcAction(2, new NpcAction.NpcTask(this.Alchemist)));
		this.npcModule.AddAction(new NpcAction(3, new NpcAction.NpcTask(this.Armorsmith)));
		this.npcModule.AddAction(new NpcAction(4, new NpcAction.NpcTask(this.Weaponsmith)));
		this.npcModule.AddAction(new NpcAction(5, new NpcAction.NpcTask(this.AlchemistConfirmed)));
		this.npcModule.AddAction(new NpcAction(6, new NpcAction.NpcTask(this.ArmorsmithConfirmed)));
		this.npcModule.AddAction(new NpcAction(7, new NpcAction.NpcTask(this.WeaponsmithConfirmed)));
		this.npcModule.AddAction(new NpcAction(8, new NpcAction.NpcTask(this.BasicBlueprintStore)));
		this.npcModule.AddAction(new NpcAction(9, new NpcAction.NpcTask(this.OpenCraft)));
		this.npcModule.AddAction(new NpcAction(10, new NpcAction.NpcTask(this.OpenSellStore)));
		this.npcModule.AddAction(new NpcAction(11, new NpcAction.NpcTask(this.ForgetProfession)));
		this.npcModule.AddAction(new NpcAction(12, new NpcAction.NpcTask(this.ForgetProfessionConfirmed)));
		this.npcModule.AddAction(new NpcAction(13, new NpcAction.NpcTask(this.ChangeProfession)));
		this.npcModule.AddAction(new NpcAction(14, new NpcAction.NpcTask(this.ChangeProfessionAlchemist)));
		this.npcModule.AddAction(new NpcAction(15, new NpcAction.NpcTask(this.ChangeProfessionAlchemistConfirmed)));
		this.npcModule.AddAction(new NpcAction(16, new NpcAction.NpcTask(this.ChangeProfessionWeaponsmith)));
		this.npcModule.AddAction(new NpcAction(17, new NpcAction.NpcTask(this.ChangeProfessionWeaponsmithConfirmed)));
		this.npcModule.AddAction(new NpcAction(18, new NpcAction.NpcTask(this.ChangeProfessionArmorsmith)));
		this.npcModule.AddAction(new NpcAction(19, new NpcAction.NpcTask(this.ChangeProfessionArmorsmithConfirmed)));
	}

	// Token: 0x06001917 RID: 6423 RVA: 0x0007F758 File Offset: 0x0007D958
	private void Profession(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		AttributeModule component2 = player.GetComponent<AttributeModule>();
		if (component2.Profession != PlayerProfession.None)
		{
			NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_you_already_have_profession", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_sorry", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		if (component2.BaseLevel < 10)
		{
			NpcDialog npcDialog2 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_level_too_low", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_ok", 99999)
			});
			component.RenderNpcDialog(npcDialog2);
			return;
		}
		NpcDialog npcDialog3 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_choose_profession", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_alchemist", 2),
			new NpcChoice("npc_herold_ironhead_choice_armorsmith", 3),
			new NpcChoice("npc_herold_ironhead_choice_weaponsmith", 4),
			new NpcChoice("npc_herold_ironhead_choice_dont_know", 99999)
		});
		component.RenderNpcDialog(npcDialog3);
	}

	// Token: 0x06001918 RID: 6424 RVA: 0x0007F864 File Offset: 0x0007DA64
	private void Alchemist(GameObject player)
	{
		NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_confirm_alchemist", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 5),
			new NpcChoice("npc_herold_ironhead_choice_not_sure", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001919 RID: 6425 RVA: 0x0007F8BC File Offset: 0x0007DABC
	private void AlchemistConfirmed(GameObject player)
	{
		this.npcModule.CloseDialog(player);
		player.GetComponent<AttributeModule>().SetProfession(PlayerProfession.Alchemist);
		EffectModule component = player.GetComponent<EffectModule>();
		component.ShowScreenMessage("player_got_alchemist_profession_message", 1, 3.5f, Array.Empty<string>());
		this.ShowStarBlastEffect(component);
	}

	// Token: 0x0600191A RID: 6426 RVA: 0x0007F908 File Offset: 0x0007DB08
	private void Armorsmith(GameObject player)
	{
		NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_confirm_armorsmith", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 6),
			new NpcChoice("npc_herold_ironhead_choice_not_sure", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x0600191B RID: 6427 RVA: 0x0007F960 File Offset: 0x0007DB60
	private void ArmorsmithConfirmed(GameObject player)
	{
		this.npcModule.CloseDialog(player);
		player.GetComponent<AttributeModule>().SetProfession(PlayerProfession.Armorsmith);
		EffectModule component = player.GetComponent<EffectModule>();
		component.ShowScreenMessage("player_got_armorsmith_profession_message", 1, 3.5f, Array.Empty<string>());
		this.ShowStarBlastEffect(component);
	}

	// Token: 0x0600191C RID: 6428 RVA: 0x0007F9AC File Offset: 0x0007DBAC
	private void Weaponsmith(GameObject player)
	{
		NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_confirm_weaponsmith", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 7),
			new NpcChoice("npc_herold_ironhead_choice_not_sure", 99999)
		});
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x0600191D RID: 6429 RVA: 0x0007FA04 File Offset: 0x0007DC04
	private void WeaponsmithConfirmed(GameObject player)
	{
		this.npcModule.CloseDialog(player);
		player.GetComponent<AttributeModule>().SetProfession(PlayerProfession.Weaponsmith);
		EffectModule component = player.GetComponent<EffectModule>();
		component.ShowScreenMessage("player_got_weaponsmith_profession_message", 1, 3.5f, Array.Empty<string>());
		this.ShowStarBlastEffect(component);
	}

	// Token: 0x0600191E RID: 6430 RVA: 0x0007FA50 File Offset: 0x0007DC50
	private void ShowStarBlastEffect(EffectModule playerEffectModule)
	{
		EffectConfig effectConfig = new EffectConfig
		{
			EffectName = "StarBlast",
			EffectScaleModifier = 2f,
			EffectSpeedModifier = 0.2f,
			SoundEffectName = "magic_explosion"
		};
		playerEffectModule.ShowEffects(effectConfig);
	}

	// Token: 0x0600191F RID: 6431 RVA: 0x0007FAA0 File Offset: 0x0007DCA0
	private void BasicBlueprintStore(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		StoreItemConfig[] array = (from i in this.itemDatabaseModule.Items
		where i.Type == ItemType.Blueprint & !i.AllowDropByLevel & i.Quality == ItemQuality.Basic
		select i into si
		orderby si.RequiredLevel, si.Value
		select si into s
		select new StoreItemConfig(this.npcModule.NpcId, s, this.GetBlueprintValue(s.Rarity))).ToArray<StoreItemConfig>();
		AttributeModule component2 = player.GetComponent<AttributeModule>();
		List<Blueprint> blueprints = this.blueprintDatabaseModule.GetBlueprintsByProfession(component2.Profession);
		array = (from si in array
		where blueprints.Any((Blueprint b) => b.Id == si.Item.BlueprintId)
		select si).ToArray<StoreItemConfig>();
		NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_blueprint_store_greet", array, new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_thanks", 99999)
		})
		{
			StoreAction = StoreAction.Buy
		};
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x0007FBCF File Offset: 0x0007DDCF
	private int GetBlueprintValue(Rarity rarity)
	{
		switch (rarity)
		{
		case Rarity.Uncommon:
			return 1000;
		case Rarity.Rare:
			return 20000;
		case Rarity.Exotic:
			return 70000;
		default:
			return 50;
		}
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x0007C472 File Offset: 0x0007A672
	private void OpenCraft(GameObject player)
	{
		player.GetComponent<PlayerModule>();
		CraftModule component = player.GetComponent<CraftModule>();
		component.TargetShowCraftWindow(component.connectionToClient);
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x0007FBFC File Offset: 0x0007DDFC
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
		NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_sell_store_title", "npc_herold_ironhead_sell_store_greet", list.ToArray(), new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_bye", 99999),
			new NpcChoice("npc_herold_ironhead_choice_basic_blueprints", 8)
		})
		{
			StoreAction = StoreAction.Sell
		};
		player.GetComponent<PlayerModule>().RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x0007FCB8 File Offset: 0x0007DEB8
	private void ForgetProfession(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (player.GetComponent<AttributeModule>().Profession == PlayerProfession.None)
		{
			NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_dont_have_profession_message", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_sorry", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		NpcDialog npcDialog2 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_forget_profession_message", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 12),
			new NpcChoice("npc_herold_ironhead_choice_dont_know", 99999)
		});
		component.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x0007FD60 File Offset: 0x0007DF60
	private void ForgetProfessionConfirmed(GameObject player)
	{
		NpcHeroldIronhead.<ForgetProfessionConfirmed>d__19 <ForgetProfessionConfirmed>d__;
		<ForgetProfessionConfirmed>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ForgetProfessionConfirmed>d__.<>4__this = this;
		<ForgetProfessionConfirmed>d__.player = player;
		<ForgetProfessionConfirmed>d__.<>1__state = -1;
		<ForgetProfessionConfirmed>d__.<>t__builder.Start<NpcHeroldIronhead.<ForgetProfessionConfirmed>d__19>(ref <ForgetProfessionConfirmed>d__);
	}

	// Token: 0x06001925 RID: 6437 RVA: 0x0007FDA0 File Offset: 0x0007DFA0
	private void ChangeProfession(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (player.GetComponent<AttributeModule>().Profession == PlayerProfession.None)
		{
			NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_dont_have_profession_message", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_sorry", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		NpcDialog npcDialog2 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_change_profession_message", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_change_alchemist", 14),
			new NpcChoice("npc_herold_ironhead_choice_change_weaponsmith", 16),
			new NpcChoice("npc_herold_ironhead_choice_change_armorsmith", 18),
			new NpcChoice("npc_herold_ironhead_choice_dont_know", 99999)
		});
		component.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x06001926 RID: 6438 RVA: 0x0007FE6C File Offset: 0x0007E06C
	private void ChangeProfessionAlchemist(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (player.GetComponent<AttributeModule>().Profession == PlayerProfession.Alchemist)
		{
			NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_cant_change_same_profession_message", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_ok", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		NpcDialog npcDialog2 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_change_profession_alchemist_message", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 15),
			new NpcChoice("npc_herold_ironhead_choice_dont_know", 99999)
		});
		component.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x06001927 RID: 6439 RVA: 0x0007FF14 File Offset: 0x0007E114
	private void ChangeProfessionAlchemistConfirmed(GameObject player)
	{
		NpcHeroldIronhead.<ChangeProfessionAlchemistConfirmed>d__22 <ChangeProfessionAlchemistConfirmed>d__;
		<ChangeProfessionAlchemistConfirmed>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ChangeProfessionAlchemistConfirmed>d__.<>4__this = this;
		<ChangeProfessionAlchemistConfirmed>d__.player = player;
		<ChangeProfessionAlchemistConfirmed>d__.<>1__state = -1;
		<ChangeProfessionAlchemistConfirmed>d__.<>t__builder.Start<NpcHeroldIronhead.<ChangeProfessionAlchemistConfirmed>d__22>(ref <ChangeProfessionAlchemistConfirmed>d__);
	}

	// Token: 0x06001928 RID: 6440 RVA: 0x0007FF54 File Offset: 0x0007E154
	private void ChangeProfessionWeaponsmith(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (player.GetComponent<AttributeModule>().Profession == PlayerProfession.Weaponsmith)
		{
			NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_cant_change_same_profession_message", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_ok", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		NpcDialog npcDialog2 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_change_profession_weaponsmith_message", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 17),
			new NpcChoice("npc_herold_ironhead_choice_dont_know", 99999)
		});
		component.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x06001929 RID: 6441 RVA: 0x0007FFFC File Offset: 0x0007E1FC
	private void ChangeProfessionWeaponsmithConfirmed(GameObject player)
	{
		NpcHeroldIronhead.<ChangeProfessionWeaponsmithConfirmed>d__24 <ChangeProfessionWeaponsmithConfirmed>d__;
		<ChangeProfessionWeaponsmithConfirmed>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ChangeProfessionWeaponsmithConfirmed>d__.<>4__this = this;
		<ChangeProfessionWeaponsmithConfirmed>d__.player = player;
		<ChangeProfessionWeaponsmithConfirmed>d__.<>1__state = -1;
		<ChangeProfessionWeaponsmithConfirmed>d__.<>t__builder.Start<NpcHeroldIronhead.<ChangeProfessionWeaponsmithConfirmed>d__24>(ref <ChangeProfessionWeaponsmithConfirmed>d__);
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x0008003C File Offset: 0x0007E23C
	private void ChangeProfessionArmorsmith(GameObject player)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		PlayerModule component = player.GetComponent<PlayerModule>();
		if (player.GetComponent<AttributeModule>().Profession == PlayerProfession.Armorsmith)
		{
			NpcDialog npcDialog = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_cant_change_same_profession_message", new NpcChoice[]
			{
				new NpcChoice("npc_herold_ironhead_choice_ok", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		NpcDialog npcDialog2 = new NpcDialog("npc_herold_ironhead_name", "npc_herold_ironhead_change_profession_armorsmith_message", new NpcChoice[]
		{
			new NpcChoice("npc_herold_ironhead_choice_profession_confirmed", 19),
			new NpcChoice("npc_herold_ironhead_choice_dont_know", 99999)
		});
		component.RenderNpcDialog(npcDialog2);
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x000800E4 File Offset: 0x0007E2E4
	private void ChangeProfessionArmorsmithConfirmed(GameObject player)
	{
		NpcHeroldIronhead.<ChangeProfessionArmorsmithConfirmed>d__26 <ChangeProfessionArmorsmithConfirmed>d__;
		<ChangeProfessionArmorsmithConfirmed>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<ChangeProfessionArmorsmithConfirmed>d__.<>4__this = this;
		<ChangeProfessionArmorsmithConfirmed>d__.player = player;
		<ChangeProfessionArmorsmithConfirmed>d__.<>1__state = -1;
		<ChangeProfessionArmorsmithConfirmed>d__.<>t__builder.Start<NpcHeroldIronhead.<ChangeProfessionArmorsmithConfirmed>d__26>(ref <ChangeProfessionArmorsmithConfirmed>d__);
	}

	// Token: 0x040015FB RID: 5627
	private NpcModule npcModule;

	// Token: 0x040015FC RID: 5628
	private ItemDatabaseModule itemDatabaseModule;

	// Token: 0x040015FD RID: 5629
	private BlueprintDatabaseModule blueprintDatabaseModule;

	// Token: 0x040015FE RID: 5630
	private readonly int forgetProfessionPrice = 50000;

	// Token: 0x040015FF RID: 5631
	private readonly int changeProfessionPrice = 15;
}
