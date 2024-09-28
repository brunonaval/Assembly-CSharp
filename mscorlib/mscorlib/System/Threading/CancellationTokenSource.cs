using System;
using System.Collections.Generic;

namespace System.Threading
{
	/// <summary>Signals to a <see cref="T:System.Threading.CancellationToken" /> that it should be canceled.</summary>
	// Token: 0x020002A8 RID: 680
	public class CancellationTokenSource : IDisposable
	{
		/// <summary>Gets whether cancellation has been requested for this <see cref="T:System.Threading.CancellationTokenSource" />.</summary>
		/// <returns>
		///   <see langword="true" /> if cancellation has been requested for this <see cref="T:System.Threading.CancellationTokenSource" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x0006F8F9 File Offset: 0x0006DAF9
		public bool IsCancellationRequested
		{
			get
			{
				return this._state >= 2;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0006F909 File Offset: 0x0006DB09
		internal bool IsCancellationCompleted
		{
			get
			{
				return this._state == 3;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0006F916 File Offset: 0x0006DB16
		internal bool IsDisposed
		{
			get
			{
				return this._disposed;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0006F91E File Offset: 0x0006DB1E
		// (set) Token: 0x06001E18 RID: 7704 RVA: 0x0006F928 File Offset: 0x0006DB28
		internal int ThreadIDExecutingCallbacks
		{
			get
			{
				return this._threadIDExecutingCallbacks;
			}
			set
			{
				this._threadIDExecutingCallbacks = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Threading.CancellationToken" /> associated with this <see cref="T:System.Threading.CancellationTokenSource" />.</summary>
		/// <returns>The <see cref="T:System.Threading.CancellationToken" /> associated with this <see cref="T:System.Threading.CancellationTokenSource" />.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The token source has been disposed.</exception>
		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0006F933 File Offset: 0x0006DB33
		public CancellationToken Token
		{
			get
			{
				this.ThrowIfDisposed();
				return new CancellationToken(this);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06001E1A RID: 7706 RVA: 0x0006F941 File Offset: 0x0006DB41
		internal bool CanBeCanceled
		{
			get
			{
				return this._state != 0;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0006F950 File Offset: 0x0006DB50
		internal WaitHandle WaitHandle
		{
			get
			{
				this.ThrowIfDisposed();
				if (this._kernelEvent != null)
				{
					return this._kernelEvent;
				}
				ManualResetEvent manualResetEvent = new ManualResetEvent(false);
				if (Interlocked.CompareExchange<ManualResetEvent>(ref this._kernelEvent, manualResetEvent, null) != null)
				{
					manualResetEvent.Dispose();
				}
				if (this.IsCancellationRequested)
				{
					this._kernelEvent.Set();
				}
				return this._kernelEvent;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001E1C RID: 7708 RVA: 0x0006F9B0 File Offset: 0x0006DBB0
		internal CancellationCallbackInfo ExecutingCallback
		{
			get
			{
				return this._executingCallback;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class.</summary>
		// Token: 0x06001E1D RID: 7709 RVA: 0x0006F9BA File Offset: 0x0006DBBA
		public CancellationTokenSource()
		{
			this._state = 1;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class that will be canceled after the specified time span.</summary>
		/// <param name="delay">The time interval to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="delay" />.<see cref="P:System.TimeSpan.TotalMilliseconds" /> is less than -1 or greater than <see cref="F:System.Int32.MaxValue" />.</exception>
		// Token: 0x06001E1E RID: 7710 RVA: 0x0006F9D4 File Offset: 0x0006DBD4
		public CancellationTokenSource(TimeSpan delay)
		{
			long num = (long)delay.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("delay");
			}
			this.InitializeWithTimer((int)num);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class that will be canceled after the specified delay in milliseconds.</summary>
		/// <param name="millisecondsDelay">The time interval in milliseconds to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="millisecondsDelay" /> is less than -1.</exception>
		// Token: 0x06001E1F RID: 7711 RVA: 0x0006FA1A File Offset: 0x0006DC1A
		public CancellationTokenSource(int millisecondsDelay)
		{
			if (millisecondsDelay < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsDelay");
			}
			this.InitializeWithTimer(millisecondsDelay);
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x0006FA41 File Offset: 0x0006DC41
		private void InitializeWithTimer(int millisecondsDelay)
		{
			this._state = 1;
			this._timer = new Timer(CancellationTokenSource.s_timerCallback, this, millisecondsDelay, -1);
		}

		/// <summary>Communicates a request for cancellation.</summary>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken" />.</exception>
		// Token: 0x06001E21 RID: 7713 RVA: 0x0006FA61 File Offset: 0x0006DC61
		public void Cancel()
		{
			this.Cancel(false);
		}

		/// <summary>Communicates a request for cancellation, and specifies whether remaining callbacks and cancelable operations should be processed if an exception occurs.</summary>
		/// <param name="throwOnFirstException">
		///   <see langword="true" /> if exceptions should immediately propagate; otherwise, <see langword="false" />.</param>
		/// <exception cref="T:System.ObjectDisposedException">This <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.AggregateException">An aggregate exception containing all the exceptions thrown by the registered callbacks on the associated <see cref="T:System.Threading.CancellationToken" />.</exception>
		// Token: 0x06001E22 RID: 7714 RVA: 0x0006FA6A File Offset: 0x0006DC6A
		public void Cancel(bool throwOnFirstException)
		{
			this.ThrowIfDisposed();
			this.NotifyCancellation(throwOnFirstException);
		}

		/// <summary>Schedules a cancel operation on this <see cref="T:System.Threading.CancellationTokenSource" /> after the specified time span.</summary>
		/// <param name="delay">The time span to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
		/// <exception cref="T:System.ObjectDisposedException">The exception thrown when this <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception that is thrown when <paramref name="delay" /> is less than -1 or greater than Int32.MaxValue.</exception>
		// Token: 0x06001E23 RID: 7715 RVA: 0x0006FA7C File Offset: 0x0006DC7C
		public void CancelAfter(TimeSpan delay)
		{
			long num = (long)delay.TotalMilliseconds;
			if (num < -1L || num > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("delay");
			}
			this.CancelAfter((int)num);
		}

		/// <summary>Schedules a cancel operation on this <see cref="T:System.Threading.CancellationTokenSource" /> after the specified number of milliseconds.</summary>
		/// <param name="millisecondsDelay">The time span to wait before canceling this <see cref="T:System.Threading.CancellationTokenSource" />.</param>
		/// <exception cref="T:System.ObjectDisposedException">The exception thrown when this <see cref="T:System.Threading.CancellationTokenSource" /> has been disposed.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The exception thrown when <paramref name="millisecondsDelay" /> is less than -1.</exception>
		// Token: 0x06001E24 RID: 7716 RVA: 0x0006FAB4 File Offset: 0x0006DCB4
		public void CancelAfter(int millisecondsDelay)
		{
			this.ThrowIfDisposed();
			if (millisecondsDelay < -1)
			{
				throw new ArgumentOutOfRangeException("millisecondsDelay");
			}
			if (this.IsCancellationRequested)
			{
				return;
			}
			if (this._timer == null)
			{
				Timer timer = new Timer(CancellationTokenSource.s_timerCallback, this, -1, -1);
				if (Interlocked.CompareExchange<Timer>(ref this._timer, timer, null) != null)
				{
					timer.Dispose();
				}
			}
			try
			{
				this._timer.Change(millisecondsDelay, -1);
			}
			catch (ObjectDisposedException)
			{
			}
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x0006FB34 File Offset: 0x0006DD34
		private static void TimerCallbackLogic(object obj)
		{
			CancellationTokenSource cancellationTokenSource = (CancellationTokenSource)obj;
			if (!cancellationTokenSource.IsDisposed)
			{
				try
				{
					cancellationTokenSource.Cancel();
				}
				catch (ObjectDisposedException)
				{
					if (!cancellationTokenSource.IsDisposed)
					{
						throw;
					}
				}
			}
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.CancellationTokenSource" /> class.</summary>
		// Token: 0x06001E26 RID: 7718 RVA: 0x0006FB78 File Offset: 0x0006DD78
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Threading.CancellationTokenSource" /> class and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x06001E27 RID: 7719 RVA: 0x0006FB88 File Offset: 0x0006DD88
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this._disposed)
			{
				Timer timer = this._timer;
				if (timer != null)
				{
					timer.Dispose();
				}
				this._registeredCallbacksLists = null;
				if (this._kernelEvent != null)
				{
					ManualResetEvent manualResetEvent = Interlocked.Exchange<ManualResetEvent>(ref this._kernelEvent, null);
					if (manualResetEvent != null && this._state != 2)
					{
						manualResetEvent.Dispose();
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x0006FBEE File Offset: 0x0006DDEE
		internal void ThrowIfDisposed()
		{
			if (this._disposed)
			{
				CancellationTokenSource.ThrowObjectDisposedException();
			}
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x0006FBFD File Offset: 0x0006DDFD
		private static void ThrowObjectDisposedException()
		{
			throw new ObjectDisposedException(null, "The CancellationTokenSource has been disposed.");
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x0006FC0C File Offset: 0x0006DE0C
		internal CancellationTokenRegistration InternalRegister(Action<object> callback, object stateForCallback, SynchronizationContext targetSyncContext, ExecutionContext executionContext)
		{
			if (!this.IsCancellationRequested)
			{
				if (this._disposed)
				{
					return default(CancellationTokenRegistration);
				}
				int num = Environment.CurrentManagedThreadId % CancellationTokenSource.s_nLists;
				CancellationCallbackInfo cancellationCallbackInfo = (targetSyncContext != null) ? new CancellationCallbackInfo.WithSyncContext(callback, stateForCallback, executionContext, this, targetSyncContext) : new CancellationCallbackInfo(callback, stateForCallback, executionContext, this);
				SparselyPopulatedArray<CancellationCallbackInfo>[] array = this._registeredCallbacksLists;
				if (array == null)
				{
					SparselyPopulatedArray<CancellationCallbackInfo>[] array2 = new SparselyPopulatedArray<CancellationCallbackInfo>[CancellationTokenSource.s_nLists];
					array = Interlocked.CompareExchange<SparselyPopulatedArray<CancellationCallbackInfo>[]>(ref this._registeredCallbacksLists, array2, null);
					if (array == null)
					{
						array = array2;
					}
				}
				SparselyPopulatedArray<CancellationCallbackInfo> sparselyPopulatedArray = Volatile.Read<SparselyPopulatedArray<CancellationCallbackInfo>>(ref array[num]);
				if (sparselyPopulatedArray == null)
				{
					SparselyPopulatedArray<CancellationCallbackInfo> value = new SparselyPopulatedArray<CancellationCallbackInfo>(4);
					Interlocked.CompareExchange<SparselyPopulatedArray<CancellationCallbackInfo>>(ref array[num], value, null);
					sparselyPopulatedArray = array[num];
				}
				SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> registrationInfo = sparselyPopulatedArray.Add(cancellationCallbackInfo);
				CancellationTokenRegistration result = new CancellationTokenRegistration(cancellationCallbackInfo, registrationInfo);
				if (!this.IsCancellationRequested)
				{
					return result;
				}
				if (!result.Unregister())
				{
					return result;
				}
			}
			callback(stateForCallback);
			return default(CancellationTokenRegistration);
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x0006FCF4 File Offset: 0x0006DEF4
		private void NotifyCancellation(bool throwOnFirstException)
		{
			if (!this.IsCancellationRequested && Interlocked.CompareExchange(ref this._state, 2, 1) == 1)
			{
				Timer timer = this._timer;
				if (timer != null)
				{
					timer.Dispose();
				}
				this.ThreadIDExecutingCallbacks = Environment.CurrentManagedThreadId;
				ManualResetEvent kernelEvent = this._kernelEvent;
				if (kernelEvent != null)
				{
					kernelEvent.Set();
				}
				this.ExecuteCallbackHandlers(throwOnFirstException);
			}
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x0006FD54 File Offset: 0x0006DF54
		private void ExecuteCallbackHandlers(bool throwOnFirstException)
		{
			LowLevelListWithIList<Exception> lowLevelListWithIList = null;
			SparselyPopulatedArray<CancellationCallbackInfo>[] registeredCallbacksLists = this._registeredCallbacksLists;
			if (registeredCallbacksLists == null)
			{
				Interlocked.Exchange(ref this._state, 3);
				return;
			}
			try
			{
				for (int i = 0; i < registeredCallbacksLists.Length; i++)
				{
					SparselyPopulatedArray<CancellationCallbackInfo> sparselyPopulatedArray = Volatile.Read<SparselyPopulatedArray<CancellationCallbackInfo>>(ref registeredCallbacksLists[i]);
					if (sparselyPopulatedArray != null)
					{
						for (SparselyPopulatedArrayFragment<CancellationCallbackInfo> sparselyPopulatedArrayFragment = sparselyPopulatedArray.Tail; sparselyPopulatedArrayFragment != null; sparselyPopulatedArrayFragment = sparselyPopulatedArrayFragment.Prev)
						{
							for (int j = sparselyPopulatedArrayFragment.Length - 1; j >= 0; j--)
							{
								this._executingCallback = sparselyPopulatedArrayFragment[j];
								if (this._executingCallback != null)
								{
									CancellationCallbackCoreWorkArguments cancellationCallbackCoreWorkArguments = new CancellationCallbackCoreWorkArguments(sparselyPopulatedArrayFragment, j);
									try
									{
										CancellationCallbackInfo.WithSyncContext withSyncContext = this._executingCallback as CancellationCallbackInfo.WithSyncContext;
										if (withSyncContext != null)
										{
											withSyncContext.TargetSyncContext.Send(new SendOrPostCallback(this.CancellationCallbackCoreWork_OnSyncContext), cancellationCallbackCoreWorkArguments);
											this.ThreadIDExecutingCallbacks = Environment.CurrentManagedThreadId;
										}
										else
										{
											this.CancellationCallbackCoreWork(cancellationCallbackCoreWorkArguments);
										}
									}
									catch (Exception item)
									{
										if (throwOnFirstException)
										{
											throw;
										}
										if (lowLevelListWithIList == null)
										{
											lowLevelListWithIList = new LowLevelListWithIList<Exception>();
										}
										lowLevelListWithIList.Add(item);
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				this._state = 3;
				this._executingCallback = null;
				Interlocked.MemoryBarrier();
			}
			if (lowLevelListWithIList != null)
			{
				throw new AggregateException(lowLevelListWithIList);
			}
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x0006FEAC File Offset: 0x0006E0AC
		private void CancellationCallbackCoreWork_OnSyncContext(object obj)
		{
			this.CancellationCallbackCoreWork((CancellationCallbackCoreWorkArguments)obj);
		}

		// Token: 0x06001E2E RID: 7726 RVA: 0x0006FEBC File Offset: 0x0006E0BC
		private void CancellationCallbackCoreWork(CancellationCallbackCoreWorkArguments args)
		{
			CancellationCallbackInfo cancellationCallbackInfo = args._currArrayFragment.SafeAtomicRemove(args._currArrayIndex, this._executingCallback);
			if (cancellationCallbackInfo == this._executingCallback)
			{
				cancellationCallbackInfo.CancellationTokenSource.ThreadIDExecutingCallbacks = Environment.CurrentManagedThreadId;
				cancellationCallbackInfo.ExecuteCallback();
			}
		}

		/// <summary>Creates a <see cref="T:System.Threading.CancellationTokenSource" /> that will be in the canceled state when any of the source tokens are in the canceled state.</summary>
		/// <param name="token1">The first cancellation token to observe.</param>
		/// <param name="token2">The second cancellation token to observe.</param>
		/// <returns>A <see cref="T:System.Threading.CancellationTokenSource" /> that is linked to the source tokens.</returns>
		/// <exception cref="T:System.ObjectDisposedException">A <see cref="T:System.Threading.CancellationTokenSource" /> associated with one of the source tokens has been disposed.</exception>
		// Token: 0x06001E2F RID: 7727 RVA: 0x0006FF04 File Offset: 0x0006E104
		public static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token1, CancellationToken token2)
		{
			if (!token1.CanBeCanceled)
			{
				return CancellationTokenSource.CreateLinkedTokenSource(token2);
			}
			if (!token2.CanBeCanceled)
			{
				return new CancellationTokenSource.Linked1CancellationTokenSource(token1);
			}
			return new CancellationTokenSource.Linked2CancellationTokenSource(token1, token2);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x0006FF2D File Offset: 0x0006E12D
		internal static CancellationTokenSource CreateLinkedTokenSource(CancellationToken token)
		{
			if (!token.CanBeCanceled)
			{
				return new CancellationTokenSource();
			}
			return new CancellationTokenSource.Linked1CancellationTokenSource(token);
		}

		/// <summary>Creates a <see cref="T:System.Threading.CancellationTokenSource" /> that will be in the canceled state when any of the source tokens in the specified array are in the canceled state.</summary>
		/// <param name="tokens">An array that contains the cancellation token instances to observe.</param>
		/// <returns>A <see cref="T:System.Threading.CancellationTokenSource" /> that is linked to the source tokens.</returns>
		/// <exception cref="T:System.ObjectDisposedException">A <see cref="T:System.Threading.CancellationTokenSource" /> associated with one of the source tokens has been disposed.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="tokens" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="tokens" /> is empty.</exception>
		// Token: 0x06001E31 RID: 7729 RVA: 0x0006FF44 File Offset: 0x0006E144
		public static CancellationTokenSource CreateLinkedTokenSource(params CancellationToken[] tokens)
		{
			if (tokens == null)
			{
				throw new ArgumentNullException("tokens");
			}
			switch (tokens.Length)
			{
			case 0:
				throw new ArgumentException("No tokens were supplied.");
			case 1:
				return CancellationTokenSource.CreateLinkedTokenSource(tokens[0]);
			case 2:
				return CancellationTokenSource.CreateLinkedTokenSource(tokens[0], tokens[1]);
			default:
				return new CancellationTokenSource.LinkedNCancellationTokenSource(tokens);
			}
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x0006FFAC File Offset: 0x0006E1AC
		internal void WaitForCallbackToComplete(CancellationCallbackInfo callbackInfo)
		{
			SpinWait spinWait = default(SpinWait);
			while (this.ExecutingCallback == callbackInfo)
			{
				spinWait.SpinOnce();
			}
		}

		// Token: 0x04001A79 RID: 6777
		internal static readonly CancellationTokenSource s_canceledSource = new CancellationTokenSource
		{
			_state = 3
		};

		// Token: 0x04001A7A RID: 6778
		internal static readonly CancellationTokenSource s_neverCanceledSource = new CancellationTokenSource
		{
			_state = 0
		};

		// Token: 0x04001A7B RID: 6779
		private static readonly int s_nLists = (PlatformHelper.ProcessorCount > 24) ? 24 : PlatformHelper.ProcessorCount;

		// Token: 0x04001A7C RID: 6780
		private volatile ManualResetEvent _kernelEvent;

		// Token: 0x04001A7D RID: 6781
		private volatile SparselyPopulatedArray<CancellationCallbackInfo>[] _registeredCallbacksLists;

		// Token: 0x04001A7E RID: 6782
		private const int CannotBeCanceled = 0;

		// Token: 0x04001A7F RID: 6783
		private const int NotCanceledState = 1;

		// Token: 0x04001A80 RID: 6784
		private const int NotifyingState = 2;

		// Token: 0x04001A81 RID: 6785
		private const int NotifyingCompleteState = 3;

		// Token: 0x04001A82 RID: 6786
		private volatile int _state;

		// Token: 0x04001A83 RID: 6787
		private volatile int _threadIDExecutingCallbacks = -1;

		// Token: 0x04001A84 RID: 6788
		private bool _disposed;

		// Token: 0x04001A85 RID: 6789
		private volatile CancellationCallbackInfo _executingCallback;

		// Token: 0x04001A86 RID: 6790
		private volatile Timer _timer;

		// Token: 0x04001A87 RID: 6791
		private static readonly TimerCallback s_timerCallback = new TimerCallback(CancellationTokenSource.TimerCallbackLogic);

		// Token: 0x020002A9 RID: 681
		private sealed class Linked1CancellationTokenSource : CancellationTokenSource
		{
			// Token: 0x06001E34 RID: 7732 RVA: 0x0007002F File Offset: 0x0006E22F
			internal Linked1CancellationTokenSource(CancellationToken token1)
			{
				this._reg1 = token1.InternalRegisterWithoutEC(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, this);
			}

			// Token: 0x06001E35 RID: 7733 RVA: 0x0007004A File Offset: 0x0006E24A
			protected override void Dispose(bool disposing)
			{
				if (!disposing || this._disposed)
				{
					return;
				}
				this._reg1.Dispose();
				base.Dispose(disposing);
			}

			// Token: 0x04001A88 RID: 6792
			private readonly CancellationTokenRegistration _reg1;
		}

		// Token: 0x020002AA RID: 682
		private sealed class Linked2CancellationTokenSource : CancellationTokenSource
		{
			// Token: 0x06001E36 RID: 7734 RVA: 0x0007006A File Offset: 0x0006E26A
			internal Linked2CancellationTokenSource(CancellationToken token1, CancellationToken token2)
			{
				this._reg1 = token1.InternalRegisterWithoutEC(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, this);
				this._reg2 = token2.InternalRegisterWithoutEC(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, this);
			}

			// Token: 0x06001E37 RID: 7735 RVA: 0x00070098 File Offset: 0x0006E298
			protected override void Dispose(bool disposing)
			{
				if (!disposing || this._disposed)
				{
					return;
				}
				this._reg1.Dispose();
				this._reg2.Dispose();
				base.Dispose(disposing);
			}

			// Token: 0x04001A89 RID: 6793
			private readonly CancellationTokenRegistration _reg1;

			// Token: 0x04001A8A RID: 6794
			private readonly CancellationTokenRegistration _reg2;
		}

		// Token: 0x020002AB RID: 683
		private sealed class LinkedNCancellationTokenSource : CancellationTokenSource
		{
			// Token: 0x06001E38 RID: 7736 RVA: 0x000700C4 File Offset: 0x0006E2C4
			internal LinkedNCancellationTokenSource(params CancellationToken[] tokens)
			{
				this._linkingRegistrations = new CancellationTokenRegistration[tokens.Length];
				for (int i = 0; i < tokens.Length; i++)
				{
					if (tokens[i].CanBeCanceled)
					{
						this._linkingRegistrations[i] = tokens[i].InternalRegisterWithoutEC(CancellationTokenSource.LinkedNCancellationTokenSource.s_linkedTokenCancelDelegate, this);
					}
				}
			}

			// Token: 0x06001E39 RID: 7737 RVA: 0x00070120 File Offset: 0x0006E320
			protected override void Dispose(bool disposing)
			{
				if (!disposing || this._disposed)
				{
					return;
				}
				CancellationTokenRegistration[] linkingRegistrations = this._linkingRegistrations;
				if (linkingRegistrations != null)
				{
					this._linkingRegistrations = null;
					for (int i = 0; i < linkingRegistrations.Length; i++)
					{
						linkingRegistrations[i].Dispose();
					}
				}
				base.Dispose(disposing);
			}

			// Token: 0x04001A8B RID: 6795
			internal static readonly Action<object> s_linkedTokenCancelDelegate = delegate(object s)
			{
				((CancellationTokenSource)s).NotifyCancellation(false);
			};

			// Token: 0x04001A8C RID: 6796
			private CancellationTokenRegistration[] _linkingRegistrations;
		}
	}
}
