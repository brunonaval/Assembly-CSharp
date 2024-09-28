using System;

namespace System.Collections.Generic
{
	// Token: 0x02000AC3 RID: 2755
	[Serializable]
	internal class ObjectComparer<T> : Comparer<T>
	{
		// Token: 0x06006282 RID: 25218 RVA: 0x00149CCA File Offset: 0x00147ECA
		public override int Compare(T x, T y)
		{
			return Comparer.Default.Compare(x, y);
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x00149CE2 File Offset: 0x00147EE2
		public override bool Equals(object obj)
		{
			return obj is ObjectComparer<T>;
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x00149C62 File Offset: 0x00147E62
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
