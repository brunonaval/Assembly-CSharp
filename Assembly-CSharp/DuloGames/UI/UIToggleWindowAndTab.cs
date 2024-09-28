using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000628 RID: 1576
	public class UIToggleWindowAndTab : MonoBehaviour
	{
		// Token: 0x060022AF RID: 8879 RVA: 0x000ABAA0 File Offset: 0x000A9CA0
		public void Toggle()
		{
			if (this.window == null || this.tab == null)
			{
				return;
			}
			if (this.window.IsOpen && this.tab.isOn)
			{
				this.window.Hide();
				return;
			}
			this.window.Show();
			this.tab.Activate();
		}

		// Token: 0x04001C2A RID: 7210
		public UIWindow window;

		// Token: 0x04001C2B RID: 7211
		public UITab tab;
	}
}
