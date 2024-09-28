using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200062A RID: 1578
	public class UITooltipFlipEffect : MonoBehaviour
	{
		// Token: 0x060022B6 RID: 8886 RVA: 0x000ABCCC File Offset: 0x000A9ECC
		protected void Awake()
		{
			if (this.m_Graphic == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Flippable = this.m_Graphic.gameObject.GetComponent<UIFlippable>();
			if (this.m_Flippable == null)
			{
				this.m_Flippable = this.m_Graphic.gameObject.AddComponent<UIFlippable>();
			}
			this.m_OriginalPivot = this.m_Graphic.rectTransform.pivot;
			this.m_OriginalAnchorMin = this.m_Graphic.rectTransform.anchorMin;
			this.m_OriginalAnchorMax = this.m_Graphic.rectTransform.anchorMax;
			this.m_OriginalPosition = this.m_Graphic.rectTransform.anchoredPosition;
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000ABD88 File Offset: 0x000A9F88
		protected void OnEnable()
		{
			if (this.m_Graphic == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Tooltip.onAnchorEvent.AddListener(new UnityAction<UITooltip.Anchor>(this.OnAnchor));
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000ABDC3 File Offset: 0x000A9FC3
		protected void OnDisable()
		{
			if (this.m_Graphic == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Tooltip.onAnchorEvent.RemoveListener(new UnityAction<UITooltip.Anchor>(this.OnAnchor));
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x000ABE00 File Offset: 0x000AA000
		public void OnAnchor(UITooltip.Anchor anchor)
		{
			if (this.m_Graphic == null || this.m_Flippable == null)
			{
				return;
			}
			RectTransform rectTransform = this.m_Graphic.rectTransform;
			switch (anchor)
			{
			case UITooltip.Anchor.BottomLeft:
			case UITooltip.Anchor.TopLeft:
			case UITooltip.Anchor.Left:
			case UITooltip.Anchor.Bottom:
				this.m_Flippable.horizontal = false;
				rectTransform.pivot = this.m_OriginalPivot;
				rectTransform.anchorMin = this.m_OriginalAnchorMin;
				rectTransform.anchorMax = this.m_OriginalAnchorMax;
				rectTransform.anchoredPosition = this.m_OriginalPosition;
				return;
			case UITooltip.Anchor.BottomRight:
			case UITooltip.Anchor.TopRight:
			case UITooltip.Anchor.Right:
				this.m_Flippable.horizontal = true;
				rectTransform.pivot = new Vector2((this.m_OriginalPivot.x == 0f) ? 1f : 0f, this.m_OriginalPivot.y);
				rectTransform.anchorMin = new Vector2((this.m_OriginalAnchorMin.x == 0f) ? 1f : 0f, this.m_OriginalAnchorMin.y);
				rectTransform.anchorMax = new Vector2((this.m_OriginalAnchorMax.x == 0f) ? 1f : 0f, this.m_OriginalAnchorMax.y);
				rectTransform.anchoredPosition = new Vector2(this.m_OriginalPosition.x * -1f, this.m_OriginalPosition.y);
				return;
			case UITooltip.Anchor.Top:
				this.m_Flippable.vertical = true;
				rectTransform.pivot = new Vector2(this.m_OriginalPivot.x, (this.m_OriginalPivot.y == 0f) ? 1f : 0f);
				rectTransform.anchorMin = new Vector2(this.m_OriginalAnchorMin.x, (this.m_OriginalAnchorMin.y == 0f) ? 1f : 0f);
				rectTransform.anchorMax = new Vector2(this.m_OriginalAnchorMax.x, (this.m_OriginalAnchorMax.y == 0f) ? 1f : 0f);
				rectTransform.anchoredPosition = new Vector2(this.m_OriginalPosition.x, this.m_OriginalPosition.y * -1f);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001C2F RID: 7215
		[SerializeField]
		private UITooltip m_Tooltip;

		// Token: 0x04001C30 RID: 7216
		[SerializeField]
		private Graphic m_Graphic;

		// Token: 0x04001C31 RID: 7217
		private UIFlippable m_Flippable;

		// Token: 0x04001C32 RID: 7218
		private Vector2 m_OriginalPivot;

		// Token: 0x04001C33 RID: 7219
		private Vector2 m_OriginalAnchorMin;

		// Token: 0x04001C34 RID: 7220
		private Vector2 m_OriginalAnchorMax;

		// Token: 0x04001C35 RID: 7221
		private Vector2 m_OriginalPosition;
	}
}
