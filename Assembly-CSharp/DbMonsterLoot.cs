using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000093 RID: 147
public static class DbMonsterLoot
{
	// Token: 0x06000181 RID: 385 RVA: 0x0000AF30 File Offset: 0x00009130
	public static Task<DataRow[]> GetLootAsync(int monsterId)
	{
		DbMonsterLoot.<GetLootAsync>d__0 <GetLootAsync>d__;
		<GetLootAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetLootAsync>d__.monsterId = monsterId;
		<GetLootAsync>d__.<>1__state = -1;
		<GetLootAsync>d__.<>t__builder.Start<DbMonsterLoot.<GetLootAsync>d__0>(ref <GetLootAsync>d__);
		return <GetLootAsync>d__.<>t__builder.Task;
	}
}
