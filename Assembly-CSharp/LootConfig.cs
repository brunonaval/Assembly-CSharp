using System;

// Token: 0x0200011A RID: 282
[Serializable]
public struct LootConfig
{
	// Token: 0x0600030A RID: 778 RVA: 0x0001403A File Offset: 0x0001223A
	public LootConfig(int itemId, int requiredLevel, Rarity rarity, int minAmount, int maxAmount, float dropChance)
	{
		this.ItemId = itemId;
		this.RequiredLevel = requiredLevel;
		this.Rarity = rarity;
		this.MinAmount = minAmount;
		this.MaxAmount = maxAmount;
		this.DropChance = dropChance;
	}

	// Token: 0x04000595 RID: 1429
	public int ItemId;

	// Token: 0x04000596 RID: 1430
	public int RequiredLevel;

	// Token: 0x04000597 RID: 1431
	public Rarity Rarity;

	// Token: 0x04000598 RID: 1432
	public int MinAmount;

	// Token: 0x04000599 RID: 1433
	public int MaxAmount;

	// Token: 0x0400059A RID: 1434
	public float DropChance;
}
