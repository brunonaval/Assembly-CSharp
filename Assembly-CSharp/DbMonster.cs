using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000091 RID: 145
public static class DbMonster
{
	// Token: 0x0600017E RID: 382 RVA: 0x0000AE2C File Offset: 0x0000902C
	public static Task<DataRow[]> GetMonstersAsync()
	{
		DbMonster.<GetMonstersAsync>d__0 <GetMonstersAsync>d__;
		<GetMonstersAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetMonstersAsync>d__.<>1__state = -1;
		<GetMonstersAsync>d__.<>t__builder.Start<DbMonster.<GetMonstersAsync>d__0>(ref <GetMonstersAsync>d__);
		return <GetMonstersAsync>d__.<>t__builder.Task;
	}
}
