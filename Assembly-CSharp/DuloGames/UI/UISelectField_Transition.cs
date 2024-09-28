using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005DB RID: 1499
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Select Field - Transition", 58)]
	[RequireComponent(typeof(UISelectField))]
	public class UISelectField_Transition : MonoBehaviour
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060020ED RID: 8429 RVA: 0x000A55F2 File Offset: 0x000A37F2
		// (set) Token: 0x060020EE RID: 8430 RVA: 0x000A55FA File Offset: 0x000A37FA
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

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060020EF RID: 8431 RVA: 0x000A5603 File Offset: 0x000A3803
		// (set) Token: 0x060020F0 RID: 8432 RVA: 0x000A560B File Offset: 0x000A380B
		public GameObject targetGameObject
		{
			get
			{
				return this.m_TargetGameObject;
			}
			set
			{
				this.m_TargetGameObject = value;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060020F1 RID: 8433 RVA: 0x000A5614 File Offset: 0x000A3814
		public Animator animator
		{
			get
			{
				if (this.m_TargetGameObject != null)
				{
					return this.m_TargetGameObject.GetComponent<Animator>();
				}
				return null;
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x060020F2 RID: 8434 RVA: 0x000A5631 File Offset: 0x000A3831
		// (set) Token: 0x060020F3 RID: 8435 RVA: 0x000A5639 File Offset: 0x000A3839
		public Selectable.Transition transition
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

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060020F4 RID: 8436 RVA: 0x000A5642 File Offset: 0x000A3842
		// (set) Token: 0x060020F5 RID: 8437 RVA: 0x000A564A File Offset: 0x000A384A
		public ColorBlockExtended colors
		{
			get
			{
				return this.m_Colors;
			}
			set
			{
				this.m_Colors = value;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060020F6 RID: 8438 RVA: 0x000A5653 File Offset: 0x000A3853
		// (set) Token: 0x060020F7 RID: 8439 RVA: 0x000A565B File Offset: 0x000A385B
		public SpriteStateExtended spriteState
		{
			get
			{
				return this.m_SpriteState;
			}
			set
			{
				this.m_SpriteState = value;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000A5664 File Offset: 0x000A3864
		// (set) Token: 0x060020F9 RID: 8441 RVA: 0x000A566C File Offset: 0x000A386C
		public AnimationTriggersExtended animationTriggers
		{
			get
			{
				return this.m_AnimationTriggers;
			}
			set
			{
				this.m_AnimationTriggers = value;
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000A5675 File Offset: 0x000A3875
		protected void Awake()
		{
			this.m_Select = base.gameObject.GetComponent<UISelectField>();
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000A5688 File Offset: 0x000A3888
		protected void OnEnable()
		{
			if (this.m_Select != null)
			{
				this.m_Select.onTransition.AddListener(new UnityAction<UISelectField.VisualState, bool>(this.OnTransition));
			}
			this.OnTransition(UISelectField.VisualState.Normal, true);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000A56BC File Offset: 0x000A38BC
		protected void OnDisable()
		{
			if (this.m_Select != null)
			{
				this.m_Select.onTransition.RemoveListener(new UnityAction<UISelectField.VisualState, bool>(this.OnTransition));
			}
			this.InstantClearState();
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000A56F0 File Offset: 0x000A38F0
		protected void InstantClearState()
		{
			Selectable.Transition transition = this.m_Transition;
			if (transition == Selectable.Transition.ColorTint)
			{
				this.StartColorTween(Color.white, true);
				return;
			}
			if (transition != Selectable.Transition.SpriteSwap)
			{
				return;
			}
			this.DoSpriteSwap(null);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000A5724 File Offset: 0x000A3924
		public void OnTransition(UISelectField.VisualState state, bool instant)
		{
			if ((this.targetGraphic == null && this.targetGameObject == null) || !base.gameObject.activeInHierarchy || this.m_Transition == Selectable.Transition.None)
			{
				return;
			}
			Color a = this.colors.normalColor;
			Sprite newSprite = null;
			string trigger = this.animationTriggers.normalTrigger;
			switch (state)
			{
			case UISelectField.VisualState.Normal:
				a = this.colors.normalColor;
				newSprite = null;
				trigger = this.animationTriggers.normalTrigger;
				break;
			case UISelectField.VisualState.Highlighted:
				a = this.colors.highlightedColor;
				newSprite = this.spriteState.highlightedSprite;
				trigger = this.animationTriggers.highlightedTrigger;
				break;
			case UISelectField.VisualState.Pressed:
				a = this.colors.pressedColor;
				newSprite = this.spriteState.pressedSprite;
				trigger = this.animationTriggers.pressedTrigger;
				break;
			case UISelectField.VisualState.Active:
				a = this.colors.activeColor;
				newSprite = this.spriteState.activeSprite;
				trigger = this.animationTriggers.activeTrigger;
				break;
			case UISelectField.VisualState.ActiveHighlighted:
				a = this.colors.activeHighlightedColor;
				newSprite = this.spriteState.activeHighlightedSprite;
				trigger = this.animationTriggers.activeHighlightedTrigger;
				break;
			case UISelectField.VisualState.ActivePressed:
				a = this.colors.activePressedColor;
				newSprite = this.spriteState.activePressedSprite;
				trigger = this.animationTriggers.activePressedTrigger;
				break;
			case UISelectField.VisualState.Disabled:
				a = this.colors.disabledColor;
				newSprite = this.spriteState.disabledSprite;
				trigger = this.animationTriggers.disabledTrigger;
				break;
			}
			switch (this.m_Transition)
			{
			case Selectable.Transition.ColorTint:
				this.StartColorTween(a * this.colors.colorMultiplier, instant || this.colors.fadeDuration == 0f);
				return;
			case Selectable.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				return;
			case Selectable.Transition.Animation:
				this.TriggerAnimation(trigger);
				return;
			default:
				return;
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000A5944 File Offset: 0x000A3B44
		private void StartColorTween(Color color, bool instant)
		{
			if (this.targetGraphic == null)
			{
				return;
			}
			if (instant)
			{
				this.targetGraphic.canvasRenderer.SetColor(color);
				return;
			}
			this.targetGraphic.CrossFadeColor(color, this.colors.fadeDuration, true, true);
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000A5994 File Offset: 0x000A3B94
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (this.targetGraphic == null)
			{
				return;
			}
			Image image = this.targetGraphic as Image;
			if (image != null)
			{
				image.overrideSprite = newSprite;
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000A59CC File Offset: 0x000A3BCC
		private void TriggerAnimation(string trigger)
		{
			Animator component = base.GetComponent<Animator>();
			if (component == null || !component.enabled || !component.isActiveAndEnabled || component.runtimeAnimatorController == null || !component.hasBoundPlayables || string.IsNullOrEmpty(trigger))
			{
				return;
			}
			component.ResetTrigger(this.animationTriggers.normalTrigger);
			component.ResetTrigger(this.animationTriggers.pressedTrigger);
			component.ResetTrigger(this.animationTriggers.highlightedTrigger);
			component.ResetTrigger(this.animationTriggers.activeTrigger);
			component.ResetTrigger(this.animationTriggers.activeHighlightedTrigger);
			component.ResetTrigger(this.animationTriggers.activePressedTrigger);
			component.ResetTrigger(this.animationTriggers.disabledTrigger);
			component.SetTrigger(trigger);
		}

		// Token: 0x04001AE4 RID: 6884
		[SerializeField]
		[Tooltip("Graphic that will have the selected transtion applied.")]
		private Graphic m_TargetGraphic;

		// Token: 0x04001AE5 RID: 6885
		[SerializeField]
		[Tooltip("GameObject that will have the selected transtion applied.")]
		private GameObject m_TargetGameObject;

		// Token: 0x04001AE6 RID: 6886
		[SerializeField]
		private Selectable.Transition m_Transition;

		// Token: 0x04001AE7 RID: 6887
		[SerializeField]
		private ColorBlockExtended m_Colors = ColorBlockExtended.defaultColorBlock;

		// Token: 0x04001AE8 RID: 6888
		[SerializeField]
		private SpriteStateExtended m_SpriteState;

		// Token: 0x04001AE9 RID: 6889
		[SerializeField]
		private AnimationTriggersExtended m_AnimationTriggers = new AnimationTriggersExtended();

		// Token: 0x04001AEA RID: 6890
		private UISelectField m_Select;
	}
}
