using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000627 RID: 1575
	[RequireComponent(typeof(Text))]
	public class UITextSetValue : MonoBehaviour
	{
		// Token: 0x060022AC RID: 8876 RVA: 0x000ABA51 File Offset: 0x000A9C51
		protected void Awake()
		{
			this.m_Text = base.gameObject.GetComponent<Text>();
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000ABA64 File Offset: 0x000A9C64
		public void SetFloat(float value)
		{
			if (this.m_Text != null)
			{
				this.m_Text.text = value.ToString(this.floatFormat);
			}
		}

		// Token: 0x04001C28 RID: 7208
		private Text m_Text;

		// Token: 0x04001C29 RID: 7209
		public string floatFormat = "0.00";
	}
}
