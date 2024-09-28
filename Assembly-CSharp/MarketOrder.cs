using System;

// Token: 0x0200011B RID: 283
public struct MarketOrder
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600030B RID: 779 RVA: 0x00014069 File Offset: 0x00012269
	public long TotalValue
	{
		get
		{
			return (long)(this.UnitValue * this.Amount);
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x0600030C RID: 780 RVA: 0x00014079 File Offset: 0x00012279
	public bool IsDefined
	{
		get
		{
			return this.Item.Id > 0 & this.Amount > 0;
		}
	}

	// Token: 0x0600030D RID: 781 RVA: 0x00014093 File Offset: 0x00012293
	public MarketOrder(int id, Item item, int amount, int unitValue, string itemName)
	{
		this.Id = id;
		this.Item = item;
		this.Amount = amount;
		this.UnitValue = unitValue;
		this.ItemName = itemName;
		this.PlayerId = 0;
		this.SellerName = null;
	}

	// Token: 0x0600030E RID: 782 RVA: 0x000140C8 File Offset: 0x000122C8
	public MarketOrder(int id, Item item, int amount, int playerId, int unitValue, string itemName, string sellerName)
	{
		this.Id = id;
		this.Item = item;
		this.Amount = amount;
		this.PlayerId = playerId;
		this.UnitValue = unitValue;
		this.ItemName = itemName;
		this.SellerName = sellerName;
	}

	// Token: 0x0400059B RID: 1435
	public int Id;

	// Token: 0x0400059C RID: 1436
	public Item Item;

	// Token: 0x0400059D RID: 1437
	public int Amount;

	// Token: 0x0400059E RID: 1438
	public int PlayerId;

	// Token: 0x0400059F RID: 1439
	public int UnitValue;

	// Token: 0x040005A0 RID: 1440
	public string ItemName;

	// Token: 0x040005A1 RID: 1441
	public string SellerName;
}
