﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks
{
	// Token: 0x0200034E RID: 846
	[DebuggerDisplay("Count = {Count}")]
	internal sealed class MultiProducerMultiConsumerQueue<T> : ConcurrentQueue<T>, IProducerConsumerQueue<T>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x0600235E RID: 9054 RVA: 0x0007E902 File Offset: 0x0007CB02
		void IProducerConsumerQueue<!0>.Enqueue(T item)
		{
			base.Enqueue(item);
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0007E90B File Offset: 0x0007CB0B
		bool IProducerConsumerQueue<!0>.TryDequeue(out T result)
		{
			return base.TryDequeue(out result);
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x0007E914 File Offset: 0x0007CB14
		bool IProducerConsumerQueue<!0>.IsEmpty
		{
			get
			{
				return base.IsEmpty;
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x0007E91C File Offset: 0x0007CB1C
		int IProducerConsumerQueue<!0>.Count
		{
			get
			{
				return base.Count;
			}
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0007E91C File Offset: 0x0007CB1C
		int IProducerConsumerQueue<!0>.GetCountSafe(object syncObj)
		{
			return base.Count;
		}
	}
}
