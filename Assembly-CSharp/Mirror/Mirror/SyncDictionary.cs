using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000070 RID: 112
	public class SyncDictionary<TKey, TValue> : SyncIDictionary<TKey, TValue>
	{
		// Token: 0x06000351 RID: 849 RVA: 0x0000C95A File Offset: 0x0000AB5A
		public SyncDictionary() : base(new Dictionary<TKey, TValue>())
		{
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0000C967 File Offset: 0x0000AB67
		public SyncDictionary(IEqualityComparer<TKey> eq) : base(new Dictionary<TKey, TValue>(eq))
		{
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000C975 File Offset: 0x0000AB75
		public SyncDictionary(IDictionary<TKey, TValue> d) : base(new Dictionary<TKey, TValue>(d))
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000C983 File Offset: 0x0000AB83
		public new Dictionary<TKey, TValue>.ValueCollection Values
		{
			get
			{
				return ((Dictionary<TKey, TValue>)this.objects).Values;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000355 RID: 853 RVA: 0x0000C995 File Offset: 0x0000AB95
		public new Dictionary<TKey, TValue>.KeyCollection Keys
		{
			get
			{
				return ((Dictionary<TKey, TValue>)this.objects).Keys;
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000C9A7 File Offset: 0x0000ABA7
		public new Dictionary<TKey, TValue>.Enumerator GetEnumerator()
		{
			return ((Dictionary<TKey, TValue>)this.objects).GetEnumerator();
		}
	}
}
