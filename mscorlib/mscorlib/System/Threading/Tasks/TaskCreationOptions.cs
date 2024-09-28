﻿using System;

namespace System.Threading.Tasks
{
	/// <summary>Specifies flags that control optional behavior for the creation and execution of tasks.</summary>
	// Token: 0x02000361 RID: 865
	[Flags]
	public enum TaskCreationOptions
	{
		/// <summary>Specifies that the default behavior should be used.</summary>
		// Token: 0x04001D06 RID: 7430
		None = 0,
		/// <summary>A hint to a <see cref="T:System.Threading.Tasks.TaskScheduler" /> to schedule a task in as fair a manner as possible, meaning that tasks scheduled sooner will be more likely to be run sooner, and tasks scheduled later will be more likely to be run later.</summary>
		// Token: 0x04001D07 RID: 7431
		PreferFairness = 1,
		/// <summary>Specifies that a task will be a long-running, coarse-grained operation involving fewer, larger components than fine-grained systems. It provides a hint to the <see cref="T:System.Threading.Tasks.TaskScheduler" /> that oversubscription may be warranted. Oversubscription lets you create more threads than the available number of hardware threads. It also provides a hint  to the task scheduler that an additional thread might be required for the task so that it does not block the forward progress of other threads or work items on the local thread-pool queue.</summary>
		// Token: 0x04001D08 RID: 7432
		LongRunning = 2,
		/// <summary>Specifies that a task is attached to a parent in the task hierarchy. By default, a child task (that is, an inner task created by an outer task) executes independently of its parent. You can use the <see cref="F:System.Threading.Tasks.TaskContinuationOptions.AttachedToParent" /> option so that the parent and child tasks are synchronized.  
		///  Note that if a parent task is configured with the <see cref="F:System.Threading.Tasks.TaskCreationOptions.DenyChildAttach" /> option, the <see cref="F:System.Threading.Tasks.TaskCreationOptions.AttachedToParent" /> option in the child task has no effect, and the child task will execute as a detached child task.  
		///  For more information, see Attached and Detached Child Tasks.</summary>
		// Token: 0x04001D09 RID: 7433
		AttachedToParent = 4,
		/// <summary>Specifies that any child task that attempts to execute as an attached child task (that is, it is created with the <see cref="F:System.Threading.Tasks.TaskCreationOptions.AttachedToParent" /> option) will not be able to attach to the parent task and will execute instead as a detached child task. For more information, see Attached and Detached Child Tasks.</summary>
		// Token: 0x04001D0A RID: 7434
		DenyChildAttach = 8,
		/// <summary>Prevents the ambient scheduler from being seen as the current scheduler in the created task. This means that operations like StartNew or ContinueWith that are performed in the created task will see <see cref="P:System.Threading.Tasks.TaskScheduler.Default" /> as the current scheduler.</summary>
		// Token: 0x04001D0B RID: 7435
		HideScheduler = 16,
		/// <summary>Forces continuations added to the current task to be executed asynchronously.  
		///  Note that the <see cref="F:System.Threading.Tasks.TaskCreationOptions.RunContinuationsAsynchronously" /> member is available in the <see cref="T:System.Threading.Tasks.TaskCreationOptions" /> enumeration starting with the .NET Framework 4.6.</summary>
		// Token: 0x04001D0C RID: 7436
		RunContinuationsAsynchronously = 64
	}
}
