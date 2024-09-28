using System;

namespace UnityEngine
{
	// Token: 0x02000574 RID: 1396
	public class RuleTile<T> : RuleTile
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x0009B65A File Offset: 0x0009985A
		public sealed override Type m_NeighborType
		{
			get
			{
				return typeof(T);
			}
		}
	}
}
