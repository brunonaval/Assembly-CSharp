using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000666 RID: 1638
	[AddComponentMenu("UI/Always On Top", 8)]
	[DisallowMultipleComponent]
	public class UIAlwaysOnTop : MonoBehaviour, IComparable
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000B1E8C File Offset: 0x000B008C
		// (set) Token: 0x0600245F RID: 9311 RVA: 0x000B1E94 File Offset: 0x000B0094
		public int order
		{
			get
			{
				return this.m_Order;
			}
			set
			{
				this.m_Order = value;
			}
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x000B1EA0 File Offset: 0x000B00A0
		public int CompareTo(object obj)
		{
			if (obj != null)
			{
				UIAlwaysOnTop uialwaysOnTop = obj as UIAlwaysOnTop;
				if (uialwaysOnTop != null)
				{
					return this.order.CompareTo(uialwaysOnTop.order);
				}
			}
			return 1;
		}

		// Token: 0x04001D93 RID: 7571
		public const int ModalBoxOrder = 99996;

		// Token: 0x04001D94 RID: 7572
		public const int SelectFieldBlockerOrder = 99997;

		// Token: 0x04001D95 RID: 7573
		public const int SelectFieldOrder = 99998;

		// Token: 0x04001D96 RID: 7574
		public const int TooltipOrder = 99999;

		// Token: 0x04001D97 RID: 7575
		[SerializeField]
		private int m_Order;
	}
}
