﻿using System;
using System.Diagnostics.Tracing;

namespace System.Collections.Concurrent
{
	// Token: 0x02000A57 RID: 2647
	[EventSource(Name = "System.Collections.Concurrent.ConcurrentCollectionsEventSource", Guid = "35167F8E-49B2-4b96-AB86-435B59336B5E")]
	internal sealed class CDSCollectionETWBCLProvider : EventSource
	{
		// Token: 0x06005EEC RID: 24300 RVA: 0x0007B578 File Offset: 0x00079778
		private CDSCollectionETWBCLProvider()
		{
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x0013F03B File Offset: 0x0013D23B
		[Event(1, Level = EventLevel.Warning)]
		public void ConcurrentStack_FastPushFailed(int spinCount)
		{
			if (base.IsEnabled(EventLevel.Warning, EventKeywords.All))
			{
				base.WriteEvent(1, spinCount);
			}
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x0013F050 File Offset: 0x0013D250
		[Event(2, Level = EventLevel.Warning)]
		public void ConcurrentStack_FastPopFailed(int spinCount)
		{
			if (base.IsEnabled(EventLevel.Warning, EventKeywords.All))
			{
				base.WriteEvent(2, spinCount);
			}
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x0013F065 File Offset: 0x0013D265
		[Event(3, Level = EventLevel.Warning)]
		public void ConcurrentDictionary_AcquiringAllLocks(int numOfBuckets)
		{
			if (base.IsEnabled(EventLevel.Warning, EventKeywords.All))
			{
				base.WriteEvent(3, numOfBuckets);
			}
		}

		// Token: 0x06005EF0 RID: 24304 RVA: 0x0013F07A File Offset: 0x0013D27A
		[Event(4, Level = EventLevel.Verbose)]
		public void ConcurrentBag_TryTakeSteals()
		{
			if (base.IsEnabled(EventLevel.Verbose, EventKeywords.All))
			{
				base.WriteEvent(4);
			}
		}

		// Token: 0x06005EF1 RID: 24305 RVA: 0x0013F08E File Offset: 0x0013D28E
		[Event(5, Level = EventLevel.Verbose)]
		public void ConcurrentBag_TryPeekSteals()
		{
			if (base.IsEnabled(EventLevel.Verbose, EventKeywords.All))
			{
				base.WriteEvent(5);
			}
		}

		// Token: 0x04003936 RID: 14646
		public static CDSCollectionETWBCLProvider Log = new CDSCollectionETWBCLProvider();

		// Token: 0x04003937 RID: 14647
		private const EventKeywords ALL_KEYWORDS = EventKeywords.All;

		// Token: 0x04003938 RID: 14648
		private const int CONCURRENTSTACK_FASTPUSHFAILED_ID = 1;

		// Token: 0x04003939 RID: 14649
		private const int CONCURRENTSTACK_FASTPOPFAILED_ID = 2;

		// Token: 0x0400393A RID: 14650
		private const int CONCURRENTDICTIONARY_ACQUIRINGALLLOCKS_ID = 3;

		// Token: 0x0400393B RID: 14651
		private const int CONCURRENTBAG_TRYTAKESTEALS_ID = 4;

		// Token: 0x0400393C RID: 14652
		private const int CONCURRENTBAG_TRYPEEKSTEALS_ID = 5;
	}
}
