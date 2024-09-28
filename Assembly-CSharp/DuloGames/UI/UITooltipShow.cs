using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000657 RID: 1623
	[AddComponentMenu("UI/Tooltip Show", 58)]
	[DisallowMultipleComponent]
	public class UITooltipShow : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060023EB RID: 9195 RVA: 0x000B028C File Offset: 0x000AE48C
		// (set) Token: 0x060023EC RID: 9196 RVA: 0x000B0294 File Offset: 0x000AE494
		public UITooltipLineContent[] contentLines
		{
			get
			{
				return this.m_ContentLines;
			}
			set
			{
				this.m_ContentLines = value;
			}
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000B02A0 File Offset: 0x000AE4A0
		public virtual void OnTooltip(bool show)
		{
			if (!base.enabled || !this.IsActive())
			{
				return;
			}
			if (show)
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				for (int i = 0; i < this.m_ContentLines.Length; i++)
				{
					UITooltipLineContent uitooltipLineContent = this.m_ContentLines[i];
					if (uitooltipLineContent.IsSpacer)
					{
						UITooltip.AddSpacer();
					}
					else if (uitooltipLineContent.LineStyle != UITooltipLines.LineStyle.Custom)
					{
						UITooltip.AddLine(uitooltipLineContent.Content, uitooltipLineContent.LineStyle);
					}
					else
					{
						UITooltip.AddLine(uitooltipLineContent.Content, uitooltipLineContent.CustomLineStyle);
					}
				}
				if (this.m_WidthMode == UITooltipShow.WidthMode.Preferred)
				{
					UITooltip.SetHorizontalFitMode(ContentSizeFitter.FitMode.PreferredSize);
				}
				if (this.m_Position == UITooltipShow.Position.Anchored)
				{
					UITooltip.AnchorToRect(base.transform as RectTransform);
				}
				if (this.m_OverrideOffset)
				{
					UITooltip.OverrideOffset(this.m_Offset);
					UITooltip.OverrideAnchoredOffset(this.m_Offset);
				}
				UITooltip.Show();
				return;
			}
			UITooltip.Hide();
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x000B0378 File Offset: 0x000AE578
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			if (base.enabled && this.IsActive())
			{
				UITooltip.InstantiateIfNecessary(base.gameObject);
				if (this.m_Delay > 0f)
				{
					base.StartCoroutine("DelayedShow");
					return;
				}
				this.InternalShowTooltip();
			}
		}

		// Token: 0x060023EF RID: 9199 RVA: 0x000B03B5 File Offset: 0x000AE5B5
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.InternalHideTooltip();
		}

		// Token: 0x060023F0 RID: 9200 RVA: 0x000B03B5 File Offset: 0x000AE5B5
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.InternalHideTooltip();
		}

		// Token: 0x060023F1 RID: 9201 RVA: 0x0000219A File Offset: 0x0000039A
		public virtual void OnPointerUp(PointerEventData eventData)
		{
		}

		// Token: 0x060023F2 RID: 9202 RVA: 0x000B03BD File Offset: 0x000AE5BD
		protected void InternalShowTooltip()
		{
			if (!this.m_IsTooltipShown)
			{
				this.m_IsTooltipShown = true;
				this.OnTooltip(true);
			}
		}

		// Token: 0x060023F3 RID: 9203 RVA: 0x000B03D5 File Offset: 0x000AE5D5
		protected void InternalHideTooltip()
		{
			base.StopCoroutine("DelayedShow");
			if (this.m_IsTooltipShown)
			{
				this.m_IsTooltipShown = false;
				this.OnTooltip(false);
			}
		}

		// Token: 0x060023F4 RID: 9204 RVA: 0x000B03F8 File Offset: 0x000AE5F8
		protected IEnumerator DelayedShow()
		{
			yield return new WaitForSeconds(this.m_Delay);
			this.InternalShowTooltip();
			yield break;
		}

		// Token: 0x04001D09 RID: 7433
		[SerializeField]
		private UITooltipShow.Position m_Position;

		// Token: 0x04001D0A RID: 7434
		[SerializeField]
		private UITooltipShow.WidthMode m_WidthMode;

		// Token: 0x04001D0B RID: 7435
		[SerializeField]
		private bool m_OverrideOffset;

		// Token: 0x04001D0C RID: 7436
		[SerializeField]
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04001D0D RID: 7437
		[SerializeField]
		[Tooltip("How long of a delay to expect before showing the tooltip.")]
		[Range(0f, 10f)]
		private float m_Delay = 1f;

		// Token: 0x04001D0E RID: 7438
		[SerializeField]
		private UITooltipLineContent[] m_ContentLines = new UITooltipLineContent[0];

		// Token: 0x04001D0F RID: 7439
		private bool m_IsTooltipShown;

		// Token: 0x02000658 RID: 1624
		public enum Position
		{
			// Token: 0x04001D11 RID: 7441
			Floating,
			// Token: 0x04001D12 RID: 7442
			Anchored
		}

		// Token: 0x02000659 RID: 1625
		public enum WidthMode
		{
			// Token: 0x04001D14 RID: 7444
			Default,
			// Token: 0x04001D15 RID: 7445
			Preferred
		}
	}
}
