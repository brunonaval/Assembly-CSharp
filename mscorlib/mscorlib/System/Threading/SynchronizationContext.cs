﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Permissions;

namespace System.Threading
{
	/// <summary>Provides the basic functionality for propagating a synchronization context in various synchronization models.</summary>
	// Token: 0x020002D1 RID: 721
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = (SecurityPermissionFlag.ControlEvidence | SecurityPermissionFlag.ControlPolicy))]
	public class SynchronizationContext
	{
		/// <summary>Sets notification that wait notification is required and prepares the callback method so it can be called more reliably when a wait occurs.</summary>
		// Token: 0x06001F46 RID: 8006 RVA: 0x000739A8 File Offset: 0x00071BA8
		[SecuritySafeCritical]
		protected void SetWaitNotificationRequired()
		{
			Type type = base.GetType();
			if (SynchronizationContext.s_cachedPreparedType1 != type && SynchronizationContext.s_cachedPreparedType2 != type && SynchronizationContext.s_cachedPreparedType3 != type && SynchronizationContext.s_cachedPreparedType4 != type && SynchronizationContext.s_cachedPreparedType5 != type)
			{
				RuntimeHelpers.PrepareDelegate(new SynchronizationContext.WaitDelegate(this.Wait));
				if (SynchronizationContext.s_cachedPreparedType1 == null)
				{
					SynchronizationContext.s_cachedPreparedType1 = type;
				}
				else if (SynchronizationContext.s_cachedPreparedType2 == null)
				{
					SynchronizationContext.s_cachedPreparedType2 = type;
				}
				else if (SynchronizationContext.s_cachedPreparedType3 == null)
				{
					SynchronizationContext.s_cachedPreparedType3 = type;
				}
				else if (SynchronizationContext.s_cachedPreparedType4 == null)
				{
					SynchronizationContext.s_cachedPreparedType4 = type;
				}
				else if (SynchronizationContext.s_cachedPreparedType5 == null)
				{
					SynchronizationContext.s_cachedPreparedType5 = type;
				}
			}
			this._props |= SynchronizationContextProperties.RequireWaitNotification;
		}

		/// <summary>Determines if wait notification is required.</summary>
		/// <returns>
		///   <see langword="true" /> if wait notification is required; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001F47 RID: 8007 RVA: 0x00073A90 File Offset: 0x00071C90
		public bool IsWaitNotificationRequired()
		{
			return (this._props & SynchronizationContextProperties.RequireWaitNotification) > SynchronizationContextProperties.None;
		}

		/// <summary>When overridden in a derived class, dispatches a synchronous message to a synchronization context.</summary>
		/// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		/// <exception cref="T:System.NotSupportedException">The method was called in a Windows Store app. The implementation of <see cref="T:System.Threading.SynchronizationContext" /> for Windows Store apps does not support the <see cref="M:System.Threading.SynchronizationContext.Send(System.Threading.SendOrPostCallback,System.Object)" /> method.</exception>
		// Token: 0x06001F48 RID: 8008 RVA: 0x00073A9D File Offset: 0x00071C9D
		public virtual void Send(SendOrPostCallback d, object state)
		{
			d(state);
		}

		/// <summary>When overridden in a derived class, dispatches an asynchronous message to a synchronization context.</summary>
		/// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
		/// <param name="state">The object passed to the delegate.</param>
		// Token: 0x06001F49 RID: 8009 RVA: 0x00073AA6 File Offset: 0x00071CA6
		public virtual void Post(SendOrPostCallback d, object state)
		{
			ThreadPool.QueueUserWorkItem(new WaitCallback(d.Invoke), state);
		}

		/// <summary>When overridden in a derived class, responds to the notification that an operation has started.</summary>
		// Token: 0x06001F4A RID: 8010 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void OperationStarted()
		{
		}

		/// <summary>When overridden in a derived class, responds to the notification that an operation has completed.</summary>
		// Token: 0x06001F4B RID: 8011 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void OperationCompleted()
		{
		}

		/// <summary>Waits for any or all the elements in the specified array to receive a signal.</summary>
		/// <param name="waitHandles">An array of type <see cref="T:System.IntPtr" /> that contains the native operating system handles.</param>
		/// <param name="waitAll">
		///   <see langword="true" /> to wait for all handles; <see langword="false" /> to wait for any handle.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>The array index of the object that satisfied the wait.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="waitHandles" /> is null.</exception>
		// Token: 0x06001F4C RID: 8012 RVA: 0x00073ABB File Offset: 0x00071CBB
		[SecurityCritical]
		[CLSCompliant(false)]
		[PrePrepareMethod]
		public virtual int Wait(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			if (waitHandles == null)
			{
				throw new ArgumentNullException("waitHandles");
			}
			return SynchronizationContext.WaitHelper(waitHandles, waitAll, millisecondsTimeout);
		}

		/// <summary>Helper function that waits for any or all the elements in the specified array to receive a signal.</summary>
		/// <param name="waitHandles">An array of type <see cref="T:System.IntPtr" /> that contains the native operating system handles.</param>
		/// <param name="waitAll">
		///   <see langword="true" /> to wait for all handles;  <see langword="false" /> to wait for any handle.</param>
		/// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="F:System.Threading.Timeout.Infinite" /> (-1) to wait indefinitely.</param>
		/// <returns>The array index of the object that satisfied the wait.</returns>
		// Token: 0x06001F4D RID: 8013 RVA: 0x00073AD4 File Offset: 0x00071CD4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[SecurityCritical]
		[CLSCompliant(false)]
		[PrePrepareMethod]
		protected unsafe static int WaitHelper(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			IntPtr* handles;
			if (waitHandles == null || waitHandles.Length == 0)
			{
				handles = null;
			}
			else
			{
				handles = &waitHandles[0];
			}
			return WaitHandle.Wait_internal(handles, waitHandles.Length, waitAll, millisecondsTimeout);
		}

		/// <summary>Sets the current synchronization context.</summary>
		/// <param name="syncContext">The <see cref="T:System.Threading.SynchronizationContext" /> object to be set.</param>
		// Token: 0x06001F4E RID: 8014 RVA: 0x00073B04 File Offset: 0x00071D04
		[SecurityCritical]
		public static void SetSynchronizationContext(SynchronizationContext syncContext)
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			mutableExecutionContext.SynchronizationContext = syncContext;
			mutableExecutionContext.SynchronizationContextNoFlow = syncContext;
		}

		/// <summary>Gets the synchronization context for the current thread.</summary>
		/// <returns>A <see cref="T:System.Threading.SynchronizationContext" /> object representing the current synchronization context.</returns>
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x00073B20 File Offset: 0x00071D20
		public static SynchronizationContext Current
		{
			get
			{
				return Thread.CurrentThread.GetExecutionContextReader().SynchronizationContext ?? SynchronizationContext.GetThreadLocalContext();
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x00073B48 File Offset: 0x00071D48
		internal static SynchronizationContext CurrentNoFlow
		{
			[FriendAccessAllowed]
			get
			{
				return Thread.CurrentThread.GetExecutionContextReader().SynchronizationContextNoFlow ?? SynchronizationContext.GetThreadLocalContext();
			}
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x0000AF5E File Offset: 0x0000915E
		private static SynchronizationContext GetThreadLocalContext()
		{
			return null;
		}

		/// <summary>When overridden in a derived class, creates a copy of the synchronization context.</summary>
		/// <returns>A new <see cref="T:System.Threading.SynchronizationContext" /> object.</returns>
		// Token: 0x06001F52 RID: 8018 RVA: 0x00073B70 File Offset: 0x00071D70
		public virtual SynchronizationContext CreateCopy()
		{
			return new SynchronizationContext();
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x00073B77 File Offset: 0x00071D77
		[SecurityCritical]
		private static int InvokeWaitMethodHelper(SynchronizationContext syncContext, IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout)
		{
			return syncContext.Wait(waitHandles, waitAll, millisecondsTimeout);
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x00073B82 File Offset: 0x00071D82
		internal static SynchronizationContext CurrentExplicit
		{
			get
			{
				return SynchronizationContext.Current;
			}
		}

		// Token: 0x04001B0B RID: 6923
		private SynchronizationContextProperties _props;

		// Token: 0x04001B0C RID: 6924
		private static Type s_cachedPreparedType1;

		// Token: 0x04001B0D RID: 6925
		private static Type s_cachedPreparedType2;

		// Token: 0x04001B0E RID: 6926
		private static Type s_cachedPreparedType3;

		// Token: 0x04001B0F RID: 6927
		private static Type s_cachedPreparedType4;

		// Token: 0x04001B10 RID: 6928
		private static Type s_cachedPreparedType5;

		// Token: 0x020002D2 RID: 722
		// (Invoke) Token: 0x06001F56 RID: 8022
		private delegate int WaitDelegate(IntPtr[] waitHandles, bool waitAll, int millisecondsTimeout);
	}
}
