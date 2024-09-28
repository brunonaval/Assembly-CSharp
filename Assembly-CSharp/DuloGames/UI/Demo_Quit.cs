using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005A3 RID: 1443
	public class Demo_Quit : MonoBehaviour
	{
		// Token: 0x06001FD6 RID: 8150 RVA: 0x000A013E File Offset: 0x0009E33E
		protected void OnEnable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.AddListener(new UnityAction(this.ExitGame));
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x000A016A File Offset: 0x0009E36A
		protected void OnDisable()
		{
			if (this.m_HookToButton != null)
			{
				this.m_HookToButton.onClick.RemoveListener(new UnityAction(this.ExitGame));
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x00021F50 File Offset: 0x00020150
		public void ExitGame()
		{
			Application.Quit();
		}

		// Token: 0x040019B7 RID: 6583
		[SerializeField]
		private Button m_HookToButton;
	}
}
