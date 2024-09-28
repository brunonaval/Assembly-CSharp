using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000621 RID: 1569
	public class UIGlobeFillInverter : MonoBehaviour
	{
		// Token: 0x0600229B RID: 8859 RVA: 0x000AB730 File Offset: 0x000A9930
		public void OnChange(float value)
		{
			if (this.m_Image != null)
			{
				this.m_Image.fillAmount = 1f - value;
			}
		}

		// Token: 0x04001C19 RID: 7193
		[SerializeField]
		private Image m_Image;
	}
}
