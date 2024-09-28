using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000626 RID: 1574
	public class UIStatsAdd : MonoBehaviour
	{
		// Token: 0x060022AA RID: 8874 RVA: 0x000ABA10 File Offset: 0x000A9C10
		public void OnButtonPress()
		{
			if (this.m_ValueText == null)
			{
				return;
			}
			this.m_ValueText.text = (int.Parse(this.m_ValueText.text) + 1).ToString();
		}

		// Token: 0x04001C27 RID: 7207
		[SerializeField]
		private Text m_ValueText;
	}
}
