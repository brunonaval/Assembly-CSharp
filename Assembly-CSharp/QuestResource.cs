using System;

// Token: 0x02000027 RID: 39
[Serializable]
public class QuestResource
{
	// Token: 0x04000069 RID: 105
	public int Id;

	// Token: 0x0400006A RID: 106
	public string Name;

	// Token: 0x0400006B RID: 107
	public int RequiredLevel;

	// Token: 0x0400006C RID: 108
	public string Description;

	// Token: 0x0400006D RID: 109
	public int NpcId;

	// Token: 0x0400006E RID: 110
	public Rank Rank;

	// Token: 0x0400006F RID: 111
	public bool IsDailyTask;

	// Token: 0x04000070 RID: 112
	public string PositiveChoice;

	// Token: 0x04000071 RID: 113
	public string NegativeChoice;

	// Token: 0x04000072 RID: 114
	public string CompletedDialog;

	// Token: 0x04000073 RID: 115
	public string CompletedChoice;

	// Token: 0x04000074 RID: 116
	public string InProgressDialog;

	// Token: 0x04000075 RID: 117
	public string InProgressChoice;

	// Token: 0x04000076 RID: 118
	public QuestRewardResource[] Rewards;

	// Token: 0x04000077 RID: 119
	public QuestObjectiveResource[] Objectives;
}
