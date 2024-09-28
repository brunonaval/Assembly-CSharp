using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class FineChestItem : ItemBase
{
	// Token: 0x06000436 RID: 1078 RVA: 0x00018984 File Offset: 0x00016B84
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

	// Token: 0x06000437 RID: 1079 RVA: 0x000189BD File Offset: 0x00016BBD
	private bool ValidateIfUserHasEmptySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.EmptySlots < this.totalLoot)
		{
			effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000438 RID: 1080 RVA: 0x000189E8 File Offset: 0x00016BE8
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

	// Token: 0x06000439 RID: 1081 RVA: 0x00018B90 File Offset: 0x00016D90
	private List<Item> GetRewardItems(ItemQuality[] itemQualities)
	{
		return (from i in ItemBase.ItemDatabaseModule.Items.ToList<Item>()
		where i.AllowDropFromChests & itemQualities.Contains(i.Quality)
		select i).ToList<Item>();
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00018BCF File Offset: 0x00016DCF
	private Rarity[] GetRewardRarities()
	{
		return new Rarity[]
		{
			Rarity.Common,
			Rarity.Uncommon
		};
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x00018BDF File Offset: 0x00016DDF
	private ItemQuality[] GetRewardQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Basic,
			ItemQuality.Fine
		};
	}

	// Token: 0x0600043C RID: 1084 RVA: 0x00018BEF File Offset: 0x00016DEF
	private ItemQuality[] GetBlueprintQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Fine
		};
	}

	// Token: 0x04000775 RID: 1909
	private readonly int minLevel = 21;

	// Token: 0x04000776 RID: 1910
	private readonly int maxLevel = 30;

	// Token: 0x04000777 RID: 1911
	private readonly int totalLoot = 3;

	// Token: 0x04000778 RID: 1912
	private readonly int maxAmount = 3;
}
