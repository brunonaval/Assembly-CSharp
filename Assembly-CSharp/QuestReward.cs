using System;

// Token: 0x0200013F RID: 319
public struct QuestReward
{
	// Token: 0x06000363 RID: 867 RVA: 0x00015954 File Offset: 0x00013B54
	public QuestReward(int id, int questId, RewardType rewardType, Vocation vocation, int itemId, string itemName, int titleId, string titleName, int amount, int requiredLevel, Rarity rarity)
	{
		this.Id = id;
		this.QuestId = questId;
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

	// Token: 0x0400067A RID: 1658
	public int Id;

	// Token: 0x0400067B RID: 1659
	public int QuestId;

	// Token: 0x0400067C RID: 1660
	public RewardType RewardType;

	// Token: 0x0400067D RID: 1661
	public Vocation Vocation;

	// Token: 0x0400067E RID: 1662
	public int ItemId;

	// Token: 0x0400067F RID: 1663
	public string ItemName;

	// Token: 0x04000680 RID: 1664
	public int TitleId;

	// Token: 0x04000681 RID: 1665
	public string TitleName;

	// Token: 0x04000682 RID: 1666
	public int Amount;

	// Token: 0x04000683 RID: 1667
	public int RequiredLevel;

	// Token: 0x04000684 RID: 1668
	public Rarity Rarity;
}
