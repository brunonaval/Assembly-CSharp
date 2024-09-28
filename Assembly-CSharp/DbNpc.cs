using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x02000097 RID: 151
public static class DbNpc
{
	// Token: 0x06000187 RID: 391 RVA: 0x0000B178 File Offset: 0x00009378
	public static Task<DataRow[]> GetNpcsAsync()
	{
		DbNpc.<GetNpcsAsync>d__0 <GetNpcsAsync>d__;
		<GetNpcsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetNpcsAsync>d__.<>1__state = -1;
		<GetNpcsAsync>d__.<>t__builder.Start<DbNpc.<GetNpcsAsync>d__0>(ref <GetNpcsAsync>d__);
		return <GetNpcsAsync>d__.<>t__builder.Task;
	}
}
