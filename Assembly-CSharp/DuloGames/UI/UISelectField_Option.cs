using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005D5 RID: 1493
	public class UISelectField_Option : Toggle, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x060020CE RID: 8398 RVA: 0x000A4DC4 File Offset: 0x000A2FC4
		protected override void Start()
		{
			base.Start();
			base.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			base.transition = Selectable.Transition.None;
			this.toggleTransition = Toggle.ToggleTransition.None;
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000A4DFC File Offset: 0x000A2FFC
		public void Initialize(UISelectField select, Text text)
		{
			this.selectField = select;
			this.textComponent = text;
			this.OnEnable();
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x000A4D6F File Offset: 0x000A2F6F
		public new bool IsPressed()
		{
			return base.IsPressed();
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x000A4D77 File Offset: 0x000A2F77
		public bool IsHighlighted(BaseEventData eventData)
		{
			return base.IsHighlighted();
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000A4E12 File Offset: 0x000A3012
		public override void OnPointerUp(PointerEventData eventData)
		{
			base.OnPointerUp(eventData);
			if (this.onPointerUp != null)
			{
				this.onPointerUp.Invoke(eventData);
			}
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000A4E2F File Offset: 0x000A302F
		public override void OnPointerDown(PointerEventData eventData)
		{
			base.OnPointerDown(eventData);
			this.DoStateTransition(Selectable.SelectionState.Normal, false);
			if (this.onSelectOption != null && this.textComponent != null)
			{
				this.onSelectOption.Invoke(this.textComponent.text);
			}
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000A4E6C File Offset: 0x000A306C
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (!base.enabled || !base.enabled || !base.gameObject.activeInHierarchy || this.selectField == null)
			{
				return;
			}
			Color a = this.selectField.optionBackgroundTransColors.normalColor;
			Sprite newSprite = null;
			string trigger = this.selectField.optionBackgroundAnimationTriggers.normalTrigger;
			if (state == Selectable.SelectionState.Disabled)
			{
				a = this.selectField.optionBackgroundTransColors.disabledColor;
				newSprite = this.selectField.optionBackgroundSpriteStates.disabledSprite;
				trigger = this.selectField.optionBackgroundAnimationTriggers.disabledTrigger;
			}
			else
			{
				switch (state)
				{
				case Selectable.SelectionState.Normal:
					a = (base.isOn ? this.selectField.optionBackgroundTransColors.activeColor : this.selectField.optionBackgroundTransColors.normalColor);
					newSprite = (base.isOn ? this.selectField.optionBackgroundSpriteStates.activeSprite : null);
					trigger = (base.isOn ? this.selectField.optionBackgroundAnimationTriggers.activeTrigger : this.selectField.optionBackgroundAnimationTriggers.normalTrigger);
					break;
				case Selectable.SelectionState.Highlighted:
					a = (base.isOn ? this.selectField.optionBackgroundTransColors.activeHighlightedColor : this.selectField.optionBackgroundTransColors.highlightedColor);
					newSprite = (base.isOn ? this.selectField.optionBackgroundSpriteStates.activeHighlightedSprite : this.selectField.optionBackgroundSpriteStates.highlightedSprite);
					trigger = (base.isOn ? this.selectField.optionBackgroundAnimationTriggers.activeHighlightedTrigger : this.selectField.optionBackgroundAnimationTriggers.highlightedTrigger);
					break;
				case Selectable.SelectionState.Pressed:
					a = (base.isOn ? this.selectField.optionBackgroundTransColors.activePressedColor : this.selectField.optionBackgroundTransColors.pressedColor);
					newSprite = (base.isOn ? this.selectField.optionBackgroundSpriteStates.activePressedSprite : this.selectField.optionBackgroundSpriteStates.pressedSprite);
					trigger = (base.isOn ? this.selectField.optionBackgroundAnimationTriggers.activePressedTrigger : this.selectField.optionBackgroundAnimationTriggers.pressedTrigger);
					break;
				}
			}
			switch (this.selectField.optionBackgroundTransitionType)
			{
			case Selectable.Transition.ColorTint:
				this.StartColorTween(a * this.selectField.optionBackgroundTransColors.colorMultiplier, instant ? 0f : this.selectField.optionBackgroundTransColors.fadeDuration);
				break;
			case Selectable.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				break;
			case Selectable.Transition.Animation:
				this.TriggerAnimation(trigger);
				break;
			}
			this.DoTextStateTransition(state, instant);
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000A510C File Offset: 0x000A330C
		private void DoTextStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.selectField != null && this.textComponent != null && this.selectField.optionTextTransitionType == UISelectField.OptionTextTransitionType.CrossFade)
			{
				Color a = this.selectField.optionTextTransitionColors.normalColor;
				if (state == Selectable.SelectionState.Disabled)
				{
					a = this.selectField.optionTextTransitionColors.disabledColor;
				}
				else
				{
					switch (state)
					{
					case Selectable.SelectionState.Normal:
						a = (base.isOn ? this.selectField.optionTextTransitionColors.activeColor : this.selectField.optionTextTransitionColors.normalColor);
						break;
					case Selectable.SelectionState.Highlighted:
						a = (base.isOn ? this.selectField.optionTextTransitionColors.activeHighlightedColor : this.selectField.optionTextTransitionColors.highlightedColor);
						break;
					case Selectable.SelectionState.Pressed:
						a = (base.isOn ? this.selectField.optionTextTransitionColors.activePressedColor : this.selectField.optionTextTransitionColors.pressedColor);
						break;
					}
				}
				this.textComponent.CrossFadeColor(a * this.selectField.optionTextTransitionColors.colorMultiplier, instant ? 0f : this.selectField.optionTextTransitionColors.fadeDuration, true, true);
			}
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000A369D File Offset: 0x000A189D
		private void StartColorTween(Color color, float duration)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(color, duration, true, true);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000A5250 File Offset: 0x000A3450
		private void DoSpriteSwap(Sprite newSprite)
		{
			Image image = base.targetGraphic as Image;
			if (image == null)
			{
				return;
			}
			image.overrideSprite = newSprite;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000A527C File Offset: 0x000A347C
		private void TriggerAnimation(string trigger)
		{
			if (this.selectField == null || base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || !base.animator.hasBoundPlayables || string.IsNullOrEmpty(trigger))
			{
				return;
			}
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.normalTrigger);
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.pressedTrigger);
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.highlightedTrigger);
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.activeTrigger);
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.activeHighlightedTrigger);
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.activePressedTrigger);
			base.animator.ResetTrigger(this.selectField.optionBackgroundAnimationTriggers.disabledTrigger);
			base.animator.SetTrigger(trigger);
		}

		// Token: 0x04001AD2 RID: 6866
		public UISelectField selectField;

		// Token: 0x04001AD3 RID: 6867
		public Text textComponent;

		// Token: 0x04001AD4 RID: 6868
		public UISelectField_Option.SelectOptionEvent onSelectOption = new UISelectField_Option.SelectOptionEvent();

		// Token: 0x04001AD5 RID: 6869
		public UISelectField_Option.PointerUpEvent onPointerUp = new UISelectField_Option.PointerUpEvent();

		// Token: 0x020005D6 RID: 1494
		[Serializable]
		public class SelectOptionEvent : UnityEvent<string>
		{
		}

		// Token: 0x020005D7 RID: 1495
		[Serializable]
		public class PointerUpEvent : UnityEvent<BaseEventData>
		{
		}
	}
}
