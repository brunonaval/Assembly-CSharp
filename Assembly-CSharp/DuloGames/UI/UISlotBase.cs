using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000609 RID: 1545
	[AddComponentMenu("UI/Icon Slots/Base Slot")]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public class UISlotBase : UIBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
	{
		// Token: 0x1700033D RID: 829
		// (get) Token: 0x060021E6 RID: 8678 RVA: 0x000A8FF5 File Offset: 0x000A71F5
		// (set) Token: 0x060021E7 RID: 8679 RVA: 0x000A8FFD File Offset: 0x000A71FD
		public bool dragAndDropEnabled
		{
			get
			{
				return this.m_DragAndDropEnabled;
			}
			set
			{
				this.m_DragAndDropEnabled = value;
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000A9006 File Offset: 0x000A7206
		// (set) Token: 0x060021E9 RID: 8681 RVA: 0x000A900E File Offset: 0x000A720E
		public bool isStatic
		{
			get
			{
				return this.m_IsStatic;
			}
			set
			{
				this.m_IsStatic = value;
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x000A9017 File Offset: 0x000A7217
		// (set) Token: 0x060021EB RID: 8683 RVA: 0x000A901F File Offset: 0x000A721F
		public bool allowThrowAway
		{
			get
			{
				return this.m_AllowThrowAway;
			}
			set
			{
				this.m_AllowThrowAway = value;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000A9028 File Offset: 0x000A7228
		// (set) Token: 0x060021ED RID: 8685 RVA: 0x000A9030 File Offset: 0x000A7230
		public UISlotBase.DragKeyModifier dragKeyModifier
		{
			get
			{
				return this.m_DragKeyModifier;
			}
			set
			{
				this.m_DragKeyModifier = value;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x000A9039 File Offset: 0x000A7239
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x000A9041 File Offset: 0x000A7241
		public bool tooltipEnabled
		{
			get
			{
				return this.m_TooltipEnabled;
			}
			set
			{
				this.m_TooltipEnabled = value;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000A904A File Offset: 0x000A724A
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x000A9052 File Offset: 0x000A7252
		public float tooltipDelay
		{
			get
			{
				return this.m_TooltipDelay;
			}
			set
			{
				this.m_TooltipDelay = value;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000A905B File Offset: 0x000A725B
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x000A9063 File Offset: 0x000A7263
		public bool pressTransitionInstaOut
		{
			get
			{
				return this.m_PressTransitionInstaOut;
			}
			set
			{
				this.m_PressTransitionInstaOut = value;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000A906C File Offset: 0x000A726C
		// (set) Token: 0x060021F5 RID: 8693 RVA: 0x000A9074 File Offset: 0x000A7274
		public bool pressForceHoverNormal
		{
			get
			{
				return this.m_PressForceHoverNormal;
			}
			set
			{
				this.m_PressForceHoverNormal = value;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060021F6 RID: 8694 RVA: 0x000A907D File Offset: 0x000A727D
		// (set) Token: 0x060021F7 RID: 8695 RVA: 0x000A9085 File Offset: 0x000A7285
		public bool dropPreformed
		{
			get
			{
				return this.m_DropPreformed;
			}
			set
			{
				this.m_DropPreformed = value;
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000A908E File Offset: 0x000A728E
		protected override void Start()
		{
			if (!this.IsAssigned() && this.iconGraphic != null && this.iconGraphic.gameObject.activeSelf)
			{
				this.iconGraphic.gameObject.SetActive(false);
			}
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000A90C9 File Offset: 0x000A72C9
		protected override void OnEnable()
		{
			base.OnEnable();
			this.EvaluateAndTransitionHoveredState(true);
			this.EvaluateAndTransitionPressedState(true);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000A90DF File Offset: 0x000A72DF
		protected override void OnDisable()
		{
			base.OnDisable();
			this.isPointerInside = false;
			this.isPointerDown = false;
			this.EvaluateAndTransitionHoveredState(true);
			this.EvaluateAndTransitionPressedState(true);
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000A9104 File Offset: 0x000A7304
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			this.isPointerInside = true;
			this.EvaluateAndTransitionHoveredState(false);
			if (base.enabled && this.IsActive() && this.m_TooltipEnabled)
			{
				if (this.m_TooltipDelay > 0f)
				{
					base.StartCoroutine("TooltipDelayedShow");
					return;
				}
				this.InternalShowTooltip();
			}
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000A9157 File Offset: 0x000A7357
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			this.isPointerInside = false;
			this.EvaluateAndTransitionHoveredState(false);
			this.InternalHideTooltip();
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x0000219A File Offset: 0x0000039A
		public virtual void OnTooltip(bool show)
		{
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000A916D File Offset: 0x000A736D
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.isPointerDown = true;
			this.EvaluateAndTransitionPressedState(false);
			this.InternalHideTooltip();
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000A9183 File Offset: 0x000A7383
		public virtual void OnPointerUp(PointerEventData eventData)
		{
			this.isPointerDown = false;
			this.EvaluateAndTransitionPressedState(this.m_PressTransitionInstaOut);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x0000219A File Offset: 0x0000039A
		public virtual void OnPointerClick(PointerEventData eventData)
		{
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000A9198 File Offset: 0x000A7398
		protected bool IsHighlighted(BaseEventData eventData)
		{
			if (!this.IsActive())
			{
				return false;
			}
			if (eventData is PointerEventData)
			{
				PointerEventData pointerEventData = eventData as PointerEventData;
				return (this.isPointerDown && !this.isPointerInside && pointerEventData.pointerPress == base.gameObject) || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == base.gameObject) || (!this.isPointerDown && this.isPointerInside && pointerEventData.pointerPress == null);
			}
			return false;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000A9226 File Offset: 0x000A7426
		protected bool IsPressed(BaseEventData eventData)
		{
			return this.IsActive() && this.isPointerInside && this.isPointerDown;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000A9240 File Offset: 0x000A7440
		protected virtual void EvaluateAndTransitionHoveredState(bool instant)
		{
			if (!this.IsActive() || this.hoverTargetGraphic == null || !this.hoverTargetGraphic.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = this.m_PressForceHoverNormal ? (this.isPointerInside && !this.isPointerDown) : this.isPointerInside;
			switch (this.hoverTransition)
			{
			case UISlotBase.Transition.ColorTint:
				this.StartColorTween(this.hoverTargetGraphic, flag ? this.hoverHighlightColor : this.hoverNormalColor, instant ? 0f : this.hoverTransitionDuration);
				return;
			case UISlotBase.Transition.SpriteSwap:
				this.DoSpriteSwap(this.hoverTargetGraphic, flag ? this.hoverOverrideSprite : null);
				return;
			case UISlotBase.Transition.Animation:
				this.TriggerHoverStateAnimation(flag ? this.hoverHighlightTrigger : this.hoverNormalTrigger);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000A9318 File Offset: 0x000A7518
		protected virtual void EvaluateAndTransitionPressedState(bool instant)
		{
			if (!this.IsActive() || this.pressTargetGraphic == null || !this.pressTargetGraphic.gameObject.activeInHierarchy)
			{
				return;
			}
			switch (this.pressTransition)
			{
			case UISlotBase.Transition.ColorTint:
				this.StartColorTween(this.pressTargetGraphic, this.isPointerDown ? this.pressPressColor : this.pressNormalColor, instant ? 0f : this.pressTransitionDuration);
				break;
			case UISlotBase.Transition.SpriteSwap:
				this.DoSpriteSwap(this.pressTargetGraphic, this.isPointerDown ? this.pressOverrideSprite : null);
				break;
			case UISlotBase.Transition.Animation:
				this.TriggerPressStateAnimation(this.isPointerDown ? this.pressPressTrigger : this.pressNormalTrigger);
				break;
			}
			if (this.m_PressForceHoverNormal)
			{
				this.EvaluateAndTransitionHoveredState(false);
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000A93E9 File Offset: 0x000A75E9
		protected virtual void StartColorTween(Graphic target, Color targetColor, float duration)
		{
			if (target == null)
			{
				return;
			}
			target.CrossFadeColor(targetColor, duration, true, true);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000A9400 File Offset: 0x000A7600
		protected virtual void DoSpriteSwap(Graphic target, Sprite newSprite)
		{
			if (target == null)
			{
				return;
			}
			Image image = target as Image;
			if (image == null)
			{
				return;
			}
			image.overrideSprite = newSprite;
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000A9430 File Offset: 0x000A7630
		private void TriggerHoverStateAnimation(string triggername)
		{
			if (this.hoverTargetGraphic == null)
			{
				return;
			}
			Animator component = this.hoverTargetGraphic.gameObject.GetComponent<Animator>();
			if (component == null || !component.enabled || !component.isActiveAndEnabled || component.runtimeAnimatorController == null || !component.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			component.ResetTrigger(this.hoverNormalTrigger);
			component.ResetTrigger(this.hoverHighlightTrigger);
			component.SetTrigger(triggername);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000A94B4 File Offset: 0x000A76B4
		private void TriggerPressStateAnimation(string triggername)
		{
			if (this.pressTargetGraphic == null)
			{
				return;
			}
			Animator component = this.pressTargetGraphic.gameObject.GetComponent<Animator>();
			if (component == null || !component.enabled || !component.isActiveAndEnabled || component.runtimeAnimatorController == null || !component.hasBoundPlayables || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			component.ResetTrigger(this.pressNormalTrigger);
			component.ResetTrigger(this.pressPressTrigger);
			component.SetTrigger(triggername);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000A9538 File Offset: 0x000A7738
		public virtual bool IsAssigned()
		{
			return this.GetIconSprite() != null || this.GetIconTexture() != null;
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x000A9556 File Offset: 0x000A7756
		public bool Assign(Sprite icon)
		{
			if (icon == null)
			{
				return false;
			}
			this.SetIcon(icon);
			return true;
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x000A956B File Offset: 0x000A776B
		public bool Assign(Texture icon)
		{
			if (icon == null)
			{
				return false;
			}
			this.SetIcon(icon);
			return true;
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000A9580 File Offset: 0x000A7780
		public virtual bool Assign(UnityEngine.Object source)
		{
			if (source is UISlotBase)
			{
				UISlotBase uislotBase = source as UISlotBase;
				if (uislotBase != null)
				{
					if (uislotBase.GetIconSprite() != null)
					{
						return this.Assign(uislotBase.GetIconSprite());
					}
					if (uislotBase.GetIconTexture() != null)
					{
						return this.Assign(uislotBase.GetIconTexture());
					}
				}
			}
			return false;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000A95DC File Offset: 0x000A77DC
		public virtual void Unassign()
		{
			this.ClearIcon();
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000A95E4 File Offset: 0x000A77E4
		public Sprite GetIconSprite()
		{
			if (this.iconGraphic == null || !(this.iconGraphic is Image))
			{
				return null;
			}
			return (this.iconGraphic as Image).sprite;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000A9613 File Offset: 0x000A7813
		public Texture GetIconTexture()
		{
			if (this.iconGraphic == null || !(this.iconGraphic is RawImage))
			{
				return null;
			}
			return (this.iconGraphic as RawImage).texture;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000A9642 File Offset: 0x000A7842
		public UnityEngine.Object GetIconAsObject()
		{
			if (this.iconGraphic == null)
			{
				return null;
			}
			if (this.iconGraphic is Image)
			{
				return this.GetIconSprite();
			}
			if (this.iconGraphic is RawImage)
			{
				return this.GetIconTexture();
			}
			return null;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000A9680 File Offset: 0x000A7880
		public void SetIcon(Sprite iconSprite)
		{
			if (this.iconGraphic == null || !(this.iconGraphic is Image))
			{
				return;
			}
			(this.iconGraphic as Image).sprite = iconSprite;
			if (iconSprite != null && !this.iconGraphic.gameObject.activeSelf)
			{
				this.iconGraphic.gameObject.SetActive(true);
			}
			if (iconSprite == null && this.iconGraphic.gameObject.activeSelf)
			{
				this.iconGraphic.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000A9714 File Offset: 0x000A7914
		public void SetIcon(Texture iconTex)
		{
			if (this.iconGraphic == null || !(this.iconGraphic is RawImage))
			{
				return;
			}
			(this.iconGraphic as RawImage).texture = iconTex;
			if (iconTex != null && !this.iconGraphic.gameObject.activeSelf)
			{
				this.iconGraphic.gameObject.SetActive(true);
			}
			if (iconTex == null && this.iconGraphic.gameObject.activeSelf)
			{
				this.iconGraphic.gameObject.SetActive(false);
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000A97A8 File Offset: 0x000A79A8
		public void ClearIcon()
		{
			if (this.iconGraphic == null)
			{
				return;
			}
			if (this.iconGraphic is Image)
			{
				(this.iconGraphic as Image).sprite = null;
			}
			if (this.iconGraphic is RawImage)
			{
				(this.iconGraphic as RawImage).texture = null;
			}
			this.iconGraphic.gameObject.SetActive(false);
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000A9814 File Offset: 0x000A7A14
		public virtual void OnBeginDrag(PointerEventData eventData)
		{
			if (!base.enabled || !this.IsAssigned() || !this.m_DragAndDropEnabled)
			{
				eventData.Reset();
				return;
			}
			if (!this.DragKeyModifierIsDown())
			{
				eventData.Reset();
				return;
			}
			this.m_DragHasBegan = true;
			this.CreateTemporaryIcon(eventData);
			eventData.Use();
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000A9864 File Offset: 0x000A7A64
		public virtual bool DragKeyModifierIsDown()
		{
			switch (this.m_DragKeyModifier)
			{
			case UISlotBase.DragKeyModifier.Control:
				return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			case UISlotBase.DragKeyModifier.Alt:
				return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
			case UISlotBase.DragKeyModifier.Shift:
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			default:
				return true;
			}
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000A98DA File Offset: 0x000A7ADA
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (this.m_DragHasBegan && this.m_CurrentDraggedObject != null)
			{
				this.UpdateDraggedPosition(eventData);
			}
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000A98FC File Offset: 0x000A7AFC
		public virtual void OnDrop(PointerEventData eventData)
		{
			UISlotBase uislotBase = (eventData.pointerPress != null) ? eventData.pointerPress.GetComponent<UISlotBase>() : null;
			if (uislotBase == null || !uislotBase.IsAssigned() || !uislotBase.dragAndDropEnabled)
			{
				return;
			}
			uislotBase.dropPreformed = true;
			if (!base.enabled || !this.m_DragAndDropEnabled)
			{
				return;
			}
			bool flag = false;
			if (!this.IsAssigned())
			{
				flag = this.Assign(uislotBase);
				if (flag && !uislotBase.isStatic)
				{
					uislotBase.Unassign();
				}
			}
			else if (!this.isStatic && !uislotBase.isStatic)
			{
				if (this.CanSwapWith(uislotBase) && uislotBase.CanSwapWith(this))
				{
					flag = uislotBase.PerformSlotSwap(this);
				}
			}
			else if (!this.isStatic && uislotBase.isStatic)
			{
				flag = this.Assign(uislotBase);
			}
			if (!flag)
			{
				this.OnAssignBySlotFailed(uislotBase);
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000A99CC File Offset: 0x000A7BCC
		public virtual void OnEndDrag(PointerEventData eventData)
		{
			if (!this.m_DragHasBegan)
			{
				return;
			}
			this.m_DragHasBegan = false;
			if (this.m_CurrentDraggedObject != null)
			{
				UnityEngine.Object.Destroy(this.m_CurrentDraggedObject);
			}
			this.m_CurrentDraggedObject = null;
			this.m_CurrentDraggingPlane = null;
			if (this.IsHighlighted(eventData))
			{
				return;
			}
			if (!this.m_DropPreformed)
			{
				this.OnThrowAway();
				return;
			}
			this.m_DropPreformed = false;
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000A9A30 File Offset: 0x000A7C30
		public virtual bool CanSwapWith(UnityEngine.Object target)
		{
			return target is UISlotBase;
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000A9A3C File Offset: 0x000A7C3C
		public virtual bool PerformSlotSwap(UnityEngine.Object targetObject)
		{
			UISlotBase uislotBase = targetObject as UISlotBase;
			UnityEngine.Object iconAsObject = uislotBase.GetIconAsObject();
			bool flag = uislotBase.Assign(this);
			bool flag2 = this.Assign(iconAsObject);
			return flag && flag2;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000A9A68 File Offset: 0x000A7C68
		protected virtual void OnAssignBySlotFailed(UnityEngine.Object source)
		{
			Debug.Log(string.Concat(new string[]
			{
				"UISlotBase (",
				base.gameObject.name,
				") failed to get assigned by (",
				(source as UISlotBase).gameObject.name,
				")."
			}));
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000A9ABE File Offset: 0x000A7CBE
		protected virtual void OnThrowAway()
		{
			if (this.m_AllowThrowAway)
			{
				this.Unassign();
				return;
			}
			this.OnThrowAwayDenied();
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x0000219A File Offset: 0x0000039A
		protected virtual void OnThrowAwayDenied()
		{
		}

		// Token: 0x0600221E RID: 8734 RVA: 0x000A9AD8 File Offset: 0x000A7CD8
		protected virtual void CreateTemporaryIcon(PointerEventData eventData)
		{
			Canvas canvas = UIUtility.FindInParents<Canvas>(base.gameObject);
			if (canvas == null || this.iconGraphic == null)
			{
				return;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((this.m_CloneTarget == null) ? this.iconGraphic.gameObject : this.m_CloneTarget);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.SetParent(canvas.transform, false);
			gameObject.transform.SetAsLastSibling();
			(gameObject.transform as RectTransform).pivot = new Vector2(0.5f, 0.5f);
			gameObject.AddComponent<UIIgnoreRaycast>();
			this.m_CurrentDraggingPlane = (canvas.transform as RectTransform);
			this.m_CurrentDraggedObject = gameObject;
			this.UpdateDraggedPosition(eventData);
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000A9BB4 File Offset: 0x000A7DB4
		private void UpdateDraggedPosition(PointerEventData data)
		{
			RectTransform component = this.m_CurrentDraggedObject.GetComponent<RectTransform>();
			Vector3 position;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.m_CurrentDraggingPlane, data.position, data.pressEventCamera, out position))
			{
				component.position = position;
				component.rotation = this.m_CurrentDraggingPlane.rotation;
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000A9C00 File Offset: 0x000A7E00
		protected void InternalShowTooltip()
		{
			if (!this.m_IsTooltipShown)
			{
				this.m_IsTooltipShown = true;
				this.OnTooltip(true);
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000A9C18 File Offset: 0x000A7E18
		protected void InternalHideTooltip()
		{
			base.StopCoroutine("TooltipDelayedShow");
			if (this.m_IsTooltipShown)
			{
				this.m_IsTooltipShown = false;
				this.OnTooltip(false);
			}
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000A9C3B File Offset: 0x000A7E3B
		protected IEnumerator TooltipDelayedShow()
		{
			yield return new WaitForSeconds(this.m_TooltipDelay);
			this.InternalShowTooltip();
			yield break;
		}

		// Token: 0x04001B9F RID: 7071
		protected GameObject m_CurrentDraggedObject;

		// Token: 0x04001BA0 RID: 7072
		protected RectTransform m_CurrentDraggingPlane;

		// Token: 0x04001BA1 RID: 7073
		public Graphic iconGraphic;

		// Token: 0x04001BA2 RID: 7074
		[SerializeField]
		[Tooltip("The game object that should be cloned on drag.")]
		private GameObject m_CloneTarget;

		// Token: 0x04001BA3 RID: 7075
		[SerializeField]
		[Tooltip("Should the drag and drop functionallty be enabled.")]
		private bool m_DragAndDropEnabled = true;

		// Token: 0x04001BA4 RID: 7076
		[SerializeField]
		[Tooltip("If set to static the slot won't be unassigned when drag and drop is preformed.")]
		private bool m_IsStatic;

		// Token: 0x04001BA5 RID: 7077
		[SerializeField]
		[Tooltip("Should the icon assigned to the slot be throwable.")]
		private bool m_AllowThrowAway = true;

		// Token: 0x04001BA6 RID: 7078
		[SerializeField]
		[Tooltip("The key which should be held down in order to begin the drag.")]
		private UISlotBase.DragKeyModifier m_DragKeyModifier;

		// Token: 0x04001BA7 RID: 7079
		[SerializeField]
		[Tooltip("Should the tooltip functionallty be enabled.")]
		private bool m_TooltipEnabled = true;

		// Token: 0x04001BA8 RID: 7080
		[SerializeField]
		[Tooltip("How long of a delay to expect before showing the tooltip.")]
		private float m_TooltipDelay = 1f;

		// Token: 0x04001BA9 RID: 7081
		public UISlotBase.Transition hoverTransition;

		// Token: 0x04001BAA RID: 7082
		public Graphic hoverTargetGraphic;

		// Token: 0x04001BAB RID: 7083
		public Color hoverNormalColor = Color.white;

		// Token: 0x04001BAC RID: 7084
		public Color hoverHighlightColor = Color.white;

		// Token: 0x04001BAD RID: 7085
		public float hoverTransitionDuration = 0.15f;

		// Token: 0x04001BAE RID: 7086
		public Sprite hoverOverrideSprite;

		// Token: 0x04001BAF RID: 7087
		public string hoverNormalTrigger = "Normal";

		// Token: 0x04001BB0 RID: 7088
		public string hoverHighlightTrigger = "Highlighted";

		// Token: 0x04001BB1 RID: 7089
		public UISlotBase.Transition pressTransition;

		// Token: 0x04001BB2 RID: 7090
		public Graphic pressTargetGraphic;

		// Token: 0x04001BB3 RID: 7091
		public Color pressNormalColor = Color.white;

		// Token: 0x04001BB4 RID: 7092
		public Color pressPressColor = new Color(0.6117f, 0.6117f, 0.6117f, 1f);

		// Token: 0x04001BB5 RID: 7093
		public float pressTransitionDuration = 0.15f;

		// Token: 0x04001BB6 RID: 7094
		public Sprite pressOverrideSprite;

		// Token: 0x04001BB7 RID: 7095
		public string pressNormalTrigger = "Normal";

		// Token: 0x04001BB8 RID: 7096
		public string pressPressTrigger = "Pressed";

		// Token: 0x04001BB9 RID: 7097
		[SerializeField]
		[Tooltip("Should the pressed state transition to normal state instantly.")]
		private bool m_PressTransitionInstaOut = true;

		// Token: 0x04001BBA RID: 7098
		[SerializeField]
		[Tooltip("Should the pressed state force normal state transition on the hover target.")]
		private bool m_PressForceHoverNormal = true;

		// Token: 0x04001BBB RID: 7099
		private bool isPointerDown;

		// Token: 0x04001BBC RID: 7100
		private bool isPointerInside;

		// Token: 0x04001BBD RID: 7101
		private bool m_DragHasBegan;

		// Token: 0x04001BBE RID: 7102
		private bool m_DropPreformed;

		// Token: 0x04001BBF RID: 7103
		private bool m_IsTooltipShown;

		// Token: 0x0200060A RID: 1546
		public enum Transition
		{
			// Token: 0x04001BC1 RID: 7105
			None,
			// Token: 0x04001BC2 RID: 7106
			ColorTint,
			// Token: 0x04001BC3 RID: 7107
			SpriteSwap,
			// Token: 0x04001BC4 RID: 7108
			Animation
		}

		// Token: 0x0200060B RID: 1547
		public enum DragKeyModifier
		{
			// Token: 0x04001BC6 RID: 7110
			None,
			// Token: 0x04001BC7 RID: 7111
			Control,
			// Token: 0x04001BC8 RID: 7112
			Alt,
			// Token: 0x04001BC9 RID: 7113
			Shift
		}
	}
}
