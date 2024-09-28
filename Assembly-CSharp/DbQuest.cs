using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000D2 RID: 210
public static class DbQuest
{
	// Token: 0x06000220 RID: 544 RVA: 0x0000FEF4 File Offset: 0x0000E0F4
	public static Task<DataRow[]> GetQuestsAsync()
	{
		DbQuest.<GetQuestsAsync>d__0 <GetQuestsAsync>d__;
		<GetQuestsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetQuestsAsync>d__.<>1__state = -1;
		<GetQuestsAsync>d__.<>t__builder.Start<DbQuest.<GetQuestsAsync>d__0>(ref <GetQuestsAsync>d__);
		return <GetQuestsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000221 RID: 545 RVA: 0x0000FF30 File Offset: 0x0000E130
	public static Task<DataRow[]> GetRewardsAsync(int questId)
	{
		DbQuest.<GetRewardsAsync>d__1 <GetRewardsAsync>d__;
		<GetRewardsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetRewardsAsync>d__.questId = questId;
		<GetRewardsAsync>d__.<>1__state = -1;
		<GetRewardsAsync>d__.<>t__builder.Start<DbQuest.<GetRewardsAsync>d__1>(ref <GetRewardsAsync>d__);
		return <GetRewardsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000222 RID: 546 RVA: 0x0000FF74 File Offset: 0x0000E174
	public static Task<DataRow[]> GetObjectivesAsync(int questId)
	{
		DbQuest.<GetObjectivesAsync>d__2 <GetObjectivesAsync>d__;
		<GetObjectivesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetObjectivesAsync>d__.questId = questId;
		<GetObjectivesAsync>d__.<>1__state = -1;
		<GetObjectivesAsync>d__.<>t__builder.Start<DbQuest.<GetObjectivesAsync>d__2>(ref <GetObjectivesAsync>d__);
		return <GetObjectivesAsync>d__.<>t__builder.Task;
	}
}
