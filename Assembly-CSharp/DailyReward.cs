using System;

// Token: 0x02000107 RID: 263
public struct DailyReward
{
	// Token: 0x060002AA RID: 682 RVA: 0x00012607 File Offset: 0x00010807
	public DailyReward(int id, int amount, Item item)
	{
		this.Id = id;
		this.Amount = amount;
		this.Item = item;
	}

	// Token: 0x04000502 RID: 1282
	public int Id;

	// Token: 0x04000503 RID: 1283
	public Item Item;

	// Token: 0x04000504 RID: 1284
	public int Amount;
}
