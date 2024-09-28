﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007E7 RID: 2023
	[StructLayout(LayoutKind.Auto)]
	public readonly struct ConfiguredValueTaskAwaitable
	{
		// Token: 0x060045E3 RID: 17891 RVA: 0x000E543A File Offset: 0x000E363A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal ConfiguredValueTaskAwaitable(ValueTask value)
		{
			this._value = value;
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x000E5443 File Offset: 0x000E3643
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter GetAwaiter()
		{
			return new ConfiguredValueTaskAwaitable.ConfiguredValueTaskAwaiter(this._value);
		}

		// Token: 0x04002D32 RID: 11570
		private readonly ValueTask _value;

		// Token: 0x020007E8 RID: 2024
		[StructLayout(LayoutKind.Auto)]
		public readonly struct ConfiguredValueTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
		{
			// Token: 0x060045E5 RID: 17893 RVA: 0x000E5450 File Offset: 0x000E3650
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal ConfiguredValueTaskAwaiter(ValueTask value)
			{
				this._value = value;
			}

			// Token: 0x17000ABA RID: 2746
			// (get) Token: 0x060045E6 RID: 17894 RVA: 0x000E5459 File Offset: 0x000E3659
			public bool IsCompleted
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					return this._value.IsCompleted;
				}
			}

			// Token: 0x060045E7 RID: 17895 RVA: 0x000E5466 File Offset: 0x000E3666
			[StackTraceHidden]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void GetResult()
			{
				this._value.ThrowIfCompletedUnsuccessfully();
			}

			// Token: 0x060045E8 RID: 17896 RVA: 0x000E5474 File Offset: 0x000E3674
			public void OnCompleted(Action continuation)
			{
				object obj = this._value._obj;
				Task task = obj as Task;
				if (task != null)
				{
					task.ConfigureAwait(this._value._continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
					return;
				}
				if (obj != null)
				{
					Unsafe.As<IValueTaskSource>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, this._value._token, ValueTaskSourceOnCompletedFlags.FlowExecutionContext | (this._value._continueOnCapturedContext ? ValueTaskSourceOnCompletedFlags.UseSchedulingContext : ValueTaskSourceOnCompletedFlags.None));
					return;
				}
				ValueTask.CompletedTask.ConfigureAwait(this._value._continueOnCapturedContext).GetAwaiter().OnCompleted(continuation);
			}

			// Token: 0x060045E9 RID: 17897 RVA: 0x000E5514 File Offset: 0x000E3714
			public void UnsafeOnCompleted(Action continuation)
			{
				object obj = this._value._obj;
				Task task = obj as Task;
				if (task != null)
				{
					task.ConfigureAwait(this._value._continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
					return;
				}
				if (obj != null)
				{
					Unsafe.As<IValueTaskSource>(obj).OnCompleted(ValueTaskAwaiter.s_invokeActionDelegate, continuation, this._value._token, this._value._continueOnCapturedContext ? ValueTaskSourceOnCompletedFlags.UseSchedulingContext : ValueTaskSourceOnCompletedFlags.None);
					return;
				}
				ValueTask.CompletedTask.ConfigureAwait(this._value._continueOnCapturedContext).GetAwaiter().UnsafeOnCompleted(continuation);
			}

			// Token: 0x04002D33 RID: 11571
			private readonly ValueTask _value;
		}
	}
}
