using System;

// Token: 0x02000132 RID: 306
public struct PlayerQuestReward
{
	// Token: 0x0600033B RID: 827 RVA: 0x000151D0 File Offset: 0x000133D0
	public PlayerQuestReward(int id, int playerQuestId, RewardType rewardType, Vocation vocation, int itemId, string itemName, int titleId, string titleName, int amount, int requiredLevel, Rarity rarity)
	{
		this.Id = id;
		this.PlayerQuestId = playerQuestId;
		this.RewardType = rewardType;
		this.Vocation = vocation;
		this.ItemId = itemId;
		this.ItemName = itemName;
		this.TitleId = titleId;
		this.TitleName = titleName;
		this.Amount = amount;
		this.RequiredLevel = requiredLevel;
		this.Rarity = rarity;
	}

	// Token: 0x04000628 RID: 1576
	public int Id;

	// Token: 0x04000629 RID: 1577
	public int PlayerQuestId;

	// Token: 0x0400062A RID: 1578
	public RewardType RewardType;

	// Token: 0x0400062B RID: 1579
	public Vocation Vocation;

	// Token: 0x0400062C RID: 1580
	public int ItemId;

	// Token: 0x0400062D RID: 1581
	public string ItemName;

	// Token: 0x0400062E RID: 1582
	public int TitleId;

	// Token: 0x0400062F RID: 1583
	public string TitleName;

	// Token: 0x04000630 RID: 1584
	public int Amount;

	// Token: 0x04000631 RID: 1585
	public int RequiredLevel;

	// Token: 0x04000632 RID: 1586
	public Rarity Rarity;
}
