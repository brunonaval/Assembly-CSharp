using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005CA RID: 1482
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Select Field", 58)]
	[RequireComponent(typeof(Image))]
	public class UISelectField : Toggle
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x000A2DC3 File Offset: 0x000A0FC3
		// (set) Token: 0x06002095 RID: 8341 RVA: 0x000A2DCB File Offset: 0x000A0FCB
		public UISelectField.Direction direction
		{
			get
			{
				return this.m_Direction;
			}
			set
			{
				this.m_Direction = value;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06002096 RID: 8342 RVA: 0x000A2DD4 File Offset: 0x000A0FD4
		public List<string> options
		{
			get
			{
				return this.m_Options;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06002097 RID: 8343 RVA: 0x000A2DDC File Offset: 0x000A0FDC
		// (set) Token: 0x06002098 RID: 8344 RVA: 0x000A2DE4 File Offset: 0x000A0FE4
		public string value
		{
			get
			{
				return this.m_SelectedItem;
			}
			set
			{
				this.SelectOption(value);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06002099 RID: 8345 RVA: 0x000A2DED File Offset: 0x000A0FED
		public int selectedOptionIndex
		{
			get
			{
				return this.GetOptionIndex(this.m_SelectedItem);
			}
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000A2DFC File Offset: 0x000A0FFC
		protected UISelectField()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x000A2FE8 File Offset: 0x000A11E8
		protected override void Awake()
		{
			base.Awake();
			if (base.targetGraphic == null)
			{
				base.targetGraphic = base.GetComponent<Image>();
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x000A300A File Offset: 0x000A120A
		protected override void Start()
		{
			base.Start();
			this.toggleTransition = Toggle.ToggleTransition.None;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000A3019 File Offset: 0x000A1219
		protected override void OnEnable()
		{
			base.OnEnable();
			this.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleValueChanged));
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000A3038 File Offset: 0x000A1238
		protected override void OnDisable()
		{
			base.OnDisable();
			this.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnToggleValueChanged));
			base.isOn = false;
			this.DoStateTransition(Selectable.SelectionState.Disabled, true);
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000A3066 File Offset: 0x000A1266
		public void Open()
		{
			base.isOn = true;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000A306F File Offset: 0x000A126F
		public void Close()
		{
			base.isOn = false;
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x060020A1 RID: 8353 RVA: 0x000A3078 File Offset: 0x000A1278
		public bool IsOpen
		{
			get
			{
				return base.isOn;
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000A3080 File Offset: 0x000A1280
		public int GetOptionIndex(string optionValue)
		{
			if (this.m_Options != null && this.m_Options.Count > 0 && !string.IsNullOrEmpty(optionValue))
			{
				for (int i = 0; i < this.m_Options.Count; i++)
				{
					if (optionValue.Equals(this.m_Options[i], StringComparison.OrdinalIgnoreCase))
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000A30DC File Offset: 0x000A12DC
		public void SelectOptionByIndex(int index)
		{
			if (this.IsOpen)
			{
				UISelectField_Option uiselectField_Option = this.m_OptionObjects[index];
				if (uiselectField_Option != null)
				{
					uiselectField_Option.isOn = true;
					return;
				}
			}
			else
			{
				this.m_SelectedItem = this.m_Options[index];
				this.TriggerChangeEvent();
			}
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000A3128 File Offset: 0x000A1328
		public void SelectOption(string optionValue)
		{
			if (string.IsNullOrEmpty(optionValue))
			{
				return;
			}
			int optionIndex = this.GetOptionIndex(optionValue);
			if (optionIndex < 0 || optionIndex >= this.m_Options.Count)
			{
				return;
			}
			this.SelectOptionByIndex(optionIndex);
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000A3160 File Offset: 0x000A1360
		public void AddOption(string optionValue)
		{
			if (this.m_Options != null)
			{
				this.m_Options.Add(optionValue);
				this.OptionListChanged();
			}
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000A317C File Offset: 0x000A137C
		public void AddOptionAtIndex(string optionValue, int index)
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (index >= this.m_Options.Count)
			{
				this.m_Options.Add(optionValue);
			}
			else
			{
				this.m_Options.Insert(index, optionValue);
			}
			this.OptionListChanged();
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000A31B6 File Offset: 0x000A13B6
		public void RemoveOption(string optionValue)
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (this.m_Options.Contains(optionValue))
			{
				this.m_Options.Remove(optionValue);
				this.OptionListChanged();
				this.ValidateSelectedOption();
			}
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000A31E8 File Offset: 0x000A13E8
		public void RemoveOptionAtIndex(int index)
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (index >= 0 && index < this.m_Options.Count)
			{
				this.m_Options.RemoveAt(index);
				this.OptionListChanged();
				this.ValidateSelectedOption();
			}
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000A321D File Offset: 0x000A141D
		public void ClearOptions()
		{
			if (this.m_Options == null)
			{
				return;
			}
			this.m_Options.Clear();
			this.OptionListChanged();
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000A3239 File Offset: 0x000A1439
		public void ValidateSelectedOption()
		{
			if (this.m_Options == null)
			{
				return;
			}
			if (!this.m_Options.Contains(this.m_SelectedItem))
			{
				this.SelectOptionByIndex(0);
			}
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000A3260 File Offset: 0x000A1460
		public void OnOptionSelect(string option)
		{
			if (string.IsNullOrEmpty(option))
			{
				return;
			}
			string selectedItem = this.m_SelectedItem;
			this.m_SelectedItem = option;
			if (!selectedItem.Equals(this.m_SelectedItem))
			{
				this.TriggerChangeEvent();
			}
			if (this.IsOpen && this.m_PointerWasUsedOnOption)
			{
				this.m_PointerWasUsedOnOption = false;
				this.Close();
				base.OnDeselect(new BaseEventData(EventSystem.current));
			}
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000A32C3 File Offset: 0x000A14C3
		public void OnOptionPointerUp(BaseEventData eventData)
		{
			this.m_PointerWasUsedOnOption = true;
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000A32CC File Offset: 0x000A14CC
		protected virtual void TriggerChangeEvent()
		{
			if (this.m_LabelText != null)
			{
				this.m_LabelText.text = this.m_SelectedItem;
			}
			if (this.onChange != null)
			{
				this.onChange.Invoke(this.selectedOptionIndex, this.m_SelectedItem);
			}
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000A330C File Offset: 0x000A150C
		private void OnToggleValueChanged(bool state)
		{
			if (!Application.isPlaying)
			{
				return;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			this.ToggleList(base.isOn);
			if (!base.isOn && this.m_Blocker != null)
			{
				UnityEngine.Object.Destroy(this.m_Blocker);
			}
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000A335C File Offset: 0x000A155C
		public override void OnDeselect(BaseEventData eventData)
		{
			if (this.m_ListObject != null && this.m_ListObject.GetComponent<UISelectField_List>().IsHighlighted(eventData))
			{
				return;
			}
			using (List<UISelectField_Option>.Enumerator enumerator = this.m_OptionObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsHighlighted(eventData))
					{
						return;
					}
				}
			}
			this.Close();
			base.OnDeselect(eventData);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000A33E0 File Offset: 0x000A15E0
		public override void OnMove(AxisEventData eventData)
		{
			if (this.IsOpen)
			{
				int num = this.selectedOptionIndex - 1;
				int num2 = this.selectedOptionIndex + 1;
				MoveDirection moveDir = eventData.moveDir;
				if (moveDir != MoveDirection.Up)
				{
					if (moveDir == MoveDirection.Down)
					{
						if (num2 < this.m_Options.Count)
						{
							this.SelectOptionByIndex(num2);
						}
					}
				}
				else if (num >= 0)
				{
					this.SelectOptionByIndex(num);
				}
				eventData.Use();
				return;
			}
			base.OnMove(eventData);
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000A3448 File Offset: 0x000A1648
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			Color a = this.colors.normalColor;
			Sprite newSprite = null;
			string trigger = this.animationTriggers.normalTrigger;
			if (state == Selectable.SelectionState.Disabled)
			{
				this.m_CurrentVisualState = UISelectField.VisualState.Disabled;
				a = this.colors.disabledColor;
				newSprite = this.spriteState.disabledSprite;
				trigger = this.animationTriggers.disabledTrigger;
			}
			else
			{
				switch (state)
				{
				case Selectable.SelectionState.Normal:
					this.m_CurrentVisualState = (base.isOn ? UISelectField.VisualState.Active : UISelectField.VisualState.Normal);
					a = (base.isOn ? this.colors.activeColor : this.colors.normalColor);
					newSprite = (base.isOn ? this.spriteState.activeSprite : null);
					trigger = (base.isOn ? this.animationTriggers.activeTrigger : this.animationTriggers.normalTrigger);
					break;
				case Selectable.SelectionState.Highlighted:
				case Selectable.SelectionState.Selected:
					this.m_CurrentVisualState = (base.isOn ? UISelectField.VisualState.ActiveHighlighted : UISelectField.VisualState.Highlighted);
					a = (base.isOn ? this.colors.activeHighlightedColor : this.colors.highlightedColor);
					newSprite = (base.isOn ? this.spriteState.activeHighlightedSprite : this.spriteState.highlightedSprite);
					trigger = (base.isOn ? this.animationTriggers.activeHighlightedTrigger : this.animationTriggers.highlightedTrigger);
					break;
				case Selectable.SelectionState.Pressed:
					this.m_CurrentVisualState = (base.isOn ? UISelectField.VisualState.ActivePressed : UISelectField.VisualState.Pressed);
					a = (base.isOn ? this.colors.activePressedColor : this.colors.pressedColor);
					newSprite = (base.isOn ? this.spriteState.activePressedSprite : this.spriteState.pressedSprite);
					trigger = (base.isOn ? this.animationTriggers.activePressedTrigger : this.animationTriggers.pressedTrigger);
					break;
				}
			}
			switch (base.transition)
			{
			case Selectable.Transition.ColorTint:
				this.StartColorTween(a * this.colors.colorMultiplier, instant ? 0f : this.colors.fadeDuration);
				break;
			case Selectable.Transition.SpriteSwap:
				this.DoSpriteSwap(newSprite);
				break;
			case Selectable.Transition.Animation:
				this.TriggerAnimation(trigger);
				break;
			}
			if (this.onTransition != null)
			{
				this.onTransition.Invoke(this.m_CurrentVisualState, instant);
			}
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000A369D File Offset: 0x000A189D
		private void StartColorTween(Color color, float duration)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(color, duration, true, true);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000A36C0 File Offset: 0x000A18C0
		private void DoSpriteSwap(Sprite newSprite)
		{
			Image image = base.targetGraphic as Image;
			if (image == null)
			{
				return;
			}
			image.overrideSprite = newSprite;
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000A36EC File Offset: 0x000A18EC
		private void TriggerAnimation(string trigger)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || !base.animator.hasBoundPlayables || string.IsNullOrEmpty(trigger))
			{
				return;
			}
			base.animator.ResetTrigger(this.animationTriggers.normalTrigger);
			base.animator.ResetTrigger(this.animationTriggers.pressedTrigger);
			base.animator.ResetTrigger(this.animationTriggers.highlightedTrigger);
			base.animator.ResetTrigger(this.animationTriggers.activeTrigger);
			base.animator.ResetTrigger(this.animationTriggers.activeHighlightedTrigger);
			base.animator.ResetTrigger(this.animationTriggers.activePressedTrigger);
			base.animator.ResetTrigger(this.animationTriggers.disabledTrigger);
			base.animator.SetTrigger(trigger);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000A37F0 File Offset: 0x000A19F0
		protected virtual void ToggleList(bool state)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.m_ListObject == null)
			{
				this.CreateList();
			}
			if (this.m_ListObject == null)
			{
				return;
			}
			if (this.m_ListCanvasGroup != null)
			{
				this.m_ListCanvasGroup.blocksRaycasts = state;
			}
			if (state)
			{
				this.m_LastNavigationMode = base.navigation.mode;
				this.m_LastSelectedGameObject = EventSystem.current.currentSelectedGameObject;
				Navigation navigation = base.navigation;
				navigation.mode = Navigation.Mode.Vertical;
				base.navigation = navigation;
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
			else
			{
				Navigation navigation2 = base.navigation;
				navigation2.mode = this.m_LastNavigationMode;
				base.navigation = navigation2;
				if (!EventSystem.current.alreadySelecting && this.m_LastSelectedGameObject != null)
				{
					EventSystem.current.SetSelectedGameObject(this.m_LastSelectedGameObject);
				}
			}
			if (state)
			{
				UIUtility.BringToFront(this.m_ListObject);
			}
			if (this.listAnimationType == UISelectField.ListAnimationType.None || this.listAnimationType == UISelectField.ListAnimationType.Fade)
			{
				float targetAlpha = state ? 1f : 0f;
				this.TweenListAlpha(targetAlpha, (this.listAnimationType == UISelectField.ListAnimationType.Fade) ? this.listAnimationDuration : 0f, true);
				return;
			}
			if (this.listAnimationType == UISelectField.ListAnimationType.Animation)
			{
				this.TriggerListAnimation(state ? this.listAnimationOpenTrigger : this.listAnimationCloseTrigger);
			}
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000A3948 File Offset: 0x000A1B48
		protected void CreateList()
		{
			this.m_LastListSize = Vector2.zero;
			this.m_OptionObjects.Clear();
			this.m_ListObject = new GameObject("UISelectField - List", new Type[]
			{
				typeof(RectTransform)
			});
			this.m_ListObject.layer = base.gameObject.layer;
			this.m_ListObject.transform.SetParent(base.transform, false);
			UISelectField_List uiselectField_List = this.m_ListObject.AddComponent<UISelectField_List>();
			this.m_ListObject.AddComponent<UIAlwaysOnTop>().order = 99998;
			this.m_ListCanvasGroup = this.m_ListObject.AddComponent<CanvasGroup>();
			RectTransform rectTransform = this.m_ListObject.transform as RectTransform;
			rectTransform.localScale = new Vector3(1f, 1f, 1f);
			rectTransform.localPosition = Vector3.zero;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.zero;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = new Vector3((float)this.listMargins.left, (float)this.listMargins.top * -1f, 0f);
			float num = (base.transform as RectTransform).sizeDelta.x;
			if (this.listMargins.left > 0)
			{
				num -= (float)this.listMargins.left;
			}
			else
			{
				num += (float)Math.Abs(this.listMargins.left);
			}
			if (this.listMargins.right > 0)
			{
				num -= (float)this.listMargins.right;
			}
			else
			{
				num += (float)Math.Abs(this.listMargins.right);
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			uiselectField_List.onDimensionsChange.AddListener(new UnityAction(this.ListDimensionsChanged));
			Image image = this.m_ListObject.AddComponent<Image>();
			if (this.listBackgroundSprite != null)
			{
				image.sprite = this.listBackgroundSprite;
			}
			image.type = this.listBackgroundSpriteType;
			image.color = this.listBackgroundColor;
			if (this.allowScrollRect && this.m_Options.Count >= this.scrollMinOptions)
			{
				rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.scrollListHeight);
				GameObject gameObject = new GameObject("Scroll Rect", new Type[]
				{
					typeof(RectTransform)
				});
				gameObject.layer = this.m_ListObject.layer;
				gameObject.transform.SetParent(this.m_ListObject.transform, false);
				RectTransform rectTransform2 = gameObject.transform as RectTransform;
				rectTransform2.localScale = new Vector3(1f, 1f, 1f);
				rectTransform2.localPosition = Vector3.zero;
				rectTransform2.anchorMin = Vector2.zero;
				rectTransform2.anchorMax = Vector2.one;
				rectTransform2.pivot = new Vector2(0f, 1f);
				rectTransform2.anchoredPosition = Vector2.zero;
				rectTransform2.offsetMin = new Vector2((float)this.listPadding.left, (float)this.listPadding.bottom);
				rectTransform2.offsetMax = new Vector2((float)this.listPadding.right * -1f, (float)this.listPadding.top * -1f);
				this.m_ScrollRect = gameObject.AddComponent<ScrollRect>();
				this.m_ScrollRect.horizontal = false;
				this.m_ScrollRect.movementType = this.scrollMovementType;
				this.m_ScrollRect.elasticity = this.scrollElasticity;
				this.m_ScrollRect.inertia = this.scrollInertia;
				this.m_ScrollRect.decelerationRate = this.scrollDecelerationRate;
				this.m_ScrollRect.scrollSensitivity = this.scrollSensitivity;
				GameObject gameObject2 = new GameObject("View Port", new Type[]
				{
					typeof(RectTransform)
				});
				gameObject2.layer = this.m_ListObject.layer;
				gameObject2.transform.SetParent(gameObject.transform, false);
				RectTransform rectTransform3 = gameObject2.transform as RectTransform;
				rectTransform3.localScale = new Vector3(1f, 1f, 1f);
				rectTransform3.localPosition = Vector3.zero;
				rectTransform3.anchorMin = Vector2.zero;
				rectTransform3.anchorMax = Vector2.one;
				rectTransform3.pivot = new Vector2(0f, 1f);
				rectTransform3.anchoredPosition = Vector2.zero;
				rectTransform3.offsetMin = Vector2.zero;
				rectTransform3.offsetMax = Vector2.zero;
				gameObject2.AddComponent<Image>().raycastTarget = false;
				gameObject2.AddComponent<Mask>().showMaskGraphic = false;
				this.m_ListContentObject = new GameObject("Content", new Type[]
				{
					typeof(RectTransform)
				});
				this.m_ListContentObject.layer = this.m_ListObject.layer;
				this.m_ListContentObject.transform.SetParent(rectTransform3, false);
				RectTransform rectTransform4 = this.m_ListContentObject.transform as RectTransform;
				rectTransform4.localScale = new Vector3(1f, 1f, 1f);
				rectTransform4.localPosition = Vector3.zero;
				rectTransform4.anchorMin = new Vector2(0f, 1f);
				rectTransform4.anchorMax = new Vector2(0f, 1f);
				rectTransform4.pivot = new Vector2(0f, 1f);
				rectTransform4.anchoredPosition = Vector2.zero;
				rectTransform4.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.sizeDelta.x);
				this.m_ListContentObject.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
				this.m_ListContentObject.AddComponent<UISelectField_List>().onDimensionsChange.AddListener(new UnityAction(this.ScrollContentDimensionsChanged));
				this.m_ScrollRect.content = rectTransform4;
				this.m_ScrollRect.viewport = rectTransform3;
				if (this.scrollBarPrefab != null)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.scrollBarPrefab, gameObject.transform);
					this.m_ScrollRect.verticalScrollbar = gameObject3.GetComponent<Scrollbar>();
					this.m_ScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;
					this.m_ScrollRect.verticalScrollbarSpacing = this.scrollbarSpacing;
				}
				this.m_ListContentObject.AddComponent<VerticalLayoutGroup>();
			}
			else
			{
				this.m_ListContentObject = this.m_ListObject;
				VerticalLayoutGroup verticalLayoutGroup = this.m_ListContentObject.AddComponent<VerticalLayoutGroup>();
				verticalLayoutGroup.padding = this.listPadding;
				verticalLayoutGroup.spacing = this.listSpacing;
			}
			this.m_ListContentObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			ToggleGroup toggleGroup = this.m_ListObject.AddComponent<ToggleGroup>();
			for (int i = 0; i < this.m_Options.Count; i++)
			{
				if (i == 0 && this.startSeparator)
				{
					this.m_StartSeparatorObject = this.CreateSeparator(i - 1);
				}
				this.CreateOption(i, toggleGroup);
				if (i < this.m_Options.Count - 1)
				{
					this.CreateSeparator(i);
				}
			}
			if (this.listAnimationType == UISelectField.ListAnimationType.None || this.listAnimationType == UISelectField.ListAnimationType.Fade)
			{
				this.m_ListCanvasGroup.alpha = 0f;
			}
			else if (this.listAnimationType == UISelectField.ListAnimationType.Animation)
			{
				this.m_ListObject.AddComponent<Animator>().runtimeAnimatorController = this.listAnimatorController;
				uiselectField_List.SetTriggers(this.listAnimationOpenTrigger, this.listAnimationCloseTrigger);
				uiselectField_List.onAnimationFinish.AddListener(new UnityAction<UISelectField_List.State>(this.OnListAnimationFinish));
			}
			if (base.navigation.mode == Navigation.Mode.None)
			{
				this.CreateBlocker();
			}
			if (this.allowScrollRect && this.m_Options.Count >= this.scrollMinOptions)
			{
				this.ListDimensionsChanged();
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000A40CC File Offset: 0x000A22CC
		protected virtual void CreateBlocker()
		{
			GameObject gameObject = new GameObject("Blocker");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.SetParent(base.transform, false);
			rectTransform.localScale = Vector3.one;
			rectTransform.localPosition = Vector3.zero;
			rectTransform.anchorMin = Vector3.zero;
			rectTransform.anchorMax = Vector3.one;
			rectTransform.sizeDelta = Vector2.zero;
			gameObject.AddComponent<Image>().color = Color.clear;
			gameObject.AddComponent<Button>().onClick.AddListener(new UnityAction(this.Close));
			gameObject.AddComponent<UIAlwaysOnTop>().order = 99997;
			UIUtility.BringToFront(gameObject);
			this.m_Blocker = gameObject;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000A4180 File Offset: 0x000A2380
		protected void CreateOption(int index, ToggleGroup toggleGroup)
		{
			if (this.m_ListContentObject == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Option " + index.ToString(), new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(this.m_ListContentObject.transform, false);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = Vector3.zero;
			UISelectField_Option uiselectField_Option = gameObject.AddComponent<UISelectField_Option>();
			if (this.optionBackgroundSprite != null)
			{
				Image image = gameObject.AddComponent<Image>();
				image.sprite = this.optionBackgroundSprite;
				image.type = this.optionBackgroundSpriteType;
				image.color = this.optionBackgroundSpriteColor;
				uiselectField_Option.targetGraphic = image;
			}
			if (this.optionBackgroundTransitionType == Selectable.Transition.Animation)
			{
				gameObject.AddComponent<Animator>().runtimeAnimatorController = this.optionBackgroundAnimatorController;
			}
			gameObject.AddComponent<VerticalLayoutGroup>().padding = this.optionPadding;
			GameObject gameObject2 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			gameObject2.transform.localScale = Vector3.one;
			gameObject2.transform.localPosition = Vector3.zero;
			(gameObject2.transform as RectTransform).pivot = new Vector2(0f, 1f);
			Text text = gameObject2.AddComponent<Text>();
			text.font = this.optionFont;
			text.fontSize = this.optionFontSize;
			text.fontStyle = this.optionFontStyle;
			text.color = this.optionColor;
			if (this.m_Options != null)
			{
				text.text = this.m_Options[index];
			}
			if (this.optionTextTransitionType == UISelectField.OptionTextTransitionType.CrossFade)
			{
				text.canvasRenderer.SetColor(this.optionTextTransitionColors.normalColor);
			}
			if (this.optionTextEffectType != UISelectField.OptionTextEffectType.None)
			{
				if (this.optionTextEffectType == UISelectField.OptionTextEffectType.Shadow)
				{
					Shadow shadow = gameObject2.AddComponent<Shadow>();
					shadow.effectColor = this.optionTextEffectColor;
					shadow.effectDistance = this.optionTextEffectDistance;
					shadow.useGraphicAlpha = this.optionTextEffectUseGraphicAlpha;
				}
				else if (this.optionTextEffectType == UISelectField.OptionTextEffectType.Outline)
				{
					Outline outline = gameObject2.AddComponent<Outline>();
					outline.effectColor = this.optionTextEffectColor;
					outline.effectDistance = this.optionTextEffectDistance;
					outline.useGraphicAlpha = this.optionTextEffectUseGraphicAlpha;
				}
			}
			if (this.optionHoverOverlay != null)
			{
				GameObject gameObject3 = new GameObject("Hover Overlay", new Type[]
				{
					typeof(RectTransform)
				});
				gameObject3.layer = base.gameObject.layer;
				gameObject3.transform.localScale = Vector3.one;
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.AddComponent<LayoutElement>().ignoreLayout = true;
				gameObject3.transform.SetParent(gameObject.transform, false);
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				Image image2 = gameObject3.AddComponent<Image>();
				image2.sprite = this.optionHoverOverlay;
				image2.color = this.optionHoverOverlayColor;
				image2.type = Image.Type.Sliced;
				(gameObject3.transform as RectTransform).pivot = new Vector2(0f, 1f);
				(gameObject3.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
				(gameObject3.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
				(gameObject3.transform as RectTransform).offsetMin = new Vector2(0f, 0f);
				(gameObject3.transform as RectTransform).offsetMax = new Vector2(0f, 0f);
				UISelectField_OptionOverlay uiselectField_OptionOverlay = gameObject.AddComponent<UISelectField_OptionOverlay>();
				uiselectField_OptionOverlay.targetGraphic = image2;
				uiselectField_OptionOverlay.transition = UISelectField_OptionOverlay.Transition.ColorTint;
				uiselectField_OptionOverlay.colorBlock = this.optionHoverOverlayColorBlock;
				uiselectField_OptionOverlay.InternalEvaluateAndTransitionToNormalState(true);
			}
			if (this.optionPressOverlay != null)
			{
				GameObject gameObject4 = new GameObject("Press Overlay", new Type[]
				{
					typeof(RectTransform)
				});
				gameObject4.layer = base.gameObject.layer;
				gameObject4.transform.localScale = Vector3.one;
				gameObject4.transform.localPosition = Vector3.zero;
				gameObject4.AddComponent<LayoutElement>().ignoreLayout = true;
				gameObject4.transform.SetParent(gameObject.transform, false);
				gameObject4.transform.localScale = new Vector3(1f, 1f, 1f);
				Image image3 = gameObject4.AddComponent<Image>();
				image3.sprite = this.optionPressOverlay;
				image3.color = this.optionPressOverlayColor;
				image3.type = Image.Type.Sliced;
				(gameObject4.transform as RectTransform).pivot = new Vector2(0f, 1f);
				(gameObject4.transform as RectTransform).anchorMin = new Vector2(0f, 0f);
				(gameObject4.transform as RectTransform).anchorMax = new Vector2(1f, 1f);
				(gameObject4.transform as RectTransform).offsetMin = new Vector2(0f, 0f);
				(gameObject4.transform as RectTransform).offsetMax = new Vector2(0f, 0f);
				UISelectField_OptionOverlay uiselectField_OptionOverlay2 = gameObject.AddComponent<UISelectField_OptionOverlay>();
				uiselectField_OptionOverlay2.targetGraphic = image3;
				uiselectField_OptionOverlay2.transition = UISelectField_OptionOverlay.Transition.ColorTint;
				uiselectField_OptionOverlay2.colorBlock = this.optionPressOverlayColorBlock;
				uiselectField_OptionOverlay2.InternalEvaluateAndTransitionToNormalState(true);
			}
			uiselectField_Option.Initialize(this, text);
			if (index == this.selectedOptionIndex)
			{
				uiselectField_Option.isOn = true;
			}
			if (toggleGroup != null)
			{
				uiselectField_Option.group = toggleGroup;
			}
			uiselectField_Option.onSelectOption.AddListener(new UnityAction<string>(this.OnOptionSelect));
			uiselectField_Option.onPointerUp.AddListener(new UnityAction<BaseEventData>(this.OnOptionPointerUp));
			if (this.m_OptionObjects != null)
			{
				this.m_OptionObjects.Add(uiselectField_Option);
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000A475C File Offset: 0x000A295C
		protected GameObject CreateSeparator(int index)
		{
			if (this.m_ListContentObject == null || this.listSeparatorSprite == null)
			{
				return null;
			}
			GameObject gameObject = new GameObject("Separator " + index.ToString(), new Type[]
			{
				typeof(RectTransform)
			});
			gameObject.transform.SetParent(this.m_ListContentObject.transform, false);
			gameObject.transform.localScale = Vector3.one;
			gameObject.transform.localPosition = Vector3.zero;
			Image image = gameObject.AddComponent<Image>();
			image.sprite = this.listSeparatorSprite;
			image.type = this.listSeparatorType;
			image.color = this.listSeparatorColor;
			gameObject.AddComponent<LayoutElement>().preferredHeight = ((this.listSeparatorHeight > 0f) ? this.listSeparatorHeight : this.listSeparatorSprite.rect.height);
			return gameObject;
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000A4843 File Offset: 0x000A2A43
		protected virtual void ListCleanup()
		{
			if (this.m_ListObject != null)
			{
				UnityEngine.Object.Destroy(this.m_ListObject);
			}
			this.m_OptionObjects.Clear();
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000A486C File Offset: 0x000A2A6C
		public virtual void PositionListForDirection(UISelectField.Direction direction)
		{
			if (this.m_ListObject == null)
			{
				return;
			}
			RectTransform rectTransform = this.m_ListObject.transform as RectTransform;
			if (direction == UISelectField.Direction.Auto)
			{
				Vector3[] array = new Vector3[4];
				rectTransform.GetWorldCorners(array);
				if (RectTransformUtility.WorldToScreenPoint(Camera.main, array[0]).y < 0f)
				{
					direction = UISelectField.Direction.Up;
				}
				else
				{
					direction = UISelectField.Direction.Down;
				}
			}
			if (direction == UISelectField.Direction.Down)
			{
				rectTransform.SetParent(base.transform, true);
				rectTransform.pivot = new Vector2(0f, 1f);
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.anchorMax = new Vector2(0f, 0f);
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (float)this.listMargins.top * -1f);
				UIUtility.BringToFront(rectTransform.gameObject);
				return;
			}
			rectTransform.SetParent(base.transform, true);
			rectTransform.pivot = new Vector2(0f, 0f);
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (float)this.listMargins.bottom);
			if (this.m_StartSeparatorObject != null)
			{
				this.m_StartSeparatorObject.transform.SetAsLastSibling();
			}
			UIUtility.BringToFront(rectTransform.gameObject);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000A49F0 File Offset: 0x000A2BF0
		protected virtual void ListDimensionsChanged()
		{
			if (!this.IsActive() || this.m_ListObject == null)
			{
				return;
			}
			if (this.m_LastListSize.Equals((this.m_ListObject.transform as RectTransform).sizeDelta))
			{
				return;
			}
			this.m_LastListSize = (this.m_ListObject.transform as RectTransform).sizeDelta;
			this.PositionListForDirection(this.m_Direction);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000A4A60 File Offset: 0x000A2C60
		protected virtual void ScrollContentDimensionsChanged()
		{
			if (!this.IsActive() || this.m_ScrollRect == null)
			{
				return;
			}
			float y = this.m_ScrollRect.content.sizeDelta.y / (float)this.m_Options.Count * (float)this.selectedOptionIndex;
			this.m_ScrollRect.content.anchoredPosition = new Vector2(this.m_ScrollRect.content.anchoredPosition.x, y);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x0000219A File Offset: 0x0000039A
		protected virtual void OptionListChanged()
		{
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000A4ADC File Offset: 0x000A2CDC
		private void TweenListAlpha(float targetAlpha, float duration, bool ignoreTimeScale)
		{
			if (this.m_ListCanvasGroup == null)
			{
				return;
			}
			float alpha = this.m_ListCanvasGroup.alpha;
			if (alpha.Equals(targetAlpha))
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = alpha,
				targetFloat = targetAlpha
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetListAlpha));
			info.AddOnFinishCallback(new UnityAction(this.OnListTweenFinished));
			info.ignoreTimeScale = ignoreTimeScale;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000A4B6C File Offset: 0x000A2D6C
		private void SetListAlpha(float alpha)
		{
			if (this.m_ListCanvasGroup == null)
			{
				return;
			}
			this.m_ListCanvasGroup.alpha = alpha;
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000A4B8C File Offset: 0x000A2D8C
		private void TriggerListAnimation(string trigger)
		{
			if (this.m_ListObject == null || string.IsNullOrEmpty(trigger))
			{
				return;
			}
			Animator component = this.m_ListObject.GetComponent<Animator>();
			if (component == null || !component.enabled || !component.isActiveAndEnabled || component.runtimeAnimatorController == null || !component.hasBoundPlayables)
			{
				return;
			}
			component.ResetTrigger(this.listAnimationOpenTrigger);
			component.ResetTrigger(this.listAnimationCloseTrigger);
			component.SetTrigger(trigger);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000A4C0B File Offset: 0x000A2E0B
		protected virtual void OnListTweenFinished()
		{
			if (!this.IsOpen)
			{
				this.ListCleanup();
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000A4C1B File Offset: 0x000A2E1B
		protected virtual void OnListAnimationFinish(UISelectField_List.State state)
		{
			if (state == UISelectField_List.State.Closed && !this.IsOpen)
			{
				this.ListCleanup();
			}
		}

		// Token: 0x04001A6A RID: 6762
		[HideInInspector]
		[SerializeField]
		private string m_SelectedItem;

		// Token: 0x04001A6B RID: 6763
		private List<UISelectField_Option> m_OptionObjects = new List<UISelectField_Option>();

		// Token: 0x04001A6C RID: 6764
		private UISelectField.VisualState m_CurrentVisualState;

		// Token: 0x04001A6D RID: 6765
		private bool m_PointerWasUsedOnOption;

		// Token: 0x04001A6E RID: 6766
		private GameObject m_ListObject;

		// Token: 0x04001A6F RID: 6767
		private ScrollRect m_ScrollRect;

		// Token: 0x04001A70 RID: 6768
		private GameObject m_ListContentObject;

		// Token: 0x04001A71 RID: 6769
		private CanvasGroup m_ListCanvasGroup;

		// Token: 0x04001A72 RID: 6770
		private Vector2 m_LastListSize = Vector2.zero;

		// Token: 0x04001A73 RID: 6771
		private GameObject m_StartSeparatorObject;

		// Token: 0x04001A74 RID: 6772
		private Navigation.Mode m_LastNavigationMode;

		// Token: 0x04001A75 RID: 6773
		private GameObject m_LastSelectedGameObject;

		// Token: 0x04001A76 RID: 6774
		private GameObject m_Blocker;

		// Token: 0x04001A77 RID: 6775
		[SerializeField]
		private UISelectField.Direction m_Direction;

		// Token: 0x04001A78 RID: 6776
		[SerializeField]
		[FormerlySerializedAs("options")]
		private List<string> m_Options = new List<string>();

		// Token: 0x04001A79 RID: 6777
		[SerializeField]
		private Text m_LabelText;

		// Token: 0x04001A7A RID: 6778
		public new ColorBlockExtended colors = ColorBlockExtended.defaultColorBlock;

		// Token: 0x04001A7B RID: 6779
		public new SpriteStateExtended spriteState;

		// Token: 0x04001A7C RID: 6780
		public new AnimationTriggersExtended animationTriggers = new AnimationTriggersExtended();

		// Token: 0x04001A7D RID: 6781
		public Sprite listBackgroundSprite;

		// Token: 0x04001A7E RID: 6782
		public Image.Type listBackgroundSpriteType = Image.Type.Sliced;

		// Token: 0x04001A7F RID: 6783
		public Color listBackgroundColor = Color.white;

		// Token: 0x04001A80 RID: 6784
		public RectOffset listMargins;

		// Token: 0x04001A81 RID: 6785
		public RectOffset listPadding;

		// Token: 0x04001A82 RID: 6786
		public float listSpacing;

		// Token: 0x04001A83 RID: 6787
		public UISelectField.ListAnimationType listAnimationType = UISelectField.ListAnimationType.Fade;

		// Token: 0x04001A84 RID: 6788
		public float listAnimationDuration = 0.1f;

		// Token: 0x04001A85 RID: 6789
		public RuntimeAnimatorController listAnimatorController;

		// Token: 0x04001A86 RID: 6790
		public string listAnimationOpenTrigger = "Open";

		// Token: 0x04001A87 RID: 6791
		public string listAnimationCloseTrigger = "Close";

		// Token: 0x04001A88 RID: 6792
		public bool allowScrollRect = true;

		// Token: 0x04001A89 RID: 6793
		public ScrollRect.MovementType scrollMovementType = ScrollRect.MovementType.Clamped;

		// Token: 0x04001A8A RID: 6794
		public float scrollElasticity = 0.1f;

		// Token: 0x04001A8B RID: 6795
		public bool scrollInertia;

		// Token: 0x04001A8C RID: 6796
		public float scrollDecelerationRate = 0.135f;

		// Token: 0x04001A8D RID: 6797
		public float scrollSensitivity = 1f;

		// Token: 0x04001A8E RID: 6798
		public int scrollMinOptions = 5;

		// Token: 0x04001A8F RID: 6799
		public float scrollListHeight = 512f;

		// Token: 0x04001A90 RID: 6800
		public GameObject scrollBarPrefab;

		// Token: 0x04001A91 RID: 6801
		public float scrollbarSpacing = 34f;

		// Token: 0x04001A92 RID: 6802
		public Font optionFont = FontData.defaultFontData.font;

		// Token: 0x04001A93 RID: 6803
		public int optionFontSize = FontData.defaultFontData.fontSize;

		// Token: 0x04001A94 RID: 6804
		public FontStyle optionFontStyle = FontData.defaultFontData.fontStyle;

		// Token: 0x04001A95 RID: 6805
		public Color optionColor = Color.white;

		// Token: 0x04001A96 RID: 6806
		public UISelectField.OptionTextTransitionType optionTextTransitionType = UISelectField.OptionTextTransitionType.CrossFade;

		// Token: 0x04001A97 RID: 6807
		public ColorBlockExtended optionTextTransitionColors = ColorBlockExtended.defaultColorBlock;

		// Token: 0x04001A98 RID: 6808
		public RectOffset optionPadding;

		// Token: 0x04001A99 RID: 6809
		public UISelectField.OptionTextEffectType optionTextEffectType;

		// Token: 0x04001A9A RID: 6810
		public Color optionTextEffectColor = new Color(0f, 0f, 0f, 128f);

		// Token: 0x04001A9B RID: 6811
		public Vector2 optionTextEffectDistance = new Vector2(1f, -1f);

		// Token: 0x04001A9C RID: 6812
		public bool optionTextEffectUseGraphicAlpha = true;

		// Token: 0x04001A9D RID: 6813
		public Sprite optionBackgroundSprite;

		// Token: 0x04001A9E RID: 6814
		public Color optionBackgroundSpriteColor = Color.white;

		// Token: 0x04001A9F RID: 6815
		public Image.Type optionBackgroundSpriteType = Image.Type.Sliced;

		// Token: 0x04001AA0 RID: 6816
		public Selectable.Transition optionBackgroundTransitionType;

		// Token: 0x04001AA1 RID: 6817
		public ColorBlockExtended optionBackgroundTransColors = ColorBlockExtended.defaultColorBlock;

		// Token: 0x04001AA2 RID: 6818
		public SpriteStateExtended optionBackgroundSpriteStates;

		// Token: 0x04001AA3 RID: 6819
		public AnimationTriggersExtended optionBackgroundAnimationTriggers = new AnimationTriggersExtended();

		// Token: 0x04001AA4 RID: 6820
		public RuntimeAnimatorController optionBackgroundAnimatorController;

		// Token: 0x04001AA5 RID: 6821
		public Sprite optionHoverOverlay;

		// Token: 0x04001AA6 RID: 6822
		public Color optionHoverOverlayColor = Color.white;

		// Token: 0x04001AA7 RID: 6823
		public ColorBlock optionHoverOverlayColorBlock = ColorBlock.defaultColorBlock;

		// Token: 0x04001AA8 RID: 6824
		public Sprite optionPressOverlay;

		// Token: 0x04001AA9 RID: 6825
		public Color optionPressOverlayColor = Color.white;

		// Token: 0x04001AAA RID: 6826
		public ColorBlock optionPressOverlayColorBlock = ColorBlock.defaultColorBlock;

		// Token: 0x04001AAB RID: 6827
		public Sprite listSeparatorSprite;

		// Token: 0x04001AAC RID: 6828
		public Image.Type listSeparatorType;

		// Token: 0x04001AAD RID: 6829
		public Color listSeparatorColor = Color.white;

		// Token: 0x04001AAE RID: 6830
		public float listSeparatorHeight;

		// Token: 0x04001AAF RID: 6831
		public bool startSeparator;

		// Token: 0x04001AB0 RID: 6832
		public UISelectField.ChangeEvent onChange = new UISelectField.ChangeEvent();

		// Token: 0x04001AB1 RID: 6833
		public UISelectField.TransitionEvent onTransition = new UISelectField.TransitionEvent();

		// Token: 0x04001AB2 RID: 6834
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x020005CB RID: 1483
		public enum Direction
		{
			// Token: 0x04001AB4 RID: 6836
			Auto,
			// Token: 0x04001AB5 RID: 6837
			Down,
			// Token: 0x04001AB6 RID: 6838
			Up
		}

		// Token: 0x020005CC RID: 1484
		public enum VisualState
		{
			// Token: 0x04001AB8 RID: 6840
			Normal,
			// Token: 0x04001AB9 RID: 6841
			Highlighted,
			// Token: 0x04001ABA RID: 6842
			Pressed,
			// Token: 0x04001ABB RID: 6843
			Active,
			// Token: 0x04001ABC RID: 6844
			ActiveHighlighted,
			// Token: 0x04001ABD RID: 6845
			ActivePressed,
			// Token: 0x04001ABE RID: 6846
			Disabled
		}

		// Token: 0x020005CD RID: 1485
		public enum ListAnimationType
		{
			// Token: 0x04001AC0 RID: 6848
			None,
			// Token: 0x04001AC1 RID: 6849
			Fade,
			// Token: 0x04001AC2 RID: 6850
			Animation
		}

		// Token: 0x020005CE RID: 1486
		public enum OptionTextTransitionType
		{
			// Token: 0x04001AC4 RID: 6852
			None,
			// Token: 0x04001AC5 RID: 6853
			CrossFade
		}

		// Token: 0x020005CF RID: 1487
		public enum OptionTextEffectType
		{
			// Token: 0x04001AC7 RID: 6855
			None,
			// Token: 0x04001AC8 RID: 6856
			Shadow,
			// Token: 0x04001AC9 RID: 6857
			Outline
		}

		// Token: 0x020005D0 RID: 1488
		[Serializable]
		public class ChangeEvent : UnityEvent<int, string>
		{
		}

		// Token: 0x020005D1 RID: 1489
		[Serializable]
		public class TransitionEvent : UnityEvent<UISelectField.VisualState, bool>
		{
		}
	}
}
