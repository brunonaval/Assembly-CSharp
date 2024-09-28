﻿using System;
using System.Threading.Tasks;

namespace System.Threading
{
	/// <summary>Represents a callback delegate that has been registered with a <see cref="T:System.Threading.CancellationToken" />.</summary>
	// Token: 0x020002A7 RID: 679
	public readonly struct CancellationTokenRegistration : IEquatable<CancellationTokenRegistration>, IDisposable, IAsyncDisposable
	{
		// Token: 0x06001E0A RID: 7690 RVA: 0x0006F71A File Offset: 0x0006D91A
		internal CancellationTokenRegistration(CancellationCallbackInfo callbackInfo, SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> registrationInfo)
		{
			this.m_callbackInfo = callbackInfo;
			this.m_registrationInfo = registrationInfo;
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x0006F72C File Offset: 0x0006D92C
		public CancellationToken Token
		{
			get
			{
				CancellationCallbackInfo callbackInfo = this.m_callbackInfo;
				if (callbackInfo == null)
				{
					return default(CancellationToken);
				}
				return callbackInfo.CancellationTokenSource.Token;
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0006F758 File Offset: 0x0006D958
		public bool Unregister()
		{
			return this.m_registrationInfo.Source != null && this.m_registrationInfo.Source.SafeAtomicRemove(this.m_registrationInfo.Index, this.m_callbackInfo) == this.m_callbackInfo;
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.CancellationTokenRegistration" /> class.</summary>
		// Token: 0x06001E0D RID: 7693 RVA: 0x0006F7AC File Offset: 0x0006D9AC
		public void Dispose()
		{
			bool flag = this.Unregister();
			CancellationCallbackInfo callbackInfo = this.m_callbackInfo;
			if (callbackInfo != null)
			{
				CancellationTokenSource cancellationTokenSource = callbackInfo.CancellationTokenSource;
				if (cancellationTokenSource.IsCancellationRequested && !cancellationTokenSource.IsCancellationCompleted && !flag && cancellationTokenSource.ThreadIDExecutingCallbacks != Environment.CurrentManagedThreadId)
				{
					cancellationTokenSource.WaitForCallbackToComplete(this.m_callbackInfo);
				}
			}
		}

		/// <summary>Determines whether two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are equal.</summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True if the instances are equal; otherwise, false.</returns>
		// Token: 0x06001E0E RID: 7694 RVA: 0x0006F7FD File Offset: 0x0006D9FD
		public static bool operator ==(CancellationTokenRegistration left, CancellationTokenRegistration right)
		{
			return left.Equals(right);
		}

		/// <summary>Determines whether two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are not equal.</summary>
		/// <param name="left">The first instance.</param>
		/// <param name="right">The second instance.</param>
		/// <returns>True if the instances are not equal; otherwise, false.</returns>
		// Token: 0x06001E0F RID: 7695 RVA: 0x0006F807 File Offset: 0x0006DA07
		public static bool operator !=(CancellationTokenRegistration left, CancellationTokenRegistration right)
		{
			return !left.Equals(right);
		}

		/// <summary>Determines whether the current <see cref="T:System.Threading.CancellationTokenRegistration" /> instance is equal to the specified <see cref="T:System.Threading.CancellationTokenRegistration" />.</summary>
		/// <param name="obj">The other object to which to compare this instance.</param>
		/// <returns>True, if both this and <paramref name="obj" /> are equal. False, otherwise.  
		///  Two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are equal if they both refer to the output of a single call to the same Register method of a <see cref="T:System.Threading.CancellationToken" />.</returns>
		// Token: 0x06001E10 RID: 7696 RVA: 0x0006F814 File Offset: 0x0006DA14
		public override bool Equals(object obj)
		{
			return obj is CancellationTokenRegistration && this.Equals((CancellationTokenRegistration)obj);
		}

		/// <summary>Determines whether the current <see cref="T:System.Threading.CancellationTokenRegistration" /> instance is equal to the specified <see cref="T:System.Threading.CancellationTokenRegistration" />.</summary>
		/// <param name="other">The other <see cref="T:System.Threading.CancellationTokenRegistration" /> to which to compare this instance.</param>
		/// <returns>True, if both this and <paramref name="other" /> are equal. False, otherwise.  
		///  Two <see cref="T:System.Threading.CancellationTokenRegistration" /> instances are equal if they both refer to the output of a single call to the same Register method of a <see cref="T:System.Threading.CancellationToken" />.</returns>
		// Token: 0x06001E11 RID: 7697 RVA: 0x0006F82C File Offset: 0x0006DA2C
		public bool Equals(CancellationTokenRegistration other)
		{
			return this.m_callbackInfo == other.m_callbackInfo && this.m_registrationInfo.Source == other.m_registrationInfo.Source && this.m_registrationInfo.Index == other.m_registrationInfo.Index;
		}

		/// <summary>Serves as a hash function for a <see cref="T:System.Threading.CancellationTokenRegistration" />.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Threading.CancellationTokenRegistration" /> instance.</returns>
		// Token: 0x06001E12 RID: 7698 RVA: 0x0006F888 File Offset: 0x0006DA88
		public override int GetHashCode()
		{
			if (this.m_registrationInfo.Source != null)
			{
				return this.m_registrationInfo.Source.GetHashCode() ^ this.m_registrationInfo.Index.GetHashCode();
			}
			return this.m_registrationInfo.Index.GetHashCode();
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x0006F8E6 File Offset: 0x0006DAE6
		public ValueTask DisposeAsync()
		{
			this.Dispose();
			return new ValueTask(Task.FromResult<object>(null));
		}

		// Token: 0x04001A77 RID: 6775
		private readonly CancellationCallbackInfo m_callbackInfo;

		// Token: 0x04001A78 RID: 6776
		private readonly SparselyPopulatedArrayAddInfo<CancellationCallbackInfo> m_registrationInfo;
	}
}
