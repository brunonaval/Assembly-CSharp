using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005F6 RID: 1526
	public interface IUIItemSlot
	{
		// Token: 0x0600219B RID: 8603
		UIItemInfo GetItemInfo();

		// Token: 0x0600219C RID: 8604
		bool Assign(UIItemInfo itemInfo, UnityEngine.Object source);

		// Token: 0x0600219D RID: 8605
		void Unassign();
	}
}
