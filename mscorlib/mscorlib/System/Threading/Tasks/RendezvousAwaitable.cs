using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace System.Threading.Tasks
{
	// Token: 0x02000319 RID: 793
	internal class RendezvousAwaitable<TResult> : ICriticalNotifyCompletion, INotifyCompletion
	{
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060021CB RID: 8651 RVA: 0x000791FA File Offset: 0x000773FA
		// (set) Token: 0x060021CC RID: 8652 RVA: 0x00079202 File Offset: 0x00077402
		public bool RunContinuationsAsynchronously { get; set; } = true;

		// Token: 0x060021CD RID: 8653 RVA: 0x0000270D File Offset: 0x0000090D
		public RendezvousAwaitable<TResult> GetAwaiter()
		{
			return this;
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060021CE RID: 8654 RVA: 0x0007920B File Offset: 0x0007740B
		public bool IsCompleted
		{
			get
			{
				return Volatile.Read<Action>(ref this._continuation) != null;
			}
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x0007921C File Offset: 0x0007741C
		public TResult GetResult()
		{
			this._continuation = null;
			ExceptionDispatchInfo error = this._error;
			if (error != null)
			{
				this._error = null;
				error.Throw();
			}
			TResult result = this._result;
			this._result = default(TResult);
			return result;
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00079259 File Offset: 0x00077459
		public void SetResult(TResult result)
		{
			this._result = result;
			this.NotifyAwaiter();
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00079268 File Offset: 0x00077468
		public void SetCanceled(CancellationToken token = default(CancellationToken))
		{
			this.SetException(token.IsCancellationRequested ? new OperationCanceledException(token) : new OperationCanceledException());
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x00079286 File Offset: 0x00077486
		public void SetException(Exception exception)
		{
			this._error = ExceptionDispatchInfo.Capture(exception);
			this.NotifyAwaiter();
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x0007929C File Offset: 0x0007749C
		private void NotifyAwaiter()
		{
			Action action = this._continuation ?? Interlocked.CompareExchange<Action>(ref this._continuation, RendezvousAwaitable<TResult>.s_completionSentinel, null);
			if (action != null)
			{
				if (this.RunContinuationsAsynchronously)
				{
					Task.Run(action);
					return;
				}
				action();
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000792DE File Offset: 0x000774DE
		public void OnCompleted(Action continuation)
		{
			if ((this._continuation ?? Interlocked.CompareExchange<Action>(ref this._continuation, continuation, null)) != null)
			{
				Task.Run(continuation);
			}
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x00079300 File Offset: 0x00077500
		public void UnsafeOnCompleted(Action continuation)
		{
			this.OnCompleted(continuation);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x00004BF9 File Offset: 0x00002DF9
		[Conditional("DEBUG")]
		private void AssertResultConsistency(bool expectedCompleted)
		{
		}

		// Token: 0x04001BE2 RID: 7138
		private static readonly Action s_completionSentinel = delegate()
		{
		};

		// Token: 0x04001BE3 RID: 7139
		private Action _continuation;

		// Token: 0x04001BE4 RID: 7140
		private ExceptionDispatchInfo _error;

		// Token: 0x04001BE5 RID: 7141
		private TResult _result;
	}
}
