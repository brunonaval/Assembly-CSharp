﻿using System;

namespace System.Threading
{
	/// <summary>Specifies the execution states of a <see cref="T:System.Threading.Thread" />.</summary>
	// Token: 0x02000299 RID: 665
	[Flags]
	public enum ThreadState
	{
		/// <summary>The thread has been started and not yet stopped.</summary>
		// Token: 0x04001A45 RID: 6725
		Running = 0,
		/// <summary>The thread is being requested to stop. This is for internal use only.</summary>
		// Token: 0x04001A46 RID: 6726
		StopRequested = 1,
		/// <summary>The thread is being requested to suspend.</summary>
		// Token: 0x04001A47 RID: 6727
		SuspendRequested = 2,
		/// <summary>The thread is being executed as a background thread, as opposed to a foreground thread. This state is controlled by setting the <see cref="P:System.Threading.Thread.IsBackground" /> property.</summary>
		// Token: 0x04001A48 RID: 6728
		Background = 4,
		/// <summary>The <see cref="M:System.Threading.Thread.Start" /> method has not been invoked on the thread.</summary>
		// Token: 0x04001A49 RID: 6729
		Unstarted = 8,
		/// <summary>The thread has stopped.</summary>
		// Token: 0x04001A4A RID: 6730
		Stopped = 16,
		/// <summary>The thread is blocked. This could be the result of calling <see cref="M:System.Threading.Thread.Sleep(System.Int32)" /> or <see cref="M:System.Threading.Thread.Join" />, of requesting a lock - for example, by calling <see cref="M:System.Threading.Monitor.Enter(System.Object)" /> or <see cref="M:System.Threading.Monitor.Wait(System.Object,System.Int32,System.Boolean)" /> - or of waiting on a thread synchronization object such as <see cref="T:System.Threading.ManualResetEvent" />.</summary>
		// Token: 0x04001A4B RID: 6731
		WaitSleepJoin = 32,
		/// <summary>The thread has been suspended.</summary>
		// Token: 0x04001A4C RID: 6732
		Suspended = 64,
		/// <summary>The <see cref="M:System.Threading.Thread.Abort(System.Object)" /> method has been invoked on the thread, but the thread has not yet received the pending <see cref="T:System.Threading.ThreadAbortException" /> that will attempt to terminate it.</summary>
		// Token: 0x04001A4D RID: 6733
		AbortRequested = 128,
		/// <summary>The thread state includes <see cref="F:System.Threading.ThreadState.AbortRequested" /> and the thread is now dead, but its state has not yet changed to <see cref="F:System.Threading.ThreadState.Stopped" />.</summary>
		// Token: 0x04001A4E RID: 6734
		Aborted = 256
	}
}