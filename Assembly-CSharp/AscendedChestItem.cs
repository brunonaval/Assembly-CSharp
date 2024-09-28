using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000174 RID: 372
public class AscendedChestItem : ItemBase
{
	// Token: 0x0600040C RID: 1036 RVA: 0x000180CC File Offset: 0x000162CC
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

	// Token: 0x0600040D RID: 1037 RVA: 0x00018105 File Offset: 0x00016305
	private bool ValidateIfUserHasEmptySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.EmptySlots < this.totalLoot)
		{
			effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00018130 File Offset: 0x00016330
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
					return;
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

	// Token: 0x0600040F RID: 1039 RVA: 0x000182D8 File Offset: 0x000164D8
	private List<Item> GetRewardItems(ItemQuality[] itemQualities)
	{
		return (from i in ItemBase.ItemDatabaseModule.Items.ToList<Item>()
		where i.AllowDropFromChests & itemQualities.Contains(i.Quality) & i.Type != ItemType.Spellbook & i.Type != ItemType.Food & i.Type != ItemType.Scroll
		select i).ToList<Item>();
	}

	// Token: 0x06000410 RID: 1040 RVA: 0x00018317 File Offset: 0x00016517
	private Rarity[] GetRewardRarities()
	{
		return new Rarity[]
		{
			Rarity.Common,
			Rarity.Uncommon,
			Rarity.Rare,
			Rarity.Exotic
		};
	}

	// Token: 0x06000411 RID: 1041 RVA: 0x0001832A File Offset: 0x0001652A
	private ItemQuality[] GetRewardQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Basic,
			ItemQuality.Fine,
			ItemQuality.Masterwork,
			ItemQuality.Ascended,
			ItemQuality.Epic
		};
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x0001833D File Offset: 0x0001653D
	private ItemQuality[] GetBlueprintQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Ascended
		};
	}

	// Token: 0x0400075D RID: 1885
	private readonly int minLevel = 51;

	// Token: 0x0400075E RID: 1886
	private readonly int maxLevel = 105;

	// Token: 0x0400075F RID: 1887
	private readonly int totalLoot = 5;

	// Token: 0x04000760 RID: 1888
	private readonly int maxAmount = 10;
}
