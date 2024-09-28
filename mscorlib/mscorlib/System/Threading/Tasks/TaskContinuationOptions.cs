﻿using System;

namespace System.Threading.Tasks
{
	/// <summary>Specifies the behavior for a task that is created by using the <see cref="M:System.Threading.Tasks.Task.ContinueWith(System.Action{System.Threading.Tasks.Task},System.Threading.CancellationToken,System.Threading.Tasks.TaskContinuationOptions,System.Threading.Tasks.TaskScheduler)" /> or <see cref="M:System.Threading.Tasks.Task`1.ContinueWith(System.Action{System.Threading.Tasks.Task{`0}},System.Threading.Tasks.TaskContinuationOptions)" /> method.</summary>
	// Token: 0x02000363 RID: 867
	[Flags]
	public enum TaskContinuationOptions
	{
		/// <summary>When no continuation options are specified, specifies that default behavior should be used when executing a continuation. The continuation runs asynchronously when the antecedent task completes, regardless of the antecedent's final <see cref="P:System.Threading.Tasks.Task.Status" /> property value. It the continuation is a child task, it is created as a detached nested task.</summary>
		// Token: 0x04001D16 RID: 7446
		None = 0,
		/// <summary>A hint to a <see cref="T:System.Threading.Tasks.TaskScheduler" /> to schedule task in the order in which they were scheduled, so that tasks scheduled sooner are more likely to run sooner, and tasks scheduled later are more likely to run later.</summary>
		// Token: 0x04001D17 RID: 7447
		PreferFairness = 1,
		/// <summary>Specifies that a continuation will be a long-running, course-grained operation. It provides a hint to the <see cref="T:System.Threading.Tasks.TaskScheduler" /> that oversubscription may be warranted.</summary>
		// Token: 0x04001D18 RID: 7448
		LongRunning = 2,
		/// <summary>Specifies that the continuation, if it is a child task, is attached to a parent in the task hierarchy. The continuation can be a child task only if its antecedent is also a child task. By default, a child task (that is, an inner task created by an outer task) executes independently of its parent. You can use the <see cref="F:System.Threading.Tasks.TaskContinuationOptions.AttachedToParent" /> option so that the parent and child tasks are synchronized.  
		///  Note that if a parent task is configured with the <see cref="F:System.Threading.Tasks.TaskCreationOptions.DenyChildAttach" /> option, the <see cref="F:System.Threading.Tasks.TaskCreationOptions.AttachedToParent" /> option in the child task has no effect, and the child task will execute as a detached child task.  
		///  For more information, see Attached and Detached Child Tasks.</summary>
		// Token: 0x04001D19 RID: 7449
		AttachedToParent = 4,
		/// <summary>Specifies that any child task (that is, any nested inner task created by this continuation) that is created with the <see cref="F:System.Threading.Tasks.TaskCreationOptions.AttachedToParent" /> option and attempts to execute as an attached child task will not be able to attach to the parent task and will execute instead as a detached child task. For more information, see Attached and Detached Child Tasks.</summary>
		// Token: 0x04001D1A RID: 7450
		DenyChildAttach = 8,
		/// <summary>Specifies that tasks created by the continuation by calling methods such as <see cref="M:System.Threading.Tasks.Task.Run(System.Action)" /> or <see cref="M:System.Threading.Tasks.Task.ContinueWith(System.Action{System.Threading.Tasks.Task})" /> see the default scheduler (<see cref="P:System.Threading.Tasks.TaskScheduler.Default" />) rather than the scheduler on which this continuation is running as the current scheduler.</summary>
		// Token: 0x04001D1B RID: 7451
		HideScheduler = 16,
		/// <summary>In the case of continuation cancellation, prevents completion of the continuation until the antecedent has completed.</summary>
		// Token: 0x04001D1C RID: 7452
		LazyCancellation = 32,
		/// <summary>Specifies that the continuation task should be run asynchronously.  This option has precedence over <see cref="F:System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously" />.</summary>
		// Token: 0x04001D1D RID: 7453
		RunContinuationsAsynchronously = 64,
		/// <summary>Specifies that the continuation task should not be scheduled if its antecedent ran to completion. An antecedent runs to completion if its <see cref="P:System.Threading.Tasks.Task.Status" /> property upon completion is <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />. This option is not valid for multi-task continuations.</summary>
		// Token: 0x04001D1E RID: 7454
		NotOnRanToCompletion = 65536,
		/// <summary>Specifies that the continuation task should not be scheduled if its antecedent threw an unhandled exception. An antecedent throws an unhandled exception if its <see cref="P:System.Threading.Tasks.Task.Status" /> property upon completion is <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />. This option is not valid for multi-task continuations.</summary>
		// Token: 0x04001D1F RID: 7455
		NotOnFaulted = 131072,
		/// <summary>Specifies that the continuation task should not be scheduled if its antecedent was canceled. An antecedent is canceled if its <see cref="P:System.Threading.Tasks.Task.Status" /> property upon completion is <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />. This option is not valid for multi-task continuations.</summary>
		// Token: 0x04001D20 RID: 7456
		NotOnCanceled = 262144,
		/// <summary>Specifies that the continuation should be scheduled only if its antecedent ran to completion. An antecedent runs to completion if its <see cref="P:System.Threading.Tasks.Task.Status" /> property upon completion is <see cref="F:System.Threading.Tasks.TaskStatus.RanToCompletion" />. This option is not valid for multi-task continuations.</summary>
		// Token: 0x04001D21 RID: 7457
		OnlyOnRanToCompletion = 393216,
		/// <summary>Specifies that the continuation task should be scheduled only if its antecedent threw an unhandled exception. An antecedent throws an unhandled exception if its <see cref="P:System.Threading.Tasks.Task.Status" /> property upon completion is <see cref="F:System.Threading.Tasks.TaskStatus.Faulted" />.  
		///  The <see cref="F:System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted" /> option guarantees that the <see cref="P:System.Threading.Tasks.Task.Exception" /> property in the antecedent is not <see langword="null" />. You can use that property to catch the exception and see which exception caused the task to fault. If you do not access the <see cref="P:System.Threading.Tasks.Task.Exception" /> property, the exception is unhandled. Also, if you attempt to access the <see cref="P:System.Threading.Tasks.Task`1.Result" /> property of a task that has been canceled or has faulted, a new exception is thrown.  
		///  This option is not valid for multi-task continuations.</summary>
		// Token: 0x04001D22 RID: 7458
		OnlyOnFaulted = 327680,
		/// <summary>Specifies that the continuation should be scheduled only if its antecedent was canceled.  An antecedent is canceled if its <see cref="P:System.Threading.Tasks.Task.Status" /> property upon completion is <see cref="F:System.Threading.Tasks.TaskStatus.Canceled" />. This option is not valid for multi-task continuations.</summary>
		// Token: 0x04001D23 RID: 7459
		OnlyOnCanceled = 196608,
		/// <summary>Specifies that the continuation task should be executed synchronously. With this option specified, the continuation runs on the same thread that causes the antecedent task to transition into its final state. If the antecedent is already complete when the continuation is created, the continuation will run on the thread that creates the continuation. If the antecedent's <see cref="T:System.Threading.CancellationTokenSource" /> is disposed in a <see langword="finally" /> block (<see langword="Finally" /> in Visual Basic), a continuation with this option will run in that <see langword="finally" /> block. Only very short-running continuations should be executed synchronously.  
		///  Because the task executes synchronously, there is no need to call a method such as <see cref="M:System.Threading.Tasks.Task.Wait" /> to ensure that the calling thread waits for the task to complete.</summary>
		// Token: 0x04001D24 RID: 7460
		ExecuteSynchronously = 524288
	}
}
