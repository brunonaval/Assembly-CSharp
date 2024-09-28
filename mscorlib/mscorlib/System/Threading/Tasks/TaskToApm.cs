using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200030C RID: 780
	internal static class TaskToApm
	{
		// Token: 0x06002182 RID: 8578 RVA: 0x000786C4 File Offset: 0x000768C4
		public static IAsyncResult Begin(Task task, AsyncCallback callback, object state)
		{
			IAsyncResult asyncResult;
			if (task.IsCompleted)
			{
				asyncResult = new TaskToApm.TaskWrapperAsyncResult(task, state, true);
				if (callback != null)
				{
					callback(asyncResult);
				}
			}
			else
			{
				IAsyncResult asyncResult3;
				if (task.AsyncState != state)
				{
					IAsyncResult asyncResult2 = new TaskToApm.TaskWrapperAsyncResult(task, state, false);
					asyncResult3 = asyncResult2;
				}
				else
				{
					asyncResult3 = task;
				}
				asyncResult = asyncResult3;
				if (callback != null)
				{
					TaskToApm.InvokeCallbackWhenTaskCompletes(task, callback, asyncResult);
				}
			}
			return asyncResult;
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00078714 File Offset: 0x00076914
		public static void End(IAsyncResult asyncResult)
		{
			TaskToApm.TaskWrapperAsyncResult taskWrapperAsyncResult = asyncResult as TaskToApm.TaskWrapperAsyncResult;
			Task task;
			if (taskWrapperAsyncResult != null)
			{
				task = taskWrapperAsyncResult.Task;
			}
			else
			{
				task = (asyncResult as Task);
			}
			if (task == null)
			{
				throw new ArgumentNullException();
			}
			task.GetAwaiter().GetResult();
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00078754 File Offset: 0x00076954
		public static TResult End<TResult>(IAsyncResult asyncResult)
		{
			TaskToApm.TaskWrapperAsyncResult taskWrapperAsyncResult = asyncResult as TaskToApm.TaskWrapperAsyncResult;
			Task<TResult> task;
			if (taskWrapperAsyncResult != null)
			{
				task = (taskWrapperAsyncResult.Task as Task<TResult>);
			}
			else
			{
				task = (asyncResult as Task<TResult>);
			}
			if (task == null)
			{
				throw new ArgumentNullException();
			}
			return task.GetAwaiter().GetResult();
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00078798 File Offset: 0x00076998
		private static void InvokeCallbackWhenTaskCompletes(Task antecedent, AsyncCallback callback, IAsyncResult asyncResult)
		{
			antecedent.ConfigureAwait(false).GetAwaiter().OnCompleted(delegate
			{
				callback(asyncResult);
			});
		}

		// Token: 0x0200030D RID: 781
		private sealed class TaskWrapperAsyncResult : IAsyncResult
		{
			// Token: 0x06002186 RID: 8582 RVA: 0x000787DC File Offset: 0x000769DC
			internal TaskWrapperAsyncResult(Task task, object state, bool completedSynchronously)
			{
				this.Task = task;
				this._state = state;
				this._completedSynchronously = completedSynchronously;
			}

			// Token: 0x170003E5 RID: 997
			// (get) Token: 0x06002187 RID: 8583 RVA: 0x000787F9 File Offset: 0x000769F9
			object IAsyncResult.AsyncState
			{
				get
				{
					return this._state;
				}
			}

			// Token: 0x170003E6 RID: 998
			// (get) Token: 0x06002188 RID: 8584 RVA: 0x00078801 File Offset: 0x00076A01
			bool IAsyncResult.CompletedSynchronously
			{
				get
				{
					return this._completedSynchronously;
				}
			}

			// Token: 0x170003E7 RID: 999
			// (get) Token: 0x06002189 RID: 8585 RVA: 0x00078809 File Offset: 0x00076A09
			bool IAsyncResult.IsCompleted
			{
				get
				{
					return this.Task.IsCompleted;
				}
			}

			// Token: 0x170003E8 RID: 1000
			// (get) Token: 0x0600218A RID: 8586 RVA: 0x00078816 File Offset: 0x00076A16
			WaitHandle IAsyncResult.AsyncWaitHandle
			{
				get
				{
					return ((IAsyncResult)this.Task).AsyncWaitHandle;
				}
			}

			// Token: 0x04001BC9 RID: 7113
			internal readonly Task Task;

			// Token: 0x04001BCA RID: 7114
			private readonly object _state;

			// Token: 0x04001BCB RID: 7115
			private readonly bool _completedSynchronously;
		}
	}
}
