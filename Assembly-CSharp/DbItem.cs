using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// Token: 0x0200007B RID: 123
public static class DbItem
{
	// Token: 0x06000142 RID: 322 RVA: 0x00009000 File Offset: 0x00007200
	public static Task<DataRow[]> GetItemsAsync()
	{
		DbItem.<GetItemsAsync>d__0 <GetItemsAsync>d__;
		<GetItemsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DataRow[]>.Create();
		<GetItemsAsync>d__.<>1__state = -1;
		<GetItemsAsync>d__.<>t__builder.Start<DbItem.<GetItemsAsync>d__0>(ref <GetItemsAsync>d__);
		return <GetItemsAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000903C File Offset: 0x0000723C
	public static Task<int> GetMaxItemIdAsync()
	{
		DbItem.<GetMaxItemIdAsync>d__1 <GetMaxItemIdAsync>d__;
		<GetMaxItemIdAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
		<GetMaxItemIdAsync>d__.<>1__state = -1;
		<GetMaxItemIdAsync>d__.<>t__builder.Start<DbItem.<GetMaxItemIdAsync>d__1>(ref <GetMaxItemIdAsync>d__);
		return <GetMaxItemIdAsync>d__.<>t__builder.Task;
	}
}
