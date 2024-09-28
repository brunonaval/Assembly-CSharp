using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x020000E2 RID: 226
public static class DbTitle
{
	// Token: 0x06000247 RID: 583 RVA: 0x00010EC8 File Offset: 0x0000F0C8
	public static Task<DataRow[]> GetTitlesAsync()
	{
		DbTitle.<GetTitlesAsync>d__0 <GetTitlesAsync>d__;
		<GetTitlesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetTitlesAsync>d__.<>1__state = -1;
		<GetTitlesAsync>d__.<>t__builder.Start<DbTitle.<GetTitlesAsync>d__0>(ref <GetTitlesAsync>d__);
		return <GetTitlesAsync>d__.<>t__builder.Task;
	}
}
