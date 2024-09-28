using System;

// Token: 0x0200013E RID: 318
public struct QuestObjective
{
	// Token: 0x06000361 RID: 865 RVA: 0x000158D9 File Offset: 0x00013AD9
	public QuestObjective(int id, int questId, ObjectiveType objectiveType, int objectiveId, int objectiveAmount, string objectiveName, string objectivePluralName, Rank objectiveRank)
	{
		this.Id = id;
		this.QuestId = questId;
		this.ObjectiveType = objectiveType;
		this.ObjectiveId = objectiveId;
		this.ObjectiveAmount = objectiveAmount;
		this.ObjectiveRank = objectiveRank;
		this.ObjectiveName = objectiveName;
		this.ObjectivePluralName = objectivePluralName;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x00015918 File Offset: 0x00013B18
	public QuestObjective(int questId, int objectiveId, int objectiveAmount, string objectiveName, string objectivePluralName)
	{
		this.Id = 0;
		this.QuestId = questId;
		this.ObjectiveType = ObjectiveType.KillMonsters;
		this.ObjectiveId = objectiveId;
		this.ObjectiveAmount = objectiveAmount;
		this.ObjectiveRank = Rank.None;
		this.ObjectiveName = objectiveName;
		this.ObjectivePluralName = objectivePluralName;
	}

	// Token: 0x04000672 RID: 1650
	public int Id;

	// Token: 0x04000673 RID: 1651
	public int QuestId;

	// Token: 0x04000674 RID: 1652
	public int ObjectiveId;

	// Token: 0x04000675 RID: 1653
	public Rank ObjectiveRank;

	// Token: 0x04000676 RID: 1654
	public int ObjectiveAmount;

	// Token: 0x04000677 RID: 1655
	public string ObjectiveName;

	// Token: 0x04000678 RID: 1656
	public string ObjectivePluralName;

	// Token: 0x04000679 RID: 1657
	public ObjectiveType ObjectiveType;
}
