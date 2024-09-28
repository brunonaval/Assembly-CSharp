using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000D6 RID: 214
public static class DbServer
{
	// Token: 0x06000229 RID: 553 RVA: 0x00010240 File Offset: 0x0000E440
	public static Task<int> GetCurrentRecordAsync(int serverId)
	{
		DbServer.<GetCurrentRecordAsync>d__0 <GetCurrentRecordAsync>d__;
		<GetCurrentRecordAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetCurrentRecordAsync>d__.serverId = serverId;
		<GetCurrentRecordAsync>d__.<>1__state = -1;
		<GetCurrentRecordAsync>d__.<>t__builder.Start<DbServer.<GetCurrentRecordAsync>d__0>(ref <GetCurrentRecordAsync>d__);
		return <GetCurrentRecordAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00010284 File Offset: 0x0000E484
	public static Task SaveNewRecordAsync(int serverId, int recordCount)
	{
		DbServer.<SaveNewRecordAsync>d__1 <SaveNewRecordAsync>d__;
		<SaveNewRecordAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<SaveNewRecordAsync>d__.serverId = serverId;
		<SaveNewRecordAsync>d__.recordCount = recordCount;
		<SaveNewRecordAsync>d__.<>1__state = -1;
		<SaveNewRecordAsync>d__.<>t__builder.Start<DbServer.<SaveNewRecordAsync>d__1>(ref <SaveNewRecordAsync>d__);
		return <SaveNewRecordAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x000102D0 File Offset: 0x0000E4D0
	public static Task<int> GetServerIdAsync(string machineName)
	{
		DbServer.<GetServerIdAsync>d__2 <GetServerIdAsync>d__;
		<GetServerIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetServerIdAsync>d__.machineName = machineName;
		<GetServerIdAsync>d__.<>1__state = -1;
		<GetServerIdAsync>d__.<>t__builder.Start<DbServer.<GetServerIdAsync>d__2>(ref <GetServerIdAsync>d__);
		return <GetServerIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600022C RID: 556 RVA: 0x00010314 File Offset: 0x0000E514
	public static Task<DataRow> GetServerAsync(int serverId)
	{
		DbServer.<GetServerAsync>d__3 <GetServerAsync>d__;
		<GetServerAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow>.Create();
		<GetServerAsync>d__.serverId = serverId;
		<GetServerAsync>d__.<>1__state = -1;
		<GetServerAsync>d__.<>t__builder.Start<DbServer.<GetServerAsync>d__3>(ref <GetServerAsync>d__);
		return <GetServerAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x00010358 File Offset: 0x0000E558
	public static Task EraseOldHistoriesAsync(int serverId)
	{
		DbServer.<EraseOldHistoriesAsync>d__4 <EraseOldHistoriesAsync>d__;
		<EraseOldHistoriesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<EraseOldHistoriesAsync>d__.serverId = serverId;
		<EraseOldHistoriesAsync>d__.<>1__state = -1;
		<EraseOldHistoriesAsync>d__.<>t__builder.Start<DbServer.<EraseOldHistoriesAsync>d__4>(ref <EraseOldHistoriesAsync>d__);
		return <EraseOldHistoriesAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0001039C File Offset: 0x0000E59C
	public static Task CreateHistoryAsync(int serverId, int playersOnline)
	{
		DbServer.<CreateHistoryAsync>d__5 <CreateHistoryAsync>d__;
		<CreateHistoryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<CreateHistoryAsync>d__.serverId = serverId;
		<CreateHistoryAsync>d__.playersOnline = playersOnline;
		<CreateHistoryAsync>d__.<>1__state = -1;
		<CreateHistoryAsync>d__.<>t__builder.Start<DbServer.<CreateHistoryAsync>d__5>(ref <CreateHistoryAsync>d__);
		return <CreateHistoryAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600022F RID: 559 RVA: 0x000103E8 File Offset: 0x0000E5E8
	public static Task UpdateCurrentPlayersAsync(int serverId, int playersOnline)
	{
		DbServer.<UpdateCurrentPlayersAsync>d__6 <UpdateCurrentPlayersAsync>d__;
		<UpdateCurrentPlayersAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateCurrentPlayersAsync>d__.serverId = serverId;
		<UpdateCurrentPlayersAsync>d__.playersOnline = playersOnline;
		<UpdateCurrentPlayersAsync>d__.<>1__state = -1;
		<UpdateCurrentPlayersAsync>d__.<>t__builder.Start<DbServer.<UpdateCurrentPlayersAsync>d__6>(ref <UpdateCurrentPlayersAsync>d__);
		return <UpdateCurrentPlayersAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x00010434 File Offset: 0x0000E634
	public static Task UpdateExperienceModifierAsync(int serverId, float expModifier)
	{
		DbServer.<UpdateExperienceModifierAsync>d__7 <UpdateExperienceModifierAsync>d__;
		<UpdateExperienceModifierAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateExperienceModifierAsync>d__.serverId = serverId;
		<UpdateExperienceModifierAsync>d__.expModifier = expModifier;
		<UpdateExperienceModifierAsync>d__.<>1__state = -1;
		<UpdateExperienceModifierAsync>d__.<>t__builder.Start<DbServer.<UpdateExperienceModifierAsync>d__7>(ref <UpdateExperienceModifierAsync>d__);
		return <UpdateExperienceModifierAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x00010480 File Offset: 0x0000E680
	public static Task UpdateCraftExperienceModifierAsync(int serverId, float craftExpModifier)
	{
		DbServer.<UpdateCraftExperienceModifierAsync>d__8 <UpdateCraftExperienceModifierAsync>d__;
		<UpdateCraftExperienceModifierAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdateCraftExperienceModifierAsync>d__.serverId = serverId;
		<UpdateCraftExperienceModifierAsync>d__.craftExpModifier = craftExpModifier;
		<UpdateCraftExperienceModifierAsync>d__.<>1__state = -1;
		<UpdateCraftExperienceModifierAsync>d__.<>t__builder.Start<DbServer.<UpdateCraftExperienceModifierAsync>d__8>(ref <UpdateCraftExperienceModifierAsync>d__);
		return <UpdateCraftExperienceModifierAsync>d__.<>t__builder.Task;
	}
}
