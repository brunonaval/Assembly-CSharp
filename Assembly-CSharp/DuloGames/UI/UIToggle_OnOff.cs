using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005E6 RID: 1510
	[ExecuteInEditMode]
	[RequireComponent(typeof(Toggle))]
	[AddComponentMenu("UI/Toggle OnOff", 58)]
	public class UIToggle_OnOff : MonoBehaviour, IEventSystemHandler
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06002163 RID: 8547 RVA: 0x000A752D File Offset: 0x000A572D
		public Toggle toggle
		{
			get
			{
				return base.gameObject.GetComponent<Toggle>();
			}
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000A753A File Offset: 0x000A573A
		protected void OnEnable()
		{
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
			this.OnValueChanged(this.toggle.isOn);
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000A7569 File Offset: 0x000A5769
		protected void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x000A7588 File Offset: 0x000A5788
		public void OnValueChanged(bool state)
		{
			if (this.m_Target == null || !base.isActiveAndEnabled)
			{
				return;
			}
			if (this.m_Transition == UIToggle_OnOff.Transition.SpriteSwap)
			{
				this.m_Target.overrideSprite = (state ? this.m_ActiveSprite : null);
				return;
			}
			if (this.m_Transition == UIToggle_OnOff.Transition.Reposition)
			{
				this.m_Target.rectTransform.anchoredPosition = (state ? this.m_ActivePosition : this.m_InactivePosition);
			}
		}

		// Token: 0x04001B2C RID: 6956
		[SerializeField]
		private Image m_Target;

		// Token: 0x04001B2D RID: 6957
		[SerializeField]
		private UIToggle_OnOff.Transition m_Transition;

		// Token: 0x04001B2E RID: 6958
		[SerializeField]
		private Sprite m_ActiveSprite;

		// Token: 0x04001B2F RID: 6959
		[SerializeField]
		private Vector2 m_InactivePosition = Vector2.zero;

		// Token: 0x04001B30 RID: 6960
		[SerializeField]
		private Vector2 m_ActivePosition = Vector2.zero;

		// Token: 0x020005E7 RID: 1511
		public enum Transition
		{
			// Token: 0x04001B32 RID: 6962
			SpriteSwap,
			// Token: 0x04001B33 RID: 6963
			Reposition
		}
	}
}
