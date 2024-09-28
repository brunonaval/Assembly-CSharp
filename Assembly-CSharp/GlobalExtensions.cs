using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public static class GlobalExtensions
{
	// Token: 0x06001E2F RID: 7727 RVA: 0x00096BCC File Offset: 0x00094DCC
	public static Task ExecuteCoroutineAsync(this MonoBehaviour monoBehavior, IEnumerator coroutine)
	{
		GlobalExtensions.<ExecuteCoroutineAsync>d__0 <ExecuteCoroutineAsync>d__;
		<ExecuteCoroutineAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
		<ExecuteCoroutineAsync>d__.monoBehavior = monoBehavior;
		<ExecuteCoroutineAsync>d__.coroutine = coroutine;
		<ExecuteCoroutineAsync>d__.<>1__state = -1;
		<ExecuteCoroutineAsync>d__.<>t__builder.Start<GlobalExtensions.<ExecuteCoroutineAsync>d__0>(ref <ExecuteCoroutineAsync>d__);
		return <ExecuteCoroutineAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06001E30 RID: 7728 RVA: 0x00096C17 File Offset: 0x00094E17
	private static IEnumerator TempCoroutine(IEnumerator coroutine, Action afterExecutionCallback)
	{
		yield return coroutine;
		afterExecutionCallback();
		yield break;
	}

	// Token: 0x06001E31 RID: 7729 RVA: 0x00096C2D File Offset: 0x00094E2D
	public static IEnumerator WaitAsCoroutine(this Task task)
	{
		yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);
		yield break;
	}
}
