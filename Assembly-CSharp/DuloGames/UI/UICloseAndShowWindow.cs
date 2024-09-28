using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000671 RID: 1649
	public class UICloseAndShowWindow : MonoBehaviour
	{
		// Token: 0x06002499 RID: 9369 RVA: 0x000B2EA0 File Offset: 0x000B10A0
		public void CloseAndShow()
		{
			if (this.m_CloseWindow != null)
			{
				this.m_CloseWindow.Hide();
			}
			if (this.m_ShowWindow != null)
			{
				this.m_ShowWindow.Show();
			}
		}

		// Token: 0x04001DBF RID: 7615
		[SerializeField]
		private UIWindow m_CloseWindow;

		// Token: 0x04001DC0 RID: 7616
		[SerializeField]
		private UIWindow m_ShowWindow;
	}
}
