using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000CB RID: 203
public static class DbPlayerTalent
{
	// Token: 0x06000211 RID: 529 RVA: 0x0000F8BC File Offset: 0x0000DABC
	public static Task<DataRow[]> GetPlayerTalentsAsync(int playerId)
	{
		DbPlayerTalent.<GetPlayerTalentsAsync>d__0 <GetPlayerTalentsAsync>d__;
		<GetPlayerTalentsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetPlayerTalentsAsync>d__.playerId = playerId;
		<GetPlayerTalentsAsync>d__.<>1__state = -1;
		<GetPlayerTalentsAsync>d__.<>t__builder.Start<DbPlayerTalent.<GetPlayerTalentsAsync>d__0>(ref <GetPlayerTalentsAsync>d__);
		return <GetPlayerTalentsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000F900 File Offset: 0x0000DB00
	public static Task AddPlayerTalentAsync(int playerId, int talentId)
	{
		DbPlayerTalent.<AddPlayerTalentAsync>d__1 <AddPlayerTalentAsync>d__;
		<AddPlayerTalentAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<AddPlayerTalentAsync>d__.playerId = playerId;
		<AddPlayerTalentAsync>d__.talentId = talentId;
		<AddPlayerTalentAsync>d__.<>1__state = -1;
		<AddPlayerTalentAsync>d__.<>t__builder.Start<DbPlayerTalent.<AddPlayerTalentAsync>d__1>(ref <AddPlayerTalentAsync>d__);
		return <AddPlayerTalentAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000F94C File Offset: 0x0000DB4C
	public static Task UpdatePlayerTalentLevelAsync(int playerId, int talentId, int level)
	{
		DbPlayerTalent.<UpdatePlayerTalentLevelAsync>d__2 <UpdatePlayerTalentLevelAsync>d__;
		<UpdatePlayerTalentLevelAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<UpdatePlayerTalentLevelAsync>d__.playerId = playerId;
		<UpdatePlayerTalentLevelAsync>d__.talentId = talentId;
		<UpdatePlayerTalentLevelAsync>d__.level = level;
		<UpdatePlayerTalentLevelAsync>d__.<>1__state = -1;
		<UpdatePlayerTalentLevelAsync>d__.<>t__builder.Start<DbPlayerTalent.<UpdatePlayerTalentLevelAsync>d__2>(ref <UpdatePlayerTalentLevelAsync>d__);
		return <UpdatePlayerTalentLevelAsync>d__.<>t__builder.Task;
	}
}
