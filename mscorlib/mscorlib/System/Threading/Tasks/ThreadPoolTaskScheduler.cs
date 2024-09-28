using System;
using System.Collections.Generic;
using Internal.Runtime.Augments;
using Internal.Threading.Tasks.Tracing;

namespace System.Threading.Tasks
{
	// Token: 0x0200037E RID: 894
	internal sealed class ThreadPoolTaskScheduler : TaskScheduler
	{
		// Token: 0x06002536 RID: 9526 RVA: 0x000844AF File Offset: 0x000826AF
		internal ThreadPoolTaskScheduler()
		{
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x000844B8 File Offset: 0x000826B8
		protected internal override void QueueTask(Task task)
		{
			if (TaskTrace.Enabled)
			{
				Task internalCurrent = Task.InternalCurrent;
				Task parent = task.m_parent;
				TaskTrace.TaskScheduled(base.Id, (internalCurrent == null) ? 0 : internalCurrent.Id, task.Id, (parent == null) ? 0 : parent.Id, (int)task.Options);
			}
			if ((task.Options & TaskCreationOptions.LongRunning) != TaskCreationOptions.None)
			{
				RuntimeThread runtimeThread = RuntimeThread.Create(ThreadPoolTaskScheduler.s_longRunningThreadWork, 0);
				runtimeThread.IsBackground = true;
				runtimeThread.Start(task);
				return;
			}
			bool forceGlobal = (task.Options & TaskCreationOptions.PreferFairness) > TaskCreationOptions.None;
			ThreadPool.UnsafeQueueCustomWorkItem(task, forceGlobal);
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x00084540 File Offset: 0x00082740
		protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
		{
			if (taskWasPreviouslyQueued && !ThreadPool.TryPopCustomWorkItem(task))
			{
				return false;
			}
			bool result = false;
			try
			{
				result = task.ExecuteEntry(false);
			}
			finally
			{
				if (taskWasPreviouslyQueued)
				{
					this.NotifyWorkItemProgress();
				}
			}
			return result;
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x00084584 File Offset: 0x00082784
		protected internal override bool TryDequeue(Task task)
		{
			return ThreadPool.TryPopCustomWorkItem(task);
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0008458C File Offset: 0x0008278C
		protected override IEnumerable<Task> GetScheduledTasks()
		{
			return this.FilterTasksFromWorkItems(ThreadPool.GetQueuedWorkItems());
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x00084599 File Offset: 0x00082799
		private IEnumerable<Task> FilterTasksFromWorkItems(IEnumerable<IThreadPoolWorkItem> tpwItems)
		{
			foreach (IThreadPoolWorkItem threadPoolWorkItem in tpwItems)
			{
				if (threadPoolWorkItem is Task)
				{
					yield return (Task)threadPoolWorkItem;
				}
			}
			IEnumerator<IThreadPoolWorkItem> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000845A9 File Offset: 0x000827A9
		internal override void NotifyWorkItemProgress()
		{
			ThreadPool.NotifyWorkItemProgress();
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600253D RID: 9533 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal override bool RequiresAtomicStartTransition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001D59 RID: 7513
		private static readonly ParameterizedThreadStart s_longRunningThreadWork = delegate(object s)
		{
			((Task)s).ExecuteEntry(false);
		};
	}
}
