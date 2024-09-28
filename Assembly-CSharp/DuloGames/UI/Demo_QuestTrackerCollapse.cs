using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005A2 RID: 1442
	[ExecuteInEditMode]
	public class Demo_QuestTrackerCollapse : UIBehaviour
	{
		// Token: 0x06001FD2 RID: 8146 RVA: 0x0009FFBC File Offset: 0x0009E1BC
		protected override void OnEnable()
		{
			base.OnEnable();
			if (this.m_Toggle != null)
			{
				this.m_Toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleStateChange));
			}
			if (this.m_Toggle != null)
			{
				this.OnToggleStateChange(this.m_Toggle.isOn);
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000A0018 File Offset: 0x0009E218
		protected override void OnDisable()
		{
			base.OnDisable();
			if (this.m_Toggle != null)
			{
				this.m_Toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleStateChange));
			}
			this.OnToggleStateChange(false);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000A0054 File Offset: 0x0009E254
		public void OnToggleStateChange(bool state)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (state)
			{
				if (this.m_Content != null)
				{
					this.m_Content.SetActive(true);
				}
				if (this.m_ArrowFlippable != null)
				{
					this.m_ArrowFlippable.vertical = !this.m_ArrowInvertFlip;
				}
				if (this.m_ArrowFlippable2 != null)
				{
					this.m_ArrowFlippable2.vertical = !this.m_ArrowInvertFlip;
					return;
				}
			}
			else
			{
				if (this.m_Content != null)
				{
					this.m_Content.SetActive(false);
				}
				if (this.m_ArrowFlippable != null)
				{
					this.m_ArrowFlippable.vertical = this.m_ArrowInvertFlip;
				}
				if (this.m_ArrowFlippable2 != null)
				{
					this.m_ArrowFlippable2.vertical = this.m_ArrowInvertFlip;
				}
			}
		}

		// Token: 0x040019B2 RID: 6578
		[SerializeField]
		private GameObject m_Content;

		// Token: 0x040019B3 RID: 6579
		[SerializeField]
		private Toggle m_Toggle;

		// Token: 0x040019B4 RID: 6580
		[SerializeField]
		private UIFlippable m_ArrowFlippable;

		// Token: 0x040019B5 RID: 6581
		[SerializeField]
		private UIFlippable m_ArrowFlippable2;

		// Token: 0x040019B6 RID: 6582
		[SerializeField]
		private bool m_ArrowInvertFlip;
	}
}
