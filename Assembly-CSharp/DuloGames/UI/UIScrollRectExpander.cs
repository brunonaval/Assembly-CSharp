using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000623 RID: 1571
	public class UIScrollRectExpander : MonoBehaviour
	{
		// Token: 0x060022A0 RID: 8864 RVA: 0x000AB81E File Offset: 0x000A9A1E
		protected void OnEnable()
		{
			if (base.gameObject.activeSelf)
			{
				this.Collapse();
			}
		}

		// Token: 0x060022A1 RID: 8865 RVA: 0x000AB833 File Offset: 0x000A9A33
		protected void OnDisable()
		{
			if (!base.gameObject.activeSelf)
			{
				this.Expand();
			}
		}

		// Token: 0x060022A2 RID: 8866 RVA: 0x000AB848 File Offset: 0x000A9A48
		private void Expand()
		{
			if (this.m_Expanded || this.m_Target == null)
			{
				return;
			}
			this.m_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.m_Target.rect.width + this.m_ExpandWidth);
			this.m_Expanded = true;
		}

		// Token: 0x060022A3 RID: 8867 RVA: 0x000AB89C File Offset: 0x000A9A9C
		private void Collapse()
		{
			if (!this.m_Expanded || this.m_Target == null)
			{
				return;
			}
			this.m_Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.m_Target.rect.width - this.m_ExpandWidth);
			this.m_Expanded = false;
		}

		// Token: 0x04001C1C RID: 7196
		[SerializeField]
		private float m_ExpandWidth;

		// Token: 0x04001C1D RID: 7197
		[SerializeField]
		private RectTransform m_Target;

		// Token: 0x04001C1E RID: 7198
		private bool m_Expanded;
	}
}
