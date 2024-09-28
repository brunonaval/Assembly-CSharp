using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000CF RID: 207
public static class DbPlayerTitle
{
	// Token: 0x0600021A RID: 538 RVA: 0x0000FC8C File Offset: 0x0000DE8C
	public static Task<DataRow[]> GetPlayerTitlesAsync(int playerId)
	{
		DbPlayerTitle.<GetPlayerTitlesAsync>d__0 <GetPlayerTitlesAsync>d__;
		<GetPlayerTitlesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetPlayerTitlesAsync>d__.playerId = playerId;
		<GetPlayerTitlesAsync>d__.<>1__state = -1;
		<GetPlayerTitlesAsync>d__.<>t__builder.Start<DbPlayerTitle.<GetPlayerTitlesAsync>d__0>(ref <GetPlayerTitlesAsync>d__);
		return <GetPlayerTitlesAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600021B RID: 539 RVA: 0x0000FCD0 File Offset: 0x0000DED0
	public static Task AddPlayerTitleAsync(int playerId, int titleId)
	{
		DbPlayerTitle.<AddPlayerTitleAsync>d__1 <AddPlayerTitleAsync>d__;
		<AddPlayerTitleAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddPlayerTitleAsync>d__.playerId = playerId;
		<AddPlayerTitleAsync>d__.titleId = titleId;
		<AddPlayerTitleAsync>d__.<>1__state = -1;
		<AddPlayerTitleAsync>d__.<>t__builder.Start<DbPlayerTitle.<AddPlayerTitleAsync>d__1>(ref <AddPlayerTitleAsync>d__);
		return <AddPlayerTitleAsync>d__.<>t__builder.Task;
	}
}
