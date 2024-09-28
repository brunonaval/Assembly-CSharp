using System;

namespace UnityEngine
{
	// Token: 0x0200056E RID: 1390
	public class IsometricRuleTile<T> : IsometricRuleTile
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001EF7 RID: 7927 RVA: 0x0009B65A File Offset: 0x0009985A
		public sealed override Type m_NeighborType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
