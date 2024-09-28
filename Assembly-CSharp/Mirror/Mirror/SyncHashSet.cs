using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x0200007C RID: 124
	public class SyncHashSet<T> : SyncSet<T>
	{
		// Token: 0x060003B2 RID: 946 RVA: 0x0000D918 File Offset: 0x0000BB18
		public SyncHashSet() : this(EqualityComparer<T>.Default)
		{
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000D925 File Offset: 0x0000BB25
		public SyncHashSet(IEqualityComparer<T> comparer) : base(new HashSet<T>(comparer ?? EqualityComparer<T>.Default))
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000D93C File Offset: 0x0000BB3C
		public new HashSet<T>.Enumerator GetEnumerator()
		{
			return ((HashSet<T>)this.objects).GetEnumerator();
		}
	}
}
