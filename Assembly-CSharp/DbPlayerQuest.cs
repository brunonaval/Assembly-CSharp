using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000BA RID: 186
public static class DbPlayerQuest
{
	// Token: 0x060001E4 RID: 484 RVA: 0x0000DF58 File Offset: 0x0000C158
	public static Task<bool> AcceptQuestAsync(int accountId, int playerId, Quest quest)
	{
		DbPlayerQuest.<AcceptQuestAsync>d__0 <AcceptQuestAsync>d__;
		<AcceptQuestAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
		<AcceptQuestAsync>d__.accountId = accountId;
		<AcceptQuestAsync>d__.playerId = playerId;
		<AcceptQuestAsync>d__.quest = quest;
		<AcceptQuestAsync>d__.<>1__state = -1;
		<AcceptQuestAsync>d__.<>t__builder.Start<DbPlayerQuest.<AcceptQuestAsync>d__0>(ref <AcceptQuestAsync>d__);
		return <AcceptQuestAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001E5 RID: 485 RVA: 0x0000DFAC File Offset: 0x0000C1AC
	public static Task AbortQuestAsync(int playerId, int questId)
	{
		DbPlayerQuest.<AbortQuestAsync>d__1 <AbortQuestAsync>d__;
		<AbortQuestAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AbortQuestAsync>d__.playerId = playerId;
		<AbortQuestAsync>d__.questId = questId;
		<AbortQuestAsync>d__.<>1__state = -1;
		<AbortQuestAsync>d__.<>t__builder.Start<DbPlayerQuest.<AbortQuestAsync>d__1>(ref <AbortQuestAsync>d__);
		return <AbortQuestAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x0000DFF8 File Offset: 0x0000C1F8
	public static Task<DataRow[]> GetPlayerQuestsAsync(int playerId)
	{
		DbPlayerQuest.<GetPlayerQuestsAsync>d__2 <GetPlayerQuestsAsync>d__;
		<GetPlayerQuestsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetPlayerQuestsAsync>d__.playerId = playerId;
		<GetPlayerQuestsAsync>d__.<>1__state = -1;
		<GetPlayerQuestsAsync>d__.<>t__builder.Start<DbPlayerQuest.<GetPlayerQuestsAsync>d__2>(ref <GetPlayerQuestsAsync>d__);
		return <GetPlayerQuestsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000E03C File Offset: 0x0000C23C
	public static Task<DataRow[]> GetRewardsAsync(int playerQuestId)
	{
		DbPlayerQuest.<GetRewardsAsync>d__3 <GetRewardsAsync>d__;
		<GetRewardsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetRewardsAsync>d__.playerQuestId = playerQuestId;
		<GetRewardsAsync>d__.<>1__state = -1;
		<GetRewardsAsync>d__.<>t__builder.Start<DbPlayerQuest.<GetRewardsAsync>d__3>(ref <GetRewardsAsync>d__);
		return <GetRewardsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001E8 RID: 488 RVA: 0x0000E080 File Offset: 0x0000C280
	public static Task<DataRow[]> GetObjectivesAsync(int playerQuestId)
	{
		DbPlayerQuest.<GetObjectivesAsync>d__4 <GetObjectivesAsync>d__;
		<GetObjectivesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetObjectivesAsync>d__.playerQuestId = playerQuestId;
		<GetObjectivesAsync>d__.<>1__state = -1;
		<GetObjectivesAsync>d__.<>t__builder.Start<DbPlayerQuest.<GetObjectivesAsync>d__4>(ref <GetObjectivesAsync>d__);
		return <GetObjectivesAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001E9 RID: 489 RVA: 0x0000E0C4 File Offset: 0x0000C2C4
	public static Task SetShowOverlayAsync(int playerId, int questId, bool showOverlay)
	{
		DbPlayerQuest.<SetShowOverlayAsync>d__5 <SetShowOverlayAsync>d__;
		<SetShowOverlayAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SetShowOverlayAsync>d__.playerId = playerId;
		<SetShowOverlayAsync>d__.questId = questId;
		<SetShowOverlayAsync>d__.showOverlay = showOverlay;
		<SetShowOverlayAsync>d__.<>1__state = -1;
		<SetShowOverlayAsync>d__.<>t__builder.Start<DbPlayerQuest.<SetShowOverlayAsync>d__5>(ref <SetShowOverlayAsync>d__);
		return <SetShowOverlayAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001EA RID: 490 RVA: 0x0000E118 File Offset: 0x0000C318
	public static Task UpdateQuestObjectiveAsync(int playerId, int questId, int objectiveType, int objectiveRank, int objectiveId, int objectiveProgress)
	{
		DbPlayerQuest.<UpdateQuestObjectiveAsync>d__6 <UpdateQuestObjectiveAsync>d__;
		<UpdateQuestObjectiveAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateQuestObjectiveAsync>d__.playerId = playerId;
		<UpdateQuestObjectiveAsync>d__.questId = questId;
		<UpdateQuestObjectiveAsync>d__.objectiveType = objectiveType;
		<UpdateQuestObjectiveAsync>d__.objectiveRank = objectiveRank;
		<UpdateQuestObjectiveAsync>d__.objectiveId = objectiveId;
		<UpdateQuestObjectiveAsync>d__.objectiveProgress = objectiveProgress;
		<UpdateQuestObjectiveAsync>d__.<>1__state = -1;
		<UpdateQuestObjectiveAsync>d__.<>t__builder.Start<DbPlayerQuest.<UpdateQuestObjectiveAsync>d__6>(ref <UpdateQuestObjectiveAsync>d__);
		return <UpdateQuestObjectiveAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000E188 File Offset: 0x0000C388
	public static Task CompleteQuestAsync(int playerId, int questId, DateTime completed)
	{
		DbPlayerQuest.<CompleteQuestAsync>d__7 <CompleteQuestAsync>d__;
		<CompleteQuestAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CompleteQuestAsync>d__.playerId = playerId;
		<CompleteQuestAsync>d__.questId = questId;
		<CompleteQuestAsync>d__.completed = completed;
		<CompleteQuestAsync>d__.<>1__state = -1;
		<CompleteQuestAsync>d__.<>t__builder.Start<DbPlayerQuest.<CompleteQuestAsync>d__7>(ref <CompleteQuestAsync>d__);
		return <CompleteQuestAsync>d__.<>t__builder.Task;
	}
}
