using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000663 RID: 1635
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Toggle Active Transition")]
	public class UIToggleActiveTransition : MonoBehaviour, IEventSystemHandler
	{
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06002446 RID: 9286 RVA: 0x000B1853 File Offset: 0x000AFA53
		// (set) Token: 0x06002447 RID: 9287 RVA: 0x000B185B File Offset: 0x000AFA5B
		public UIToggleActiveTransition.Transition transition
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

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x000B1864 File Offset: 0x000AFA64
		// (set) Token: 0x06002449 RID: 9289 RVA: 0x000B186C File Offset: 0x000AFA6C
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

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000B1875 File Offset: 0x000AFA75
		// (set) Token: 0x0600244B RID: 9291 RVA: 0x000B187D File Offset: 0x000AFA7D
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

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600244C RID: 9292 RVA: 0x000B1886 File Offset: 0x000AFA86
		// (set) Token: 0x0600244D RID: 9293 RVA: 0x000B188E File Offset: 0x000AFA8E
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

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600244E RID: 9294 RVA: 0x000B1897 File Offset: 0x000AFA97
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

		// Token: 0x0600244F RID: 9295 RVA: 0x000B18B4 File Offset: 0x000AFAB4
		protected UIToggleActiveTransition()
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

		// Token: 0x06002450 RID: 9296 RVA: 0x000B1966 File Offset: 0x000AFB66
		protected void Awake()
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

		// Token: 0x06002451 RID: 9297 RVA: 0x000B19A6 File Offset: 0x000AFBA6
		protected void OnEnable()
		{
			if (this.m_TargetToggle != null)
			{
				this.m_TargetToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChange));
			}
			this.InternalEvaluateAndTransitionToNormalState(true);
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000B19D9 File Offset: 0x000AFBD9
		protected void OnDisable()
		{
			if (this.m_TargetToggle != null)
			{
				this.m_TargetToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleValueChange));
			}
			this.InstantClearState();
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000B1A0B File Offset: 0x000AFC0B
		private void InternalEvaluateAndTransitionToNormalState(bool instant)
		{
			this.DoStateTransition(this.m_Active ? UIToggleActiveTransition.VisualState.Active : UIToggleActiveTransition.VisualState.Normal, instant);
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000B1A20 File Offset: 0x000AFC20
		protected void OnToggleValueChange(bool value)
		{
			if (this.m_TargetToggle == null)
			{
				return;
			}
			this.m_Active = this.m_TargetToggle.isOn;
			if (this.m_Transition == UIToggleActiveTransition.Transition.Animation)
			{
				if (this.targetGameObject == null || this.animator == null || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(this.m_ActiveBool))
				{
					return;
				}
				this.animator.SetBool(this.m_ActiveBool, this.m_Active);
			}
			this.DoStateTransition(this.m_Active ? UIToggleActiveTransition.VisualState.Active : UIToggleActiveTransition.VisualState.Normal, false);
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x000B1ACC File Offset: 0x000AFCCC
		protected void InstantClearState()
		{
			switch (this.m_Transition)
			{
			case UIToggleActiveTransition.Transition.ColorTint:
				this.StartColorTween(Color.white, true);
				return;
			case UIToggleActiveTransition.Transition.SpriteSwap:
				this.DoSpriteSwap(null);
				return;
			case UIToggleActiveTransition.Transition.Animation:
				break;
			case UIToggleActiveTransition.Transition.TextColor:
				this.SetTextColor(this.m_NormalColor);
				return;
			case UIToggleActiveTransition.Transition.CanvasGroup:
				this.SetCanvasGroupAlpha(1f);
				break;
			default:
				return;
			}
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x000B1B2C File Offset: 0x000AFD2C
		protected virtual void DoStateTransition(UIToggleActiveTransition.VisualState state, bool instant)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Color color = this.m_NormalColor;
			Sprite newSprite = null;
			string normalTrigger = this.m_NormalTrigger;
			float targetAlpha = this.m_NormalAlpha;
			if (state != UIToggleActiveTransition.VisualState.Normal)
			{
				if (state == UIToggleActiveTransition.VisualState.Active)
				{
					color = this.m_ActiveColor;
					newSprite = this.m_ActiveSprite;
					normalTrigger = this.m_NormalTrigger;
					targetAlpha = this.m_ActiveAlpha;
				}
			}
			else
			{
				color = this.m_NormalColor;
				newSprite = null;
				normalTrigger = this.m_NormalTrigger;
				targetAlpha = this.m_NormalAlpha;
			}
			switch (this.m_Transition)
			{
			case UIToggleActiveTransition.Transition.ColorTint:
				this.StartColorTween(color * this.m_ColorMultiplier, instant);
				return;
			case UIToggleActiveTransition.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				return;
			case UIToggleActiveTransition.Transition.Animation:
				this.TriggerAnimation(normalTrigger);
				return;
			case UIToggleActiveTransition.Transition.TextColor:
				this.StartTextColorTween(color, false);
				return;
			case UIToggleActiveTransition.Transition.CanvasGroup:
				this.StartCanvasGroupTween(targetAlpha, instant);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000B1BF8 File Offset: 0x000AFDF8
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

		// Token: 0x06002458 RID: 9304 RVA: 0x000B1C54 File Offset: 0x000AFE54
		private void DoSpriteSwap(Sprite newSprite)
		{
			Image image = this.m_TargetGraphic as Image;
			if (image == null)
			{
				return;
			}
			image.overrideSprite = newSprite;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x000B1C80 File Offset: 0x000AFE80
		private void TriggerAnimation(string triggername)
		{
			if (this.targetGameObject == null || this.animator == null || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null || !this.animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			this.animator.ResetTrigger(this.m_NormalTrigger);
			this.animator.SetTrigger(triggername);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000B1CFC File Offset: 0x000AFEFC
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

		// Token: 0x0600245B RID: 9307 RVA: 0x000B1DAC File Offset: 0x000AFFAC
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

		// Token: 0x0600245C RID: 9308 RVA: 0x000B1DDC File Offset: 0x000AFFDC
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

		// Token: 0x0600245D RID: 9309 RVA: 0x000B1E6F File Offset: 0x000B006F
		private void SetCanvasGroupAlpha(float alpha)
		{
			if (this.m_TargetCanvasGroup == null)
			{
				return;
			}
			this.m_TargetCanvasGroup.alpha = alpha;
		}

		// Token: 0x04001D78 RID: 7544
		[SerializeField]
		private UIToggleActiveTransition.Transition m_Transition;

		// Token: 0x04001D79 RID: 7545
		[SerializeField]
		private Color m_NormalColor = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04001D7A RID: 7546
		[SerializeField]
		private Color m_ActiveColor = Color.white;

		// Token: 0x04001D7B RID: 7547
		[SerializeField]
		private float m_Duration = 0.1f;

		// Token: 0x04001D7C RID: 7548
		[SerializeField]
		[Range(1f, 6f)]
		private float m_ColorMultiplier = 1f;

		// Token: 0x04001D7D RID: 7549
		[SerializeField]
		private Sprite m_ActiveSprite;

		// Token: 0x04001D7E RID: 7550
		[SerializeField]
		private string m_NormalTrigger = "Normal";

		// Token: 0x04001D7F RID: 7551
		[SerializeField]
		private string m_ActiveBool = "Active";

		// Token: 0x04001D80 RID: 7552
		[SerializeField]
		[Range(0f, 1f)]
		private float m_NormalAlpha;

		// Token: 0x04001D81 RID: 7553
		[SerializeField]
		[Range(0f, 1f)]
		private float m_ActiveAlpha = 1f;

		// Token: 0x04001D82 RID: 7554
		[SerializeField]
		[Tooltip("Graphic that will have the selected transtion applied.")]
		private Graphic m_TargetGraphic;

		// Token: 0x04001D83 RID: 7555
		[SerializeField]
		[Tooltip("GameObject that will have the selected transtion applied.")]
		private GameObject m_TargetGameObject;

		// Token: 0x04001D84 RID: 7556
		[SerializeField]
		[Tooltip("CanvasGroup that will have the selected transtion applied.")]
		private CanvasGroup m_TargetCanvasGroup;

		// Token: 0x04001D85 RID: 7557
		[SerializeField]
		private Toggle m_TargetToggle;

		// Token: 0x04001D86 RID: 7558
		private bool m_Active;

		// Token: 0x04001D87 RID: 7559
		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		// Token: 0x04001D88 RID: 7560
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x02000664 RID: 1636
		public enum VisualState
		{
			// Token: 0x04001D8A RID: 7562
			Normal,
			// Token: 0x04001D8B RID: 7563
			Active
		}

		// Token: 0x02000665 RID: 1637
		public enum Transition
		{
			// Token: 0x04001D8D RID: 7565
			None,
			// Token: 0x04001D8E RID: 7566
			ColorTint,
			// Token: 0x04001D8F RID: 7567
			SpriteSwap,
			// Token: 0x04001D90 RID: 7568
			Animation,
			// Token: 0x04001D91 RID: 7569
			TextColor,
			// Token: 0x04001D92 RID: 7570
			CanvasGroup
		}
	}
}
