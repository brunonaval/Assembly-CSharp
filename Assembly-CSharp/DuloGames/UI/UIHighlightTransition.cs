using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200065D RID: 1629
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Highlight Transition")]
	public class UIHighlightTransition : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x000B0A26 File Offset: 0x000AEC26
		// (set) Token: 0x06002410 RID: 9232 RVA: 0x000B0A2E File Offset: 0x000AEC2E
		public UIHighlightTransition.Transition transition
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

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x000B0A37 File Offset: 0x000AEC37
		// (set) Token: 0x06002412 RID: 9234 RVA: 0x000B0A3F File Offset: 0x000AEC3F
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

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x000B0A48 File Offset: 0x000AEC48
		// (set) Token: 0x06002414 RID: 9236 RVA: 0x000B0A50 File Offset: 0x000AEC50
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

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x000B0A59 File Offset: 0x000AEC59
		// (set) Token: 0x06002416 RID: 9238 RVA: 0x000B0A61 File Offset: 0x000AEC61
		public CanvasGroup targetCanvasGroup
		{
			get
			{
				return this.m_TargetCanvasGroup;
			}
			set
			{
				this.m_TargetCanvasGroup = value;
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x000B0A6A File Offset: 0x000AEC6A
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

		// Token: 0x06002418 RID: 9240 RVA: 0x000B0A88 File Offset: 0x000AEC88
		protected UIHighlightTransition()
		{
			if (this.m_ColorTweenRunner == null)
			{
				this.m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_ColorTweenRunner.Init(this);
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002419 RID: 9241 RVA: 0x000B0BB4 File Offset: 0x000AEDB4
		protected void Awake()
		{
			if (this.m_UseToggle)
			{
				if (this.m_TargetToggle == null)
				{
					this.m_TargetToggle = base.gameObject.GetComponent<Toggle>();
				}
				if (this.m_TargetToggle != null)
				{
					this.m_Active = this.m_TargetToggle.isOn;
				}
			}
			this.m_Selectable = base.gameObject.GetComponent<Selectable>();
		}

		// Token: 0x0600241A RID: 9242 RVA: 0x000B0C18 File Offset: 0x000AEE18
		protected void OnEnable()
		{
			if (this.m_TargetToggle != null)
			{
				this.m_TargetToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChange));
			}
			this.InternalEvaluateAndTransitionToNormalState(true);
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000B0C4B File Offset: 0x000AEE4B
		protected void OnDisable()
		{
			if (this.m_TargetToggle != null)
			{
				this.m_TargetToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleValueChange));
			}
			this.InstantClearState();
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000B0C7D File Offset: 0x000AEE7D
		private void InternalEvaluateAndTransitionToNormalState(bool instant)
		{
			this.DoStateTransition(this.m_Active ? UIHighlightTransition.VisualState.Active : UIHighlightTransition.VisualState.Normal, instant);
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000B0C94 File Offset: 0x000AEE94
		protected void OnCanvasGroupChanged()
		{
			bool flag = true;
			Transform transform = base.transform;
			while (transform != null)
			{
				transform.GetComponents<CanvasGroup>(this.m_CanvasGroupCache);
				bool flag2 = false;
				for (int i = 0; i < this.m_CanvasGroupCache.Count; i++)
				{
					if (!this.m_CanvasGroupCache[i].interactable)
					{
						flag = false;
						flag2 = true;
					}
					if (this.m_CanvasGroupCache[i].ignoreParentGroups)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					break;
				}
				transform = transform.parent;
			}
			if (flag != this.m_GroupsAllowInteraction)
			{
				this.m_GroupsAllowInteraction = flag;
				this.InternalEvaluateAndTransitionToNormalState(true);
			}
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x000B0D26 File Offset: 0x000AEF26
		public virtual bool IsInteractable()
		{
			if (this.m_Selectable != null)
			{
				return this.m_Selectable.IsInteractable() && this.m_GroupsAllowInteraction;
			}
			return this.m_GroupsAllowInteraction;
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x000B0D54 File Offset: 0x000AEF54
		protected void OnToggleValueChange(bool value)
		{
			if (!this.m_UseToggle || this.m_TargetToggle == null)
			{
				return;
			}
			this.m_Active = this.m_TargetToggle.isOn;
			if (this.m_Transition == UIHighlightTransition.Transition.Animation)
			{
				if (this.targetGameObject == null || this.animator == null || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(this.m_ActiveBool))
				{
					return;
				}
				this.animator.SetBool(this.m_ActiveBool, this.m_Active);
			}
			this.DoStateTransition(this.m_Active ? UIHighlightTransition.VisualState.Active : (this.m_Selected ? UIHighlightTransition.VisualState.Selected : (this.m_Highlighted ? UIHighlightTransition.VisualState.Highlighted : UIHighlightTransition.VisualState.Normal)), false);
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x000B0E1C File Offset: 0x000AF01C
		protected void InstantClearState()
		{
			switch (this.m_Transition)
			{
			case UIHighlightTransition.Transition.ColorTint:
				this.StartColorTween(Color.white, true);
				return;
			case UIHighlightTransition.Transition.SpriteSwap:
				this.DoSpriteSwap(null);
				return;
			case UIHighlightTransition.Transition.Animation:
				break;
			case UIHighlightTransition.Transition.TextColor:
				this.SetTextColor(this.m_NormalColor);
				return;
			case UIHighlightTransition.Transition.CanvasGroup:
				this.SetCanvasGroupAlpha(1f);
				break;
			default:
				return;
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000B0E7A File Offset: 0x000AF07A
		public void OnSelect(BaseEventData eventData)
		{
			this.m_Selected = true;
			if (this.m_Active)
			{
				return;
			}
			this.DoStateTransition(UIHighlightTransition.VisualState.Selected, false);
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x000B0E94 File Offset: 0x000AF094
		public void OnDeselect(BaseEventData eventData)
		{
			this.m_Selected = false;
			if (this.m_Active)
			{
				return;
			}
			this.DoStateTransition(this.m_Highlighted ? UIHighlightTransition.VisualState.Highlighted : UIHighlightTransition.VisualState.Normal, false);
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.m_Highlighted = true;
			if (!this.m_Selected && !this.m_Pressed && !this.m_Active)
			{
				this.DoStateTransition(UIHighlightTransition.VisualState.Highlighted, false);
			}
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x000B0EE2 File Offset: 0x000AF0E2
		public void OnPointerExit(PointerEventData eventData)
		{
			this.m_Highlighted = false;
			if (!this.m_Selected && !this.m_Pressed && !this.m_Active)
			{
				this.DoStateTransition(UIHighlightTransition.VisualState.Normal, false);
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000B0F0B File Offset: 0x000AF10B
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
			if (this.m_Active)
			{
				return;
			}
			this.m_Pressed = true;
			this.DoStateTransition(UIHighlightTransition.VisualState.Pressed, false);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000B0F38 File Offset: 0x000AF138
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Pressed = false;
			UIHighlightTransition.VisualState state = UIHighlightTransition.VisualState.Normal;
			if (this.m_Active)
			{
				state = UIHighlightTransition.VisualState.Active;
			}
			else if (this.m_Selected)
			{
				state = UIHighlightTransition.VisualState.Selected;
			}
			else if (this.m_Highlighted)
			{
				state = UIHighlightTransition.VisualState.Highlighted;
			}
			this.DoStateTransition(state, false);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000B0F84 File Offset: 0x000AF184
		protected virtual void DoStateTransition(UIHighlightTransition.VisualState state, bool instant)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.IsInteractable())
			{
				state = UIHighlightTransition.VisualState.Normal;
			}
			Color color = this.m_NormalColor;
			Sprite newSprite = null;
			string triggername = this.m_NormalTrigger;
			float targetAlpha = this.m_NormalAlpha;
			switch (state)
			{
			case UIHighlightTransition.VisualState.Normal:
				color = this.m_NormalColor;
				newSprite = null;
				triggername = this.m_NormalTrigger;
				targetAlpha = this.m_NormalAlpha;
				break;
			case UIHighlightTransition.VisualState.Highlighted:
				color = this.m_HighlightedColor;
				newSprite = this.m_HighlightedSprite;
				triggername = this.m_HighlightedTrigger;
				targetAlpha = this.m_HighlightedAlpha;
				break;
			case UIHighlightTransition.VisualState.Selected:
				color = this.m_SelectedColor;
				newSprite = this.m_SelectedSprite;
				triggername = this.m_SelectedTrigger;
				targetAlpha = this.m_SelectedAlpha;
				break;
			case UIHighlightTransition.VisualState.Pressed:
				color = this.m_PressedColor;
				newSprite = this.m_PressedSprite;
				triggername = this.m_PressedTrigger;
				targetAlpha = this.m_PressedAlpha;
				break;
			case UIHighlightTransition.VisualState.Active:
				color = this.m_ActiveColor;
				newSprite = this.m_ActiveSprite;
				targetAlpha = this.m_ActiveAlpha;
				break;
			}
			switch (this.m_Transition)
			{
			case UIHighlightTransition.Transition.ColorTint:
				this.StartColorTween(color * this.m_ColorMultiplier, instant);
				return;
			case UIHighlightTransition.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				return;
			case UIHighlightTransition.Transition.Animation:
				this.TriggerAnimation(triggername);
				return;
			case UIHighlightTransition.Transition.TextColor:
				this.StartTextColorTween(color, false);
				return;
			case UIHighlightTransition.Transition.CanvasGroup:
				this.StartCanvasGroupTween(targetAlpha, instant);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000B10C4 File Offset: 0x000AF2C4
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (this.m_TargetGraphic == null)
			{
				return;
			}
			if (instant || this.m_Duration == 0f || !Application.isPlaying)
			{
				this.m_TargetGraphic.canvasRenderer.SetColor(targetColor);
				return;
			}
			this.m_TargetGraphic.CrossFadeColor(targetColor, this.m_Duration, true, true);
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000B1120 File Offset: 0x000AF320
		private void DoSpriteSwap(Sprite newSprite)
		{
			Image image = this.m_TargetGraphic as Image;
			if (image == null)
			{
				return;
			}
			image.overrideSprite = newSprite;
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000B114C File Offset: 0x000AF34C
		private void TriggerAnimation(string triggername)
		{
			if (this.targetGameObject == null || this.animator == null || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null || !this.animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			this.animator.ResetTrigger(this.m_HighlightedTrigger);
			this.animator.ResetTrigger(this.m_SelectedTrigger);
			this.animator.ResetTrigger(this.m_PressedTrigger);
			this.animator.SetTrigger(triggername);
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000B11EC File Offset: 0x000AF3EC
		private void StartTextColorTween(Color targetColor, bool instant)
		{
			if (this.m_TargetGraphic == null)
			{
				return;
			}
			if (!(this.m_TargetGraphic is Text))
			{
				return;
			}
			if (instant || this.m_Duration == 0f || !Application.isPlaying)
			{
				(this.m_TargetGraphic as Text).color = targetColor;
				return;
			}
			ColorTween info = new ColorTween
			{
				duration = this.m_Duration,
				startColor = (this.m_TargetGraphic as Text).color,
				targetColor = targetColor
			};
			info.AddOnChangedCallback(new UnityAction<Color>(this.SetTextColor));
			info.ignoreTimeScale = true;
			this.m_ColorTweenRunner.StartTween(info);
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000B129C File Offset: 0x000AF49C
		private void SetTextColor(Color targetColor)
		{
			if (this.m_TargetGraphic == null)
			{
				return;
			}
			if (this.m_TargetGraphic is Text)
			{
				(this.m_TargetGraphic as Text).color = targetColor;
			}
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x000B12CC File Offset: 0x000AF4CC
		private void StartCanvasGroupTween(float targetAlpha, bool instant)
		{
			if (this.m_TargetCanvasGroup == null)
			{
				return;
			}
			if (instant || this.m_Duration == 0f || !Application.isPlaying)
			{
				this.SetCanvasGroupAlpha(targetAlpha);
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = this.m_Duration,
				startFloat = this.m_TargetCanvasGroup.alpha,
				targetFloat = targetAlpha
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetCanvasGroupAlpha));
			info.ignoreTimeScale = true;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x000B135F File Offset: 0x000AF55F
		private void SetCanvasGroupAlpha(float alpha)
		{
			if (this.m_TargetCanvasGroup == null)
			{
				return;
			}
			this.m_TargetCanvasGroup.alpha = alpha;
		}

		// Token: 0x04001D30 RID: 7472
		[SerializeField]
		private UIHighlightTransition.Transition m_Transition;

		// Token: 0x04001D31 RID: 7473
		[SerializeField]
		private Color m_NormalColor = ColorBlock.defaultColorBlock.normalColor;

		// Token: 0x04001D32 RID: 7474
		[SerializeField]
		private Color m_HighlightedColor = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001D33 RID: 7475
		[SerializeField]
		private Color m_SelectedColor = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001D34 RID: 7476
		[SerializeField]
		private Color m_PressedColor = ColorBlock.defaultColorBlock.pressedColor;

		// Token: 0x04001D35 RID: 7477
		[SerializeField]
		private Color m_ActiveColor = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001D36 RID: 7478
		[SerializeField]
		private float m_Duration = 0.1f;

		// Token: 0x04001D37 RID: 7479
		[SerializeField]
		[Range(1f, 6f)]
		private float m_ColorMultiplier = 1f;

		// Token: 0x04001D38 RID: 7480
		[SerializeField]
		private Sprite m_HighlightedSprite;

		// Token: 0x04001D39 RID: 7481
		[SerializeField]
		private Sprite m_SelectedSprite;

		// Token: 0x04001D3A RID: 7482
		[SerializeField]
		private Sprite m_PressedSprite;

		// Token: 0x04001D3B RID: 7483
		[SerializeField]
		private Sprite m_ActiveSprite;

		// Token: 0x04001D3C RID: 7484
		[SerializeField]
		private string m_NormalTrigger = "Normal";

		// Token: 0x04001D3D RID: 7485
		[SerializeField]
		private string m_HighlightedTrigger = "Highlighted";

		// Token: 0x04001D3E RID: 7486
		[SerializeField]
		private string m_SelectedTrigger = "Selected";

		// Token: 0x04001D3F RID: 7487
		[SerializeField]
		private string m_PressedTrigger = "Pressed";

		// Token: 0x04001D40 RID: 7488
		[SerializeField]
		private string m_ActiveBool = "Active";

		// Token: 0x04001D41 RID: 7489
		[SerializeField]
		[Range(0f, 1f)]
		private float m_NormalAlpha;

		// Token: 0x04001D42 RID: 7490
		[SerializeField]
		[Range(0f, 1f)]
		private float m_HighlightedAlpha = 1f;

		// Token: 0x04001D43 RID: 7491
		[SerializeField]
		[Range(0f, 1f)]
		private float m_SelectedAlpha = 1f;

		// Token: 0x04001D44 RID: 7492
		[SerializeField]
		[Range(0f, 1f)]
		private float m_PressedAlpha = 1f;

		// Token: 0x04001D45 RID: 7493
		[SerializeField]
		[Range(0f, 1f)]
		private float m_ActiveAlpha = 1f;

		// Token: 0x04001D46 RID: 7494
		[SerializeField]
		[Tooltip("Graphic that will have the selected transtion applied.")]
		private Graphic m_TargetGraphic;

		// Token: 0x04001D47 RID: 7495
		[SerializeField]
		[Tooltip("GameObject that will have the selected transtion applied.")]
		private GameObject m_TargetGameObject;

		// Token: 0x04001D48 RID: 7496
		[SerializeField]
		[Tooltip("CanvasGroup that will have the selected transtion applied.")]
		private CanvasGroup m_TargetCanvasGroup;

		// Token: 0x04001D49 RID: 7497
		[SerializeField]
		private bool m_UseToggle;

		// Token: 0x04001D4A RID: 7498
		[SerializeField]
		private Toggle m_TargetToggle;

		// Token: 0x04001D4B RID: 7499
		private bool m_Highlighted;

		// Token: 0x04001D4C RID: 7500
		private bool m_Selected;

		// Token: 0x04001D4D RID: 7501
		private bool m_Pressed;

		// Token: 0x04001D4E RID: 7502
		private bool m_Active;

		// Token: 0x04001D4F RID: 7503
		private Selectable m_Selectable;

		// Token: 0x04001D50 RID: 7504
		private bool m_GroupsAllowInteraction = true;

		// Token: 0x04001D51 RID: 7505
		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		// Token: 0x04001D52 RID: 7506
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x04001D53 RID: 7507
		private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

		// Token: 0x0200065E RID: 1630
		public enum VisualState
		{
			// Token: 0x04001D55 RID: 7509
			Normal,
			// Token: 0x04001D56 RID: 7510
			Highlighted,
			// Token: 0x04001D57 RID: 7511
			Selected,
			// Token: 0x04001D58 RID: 7512
			Pressed,
			// Token: 0x04001D59 RID: 7513
			Active
		}

		// Token: 0x0200065F RID: 1631
		public enum Transition
		{
			// Token: 0x04001D5B RID: 7515
			None,
			// Token: 0x04001D5C RID: 7516
			ColorTint,
			// Token: 0x04001D5D RID: 7517
			SpriteSwap,
			// Token: 0x04001D5E RID: 7518
			Animation,
			// Token: 0x04001D5F RID: 7519
			TextColor,
			// Token: 0x04001D60 RID: 7520
			CanvasGroup
		}
	}
}
