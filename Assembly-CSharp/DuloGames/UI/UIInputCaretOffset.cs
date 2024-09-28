using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000622 RID: 1570
	[RequireComponent(typeof(InputField))]
	public class UIInputCaretOffset : MonoBehaviour
	{
		// Token: 0x0600229D RID: 8861 RVA: 0x000AB752 File Offset: 0x000A9952
		protected void OnTransformChildrenChanged()
		{
			base.Invoke("ApplyOffset", 0.5f);
		}

		// Token: 0x0600229E RID: 8862 RVA: 0x000AB764 File Offset: 0x000A9964
		public void ApplyOffset()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.gameObject.name.ToLower().Contains("caret"))
				{
					RectTransform rectTransform = transform as RectTransform;
					this.m_InitialPosition = rectTransform.anchoredPosition;
					rectTransform.anchoredPosition = this.m_InitialPosition + this.m_Offset;
					break;
				}
			}
		}

		// Token: 0x04001C1A RID: 7194
		[SerializeField]
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04001C1B RID: 7195
		private Vector2 m_InitialPosition = Vector2.zero;
	}
}
