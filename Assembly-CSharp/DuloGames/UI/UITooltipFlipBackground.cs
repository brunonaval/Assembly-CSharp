using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000629 RID: 1577
	public class UITooltipFlipBackground : MonoBehaviour
	{
		// Token: 0x060022B1 RID: 8881 RVA: 0x000ABB08 File Offset: 0x000A9D08
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
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000ABB6C File Offset: 0x000A9D6C
		protected void OnEnable()
		{
			if (this.m_Graphic == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Tooltip.onAnchorEvent.AddListener(new UnityAction<UITooltip.Anchor>(this.OnAnchor));
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000ABBA7 File Offset: 0x000A9DA7
		protected void OnDisable()
		{
			if (this.m_Graphic == null || this.m_Tooltip == null)
			{
				return;
			}
			this.m_Tooltip.onAnchorEvent.RemoveListener(new UnityAction<UITooltip.Anchor>(this.OnAnchor));
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000ABBE4 File Offset: 0x000A9DE4
		public void OnAnchor(UITooltip.Anchor anchor)
		{
			if (this.m_Graphic == null || this.m_Flippable == null)
			{
				return;
			}
			switch (anchor)
			{
			case UITooltip.Anchor.BottomLeft:
				this.m_Flippable.horizontal = false;
				this.m_Flippable.vertical = false;
				return;
			case UITooltip.Anchor.BottomRight:
				this.m_Flippable.horizontal = true;
				this.m_Flippable.vertical = false;
				return;
			case UITooltip.Anchor.TopLeft:
				this.m_Flippable.horizontal = false;
				this.m_Flippable.vertical = true;
				return;
			case UITooltip.Anchor.TopRight:
				this.m_Flippable.horizontal = true;
				this.m_Flippable.vertical = true;
				return;
			case UITooltip.Anchor.Left:
				this.m_Flippable.horizontal = false;
				return;
			case UITooltip.Anchor.Right:
				this.m_Flippable.horizontal = true;
				return;
			case UITooltip.Anchor.Top:
				this.m_Flippable.vertical = true;
				return;
			case UITooltip.Anchor.Bottom:
				this.m_Flippable.vertical = false;
				return;
			default:
				return;
			}
		}

		// Token: 0x04001C2C RID: 7212
		[SerializeField]
		private UITooltip m_Tooltip;

		// Token: 0x04001C2D RID: 7213
		[SerializeField]
		private Graphic m_Graphic;

		// Token: 0x04001C2E RID: 7214
		private UIFlippable m_Flippable;
	}
}
