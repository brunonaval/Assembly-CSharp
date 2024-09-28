﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace System
{
	// Token: 0x02000150 RID: 336
	internal class LazyHelper
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x000328B9 File Offset: 0x00030AB9
		internal LazyState State { get; }

		// Token: 0x06000C9B RID: 3227 RVA: 0x000328C1 File Offset: 0x00030AC1
		internal LazyHelper(LazyState state)
		{
			this.State = state;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x000328D0 File Offset: 0x00030AD0
		internal LazyHelper(LazyThreadSafetyMode mode, Exception exception)
		{
			switch (mode)
			{
			case LazyThreadSafetyMode.None:
				this.State = 2;
				break;
			case LazyThreadSafetyMode.PublicationOnly:
				this.State = 6;
				break;
			case LazyThreadSafetyMode.ExecutionAndPublication:
				this.State = 9;
				break;
			}
			this._exceptionDispatch = ExceptionDispatchInfo.Capture(exception);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0003291D File Offset: 0x00030B1D
		internal void ThrowException()
		{
			this._exceptionDispatch.Throw();
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0003292C File Offset: 0x00030B2C
		private LazyThreadSafetyMode GetMode()
		{
			switch (this.State)
			{
			case LazyState.NoneViaConstructor:
			case LazyState.NoneViaFactory:
			case LazyState.NoneException:
				return LazyThreadSafetyMode.None;
			case LazyState.PublicationOnlyViaConstructor:
			case LazyState.PublicationOnlyViaFactory:
			case LazyState.PublicationOnlyWait:
			case LazyState.PublicationOnlyException:
				return LazyThreadSafetyMode.PublicationOnly;
			case LazyState.ExecutionAndPublicationViaConstructor:
			case LazyState.ExecutionAndPublicationViaFactory:
			case LazyState.ExecutionAndPublicationException:
				return LazyThreadSafetyMode.ExecutionAndPublication;
			default:
				return LazyThreadSafetyMode.None;
			}
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00032978 File Offset: 0x00030B78
		internal static LazyThreadSafetyMode? GetMode(LazyHelper state)
		{
			if (state == null)
			{
				return null;
			}
			return new LazyThreadSafetyMode?(state.GetMode());
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0003299D File Offset: 0x00030B9D
		internal static bool GetIsValueFaulted(LazyHelper state)
		{
			return ((state != null) ? state._exceptionDispatch : null) != null;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x000329B0 File Offset: 0x00030BB0
		internal static LazyHelper Create(LazyThreadSafetyMode mode, bool useDefaultConstructor)
		{
			switch (mode)
			{
			case LazyThreadSafetyMode.None:
				if (!useDefaultConstructor)
				{
					return LazyHelper.NoneViaFactory;
				}
				return LazyHelper.NoneViaConstructor;
			case LazyThreadSafetyMode.PublicationOnly:
				if (!useDefaultConstructor)
				{
					return LazyHelper.PublicationOnlyViaFactory;
				}
				return LazyHelper.PublicationOnlyViaConstructor;
			case LazyThreadSafetyMode.ExecutionAndPublication:
				return new LazyHelper(useDefaultConstructor ? LazyState.ExecutionAndPublicationViaConstructor : LazyState.ExecutionAndPublicationViaFactory);
			default:
				throw new ArgumentOutOfRangeException("mode", "The mode argument specifies an invalid value.");
			}
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00032A0C File Offset: 0x00030C0C
		internal static object CreateViaDefaultConstructor(Type type)
		{
			object result;
			try
			{
				result = Activator.CreateInstance(type);
			}
			catch (MissingMethodException)
			{
				throw new MissingMemberException("The lazily-initialized type does not have a public, parameterless constructor.");
			}
			return result;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00032A40 File Offset: 0x00030C40
		internal static LazyThreadSafetyMode GetModeFromIsThreadSafe(bool isThreadSafe)
		{
			if (!isThreadSafe)
			{
				return LazyThreadSafetyMode.None;
			}
			return LazyThreadSafetyMode.ExecutionAndPublication;
		}

		// Token: 0x04001266 RID: 4710
		internal static readonly LazyHelper NoneViaConstructor = new LazyHelper(LazyState.NoneViaConstructor);

		// Token: 0x04001267 RID: 4711
		internal static readonly LazyHelper NoneViaFactory = new LazyHelper(LazyState.NoneViaFactory);

		// Token: 0x04001268 RID: 4712
		internal static readonly LazyHelper PublicationOnlyViaConstructor = new LazyHelper(LazyState.PublicationOnlyViaConstructor);

		// Token: 0x04001269 RID: 4713
		internal static readonly LazyHelper PublicationOnlyViaFactory = new LazyHelper(LazyState.PublicationOnlyViaFactory);

		// Token: 0x0400126A RID: 4714
		internal static readonly LazyHelper PublicationOnlyWaitForOtherThreadToPublish = new LazyHelper(LazyState.PublicationOnlyWait);

		// Token: 0x0400126C RID: 4716
		private readonly ExceptionDispatchInfo _exceptionDispatch;
	}
}
