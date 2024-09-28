using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class VikingChestItem : ItemBase
{
	// Token: 0x06000444 RID: 1092 RVA: 0x00018C48 File Offset: 0x00016E48
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

	// Token: 0x06000445 RID: 1093 RVA: 0x00018C81 File Offset: 0x00016E81
	private bool ValidateIfUserHasEmptySlots(InventoryModule inventoryModule, EffectModule effectModule)
	{
		if (inventoryModule.EmptySlots < this.totalLoot)
		{
			effectModule.ShowScreenMessage("inventory_full_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00018CAC File Offset: 0x00016EAC
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

	// Token: 0x06000447 RID: 1095 RVA: 0x00018E54 File Offset: 0x00017054
	private List<Item> GetRewardItems(ItemQuality[] itemQualities)
	{
		IEnumerable<Item> first = (from i in ItemBase.ItemDatabaseModule.Items.ToList<Item>()
		where i.AllowDropFromChests & itemQualities.Contains(i.Quality) & i.Type != ItemType.Spellbook & i.Type != ItemType.Food & i.Type != ItemType.Scroll
		select i).ToList<Item>();
		IEnumerable<Item> second = from i in ItemBase.ItemDatabaseModule.Items
		where this.extendedItemIds.Contains(i.Id)
		select i;
		return first.Concat(second).Distinct<Item>().ToList<Item>();
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x00017FC3 File Offset: 0x000161C3
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

	// Token: 0x06000449 RID: 1097 RVA: 0x00017FD6 File Offset: 0x000161D6
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

	// Token: 0x0600044A RID: 1098 RVA: 0x00017FE9 File Offset: 0x000161E9
	private ItemQuality[] GetBlueprintQualities()
	{
		return new ItemQuality[]
		{
			ItemQuality.Epic,
			ItemQuality.Perfect,
			ItemQuality.Ancient
		};
	}

	// Token: 0x0400077D RID: 1917
	private readonly int[] extendedItemIds = new int[]
	{
		448,
		398,
		381,
		382,
		383,
		384,
		385,
		386,
		387
	};

	// Token: 0x0400077E RID: 1918
	private readonly int minLevel = 800;

	// Token: 0x0400077F RID: 1919
	private readonly int maxLevel = 850;

	// Token: 0x04000780 RID: 1920
	private readonly int totalLoot = 6;

	// Token: 0x04000781 RID: 1921
	private readonly int maxAmount = 30;
}
