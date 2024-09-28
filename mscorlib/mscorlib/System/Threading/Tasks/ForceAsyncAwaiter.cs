﻿using System;
using System.Runtime.CompilerServices;

namespace System.Threading.Tasks
{
	// Token: 0x02000318 RID: 792
	internal readonly struct ForceAsyncAwaiter : ICriticalNotifyCompletion, INotifyCompletion
	{
		// Token: 0x060021C5 RID: 8645 RVA: 0x00079171 File Offset: 0x00077371
		internal ForceAsyncAwaiter(Task task)
		{
			this._task = task;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0007917A File Offset: 0x0007737A
		public ForceAsyncAwaiter GetAwaiter()
		{
			return this;
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060021C7 RID: 8647 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public bool IsCompleted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x00079184 File Offset: 0x00077384
		public void GetResult()
		{
			this._task.GetAwaiter().GetResult();
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000791A4 File Offset: 0x000773A4
		public void OnCompleted(Action action)
		{
			this._task.ConfigureAwait(false).GetAwaiter().OnCompleted(action);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000791D0 File Offset: 0x000773D0
		public void UnsafeOnCompleted(Action action)
		{
			this._task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(action);
		}

		// Token: 0x04001BE1 RID: 7137
		private readonly Task _task;
	}
}
