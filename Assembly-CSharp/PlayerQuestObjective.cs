using System;

// Token: 0x02000131 RID: 305
public struct PlayerQuestObjective
{
	// Token: 0x0600033A RID: 826 RVA: 0x0001517C File Offset: 0x0001337C
	public PlayerQuestObjective(int id, int playerQuestId, ObjectiveType objectiveType, int objectiveId, int objectiveAmount, string objectiveName, string objectivePluralName, int objectiveProgress, Rank objectiveRank)
	{
		this.Id = id;
		this.PlayerQuestId = playerQuestId;
		this.ObjectiveType = objectiveType;
		this.ObjectiveId = objectiveId;
		this.ObjectiveAmount = objectiveAmount;
		this.ObjectiveProgress = objectiveProgress;
		this.ObjectiveName = objectiveName;
		this.ObjectivePluralName = objectivePluralName;
		this.ObjectiveRank = objectiveRank;
	}

	// Token: 0x0400061F RID: 1567
	public int Id;

	// Token: 0x04000620 RID: 1568
	public int ObjectiveId;

	// Token: 0x04000621 RID: 1569
	public int PlayerQuestId;

	// Token: 0x04000622 RID: 1570
	public Rank ObjectiveRank;

	// Token: 0x04000623 RID: 1571
	public int ObjectiveAmount;

	// Token: 0x04000624 RID: 1572
	public string ObjectiveName;

	// Token: 0x04000625 RID: 1573
	public string ObjectivePluralName;

	// Token: 0x04000626 RID: 1574
	public int ObjectiveProgress;

	// Token: 0x04000627 RID: 1575
	public ObjectiveType ObjectiveType;
}
