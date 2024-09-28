﻿using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000360 RID: 864
	internal class SystemThreadingTasks_TaskDebugView
	{
		// Token: 0x0600245D RID: 9309 RVA: 0x000824C7 File Offset: 0x000806C7
		public SystemThreadingTasks_TaskDebugView(Task task)
		{
			this.m_task = task;
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000824D6 File Offset: 0x000806D6
		public object AsyncState
		{
			get
			{
				return this.m_task.AsyncState;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000824E3 File Offset: 0x000806E3
		public TaskCreationOptions CreationOptions
		{
			get
			{
				return this.m_task.CreationOptions;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000824F0 File Offset: 0x000806F0
		public Exception Exception
		{
			get
			{
				return this.m_task.Exception;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06002461 RID: 9313 RVA: 0x000824FD File Offset: 0x000806FD
		public int Id
		{
			get
			{
				return this.m_task.Id;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x0008250C File Offset: 0x0008070C
		public bool CancellationPending
		{
			get
			{
				return this.m_task.Status == TaskStatus.WaitingToRun && this.m_task.CancellationToken.IsCancellationRequested;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06002463 RID: 9315 RVA: 0x0008253C File Offset: 0x0008073C
		public TaskStatus Status
		{
			get
			{
				return this.m_task.Status;
			}
		}

		// Token: 0x04001D04 RID: 7428
		private Task m_task;
	}
}
