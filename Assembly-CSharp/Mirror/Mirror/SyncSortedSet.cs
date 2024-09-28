using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x0200007D RID: 125
	public class SyncSortedSet<T> : SyncSet<T>
	{
		// Token: 0x060003B5 RID: 949 RVA: 0x0000D94E File Offset: 0x0000BB4E
		public SyncSortedSet() : this(Comparer<T>.Default)
		{
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D95B File Offset: 0x0000BB5B
		public SyncSortedSet(IComparer<T> comparer) : base(new SortedSet<T>(comparer ?? Comparer<T>.Default))
		{
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D972 File Offset: 0x0000BB72
		public new SortedSet<T>.Enumerator GetEnumerator()
		{
			return ((SortedSet<T>)this.objects).GetEnumerator();
		}
	}
}
