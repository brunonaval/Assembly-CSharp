using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005A5 RID: 1445
	public class Demo_XPTooltip : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x06001FDF RID: 8159 RVA: 0x000A0364 File Offset: 0x0009E564
		protected override void Awake()
		{
			base.Awake();
			if (this.m_TooltipObject != null)
			{
				RectTransform rectTransform = this.m_TooltipObject.transform as RectTransform;
				rectTransform.anchorMin = new Vector2(0f, 1f);
				rectTransform.anchorMax = new Vector2(0f, 1f);
				rectTransform.pivot = new Vector2(0.5f, 0f);
				this.m_TooltipObject.SetActive(false);
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000A03E0 File Offset: 0x0009E5E0
		public virtual void OnTooltip(bool show)
		{
			if (this.m_TooltipObject == null || this.m_FillImage == null)
			{
				return;
			}
			RectTransform rectTransform = this.m_TooltipObject.transform as RectTransform;
			RectTransform rectTransform2 = this.m_FillImage.transform as RectTransform;
			if (show)
			{
				rectTransform.SetParent(this.m_FillImage.transform, true);
				rectTransform.anchoredPosition = new Vector2(rectTransform2.rect.width * this.m_FillImage.fillAmount, this.m_OffsetY);
				UIUtility.BringToFront(this.m_TooltipObject);
				this.m_TooltipObject.SetActive(true);
				return;
			}
			this.m_TooltipObject.SetActive(false);
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000A0490 File Offset: 0x0009E690
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			if (base.enabled && this.IsActive())
			{
				if (this.m_Delay > 0f)
				{
					base.StartCoroutine("DelayedShow");
					return;
				}
				this.InternalShowTooltip();
			}
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000A04C2 File Offset: 0x0009E6C2
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.InternalHideTooltip();
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000A04CA File Offset: 0x0009E6CA
		protected void InternalShowTooltip()
		{
			if (!this.m_IsTooltipShown)
			{
				this.m_IsTooltipShown = true;
				this.OnTooltip(true);
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000A04E2 File Offset: 0x0009E6E2
		protected void InternalHideTooltip()
		{
			base.StopCoroutine("DelayedShow");
			if (this.m_IsTooltipShown)
			{
				this.m_IsTooltipShown = false;
				this.OnTooltip(false);
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000A0505 File Offset: 0x0009E705
		protected IEnumerator DelayedShow()
		{
			yield return new WaitForSeconds(this.m_Delay);
			this.InternalShowTooltip();
			yield break;
		}

		// Token: 0x040019B9 RID: 6585
		[SerializeField]
		private GameObject m_TooltipObject;

		// Token: 0x040019BA RID: 6586
		[SerializeField]
		private Image m_FillImage;

		// Token: 0x040019BB RID: 6587
		[SerializeField]
		private float m_OffsetY;

		// Token: 0x040019BC RID: 6588
		[SerializeField]
		[Tooltip("How long of a delay to expect before showing the tooltip.")]
		[Range(0f, 10f)]
		private float m_Delay = 1f;

		// Token: 0x040019BD RID: 6589
		private bool m_IsTooltipShown;
	}
}
