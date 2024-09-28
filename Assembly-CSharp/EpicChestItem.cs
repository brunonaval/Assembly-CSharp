using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200017A RID: 378
public class EpicChestItem : ItemBase
{
	// Token: 0x06000428 RID: 1064 RVA: 0x0001867C File Offset: 0x0001687C
	public override bool UseItem(ItemBaseConfig itemBaseConfig)
	{
		EffectModule component = itemBaseConfig.UserObject.GetComponent<EffectModule>();
		InventoryModule component2 = itemBaseConfig.UserObject.GetComponent<InventoryModule>();
		if (!this.ValidateIfUserHasEmptySlots(component2, component))
		{
			return false;
		}
		this.AddRewardsToUser(component2);
		return true;
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x000186B5 File Offset: 0x000168B5
	private bool ValidateIfUserHasEmptySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.EmptySlots < this.totalLoot)
		{
			effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x000186E0 File Offset: 0x000168E0
	private void AddRewardsToUser(InventoryModule inventoryModule)
	{
		Rarity[] rewardRarities = this.GetRewardRarities();
		ItemQuality[] rewardQualities = this.GetRewardQualities();
		List<Item> list = this.GetRewardItems(rewardQualities);
		ItemQuality[] blueprintQualities = this.GetBlueprintQualities();
		int j = 0;
		bool flag = false;
		bool flag2 = false;
		while (j < this.totalLoot)
		{
			if (flag)
			{
				list = (from i in list
				where i.Type != ItemType.Blueprint
				select i).ToList<Item>();
			}
			if (flag2)
			{
				list = (from i in list
				where i.Category != ItemCategory.Armor & i.Category != ItemCategory.Weapon & i.Category != ItemCategory.Acessory & i.Category != ItemCategory.Projectile
				select i).ToList<Item>();
			}
			int index = UnityEngine.Random.Range(0, list.Count);
			Item item = list[index];
			if (item.Type == ItemType.Blueprint)
			{
				if (flag | !blueprintQualities.Contains(item.Quality))
				{
					continue;
				}
				flag = true;
			}
			if (item.Category == ItemCategory.Armor | item.Category == ItemCategory.Weapon | item.Category == ItemCategory.Acessory | item.Category == ItemCategory.Projectile)
			{
				if (flag2)
				{
					continue;
				}
				int num = UnityEngine.Random.Range(0, rewardRarities.Length);
				item.Rarity = rewardRarities[num];
				item.RequiredLevel = UnityEngine.Random.Range(this.minLevel, this.maxLevel + 1);
				flag2 = true;
			}
			int amount = 1;
			if (item.Stackable & item.Type != ItemType.Blueprint & item.Type != ItemType.Reagent)
			{
				amount = UnityEngine.Random.Range(1, this.maxAmount + 1);
			}
			if (item.Category == ItemCategory.Material)
			{
				item.RequiredLevel = 1;
			}
			inventoryModule.AddItem(item, -1, amount, true);
			j++;
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x00018888 File Offset: 0x00016A88
	private List<Item> GetRewardItems(ItemQuality[] itemQualities)
	{
		return (from i in ItemBase.ItemDatabaseModule.Items.ToList<Item>()
		where i.AllowDropFromChests & itemQualities.Contains(i.Quality) & i.Type != ItemType.Spellbook & i.Type != ItemType.Food & i.Type != ItemType.Scroll
		select i).ToList<Item>();
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x000188C7 File Offset: 0x00016AC7
	private Rarity[] GetRewardRarities()
	{
		return new Rarity[]
		{
			Rarity.Common,
			Rarity.Uncommon,
			Rarity.Rare,
			Rarity.Exotic,
			Rarity.Legendary
		};
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x000188DA File Offset: 0x00016ADA
	private ItemQuality[] GetRewardQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Basic,
			ItemQuality.Fine,
			ItemQuality.Masterwork,
			ItemQuality.Ascended,
			ItemQuality.Epic,
			ItemQuality.Perfect
		};
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x000188ED File Offset: 0x00016AED
	private ItemQuality[] GetBlueprintQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Epic,
			ItemQuality.Perfect
		};
	}

	// Token: 0x0400076D RID: 1901
	private readonly int minLevel = 106;

	// Token: 0x0400076E RID: 1902
	private readonly int maxLevel = 200;

	// Token: 0x0400076F RID: 1903
	private readonly int totalLoot = 6;

	// Token: 0x04000770 RID: 1904
	private readonly int maxAmount = 30;
}
