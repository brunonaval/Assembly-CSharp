using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200019F RID: 415
public class MasterworkChestItem : ItemBase
{
	// Token: 0x060004B7 RID: 1207 RVA: 0x0001A678 File Offset: 0x00018878
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

	// Token: 0x060004B8 RID: 1208 RVA: 0x0001A6B1 File Offset: 0x000188B1
	private bool ValidateIfUserHasEmptySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.EmptySlots < this.totalLoot)
		{
			effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0001A6DC File Offset: 0x000188DC
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

	// Token: 0x060004BA RID: 1210 RVA: 0x0001A884 File Offset: 0x00018A84
	private List<Item> GetRewardItems(ItemQuality[] itemQualities)
	{
		return (from i in ItemBase.ItemDatabaseModule.Items.ToList<Item>()
		where i.AllowDropFromChests & itemQualities.Contains(i.Quality) & i.Type != ItemType.Spellbook & i.Type != ItemType.Scroll
		select i).ToList<Item>();
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001A8C3 File Offset: 0x00018AC3
	private Rarity[] GetRewardRarities()
	{
		return new Rarity[]
		{
			Rarity.Common,
			Rarity.Uncommon,
			Rarity.Rare
		};
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001A8D6 File Offset: 0x00018AD6
	private ItemQuality[] GetRewardQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Basic,
			ItemQuality.Fine,
			ItemQuality.Masterwork
		};
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001A8E9 File Offset: 0x00018AE9
	private ItemQuality[] GetBlueprintQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Masterwork
		};
	}

	// Token: 0x040007A3 RID: 1955
	private readonly int minLevel = 31;

	// Token: 0x040007A4 RID: 1956
	private readonly int maxLevel = 50;

	// Token: 0x040007A5 RID: 1957
	private readonly int totalLoot = 4;

	// Token: 0x040007A6 RID: 1958
	private readonly int maxAmount = 7;
}
