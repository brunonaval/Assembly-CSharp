using System;

// Token: 0x02000135 RID: 309
public class PossibleLoot
{
	// Token: 0x06000342 RID: 834 RVA: 0x00015279 File Offset: 0x00013479
	public PossibleLoot(Item item, int minAmount, int maxAmount, float dropChance)
	{
		this.Item = item;
		this.MinAmount = minAmount;
		this.MaxAmount = maxAmount;
		this.DropChance = dropChance;
	}

	// Token: 0x04000639 RID: 1593
	public Item Item;

	// Token: 0x0400063A RID: 1594
	public int MinAmount;

	// Token: 0x0400063B RID: 1595
	public int MaxAmount;

	// Token: 0x0400063C RID: 1596
	public float DropChance;
}
