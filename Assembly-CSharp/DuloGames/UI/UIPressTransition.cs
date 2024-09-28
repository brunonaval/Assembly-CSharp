using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000660 RID: 1632
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Press Transition")]
	public class UIPressTransition : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x0600242F RID: 9263 RVA: 0x000B137C File Offset: 0x000AF57C
		// (set) Token: 0x06002430 RID: 9264 RVA: 0x000B1384 File Offset: 0x000AF584
		public UIPressTransition.Transition transition
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

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06002431 RID: 9265 RVA: 0x000B138D File Offset: 0x000AF58D
		// (set) Token: 0x06002432 RID: 9266 RVA: 0x000B1395 File Offset: 0x000AF595
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

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06002433 RID: 9267 RVA: 0x000B139E File Offset: 0x000AF59E
		// (set) Token: 0x06002434 RID: 9268 RVA: 0x000B13A6 File Offset: 0x000AF5A6
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

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06002435 RID: 9269 RVA: 0x000B13AF File Offset: 0x000AF5AF
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

		// Token: 0x06002436 RID: 9270 RVA: 0x000B13CC File Offset: 0x000AF5CC
		protected UIPressTransition()
		{
			if (this.m_ColorTweenRunner == null)
			{
				this.m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			this.m_ColorTweenRunner.Init(this);
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000B145C File Offset: 0x000AF65C
		protected void Awake()
		{
			this.m_Selectable = base.gameObject.GetComponent<Selectable>();
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x000B146F File Offset: 0x000AF66F
		protected void OnEnable()
		{
			this.InternalEvaluateAndTransitionToNormalState(true);
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x000B1478 File Offset: 0x000AF678
		protected void OnDisable()
		{
			this.InstantClearState();
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x000B1480 File Offset: 0x000AF680
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

		// Token: 0x0600243B RID: 9275 RVA: 0x000B1512 File Offset: 0x000AF712
		public virtual bool IsInteractable()
		{
			if (this.m_Selectable != null)
			{
				return this.m_Selectable.IsInteractable() && this.m_GroupsAllowInteraction;
			}
			return this.m_GroupsAllowInteraction;
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000B1540 File Offset: 0x000AF740
		protected void InstantClearState()
		{
			switch (this.m_Transition)
			{
			case UIPressTransition.Transition.ColorTint:
				this.StartColorTween(Color.white, true);
				return;
			case UIPressTransition.Transition.SpriteSwap:
				this.DoSpriteSwap(null);
				return;
			case UIPressTransition.Transition.Animation:
				break;
			case UIPressTransition.Transition.TextColor:
				this.SetTextColor(Color.white);
				break;
			default:
				return;
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x000B158D File Offset: 0x000AF78D
		private void InternalEvaluateAndTransitionToNormalState(bool instant)
		{
			this.DoStateTransition(UIPressTransition.VisualState.Normal, instant);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x000B1597 File Offset: 0x000AF797
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.DoStateTransition(UIPressTransition.VisualState.Pressed, false);
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000B15A1 File Offset: 0x000AF7A1
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.DoStateTransition(UIPressTransition.VisualState.Normal, false);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000B15AC File Offset: 0x000AF7AC
		protected virtual void DoStateTransition(UIPressTransition.VisualState state, bool instant)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.IsInteractable())
			{
				state = UIPressTransition.VisualState.Normal;
			}
			Color color = this.m_NormalColor;
			Sprite newSprite = null;
			string triggername = this.m_NormalTrigger;
			if (state != UIPressTransition.VisualState.Normal)
			{
				if (state == UIPressTransition.VisualState.Pressed)
				{
					color = this.m_PressedColor;
					newSprite = this.m_PressedSprite;
					triggername = this.m_PressedTrigger;
				}
			}
			else
			{
				color = this.m_NormalColor;
				newSprite = null;
				triggername = this.m_NormalTrigger;
			}
			switch (this.m_Transition)
			{
			case UIPressTransition.Transition.ColorTint:
				this.StartColorTween(color * this.m_ColorMultiplier, instant);
				return;
			case UIPressTransition.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				return;
			case UIPressTransition.Transition.Animation:
				this.TriggerAnimation(triggername);
				return;
			case UIPressTransition.Transition.TextColor:
				this.StartTextColorTween(color, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000B1660 File Offset: 0x000AF860
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

		// Token: 0x06002442 RID: 9282 RVA: 0x000B16BC File Offset: 0x000AF8BC
		private void DoSpriteSwap(Sprite newSprite)
		{
			Image image = this.m_TargetGraphic as Image;
			if (image == null)
			{
				return;
			}
			image.overrideSprite = newSprite;
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x000B16E8 File Offset: 0x000AF8E8
		private void TriggerAnimation(string triggername)
		{
			if (this.targetGameObject == null)
			{
				return;
			}
			if (this.animator == null || !this.animator.enabled || !this.animator.isActiveAndEnabled || this.animator.runtimeAnimatorController == null || !this.animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			this.animator.ResetTrigger(this.m_PressedTrigger);
			this.animator.SetTrigger(triggername);
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x000B1774 File Offset: 0x000AF974
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

		// Token: 0x06002445 RID: 9285 RVA: 0x000B1824 File Offset: 0x000AFA24
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

		// Token: 0x04001D61 RID: 7521
		[SerializeField]
		private UIPressTransition.Transition m_Transition;

		// Token: 0x04001D62 RID: 7522
		[SerializeField]
		private Color m_NormalColor = ColorBlock.defaultColorBlock.normalColor;

		// Token: 0x04001D63 RID: 7523
		[SerializeField]
		private Color m_PressedColor = ColorBlock.defaultColorBlock.pressedColor;

		// Token: 0x04001D64 RID: 7524
		[SerializeField]
		private float m_Duration = 0.1f;

		// Token: 0x04001D65 RID: 7525
		[SerializeField]
		[Range(1f, 6f)]
		private float m_ColorMultiplier = 1f;

		// Token: 0x04001D66 RID: 7526
		[SerializeField]
		private Sprite m_PressedSprite;

		// Token: 0x04001D67 RID: 7527
		[SerializeField]
		private string m_NormalTrigger = "Normal";

		// Token: 0x04001D68 RID: 7528
		[SerializeField]
		private string m_PressedTrigger = "Pressed";

		// Token: 0x04001D69 RID: 7529
		[SerializeField]
		[Tooltip("Graphic that will have the selected transtion applied.")]
		private Graphic m_TargetGraphic;

		// Token: 0x04001D6A RID: 7530
		[SerializeField]
		[Tooltip("GameObject that will have the selected transtion applied.")]
		private GameObject m_TargetGameObject;

		// Token: 0x04001D6B RID: 7531
		private Selectable m_Selectable;

		// Token: 0x04001D6C RID: 7532
		private bool m_GroupsAllowInteraction = true;

		// Token: 0x04001D6D RID: 7533
		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		// Token: 0x04001D6E RID: 7534
		private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

		// Token: 0x02000661 RID: 1633
		public enum VisualState
		{
			// Token: 0x04001D70 RID: 7536
			Normal,
			// Token: 0x04001D71 RID: 7537
			Pressed
		}

		// Token: 0x02000662 RID: 1634
		public enum Transition
		{
			// Token: 0x04001D73 RID: 7539
			None,
			// Token: 0x04001D74 RID: 7540
			ColorTint,
			// Token: 0x04001D75 RID: 7541
			SpriteSwap,
			// Token: 0x04001D76 RID: 7542
			Animation,
			// Token: 0x04001D77 RID: 7543
			TextColor
		}
	}
}
