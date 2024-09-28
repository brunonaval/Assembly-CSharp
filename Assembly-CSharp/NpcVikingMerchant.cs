using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class NpcVikingMerchant : MonoBehaviour
{
	// Token: 0x06001A47 RID: 6727 RVA: 0x00086C1C File Offset: 0x00084E1C
	private void Awake()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		GameObject.FindGameObjectWithTag("DatabaseManager").TryGetComponent<ItemDatabaseModule>(out this.itemDatabaseModule);
		base.TryGetComponent<NpcModule>(out this.npcModule);
		NpcDialog handshakeDialog = new NpcDialog("npc_viking_merchant_name", "npc_viking_merchant_handshake", new NpcChoice[]
		{
			new NpcChoice("npc_viking_merchant_store_choice", 1),
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		this.npcModule.SetHandshakeDialog(handshakeDialog);
		this.npcModule.AddAction(new NpcAction(1, new NpcAction.NpcTask(this.OpenStore)));
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x00086CBC File Offset: 0x00084EBC
	private void Start()
	{
		this.materials = (from i in this.itemDatabaseModule.Items
		where i.Category == ItemCategory.Material & i.Type == ItemType.Trinket & i.Rarity < Rarity.Exotic & i.Quality == ItemQuality.Basic
		orderby i.Rarity
		select i).ToList<Item>();
		using (List<Item>.Enumerator enumerator = this.materials.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Item material = enumerator.Current;
				this.npcModule.AddAction(new NpcAction(this.buyMaterialActionId + material.Id, delegate(GameObject player)
				{
					this.BuyMaterial(player, material);
				}));
			}
		}
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x00086DA8 File Offset: 0x00084FA8
	private void OpenStore(GameObject player)
	{
		PlayerModule playerModule;
		player.TryGetComponent<PlayerModule>(out playerModule);
		NpcDialog npcDialog = new NpcDialog("npc_viking_merchant_name", "npc_viking_merchant_store_dialog", Array.Empty<NpcChoice>());
		npcDialog.AddChoice(new NpcChoice("default_npc_cancel_choice", 99999));
		foreach (Item item in this.materials)
		{
			int num = NpcVikingMerchant.CalculateMaterialValue(item);
			NpcChoice choice = new NpcChoice(item.Name, " - ", string.Format("{0} ", num), "item_viking_coins", this.buyMaterialActionId + item.Id);
			npcDialog.AddChoice(choice);
		}
		npcDialog.AddChoice(new NpcChoice("default_npc_cancel_choice", 99999));
		playerModule.RenderNpcDialog(npcDialog);
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x00086E8C File Offset: 0x0008508C
	private static int CalculateMaterialValue(Item material)
	{
		int num = 750;
		if (material.Rarity == Rarity.Uncommon)
		{
			num *= 2;
		}
		if (material.Rarity == Rarity.Rare)
		{
			num *= 3;
		}
		return num;
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x00086EBC File Offset: 0x000850BC
	private void BuyMaterial(GameObject player, Item material)
	{
		PlayerModule component = player.GetComponent<PlayerModule>();
		InventoryModule component2 = player.GetComponent<InventoryModule>();
		NpcDialog npcDialog;
		if (component2.EmptySlots == 0)
		{
			npcDialog = new NpcDialog("npc_viking_merchant_name", "default_npc_not_enough_inventory_slots_dialog", new NpcChoice[]
			{
				new NpcChoice("default_npc_bye_choice", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		int amount = component2.GetAmount(this.vikingCoinItemId);
		int num = NpcVikingMerchant.CalculateMaterialValue(material);
		if (num > amount)
		{
			npcDialog = new NpcDialog("npc_viking_merchant_name", "default_npc_not_enough_money_dialog", new NpcChoice[]
			{
				new NpcChoice("default_npc_bye_choice", 99999)
			});
			component.RenderNpcDialog(npcDialog);
			return;
		}
		component2.ConsumeItem(this.vikingCoinItemId, num);
		component2.AddItem(material, -1, 750, true);
		npcDialog = new NpcDialog("npc_viking_merchant_name", "default_npc_thanks_for_purchase_dialog", new NpcChoice[]
		{
			new NpcChoice("default_npc_bye_choice", 99999)
		});
		component.RenderNpcDialog(npcDialog);
	}

	// Token: 0x04001670 RID: 5744
	private readonly int vikingCoinItemId = 448;

	// Token: 0x04001671 RID: 5745
	private readonly int buyMaterialActionId = 10000;

	// Token: 0x04001672 RID: 5746
	private List<Item> materials;

	// Token: 0x04001673 RID: 5747
	private NpcModule npcModule;

	// Token: 0x04001674 RID: 5748
	private ItemDatabaseModule itemDatabaseModule;
}
