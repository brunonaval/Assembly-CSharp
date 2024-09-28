using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200080C RID: 2060
	public readonly struct ValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
	{
		// Token: 0x0600462B RID: 17963 RVA: 0x000E5978 File Offset: 0x000E3B78
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ValueTaskAwaiter(ValueTask value)
		{
			this._value = value;
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x000E5981 File Offset: 0x000E3B81
		public bool IsCompleted
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this._value.IsCompleted;
			}
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x000E598E File Offset: 0x000E3B8E
		[StackTraceHidden]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetResult()
		{
			this._value.ThrowIfCompletedUnsuccessfully();
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x000E599C File Offset: 0x000E3B9C
		public void OnCompleted(Action continuation)
		{
			object obj = this._value._obj;
			Task task = obj as Task;
			if (task != null)
			{
				task.GetAwaiter().OnCompleted(continuation);
				return;
			}
			if (obj != null)
			{
				Unsafe.As<IValueTaskSource>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, this._value._token, ValueTaskSourceOnCompletedFlags.UseSchedulingContext | ValueTaskSourceOnCompletedFlags.FlowExecutionContext);
				return;
			}
			ValueTask.CompletedTask.GetAwaiter().OnCompleted(continuation);
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x000E5A04 File Offset: 0x000E3C04
		public void UnsafeOnCompleted(Action continuation)
		{
			object obj = this._value._obj;
			Task task = obj as Task;
			if (task != null)
			{
				task.GetAwaiter().UnsafeOnCompleted(continuation);
				return;
			}
			if (obj != null)
			{
				Unsafe.As<IValueTaskSource>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, this._value._token, ValueTaskSourceOnCompletedFlags.UseSchedulingContext);
				return;
			}
			ValueTask.CompletedTask.GetAwaiter().UnsafeOnCompleted(continuation);
		}

		// Token: 0x04002D45 RID: 11589
		internal static readonly Action<object> s_invokeActionDelegate = delegate(object state)
		{
			Action action = state as Action;
			if (action == null)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.state);
				return;
			}
			action();
		};

		// Token: 0x04002D46 RID: 11590
		private readonly ValueTask _value;
	}
}
