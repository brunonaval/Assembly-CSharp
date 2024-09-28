using System;

// Token: 0x02000026 RID: 38
[Serializable]
public class QuestRewardResource
{
	// Token: 0x0400005E RID: 94
	public int Id;

	// Token: 0x0400005F RID: 95
	public int QuestId;

	// Token: 0x04000060 RID: 96
	public RewardType RewardType;

	// Token: 0x04000061 RID: 97
	public Vocation Vocation;

	// Token: 0x04000062 RID: 98
	public int ItemId;

	// Token: 0x04000063 RID: 99
	public string ItemName;

	// Token: 0x04000064 RID: 100
	public int TitleId;

	// Token: 0x04000065 RID: 101
	public string TitleName;

	// Token: 0x04000066 RID: 102
	public int Amount;

	// Token: 0x04000067 RID: 103
	public int RequiredLevel;

	// Token: 0x04000068 RID: 104
	public Rarity Rarity;
}
