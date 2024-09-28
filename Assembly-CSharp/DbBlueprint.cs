using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000058 RID: 88
public static class DbBlueprint
{
	// Token: 0x060000E8 RID: 232 RVA: 0x00006668 File Offset: 0x00004868
	public static Task<int> GetMaxBlueprintIdAsync()
	{
		DbBlueprint.<GetMaxBlueprintIdAsync>d__0 <GetMaxBlueprintIdAsync>d__;
		<GetMaxBlueprintIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetMaxBlueprintIdAsync>d__.<>1__state = -1;
		<GetMaxBlueprintIdAsync>d__.<>t__builder.Start<DbBlueprint.<GetMaxBlueprintIdAsync>d__0>(ref <GetMaxBlueprintIdAsync>d__);
		return <GetMaxBlueprintIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x000066A4 File Offset: 0x000048A4
	public static Task<DataRow[]> GetBueprintsAsync()
	{
		DbBlueprint.<GetBueprintsAsync>d__1 <GetBueprintsAsync>d__;
		<GetBueprintsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetBueprintsAsync>d__.<>1__state = -1;
		<GetBueprintsAsync>d__.<>t__builder.Start<DbBlueprint.<GetBueprintsAsync>d__1>(ref <GetBueprintsAsync>d__);
		return <GetBueprintsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000066E0 File Offset: 0x000048E0
	public static Task<DataRow[]> GetBlueprintMaterials(int blueprintId)
	{
		DbBlueprint.<GetBlueprintMaterials>d__2 <GetBlueprintMaterials>d__;
		<GetBlueprintMaterials>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetBlueprintMaterials>d__.blueprintId = blueprintId;
		<GetBlueprintMaterials>d__.<>1__state = -1;
		<GetBlueprintMaterials>d__.<>t__builder.Start<DbBlueprint.<GetBlueprintMaterials>d__2>(ref <GetBlueprintMaterials>d__);
		return <GetBlueprintMaterials>d__.<>t__builder.Task;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00006724 File Offset: 0x00004924
	public static Task<DataRow[]> GetPlayerBlueprintsAsync(int playerId)
	{
		DbBlueprint.<GetPlayerBlueprintsAsync>d__3 <GetPlayerBlueprintsAsync>d__;
		<GetPlayerBlueprintsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetPlayerBlueprintsAsync>d__.playerId = playerId;
		<GetPlayerBlueprintsAsync>d__.<>1__state = -1;
		<GetPlayerBlueprintsAsync>d__.<>t__builder.Start<DbBlueprint.<GetPlayerBlueprintsAsync>d__3>(ref <GetPlayerBlueprintsAsync>d__);
		return <GetPlayerBlueprintsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00006768 File Offset: 0x00004968
	public static Task InsertOnPlayerBlueprintAsync(int playerId, int blueprintId)
	{
		DbBlueprint.<InsertOnPlayerBlueprintAsync>d__4 <InsertOnPlayerBlueprintAsync>d__;
		<InsertOnPlayerBlueprintAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<InsertOnPlayerBlueprintAsync>d__.playerId = playerId;
		<InsertOnPlayerBlueprintAsync>d__.blueprintId = blueprintId;
		<InsertOnPlayerBlueprintAsync>d__.<>1__state = -1;
		<InsertOnPlayerBlueprintAsync>d__.<>t__builder.Start<DbBlueprint.<InsertOnPlayerBlueprintAsync>d__4>(ref <InsertOnPlayerBlueprintAsync>d__);
		return <InsertOnPlayerBlueprintAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x000067B4 File Offset: 0x000049B4
	public static Task RemoveAllPlayerBlueprintsAsync(int playerId)
	{
		DbBlueprint.<RemoveAllPlayerBlueprintsAsync>d__5 <RemoveAllPlayerBlueprintsAsync>d__;
		<RemoveAllPlayerBlueprintsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<RemoveAllPlayerBlueprintsAsync>d__.playerId = playerId;
		<RemoveAllPlayerBlueprintsAsync>d__.<>1__state = -1;
		<RemoveAllPlayerBlueprintsAsync>d__.<>t__builder.Start<DbBlueprint.<RemoveAllPlayerBlueprintsAsync>d__5>(ref <RemoveAllPlayerBlueprintsAsync>d__);
		return <RemoveAllPlayerBlueprintsAsync>d__.<>t__builder.Task;
	}
}
