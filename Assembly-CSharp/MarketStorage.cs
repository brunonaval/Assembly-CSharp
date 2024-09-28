using System;

// Token: 0x0200011C RID: 284
public struct MarketStorage
{
	// Token: 0x1700005E RID: 94
	// (get) Token: 0x0600030F RID: 783 RVA: 0x000140FF File Offset: 0x000122FF
	public bool IsDefined
	{
		get
		{
			return this.AccountId > 0 & this.Item.IsDefined & this.Amount > 0;
		}
	}

	// Token: 0x06000310 RID: 784 RVA: 0x00014120 File Offset: 0x00012320
	public MarketStorage(int id, int accountId, Item item, int amount)
	{
		this.Id = id;
		this.AccountId = accountId;
		this.Item = item;
		this.Amount = amount;
	}

	// Token: 0x040005A2 RID: 1442
	public int Id;

	// Token: 0x040005A3 RID: 1443
	public int AccountId;

	// Token: 0x040005A4 RID: 1444
	public int Amount;

	// Token: 0x040005A5 RID: 1445
	public Item Item;
}
