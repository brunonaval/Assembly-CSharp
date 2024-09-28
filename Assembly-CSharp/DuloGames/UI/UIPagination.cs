using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200066D RID: 1645
	[AddComponentMenu("UI/Pagination", 82)]
	public class UIPagination : MonoBehaviour
	{
		// Token: 0x06002480 RID: 9344 RVA: 0x000B27DC File Offset: 0x000B09DC
		private void Start()
		{
			if (this.m_ButtonPrev != null)
			{
				this.m_ButtonPrev.onClick.AddListener(new UnityAction(this.OnPrevClick));
			}
			if (this.m_ButtonNext != null)
			{
				this.m_ButtonNext.onClick.AddListener(new UnityAction(this.OnNextClick));
			}
			if (this.m_PagesContainer != null && this.m_PagesContainer.childCount > 0)
			{
				for (int i = 0; i < this.m_PagesContainer.childCount; i++)
				{
					if (this.m_PagesContainer.GetChild(i).gameObject.activeSelf)
					{
						this.activePage = i;
						break;
					}
				}
			}
			this.UpdatePagesVisibility();
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000B2898 File Offset: 0x000B0A98
		private void UpdatePagesVisibility()
		{
			if (this.m_PagesContainer == null)
			{
				return;
			}
			if (this.m_PagesContainer.childCount > 0)
			{
				for (int i = 0; i < this.m_PagesContainer.childCount; i++)
				{
					this.m_PagesContainer.GetChild(i).gameObject.SetActive(i == this.activePage);
				}
			}
			if (this.m_LabelText != null)
			{
				this.m_LabelText.text = string.Concat(new string[]
				{
					"<color=#",
					CommonColorBuffer.ColorToString(this.m_LabelActiveColor),
					">",
					(this.activePage + 1).ToString(),
					"</color> / ",
					this.m_PagesContainer.childCount.ToString()
				});
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000B2974 File Offset: 0x000B0B74
		private void OnPrevClick()
		{
			if (!base.isActiveAndEnabled || this.m_PagesContainer == null)
			{
				return;
			}
			if (this.activePage == 0)
			{
				this.activePage = this.m_PagesContainer.childCount - 1;
			}
			else
			{
				this.activePage--;
			}
			this.UpdatePagesVisibility();
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x000B29CC File Offset: 0x000B0BCC
		private void OnNextClick()
		{
			if (!base.isActiveAndEnabled || this.m_PagesContainer == null)
			{
				return;
			}
			if (this.activePage == this.m_PagesContainer.childCount - 1)
			{
				this.activePage = 0;
			}
			else
			{
				this.activePage++;
			}
			this.UpdatePagesVisibility();
		}

		// Token: 0x04001DB3 RID: 7603
		[SerializeField]
		private Transform m_PagesContainer;

		// Token: 0x04001DB4 RID: 7604
		[SerializeField]
		private Button m_ButtonPrev;

		// Token: 0x04001DB5 RID: 7605
		[SerializeField]
		private Button m_ButtonNext;

		// Token: 0x04001DB6 RID: 7606
		[SerializeField]
		private Text m_LabelText;

		// Token: 0x04001DB7 RID: 7607
		[SerializeField]
		private Color m_LabelActiveColor = Color.white;

		// Token: 0x04001DB8 RID: 7608
		private int activePage;
	}
}
