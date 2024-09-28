using System;

namespace UnityEngine
{
	// Token: 0x0200056C RID: 1388
	public class HexagonalRuleTile<T> : HexagonalRuleTile
	{
		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x0009B65A File Offset: 0x0009985A
		public sealed override Type m_NeighborType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
