using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200065B RID: 1627
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effect Transition")]
	public class UIEffectTransition : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
		// Token: 0x060023FC RID: 9212 RVA: 0x000B049C File Offset: 0x000AE69C
		protected UIEffectTransition()
		{
			if (this.m_ColorTweenRunner == null)
			{
				this.m_ColorTweenRunner = new TweenRunner<ColorTween>();
			}
			this.m_ColorTweenRunner.Init(this);
		}

		// Token: 0x060023FD RID: 9213 RVA: 0x000B053C File Offset: 0x000AE73C
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

		// Token: 0x060023FE RID: 9214 RVA: 0x000B05A0 File Offset: 0x000AE7A0
		protected void OnEnable()
		{
			if (this.m_TargetToggle != null)
			{
				this.m_TargetToggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChange));
			}
			this.InternalEvaluateAndTransitionToNormalState(true);
		}

		// Token: 0x060023FF RID: 9215 RVA: 0x000B05D3 File Offset: 0x000AE7D3
		protected void OnDisable()
		{
			if (this.m_TargetToggle != null)
			{
				this.m_TargetToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleValueChange));
			}
			this.InstantClearState();
		}

		// Token: 0x06002400 RID: 9216 RVA: 0x000B0608 File Offset: 0x000AE808
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

		// Token: 0x06002401 RID: 9217 RVA: 0x000B069A File Offset: 0x000AE89A
		public virtual bool IsInteractable()
		{
			if (this.m_Selectable != null)
			{
				return this.m_Selectable.IsInteractable() && this.m_GroupsAllowInteraction;
			}
			return this.m_GroupsAllowInteraction;
		}

		// Token: 0x06002402 RID: 9218 RVA: 0x000B06C8 File Offset: 0x000AE8C8
		protected void OnToggleValueChange(bool value)
		{
			if (!this.m_UseToggle || this.m_TargetToggle == null)
			{
				return;
			}
			this.m_Active = this.m_TargetToggle.isOn;
			if (!this.m_TargetToggle.isOn)
			{
				this.DoStateTransition(this.m_Selected ? UIEffectTransition.VisualState.Selected : UIEffectTransition.VisualState.Normal, false);
			}
		}

		// Token: 0x06002403 RID: 9219 RVA: 0x000B071D File Offset: 0x000AE91D
		protected void InstantClearState()
		{
			this.SetEffectColor(Color.white);
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000B072A File Offset: 0x000AE92A
		private void InternalEvaluateAndTransitionToNormalState(bool instant)
		{
			this.DoStateTransition(this.m_Active ? UIEffectTransition.VisualState.Active : UIEffectTransition.VisualState.Normal, instant);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000B073F File Offset: 0x000AE93F
		public void OnSelect(BaseEventData eventData)
		{
			this.m_Selected = true;
			if (this.m_Active)
			{
				return;
			}
			this.DoStateTransition(UIEffectTransition.VisualState.Selected, false);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000B0759 File Offset: 0x000AE959
		public void OnDeselect(BaseEventData eventData)
		{
			this.m_Selected = false;
			if (this.m_Active)
			{
				return;
			}
			this.DoStateTransition(this.m_Highlighted ? UIEffectTransition.VisualState.Highlighted : UIEffectTransition.VisualState.Normal, false);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000B077E File Offset: 0x000AE97E
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.m_Highlighted = true;
			if (!this.m_Selected && !this.m_Pressed && !this.m_Active)
			{
				this.DoStateTransition(UIEffectTransition.VisualState.Highlighted, false);
			}
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000B07A7 File Offset: 0x000AE9A7
		public void OnPointerExit(PointerEventData eventData)
		{
			this.m_Highlighted = false;
			if (!this.m_Selected && !this.m_Pressed && !this.m_Active)
			{
				this.DoStateTransition(UIEffectTransition.VisualState.Normal, false);
			}
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000B07D0 File Offset: 0x000AE9D0
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
			this.DoStateTransition(UIEffectTransition.VisualState.Pressed, false);
		}

		// Token: 0x0600240A RID: 9226 RVA: 0x000B07F4 File Offset: 0x000AE9F4
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.m_Pressed = false;
			UIEffectTransition.VisualState state = UIEffectTransition.VisualState.Normal;
			if (this.m_Active)
			{
				state = UIEffectTransition.VisualState.Active;
			}
			else if (this.m_Selected)
			{
				state = UIEffectTransition.VisualState.Selected;
			}
			else if (this.m_Highlighted)
			{
				state = UIEffectTransition.VisualState.Highlighted;
			}
			this.DoStateTransition(state, false);
		}

		// Token: 0x0600240B RID: 9227 RVA: 0x000B0840 File Offset: 0x000AEA40
		protected virtual void DoStateTransition(UIEffectTransition.VisualState state, bool instant)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (!this.IsInteractable())
			{
				state = UIEffectTransition.VisualState.Normal;
			}
			Color targetColor = this.m_NormalColor;
			switch (state)
			{
			case UIEffectTransition.VisualState.Normal:
				targetColor = this.m_NormalColor;
				break;
			case UIEffectTransition.VisualState.Highlighted:
				targetColor = this.m_HighlightedColor;
				break;
			case UIEffectTransition.VisualState.Selected:
				targetColor = this.m_SelectedColor;
				break;
			case UIEffectTransition.VisualState.Pressed:
				targetColor = this.m_PressedColor;
				break;
			case UIEffectTransition.VisualState.Active:
				targetColor = this.m_ActiveColor;
				break;
			}
			this.StartEffectColorTween(targetColor, false);
		}

		// Token: 0x0600240C RID: 9228 RVA: 0x000B08BC File Offset: 0x000AEABC
		private void StartEffectColorTween(Color targetColor, bool instant)
		{
			if (this.m_TargetEffect == null)
			{
				return;
			}
			if (!(this.m_TargetEffect is Shadow) && !(this.m_TargetEffect is Outline))
			{
				return;
			}
			if (instant || this.m_Duration == 0f || !Application.isPlaying)
			{
				this.SetEffectColor(targetColor);
				return;
			}
			ColorTween info = new ColorTween
			{
				duration = this.m_Duration,
				startColor = this.GetEffectColor(),
				targetColor = targetColor
			};
			info.AddOnChangedCallback(new UnityAction<Color>(this.SetEffectColor));
			info.ignoreTimeScale = true;
			this.m_ColorTweenRunner.StartTween(info);
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000B0968 File Offset: 0x000AEB68
		private void SetEffectColor(Color targetColor)
		{
			if (this.m_TargetEffect == null)
			{
				return;
			}
			if (this.m_TargetEffect is Shadow)
			{
				(this.m_TargetEffect as Shadow).effectColor = targetColor;
				return;
			}
			if (this.m_TargetEffect is Outline)
			{
				(this.m_TargetEffect as Outline).effectColor = targetColor;
			}
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000B09C4 File Offset: 0x000AEBC4
		private Color GetEffectColor()
		{
			if (this.m_TargetEffect == null)
			{
				return Color.white;
			}
			if (this.m_TargetEffect is Shadow)
			{
				return (this.m_TargetEffect as Shadow).effectColor;
			}
			if (this.m_TargetEffect is Outline)
			{
				return (this.m_TargetEffect as Outline).effectColor;
			}
			return Color.white;
		}

		// Token: 0x04001D19 RID: 7449
		[SerializeField]
		[Tooltip("Graphic that will have the selected transtion applied.")]
		private BaseMeshEffect m_TargetEffect;

		// Token: 0x04001D1A RID: 7450
		[SerializeField]
		private Color m_NormalColor = ColorBlock.defaultColorBlock.normalColor;

		// Token: 0x04001D1B RID: 7451
		[SerializeField]
		private Color m_HighlightedColor = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001D1C RID: 7452
		[SerializeField]
		private Color m_SelectedColor = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001D1D RID: 7453
		[SerializeField]
		private Color m_PressedColor = ColorBlock.defaultColorBlock.pressedColor;

		// Token: 0x04001D1E RID: 7454
		[SerializeField]
		private float m_Duration = 0.1f;

		// Token: 0x04001D1F RID: 7455
		[SerializeField]
		private bool m_UseToggle;

		// Token: 0x04001D20 RID: 7456
		[SerializeField]
		private Toggle m_TargetToggle;

		// Token: 0x04001D21 RID: 7457
		[SerializeField]
		private Color m_ActiveColor = ColorBlock.defaultColorBlock.highlightedColor;

		// Token: 0x04001D22 RID: 7458
		private bool m_Highlighted;

		// Token: 0x04001D23 RID: 7459
		private bool m_Selected;

		// Token: 0x04001D24 RID: 7460
		private bool m_Pressed;

		// Token: 0x04001D25 RID: 7461
		private bool m_Active;

		// Token: 0x04001D26 RID: 7462
		private Selectable m_Selectable;

		// Token: 0x04001D27 RID: 7463
		private bool m_GroupsAllowInteraction = true;

		// Token: 0x04001D28 RID: 7464
		[NonSerialized]
		private readonly TweenRunner<ColorTween> m_ColorTweenRunner;

		// Token: 0x04001D29 RID: 7465
		private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

		// Token: 0x0200065C RID: 1628
		public enum VisualState
		{
			// Token: 0x04001D2B RID: 7467
			Normal,
			// Token: 0x04001D2C RID: 7468
			Highlighted,
			// Token: 0x04001D2D RID: 7469
			Selected,
			// Token: 0x04001D2E RID: 7470
			Pressed,
			// Token: 0x04001D2F RID: 7471
			Active
		}
	}
}
