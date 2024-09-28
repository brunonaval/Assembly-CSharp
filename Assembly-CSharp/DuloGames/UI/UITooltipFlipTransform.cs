using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI
{
	// Token: 0x0200062B RID: 1579
	public class UITooltipFlipTransform : MonoBehaviour
	{
		// Token: 0x060022BB RID: 8891 RVA: 0x000AC03C File Offset: 0x000AA23C
		protected void Awake()
		{
			if (this.m_Transform == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_OriginalPivot = this.m_Transform.pivot;
			this.m_OriginalAnchorMin = this.m_Transform.anchorMin;
			this.m_OriginalAnchorMax = this.m_Transform.anchorMax;
			this.m_OriginalPosition = this.m_Transform.anchoredPosition;
		}

		// Token: 0x060022BC RID: 8892 RVA: 0x000AC0AA File Offset: 0x000AA2AA
		protected void OnEnable()
		{
			if (this.m_Transform == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Tooltip.onAnchorEvent.AddListener(new UnityAction<UITooltip.Anchor>(this.OnAnchor));
		}

		// Token: 0x060022BD RID: 8893 RVA: 0x000AC0E5 File Offset: 0x000AA2E5
		protected void OnDisable()
		{
			if (this.m_Transform == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Tooltip.onAnchorEvent.RemoveListener(new UnityAction<UITooltip.Anchor>(this.OnAnchor));
		}

		// Token: 0x060022BE RID: 8894 RVA: 0x000AC120 File Offset: 0x000AA320
		public void OnAnchor(UITooltip.Anchor anchor)
		{
			if (this.m_Transform == null)
			{
				return;
			}
			switch (anchor)
			{
			case UITooltip.Anchor.BottomLeft:
			case UITooltip.Anchor.TopLeft:
			case UITooltip.Anchor.Left:
			case UITooltip.Anchor.Bottom:
				this.m_Transform.pivot = this.m_OriginalPivot;
				this.m_Transform.anchorMin = this.m_OriginalAnchorMin;
				this.m_Transform.anchorMax = this.m_OriginalAnchorMax;
				this.m_Transform.anchoredPosition = this.m_OriginalPosition;
				return;
			case UITooltip.Anchor.BottomRight:
			case UITooltip.Anchor.TopRight:
			case UITooltip.Anchor.Right:
				this.m_Transform.pivot = new Vector2((this.m_OriginalPivot.x == 0f) ? 1f : 0f, this.m_OriginalPivot.y);
				this.m_Transform.anchorMin = new Vector2((this.m_OriginalAnchorMin.x == 0f) ? 1f : 0f, this.m_OriginalAnchorMin.y);
				this.m_Transform.anchorMax = new Vector2((this.m_OriginalAnchorMax.x == 0f) ? 1f : 0f, this.m_OriginalAnchorMax.y);
				this.m_Transform.anchoredPosition = new Vector2(this.m_OriginalPosition.x * -1f, this.m_OriginalPosition.y);
				return;
			case UITooltip.Anchor.Top:
				this.m_Transform.pivot = new Vector2(this.m_OriginalPivot.x, (this.m_OriginalPivot.y == 0f) ? 1f : 0f);
				this.m_Transform.anchorMin = new Vector2(this.m_OriginalAnchorMin.x, (this.m_OriginalAnchorMin.y == 0f) ? 1f : 0f);
				this.m_Transform.anchorMax = new Vector2(this.m_OriginalAnchorMax.x, (this.m_OriginalAnchorMax.y == 0f) ? 1f : 0f);
				this.m_Transform.anchoredPosition = new Vector2(this.m_OriginalPosition.x, this.m_OriginalPosition.y * -1f);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001C36 RID: 7222
		[SerializeField]
		private UITooltip m_Tooltip;

		// Token: 0x04001C37 RID: 7223
		[SerializeField]
		private RectTransform m_Transform;

		// Token: 0x04001C38 RID: 7224
		private Vector2 m_OriginalPivot;

		// Token: 0x04001C39 RID: 7225
		private Vector2 m_OriginalAnchorMin;

		// Token: 0x04001C3A RID: 7226
		private Vector2 m_OriginalAnchorMax;

		// Token: 0x04001C3B RID: 7227
		private Vector2 m_OriginalPosition;
	}
}
