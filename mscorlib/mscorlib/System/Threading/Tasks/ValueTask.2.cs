﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Sources;

namespace System.Threading.Tasks
{
	// Token: 0x02000312 RID: 786
	[AsyncMethodBuilder(typeof(AsyncValueTaskMethodBuilder<>))]
	[StructLayout(LayoutKind.Auto)]
	public readonly struct ValueTask<TResult> : IEquatable<ValueTask<TResult>>
	{
		// Token: 0x060021A6 RID: 8614 RVA: 0x00078C34 File Offset: 0x00076E34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTask(TResult result)
		{
			this._result = result;
			this._obj = null;
			this._continueOnCapturedContext = true;
			this._token = 0;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00078C52 File Offset: 0x00076E52
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTask(Task<TResult> task)
		{
			if (task == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.task);
			}
			this._obj = task;
			this._result = default(TResult);
			this._continueOnCapturedContext = true;
			this._token = 0;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00078C7F File Offset: 0x00076E7F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTask(IValueTaskSource<TResult> source, short token)
		{
			if (source == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
			}
			this._obj = source;
			this._token = token;
			this._result = default(TResult);
			this._continueOnCapturedContext = true;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00078CAC File Offset: 0x00076EAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ValueTask(object obj, TResult result, short token, bool continueOnCapturedContext)
		{
			this._obj = obj;
			this._result = result;
			this._token = token;
			this._continueOnCapturedContext = continueOnCapturedContext;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00078CCC File Offset: 0x00076ECC
		public override int GetHashCode()
		{
			if (this._obj != null)
			{
				return this._obj.GetHashCode();
			}
			if (this._result == null)
			{
				return 0;
			}
			TResult result = this._result;
			return result.GetHashCode();
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00078D10 File Offset: 0x00076F10
		public override bool Equals(object obj)
		{
			return obj is ValueTask<TResult> && this.Equals((ValueTask<TResult>)obj);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x00078D28 File Offset: 0x00076F28
		public bool Equals(ValueTask<TResult> other)
		{
			if (this._obj == null && other._obj == null)
			{
				return EqualityComparer<TResult>.Default.Equals(this._result, other._result);
			}
			return this._obj == other._obj && this._token == other._token;
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00078D7A File Offset: 0x00076F7A
		public static bool operator ==(ValueTask<TResult> left, ValueTask<TResult> right)
		{
			return left.Equals(right);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00078D84 File Offset: 0x00076F84
		public static bool operator !=(ValueTask<TResult> left, ValueTask<TResult> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x00078D94 File Offset: 0x00076F94
		public Task<TResult> AsTask()
		{
			object obj = this._obj;
			if (obj == null)
			{
				return AsyncTaskMethodBuilder<TResult>.GetTaskForResult(this._result);
			}
			Task<TResult> task = obj as Task<TResult>;
			if (task != null)
			{
				return task;
			}
			return this.GetTaskForValueTaskSource(Unsafe.As<IValueTaskSource<TResult>>(obj));
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00078DCF File Offset: 0x00076FCF
		public ValueTask<TResult> Preserve()
		{
			if (this._obj != null)
			{
				return new ValueTask<TResult>(this.AsTask());
			}
			return this;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00078DEC File Offset: 0x00076FEC
		private Task<TResult> GetTaskForValueTaskSource(IValueTaskSource<TResult> t)
		{
			ValueTaskSourceStatus status = t.GetStatus(this._token);
			if (status != ValueTaskSourceStatus.Pending)
			{
				try
				{
					return AsyncTaskMethodBuilder<TResult>.GetTaskForResult(t.GetResult(this._token));
				}
				catch (Exception ex)
				{
					if (status != ValueTaskSourceStatus.Canceled)
					{
						return Task.FromException<TResult>(ex);
					}
					OperationCanceledException ex2 = ex as OperationCanceledException;
					if (ex2 != null)
					{
						Task<TResult> task = new Task<TResult>();
						task.TrySetCanceled(ex2.CancellationToken, ex2);
						return task;
					}
					Task<TResult> task2 = ValueTask<TResult>.s_canceledTask;
					if (task2 == null)
					{
						task2 = Task.FromCanceled<TResult>(new CancellationToken(true));
						ValueTask<TResult>.s_canceledTask = task2;
					}
					return task2;
				}
			}
			return new ValueTask<TResult>.ValueTaskSourceAsTask(t, this._token);
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x060021B2 RID: 8626 RVA: 0x00078E90 File Offset: 0x00077090
		public bool IsCompleted
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				object obj = this._obj;
				if (obj == null)
				{
					return true;
				}
				Task<TResult> task = obj as Task<TResult>;
				if (task != null)
				{
					return task.IsCompleted;
				}
				return Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) > ValueTaskSourceStatus.Pending;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x060021B3 RID: 8627 RVA: 0x00078ED0 File Offset: 0x000770D0
		public bool IsCompletedSuccessfully
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				object obj = this._obj;
				if (obj == null)
				{
					return true;
				}
				Task<TResult> task = obj as Task<TResult>;
				if (task != null)
				{
					return task.IsCompletedSuccessfully;
				}
				return Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Succeeded;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060021B4 RID: 8628 RVA: 0x00078F10 File Offset: 0x00077110
		public bool IsFaulted
		{
			get
			{
				object obj = this._obj;
				if (obj == null)
				{
					return false;
				}
				Task<TResult> task = obj as Task<TResult>;
				if (task != null)
				{
					return task.IsFaulted;
				}
				return Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Faulted;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x00078F50 File Offset: 0x00077150
		public bool IsCanceled
		{
			get
			{
				object obj = this._obj;
				if (obj == null)
				{
					return false;
				}
				Task<TResult> task = obj as Task<TResult>;
				if (task != null)
				{
					return task.IsCanceled;
				}
				return Unsafe.As<IValueTaskSource<TResult>>(obj).GetStatus(this._token) == ValueTaskSourceStatus.Canceled;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x00078F90 File Offset: 0x00077190
		public TResult Result
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				object obj = this._obj;
				if (obj == null)
				{
					return this._result;
				}
				Task<TResult> task = obj as Task<TResult>;
				if (task != null)
				{
					TaskAwaiter.ValidateEnd(task);
					return task.ResultOnSuccess;
				}
				return Unsafe.As<IValueTaskSource<TResult>>(obj).GetResult(this._token);
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x00078FD6 File Offset: 0x000771D6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTaskAwaiter<TResult> GetAwaiter()
		{
			return new ValueTaskAwaiter<TResult>(this);
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x00078FE3 File Offset: 0x000771E3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredValueTaskAwaitable<TResult> ConfigureAwait(bool continueOnCapturedContext)
		{
			return new ConfiguredValueTaskAwaitable<TResult>(new ValueTask<TResult>(this._obj, this._result, this._token, continueOnCapturedContext));
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x00079004 File Offset: 0x00077204
		public override string ToString()
		{
			if (this.IsCompletedSuccessfully)
			{
				TResult result = this.Result;
				if (result != null)
				{
					return result.ToString();
				}
			}
			return string.Empty;
		}

		// Token: 0x04001BD6 RID: 7126
		private static Task<TResult> s_canceledTask;

		// Token: 0x04001BD7 RID: 7127
		internal readonly object _obj;

		// Token: 0x04001BD8 RID: 7128
		internal readonly TResult _result;

		// Token: 0x04001BD9 RID: 7129
		internal readonly short _token;

		// Token: 0x04001BDA RID: 7130
		internal readonly bool _continueOnCapturedContext;

		// Token: 0x02000313 RID: 787
		private sealed class ValueTaskSourceAsTask : Task<TResult>
		{
			// Token: 0x060021BA RID: 8634 RVA: 0x0007903B File Offset: 0x0007723B
			public ValueTaskSourceAsTask(IValueTaskSource<TResult> source, short token)
			{
				this._source = source;
				this._token = token;
				source.OnCompleted(ValueTask<TResult>.ValueTaskSourceAsTask.s_completionAction, this, token, ValueTaskSourceOnCompletedFlags.None);
			}

			// Token: 0x04001BDB RID: 7131
			private static readonly Action<object> s_completionAction = delegate(object state)
			{
				ValueTask<TResult>.ValueTaskSourceAsTask valueTaskSourceAsTask = state as ValueTask<TResult>.ValueTaskSourceAsTask;
				if (valueTaskSourceAsTask != null)
				{
					IValueTaskSource<TResult> source = valueTaskSourceAsTask._source;
					if (source != null)
					{
						valueTaskSourceAsTask._source = null;
						ValueTaskSourceStatus status = source.GetStatus(valueTaskSourceAsTask._token);
						try
						{
							valueTaskSourceAsTask.TrySetResult(source.GetResult(valueTaskSourceAsTask._token));
						}
						catch (Exception ex)
						{
							if (status == ValueTaskSourceStatus.Canceled)
							{
								OperationCanceledException ex2 = ex as OperationCanceledException;
								if (ex2 != null)
								{
									valueTaskSourceAsTask.TrySetCanceled(ex2.CancellationToken, ex2);
								}
								else
								{
									valueTaskSourceAsTask.TrySetCanceled(new CancellationToken(true));
								}
							}
							else
							{
								valueTaskSourceAsTask.TrySetException(ex);
							}
						}
						return;
					}
				}
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.state);
			};

			// Token: 0x04001BDC RID: 7132
			private IValueTaskSource<TResult> _source;

			// Token: 0x04001BDD RID: 7133
			private readonly short _token;
		}
	}
}
