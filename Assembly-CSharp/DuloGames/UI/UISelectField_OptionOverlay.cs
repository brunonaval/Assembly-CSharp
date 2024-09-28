using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005D8 RID: 1496
	public class UISelectField_OptionOverlay : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x000A53DF File Offset: 0x000A35DF
		// (set) Token: 0x060020DD RID: 8413 RVA: 0x000A53E7 File Offset: 0x000A35E7
		public UISelectField_OptionOverlay.Transition transition
		{
			get
			{
				return this.m_Transition;
			}
			set
			{
				this.m_Transition = value;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x000A53F0 File Offset: 0x000A35F0
		// (set) Token: 0x060020DF RID: 8415 RVA: 0x000A53F8 File Offset: 0x000A35F8
		public ColorBlock colorBlock
		{
			get
			{
				return this.m_ColorBlock;
			}
			set
			{
				this.m_ColorBlock = value;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x060020E0 RID: 8416 RVA: 0x000A5401 File Offset: 0x000A3601
		// (set) Token: 0x060020E1 RID: 8417 RVA: 0x000A5409 File Offset: 0x000A3609
		public Graphic targetGraphic
		{
			get
			{
				return this.m_TargetGraphic;
			}
			set
			{
				this.m_TargetGraphic = value;
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000A5412 File Offset: 0x000A3612
		protected void OnEnable()
		{
			this.InternalEvaluateAndTransitionToNormalState(true);
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000A541B File Offset: 0x000A361B
		protected void OnDisable()
		{
			this.InstantClearState();
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000A5423 File Offset: 0x000A3623
		protected void InstantClearState()
		{
			if (this.m_Transition == UISelectField_OptionOverlay.Transition.ColorTint)
			{
				this.StartColorTween(Color.white, true);
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000A543A File Offset: 0x000A363A
		public void InternalEvaluateAndTransitionToNormalState(bool instant)
		{
			this.DoStateTransition(UISelectField_OptionOverlay.VisualState.Normal, instant);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000A5444 File Offset: 0x000A3644
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.m_Highlighted = true;
			if (!this.m_Selected && !this.m_Pressed)
			{
				this.DoStateTransition(UISelectField_OptionOverlay.VisualState.Highlighted, false);
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000A5465 File Offset: 0x000A3665
		public void OnPointerExit(PointerEventData eventData)
		{
			this.m_Highlighted = false;
			if (!this.m_Selected && !this.m_Pressed)
			{
				this.DoStateTransition(UISelectField_OptionOverlay.VisualState.Normal, false);
			}
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000A5486 File Offset: 0x000A3686
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (!this.m_Highlighted)
			{
				return;
			}
			this.m_Pressed = true;
			this.DoStateTransition(UISelectField_OptionOverlay.VisualState.Pressed, false);
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000A54AC File Offset: 0x000A36AC
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Pressed = false;
			UISelectField_OptionOverlay.VisualState state = UISelectField_OptionOverlay.VisualState.Normal;
			if (this.m_Selected)
			{
				state = UISelectField_OptionOverlay.VisualState.Selected;
			}
			else if (this.m_Highlighted)
			{
				state = UISelectField_OptionOverlay.VisualState.Highlighted;
			}
			this.DoStateTransition(state, false);
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000A54EC File Offset: 0x000A36EC
		protected virtual void DoStateTransition(UISelectField_OptionOverlay.VisualState state, bool instant)
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			Color a = this.m_ColorBlock.normalColor;
			switch (state)
			{
			case UISelectField_OptionOverlay.VisualState.Normal:
				a = this.m_ColorBlock.normalColor;
				break;
			case UISelectField_OptionOverlay.VisualState.Highlighted:
				a = this.m_ColorBlock.highlightedColor;
				break;
			case UISelectField_OptionOverlay.VisualState.Pressed:
				a = this.m_ColorBlock.pressedColor;
				break;
			}
			if (this.m_Transition == UISelectField_OptionOverlay.Transition.ColorTint)
			{
				this.StartColorTween(a * this.m_ColorBlock.colorMultiplier, instant);
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000A557C File Offset: 0x000A377C
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (this.m_TargetGraphic == null)
			{
				return;
			}
			if (instant || this.m_ColorBlock.fadeDuration == 0f || !Application.isPlaying)
			{
				this.m_TargetGraphic.canvasRenderer.SetColor(targetColor);
				return;
			}
			this.m_TargetGraphic.CrossFadeColor(targetColor, this.m_ColorBlock.fadeDuration, true, true);
		}

		// Token: 0x04001AD6 RID: 6870
		[SerializeField]
		private UISelectField_OptionOverlay.Transition m_Transition;

		// Token: 0x04001AD7 RID: 6871
		[SerializeField]
		private ColorBlock m_ColorBlock = ColorBlock.defaultColorBlock;

		// Token: 0x04001AD8 RID: 6872
		[SerializeField]
		[Tooltip("Graphic that will have the selected transtion applied.")]
		private Graphic m_TargetGraphic;

		// Token: 0x04001AD9 RID: 6873
		private bool m_Highlighted;

		// Token: 0x04001ADA RID: 6874
		private bool m_Selected;

		// Token: 0x04001ADB RID: 6875
		private bool m_Pressed;

		// Token: 0x020005D9 RID: 1497
		public enum VisualState
		{
			// Token: 0x04001ADD RID: 6877
			Normal,
			// Token: 0x04001ADE RID: 6878
			Highlighted,
			// Token: 0x04001ADF RID: 6879
			Selected,
			// Token: 0x04001AE0 RID: 6880
			Pressed
		}

		// Token: 0x020005DA RID: 1498
		public enum Transition
		{
			// Token: 0x04001AE2 RID: 6882
			None,
			// Token: 0x04001AE3 RID: 6883
			ColorTint
		}
	}
}
