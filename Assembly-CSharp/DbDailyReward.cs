using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x0200005F RID: 95
public static class DbDailyReward
{
	// Token: 0x060000FA RID: 250 RVA: 0x00006D30 File Offset: 0x00004F30
	public static Task<DataRow[]> GetDailyRewardsAsync()
	{
		DbDailyReward.<GetDailyRewardsAsync>d__0 <GetDailyRewardsAsync>d__;
		<GetDailyRewardsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetDailyRewardsAsync>d__.<>1__state = -1;
		<GetDailyRewardsAsync>d__.<>t__builder.Start<DbDailyReward.<GetDailyRewardsAsync>d__0>(ref <GetDailyRewardsAsync>d__);
		return <GetDailyRewardsAsync>d__.<>t__builder.Task;
	}
}
