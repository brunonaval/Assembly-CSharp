using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000171 RID: 369
public class AncientChestItem : ItemBase
{
	// Token: 0x060003FE RID: 1022 RVA: 0x00017D78 File Offset: 0x00015F78
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

	// Token: 0x060003FF RID: 1023 RVA: 0x00017DB1 File Offset: 0x00015FB1
	private bool ValidateIfUserHasEmptySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.EmptySlots < this.totalLoot)
		{
			effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x00017DDC File Offset: 0x00015FDC
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

	// Token: 0x06000401 RID: 1025 RVA: 0x00017F84 File Offset: 0x00016184
	private List<Item> GetRewardItems(ItemQuality[] itemQualities)
	{
		return (from i in ItemBase.ItemDatabaseModule.Items.ToList<Item>()
		where i.AllowDropFromChests & itemQualities.Contains(i.Quality) & i.Type != ItemType.Spellbook & i.Type != ItemType.Food & i.Type != ItemType.Scroll
		select i).ToList<Item>();
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x00017FC3 File Offset: 0x000161C3
	private Rarity[] GetRewardRarities()
	{
		return new Rarity[]
		{
			Rarity.Common,
			Rarity.Uncommon,
			Rarity.Rare,
			Rarity.Exotic,
			Rarity.Legendary,
			Rarity.Divine
		};
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00017FD6 File Offset: 0x000161D6
	private ItemQuality[] GetRewardQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Basic,
			ItemQuality.Fine,
			ItemQuality.Masterwork,
			ItemQuality.Ascended,
			ItemQuality.Epic,
			ItemQuality.Perfect,
			ItemQuality.Ancient
		};
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00017FE9 File Offset: 0x000161E9
	private ItemQuality[] GetBlueprintQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Epic,
			ItemQuality.Perfect,
			ItemQuality.Ancient
		};
	}

	// Token: 0x04000755 RID: 1877
	private readonly int minLevel = 201;

	// Token: 0x04000756 RID: 1878
	private readonly int maxLevel = 400;

	// Token: 0x04000757 RID: 1879
	private readonly int totalLoot = 6;

	// Token: 0x04000758 RID: 1880
	private readonly int maxAmount = 30;
}
