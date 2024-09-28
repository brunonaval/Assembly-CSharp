using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005E4 RID: 1508
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Tab", 58)]
	public class UITab : Toggle
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06002143 RID: 8515 RVA: 0x000A6DE7 File Offset: 0x000A4FE7
		// (set) Token: 0x06002144 RID: 8516 RVA: 0x000A6DEF File Offset: 0x000A4FEF
		public GameObject targetContent
		{
			get
			{
				return this.m_TargetContent;
			}
			set
			{
				this.m_TargetContent = value;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x000A6DF8 File Offset: 0x000A4FF8
		// (set) Token: 0x06002146 RID: 8518 RVA: 0x000A6E00 File Offset: 0x000A5000
		public Image imageTarget
		{
			get
			{
				return this.m_ImageTarget;
			}
			set
			{
				this.m_ImageTarget = value;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06002147 RID: 8519 RVA: 0x000A6E09 File Offset: 0x000A5009
		// (set) Token: 0x06002148 RID: 8520 RVA: 0x000A6E11 File Offset: 0x000A5011
		public Selectable.Transition imageTransition
		{
			get
			{
				return this.m_ImageTransition;
			}
			set
			{
				this.m_ImageTransition = value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x000A6E1A File Offset: 0x000A501A
		// (set) Token: 0x0600214A RID: 8522 RVA: 0x000A6E22 File Offset: 0x000A5022
		public ColorBlockExtended imageColors
		{
			get
			{
				return this.m_ImageColors;
			}
			set
			{
				this.m_ImageColors = value;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x000A6E2B File Offset: 0x000A502B
		// (set) Token: 0x0600214C RID: 8524 RVA: 0x000A6E33 File Offset: 0x000A5033
		public SpriteStateExtended imageSpriteState
		{
			get
			{
				return this.m_ImageSpriteState;
			}
			set
			{
				this.m_ImageSpriteState = value;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x000A6E3C File Offset: 0x000A503C
		// (set) Token: 0x0600214E RID: 8526 RVA: 0x000A6E44 File Offset: 0x000A5044
		public AnimationTriggersExtended imageAnimationTriggers
		{
			get
			{
				return this.m_ImageAnimationTriggers;
			}
			set
			{
				this.m_ImageAnimationTriggers = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600214F RID: 8527 RVA: 0x000A6E4D File Offset: 0x000A504D
		// (set) Token: 0x06002150 RID: 8528 RVA: 0x000A6E55 File Offset: 0x000A5055
		public Text textTarget
		{
			get
			{
				return this.m_TextTarget;
			}
			set
			{
				this.m_TextTarget = value;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06002151 RID: 8529 RVA: 0x000A6E5E File Offset: 0x000A505E
		// (set) Token: 0x06002152 RID: 8530 RVA: 0x000A6E66 File Offset: 0x000A5066
		public UITab.TextTransition textTransition
		{
			get
			{
				return this.m_TextTransition;
			}
			set
			{
				this.m_TextTransition = value;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06002153 RID: 8531 RVA: 0x000A6E6F File Offset: 0x000A506F
		// (set) Token: 0x06002154 RID: 8532 RVA: 0x000A6E77 File Offset: 0x000A5077
		public ColorBlockExtended textColors
		{
			get
			{
				return this.m_TextColors;
			}
			set
			{
				this.m_TextColors = value;
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000A6E80 File Offset: 0x000A5080
		protected UITab()
		{
			if (this.m_ColorTweenRunner == null)
			{
				this.m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			this.m_ColorTweenRunner.Init(this);
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000A6ED4 File Offset: 0x000A50D4
		protected override void Awake()
		{
			base.Awake();
			if (base.group == null)
			{
				ToggleGroup toggleGroup = UIUtility.FindInParents<ToggleGroup>(base.gameObject);
				if (toggleGroup != null)
				{
					base.group = toggleGroup;
					return;
				}
				base.group = base.transform.parent.gameObject.AddComponent<ToggleGroup>();
			}
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000A6F2D File Offset: 0x000A512D
		protected override void OnEnable()
		{
			base.OnEnable();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleStateChanged));
			this.InternalEvaluateAndTransitionState(true);
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000A6F53 File Offset: 0x000A5153
		protected override void OnDisable()
		{
			base.OnDisable();
			this.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleStateChanged));
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000A6F72 File Offset: 0x000A5172
		protected void OnToggleStateChanged(bool state)
		{
			if (!this.IsActive() || !Application.isPlaying)
			{
				return;
			}
			this.InternalEvaluateAndTransitionState(false);
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000A6F8B File Offset: 0x000A518B
		public void EvaluateAndToggleContent()
		{
			if (this.m_TargetContent != null)
			{
				this.m_TargetContent.SetActive(base.isOn);
			}
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000A6FAC File Offset: 0x000A51AC
		private void InternalEvaluateAndTransitionState(bool instant)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.EvaluateAndToggleContent();
			if (this.graphic != null && this.graphic.transform.childCount > 0)
			{
				float num = (!base.isOn) ? 0f : 1f;
				foreach (object obj in this.graphic.transform)
				{
					Graphic component = ((Transform)obj).GetComponent<Graphic>();
					if (component != null && !component.canvasRenderer.GetAlpha().Equals(num))
					{
						if (instant)
						{
							component.canvasRenderer.SetAlpha(num);
						}
						else
						{
							component.CrossFadeAlpha(num, 0.1f, true);
						}
					}
				}
			}
			this.DoStateTransition(this.m_CurrentState, instant);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000A70A0 File Offset: 0x000A52A0
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.m_CurrentState = state;
			Color a = this.m_ImageColors.normalColor;
			Color a2 = this.m_TextColors.normalColor;
			Sprite newSprite = null;
			string triggername = this.m_ImageAnimationTriggers.normalTrigger;
			switch (state)
			{
			case Selectable.SelectionState.Normal:
				a = ((!base.isOn) ? this.m_ImageColors.normalColor : this.m_ImageColors.activeColor);
				a2 = ((!base.isOn) ? this.m_TextColors.normalColor : this.m_TextColors.activeColor);
				newSprite = ((!base.isOn) ? null : this.m_ImageSpriteState.activeSprite);
				triggername = ((!base.isOn) ? this.m_ImageAnimationTriggers.normalTrigger : this.m_ImageAnimationTriggers.activeTrigger);
				break;
			case Selectable.SelectionState.Highlighted:
				a = ((!base.isOn) ? this.m_ImageColors.highlightedColor : this.m_ImageColors.activeHighlightedColor);
				a2 = ((!base.isOn) ? this.m_TextColors.highlightedColor : this.m_TextColors.activeHighlightedColor);
				newSprite = ((!base.isOn) ? this.m_ImageSpriteState.highlightedSprite : this.m_ImageSpriteState.activeHighlightedSprite);
				triggername = ((!base.isOn) ? this.m_ImageAnimationTriggers.highlightedTrigger : this.m_ImageAnimationTriggers.activeHighlightedTrigger);
				break;
			case Selectable.SelectionState.Pressed:
				a = ((!base.isOn) ? this.m_ImageColors.pressedColor : this.m_ImageColors.activePressedColor);
				a2 = ((!base.isOn) ? this.m_TextColors.pressedColor : this.m_TextColors.activePressedColor);
				newSprite = ((!base.isOn) ? this.m_ImageSpriteState.pressedSprite : this.m_ImageSpriteState.activePressedSprite);
				triggername = ((!base.isOn) ? this.m_ImageAnimationTriggers.pressedTrigger : this.m_ImageAnimationTriggers.activePressedTrigger);
				break;
			case Selectable.SelectionState.Disabled:
				a = this.m_ImageColors.disabledColor;
				a2 = this.m_TextColors.disabledColor;
				newSprite = this.m_ImageSpriteState.disabledSprite;
				triggername = this.m_ImageAnimationTriggers.disabledTrigger;
				break;
			}
			if (base.gameObject.activeInHierarchy)
			{
				switch (this.m_ImageTransition)
				{
				case Selectable.Transition.ColorTint:
					this.StartColorTween(this.m_ImageTarget, a * this.m_ImageColors.colorMultiplier, instant ? 0f : this.m_ImageColors.fadeDuration);
					break;
				case Selectable.Transition.SpriteSwap:
					this.DoSpriteSwap(this.m_ImageTarget, newSprite);
					break;
				case Selectable.Transition.Animation:
					this.TriggerAnimation(this.m_ImageTarget.gameObject, triggername);
					break;
				}
				if (this.m_TextTransition == UITab.TextTransition.ColorTint)
				{
					this.StartColorTweenText(a2 * this.m_TextColors.colorMultiplier, instant ? 0f : this.m_TextColors.fadeDuration);
				}
			}
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000A60C4 File Offset: 0x000A42C4
		private void StartColorTween(Graphic target, Color targetColor, float duration)
		{
			if (target == null)
			{
				return;
			}
			if (!Application.isPlaying || duration == 0f)
			{
				target.canvasRenderer.SetColor(targetColor);
				return;
			}
			target.CrossFadeColor(targetColor, duration, true, true);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000A737C File Offset: 0x000A557C
		private void StartColorTweenText(Color targetColor, float duration)
		{
			if (this.m_TextTarget == null)
			{
				return;
			}
			if (!Application.isPlaying || duration == 0f)
			{
				this.m_TextTarget.color = targetColor;
				return;
			}
			ColorTween info = new ColorTween
			{
				duration = duration,
				startColor = this.m_TextTarget.color,
				targetColor = targetColor
			};
			info.AddOnChangedCallback(new UnityAction<Color>(this.SetTextColor));
			info.ignoreTimeScale = true;
			this.m_ColorTweenRunner.StartTween(info);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000A7407 File Offset: 0x000A5607
		private void SetTextColor(Color color)
		{
			if (this.m_TextTarget == null)
			{
				return;
			}
			this.m_TextTarget.color = color;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000A7424 File Offset: 0x000A5624
		private void DoSpriteSwap(Image target, Sprite newSprite)
		{
			if (target == null)
			{
				return;
			}
			if (!target.overrideSprite.Equals(newSprite))
			{
				target.overrideSprite = newSprite;
			}
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000A7448 File Offset: 0x000A5648
		private void TriggerAnimation(GameObject target, string triggername)
		{
			if (target == null)
			{
				return;
			}
			Animator component = target.GetComponent<Animator>();
			if (component == null || !component.enabled || !component.isActiveAndEnabled || component.runtimeAnimatorController == null || !component.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			component.ResetTrigger(this.m_ImageAnimationTriggers.normalTrigger);
			component.ResetTrigger(this.m_ImageAnimationTriggers.pressedTrigger);
			component.ResetTrigger(this.m_ImageAnimationTriggers.highlightedTrigger);
			component.ResetTrigger(this.m_ImageAnimationTriggers.activeTrigger);
			component.ResetTrigger(this.m_ImageAnimationTriggers.activeHighlightedTrigger);
			component.ResetTrigger(this.m_ImageAnimationTriggers.activePressedTrigger);
			component.ResetTrigger(this.m_ImageAnimationTriggers.disabledTrigger);
			component.SetTrigger(triggername);
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000A751C File Offset: 0x000A571C
		public void Activate()
		{
			if (!base.isOn)
			{
				base.isOn = true;
			}
		}

		// Token: 0x04001B1E RID: 6942
		[SerializeField]
		private GameObject m_TargetContent;

		// Token: 0x04001B1F RID: 6943
		[SerializeField]
		private Image m_ImageTarget;

		// Token: 0x04001B20 RID: 6944
		[SerializeField]
		private Selectable.Transition m_ImageTransition;

		// Token: 0x04001B21 RID: 6945
		[SerializeField]
		private ColorBlockExtended m_ImageColors = ColorBlockExtended.defaultColorBlock;

		// Token: 0x04001B22 RID: 6946
		[SerializeField]
		private SpriteStateExtended m_ImageSpriteState;

		// Token: 0x04001B23 RID: 6947
		[SerializeField]
		private AnimationTriggersExtended m_ImageAnimationTriggers = new AnimationTriggersExtended();

		// Token: 0x04001B24 RID: 6948
		[SerializeField]
		private Text m_TextTarget;

		// Token: 0x04001B25 RID: 6949
		[SerializeField]
		private UITab.TextTransition m_TextTransition;

		// Token: 0x04001B26 RID: 6950
		[SerializeField]
		private ColorBlockExtended m_TextColors = ColorBlockExtended.defaultColorBlock;

		// Token: 0x04001B27 RID: 6951
		private Selectable.SelectionState m_CurrentState;

		// Token: 0x04001B28 RID: 6952
		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		// Token: 0x020005E5 RID: 1509
		public enum TextTransition
		{
			// Token: 0x04001B2A RID: 6954
			None,
			// Token: 0x04001B2B RID: 6955
			ColorTint
		}
	}
}
