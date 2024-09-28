using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000620 RID: 1568
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class UICanvasAspectResize : MonoBehaviour
	{
		// Token: 0x06002298 RID: 8856 RVA: 0x000AB6CF File Offset: 0x000A98CF
		protected void Awake()
		{
			this.m_RectTransform = (base.transform as RectTransform);
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000AB6E4 File Offset: 0x000A98E4
		private void Update()
		{
			if (this.m_Camera == null)
			{
				return;
			}
			this.m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.m_Camera.aspect * this.m_RectTransform.rect.size.y);
		}

		// Token: 0x04001C17 RID: 7191
		[SerializeField]
		private Camera m_Camera;

		// Token: 0x04001C18 RID: 7192
		private RectTransform m_RectTransform;
	}
}
