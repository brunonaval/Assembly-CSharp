using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000646 RID: 1606
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Tooltip", 58)]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(VerticalLayoutGroup))]
	[RequireComponent(typeof(ContentSizeFitter))]
	public class UITooltip : MonoBehaviour
	{
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06002374 RID: 9076 RVA: 0x000AE47D File Offset: 0x000AC67D
		public static UITooltip Instance
		{
			get
			{
				return UITooltip.mInstance;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06002375 RID: 9077 RVA: 0x000AE484 File Offset: 0x000AC684
		// (set) Token: 0x06002376 RID: 9078 RVA: 0x000AE48C File Offset: 0x000AC68C
		public float defaultWidth
		{
			get
			{
				return this.m_DefaultWidth;
			}
			set
			{
				this.m_DefaultWidth = value;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06002377 RID: 9079 RVA: 0x000AE495 File Offset: 0x000AC695
		// (set) Token: 0x06002378 RID: 9080 RVA: 0x000AE49D File Offset: 0x000AC69D
		public bool followMouse
		{
			get
			{
				return this.m_followMouse;
			}
			set
			{
				this.m_followMouse = value;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06002379 RID: 9081 RVA: 0x000AE4A6 File Offset: 0x000AC6A6
		// (set) Token: 0x0600237A RID: 9082 RVA: 0x000AE4AE File Offset: 0x000AC6AE
		public Vector2 offset
		{
			get
			{
				return this.m_Offset;
			}
			set
			{
				this.m_Offset = value;
				this.m_OriginalOffset = value;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600237B RID: 9083 RVA: 0x000AE4BE File Offset: 0x000AC6BE
		// (set) Token: 0x0600237C RID: 9084 RVA: 0x000AE4C6 File Offset: 0x000AC6C6
		public UITooltip.Anchoring anchoring
		{
			get
			{
				return this.m_Anchoring;
			}
			set
			{
				this.m_Anchoring = value;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x0600237D RID: 9085 RVA: 0x000AE4CF File Offset: 0x000AC6CF
		// (set) Token: 0x0600237E RID: 9086 RVA: 0x000AE4D7 File Offset: 0x000AC6D7
		public Vector2 anchoredOffset
		{
			get
			{
				return this.m_AnchoredOffset;
			}
			set
			{
				this.m_AnchoredOffset = value;
				this.m_OriginalAnchoredOffset = value;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600237F RID: 9087 RVA: 0x000AE4E7 File Offset: 0x000AC6E7
		public float alpha
		{
			get
			{
				if (!(this.m_CanvasGroup != null))
				{
					return 1f;
				}
				return this.m_CanvasGroup.alpha;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06002380 RID: 9088 RVA: 0x000AE508 File Offset: 0x000AC708
		public UITooltip.VisualState visualState
		{
			get
			{
				return this.m_VisualState;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06002381 RID: 9089 RVA: 0x000AE510 File Offset: 0x000AC710
		public Camera uiCamera
		{
			get
			{
				if (this.m_Canvas == null)
				{
					return null;
				}
				if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay || (this.m_Canvas.renderMode == RenderMode.ScreenSpaceCamera && this.m_Canvas.worldCamera == null))
				{
					return null;
				}
				if (this.m_Canvas.worldCamera != null)
				{
					return this.m_Canvas.worldCamera;
				}
				return Camera.main;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x06002382 RID: 9090 RVA: 0x000AE581 File Offset: 0x000AC781
		// (set) Token: 0x06002383 RID: 9091 RVA: 0x000AE589 File Offset: 0x000AC789
		public UITooltip.Transition transition
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

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x06002384 RID: 9092 RVA: 0x000AE592 File Offset: 0x000AC792
		// (set) Token: 0x06002385 RID: 9093 RVA: 0x000AE59A File Offset: 0x000AC79A
		public TweenEasing transitionEasing
		{
			get
			{
				return this.m_TransitionEasing;
			}
			set
			{
				this.m_TransitionEasing = value;
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06002386 RID: 9094 RVA: 0x000AE5A3 File Offset: 0x000AC7A3
		// (set) Token: 0x06002387 RID: 9095 RVA: 0x000AE5AB File Offset: 0x000AC7AB
		public float transitionDuration
		{
			get
			{
				return this.m_TransitionDuration;
			}
			set
			{
				this.m_TransitionDuration = value;
			}
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000AE5B4 File Offset: 0x000AC7B4
		protected UITooltip()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000AE640 File Offset: 0x000AC840
		protected virtual void Awake()
		{
			UITooltip.mInstance = this;
			this.m_Rect = base.gameObject.GetComponent<RectTransform>();
			this.m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
			this.m_CanvasGroup.blocksRaycasts = false;
			this.m_CanvasGroup.interactable = false;
			this.m_SizeFitter = base.gameObject.GetComponent<ContentSizeFitter>();
			this.m_SizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			VerticalLayoutGroup component = base.gameObject.GetComponent<VerticalLayoutGroup>();
			component.childControlHeight = true;
			component.childControlWidth = true;
			this.m_OriginalOffset = this.m_Offset;
			this.m_OriginalAnchoredOffset = this.m_AnchoredOffset;
			if (base.gameObject.GetComponent<UIAlwaysOnTop>() == null)
			{
				base.gameObject.AddComponent<UIAlwaysOnTop>().order = 99999;
			}
			this.SetAlpha(0f);
			this.m_VisualState = UITooltip.VisualState.Hidden;
			this.InternalOnHide();
		}

		// Token: 0x0600238A RID: 9098 RVA: 0x000AE71A File Offset: 0x000AC91A
		protected virtual void Start()
		{
			this.m_Rect.anchorMin = new Vector2(0.5f, 0.5f);
			this.m_Rect.anchorMax = new Vector2(0.5f, 0.5f);
		}

		// Token: 0x0600238B RID: 9099 RVA: 0x000AE750 File Offset: 0x000AC950
		protected virtual void OnDestroy()
		{
			UITooltip.mInstance = null;
		}

		// Token: 0x0600238C RID: 9100 RVA: 0x000AE758 File Offset: 0x000AC958
		protected virtual void OnCanvasGroupChanged()
		{
			this.m_Canvas = UIUtility.FindInParents<Canvas>(base.gameObject);
		}

		// Token: 0x0600238D RID: 9101 RVA: 0x000A2602 File Offset: 0x000A0802
		public virtual bool IsActive()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		// Token: 0x0600238E RID: 9102 RVA: 0x000AE76B File Offset: 0x000AC96B
		protected virtual void Update()
		{
			if (this.m_followMouse && base.enabled && this.IsActive() && this.alpha > 0f)
			{
				this.UpdatePositionAndPivot();
			}
		}

		// Token: 0x0600238F RID: 9103 RVA: 0x000AE798 File Offset: 0x000AC998
		public virtual void UpdatePositionAndPivot()
		{
			if (this.m_Canvas == null)
			{
				return;
			}
			this.UpdatePivot();
			if (this.m_AnchorToTarget == null)
			{
				Vector2 a = new Vector2((this.m_Rect.pivot.x == 1f) ? (this.m_Offset.x * -1f) : this.m_Offset.x, (this.m_Rect.pivot.y == 1f) ? (this.m_Offset.y * -1f) : this.m_Offset.y);
				Vector2 b;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_Canvas.transform as RectTransform, Input.mousePosition, this.uiCamera, out b))
				{
					this.m_Rect.anchoredPosition = a + b;
				}
			}
			if (this.m_AnchorToTarget != null)
			{
				if (this.m_Anchoring == UITooltip.Anchoring.Corners)
				{
					Vector3[] array = new Vector3[4];
					this.m_AnchorToTarget.GetWorldCorners(array);
					UITooltip.Corner oppositeCorner = UITooltip.GetOppositeCorner(UITooltip.VectorPivotToCorner(this.m_Rect.pivot));
					Vector2 a2 = new Vector2((this.m_Rect.pivot.x == 1f) ? (this.m_AnchoredOffset.x * -1f) : this.m_AnchoredOffset.x, (this.m_Rect.pivot.y == 1f) ? (this.m_AnchoredOffset.y * -1f) : this.m_AnchoredOffset.y);
					Vector2 b2 = this.m_Canvas.transform.InverseTransformPoint(array[(int)oppositeCorner]);
					this.m_Rect.anchoredPosition = a2 + b2;
				}
				else if (this.m_Anchoring == UITooltip.Anchoring.LeftOrRight || this.m_Anchoring == UITooltip.Anchoring.TopOrBottom)
				{
					Vector3[] array2 = new Vector3[4];
					this.m_AnchorToTarget.GetWorldCorners(array2);
					Vector2 a3 = this.m_Canvas.transform.InverseTransformPoint(array2[1]);
					if (this.m_Anchoring == UITooltip.Anchoring.LeftOrRight)
					{
						Vector2 b3 = new Vector2((this.m_Rect.pivot.x == 1f) ? (this.m_AnchoredOffset.x * -1f) : this.m_AnchoredOffset.x, this.m_AnchoredOffset.y);
						if (this.m_Rect.pivot.x == 0f)
						{
							this.m_Rect.anchoredPosition = a3 + b3 + new Vector2(this.m_AnchorToTarget.rect.width, this.m_AnchorToTarget.rect.height / 2f * -1f);
						}
						else
						{
							this.m_Rect.anchoredPosition = a3 + b3 + new Vector2(0f, this.m_AnchorToTarget.rect.height / 2f * -1f);
						}
					}
					else if (this.m_Anchoring == UITooltip.Anchoring.TopOrBottom)
					{
						Vector2 b4 = new Vector2(this.m_AnchoredOffset.x, (this.m_Rect.pivot.y == 1f) ? (this.m_AnchoredOffset.y * -1f) : this.m_AnchoredOffset.y);
						if (this.m_Rect.pivot.y == 0f)
						{
							this.m_Rect.anchoredPosition = a3 + b4 + new Vector2(this.m_AnchorToTarget.rect.width / 2f, 0f);
						}
						else
						{
							this.m_Rect.anchoredPosition = a3 + b4 + new Vector2(this.m_AnchorToTarget.rect.width / 2f, this.m_AnchorToTarget.rect.height * -1f);
						}
					}
				}
			}
			this.m_Rect.anchoredPosition = new Vector2(Mathf.Round(this.m_Rect.anchoredPosition.x), Mathf.Round(this.m_Rect.anchoredPosition.y));
			this.m_Rect.anchoredPosition = new Vector2(this.m_Rect.anchoredPosition.x + this.m_Rect.anchoredPosition.x % 2f, this.m_Rect.anchoredPosition.y + this.m_Rect.anchoredPosition.y % 2f);
		}

		// Token: 0x06002390 RID: 9104 RVA: 0x000AEC4C File Offset: 0x000ACE4C
		public void UpdatePivot()
		{
			Vector3 mousePosition = Input.mousePosition;
			if (this.m_Anchoring == UITooltip.Anchoring.Corners)
			{
				Vector2 pivot = new Vector2((mousePosition.x > (float)Screen.width / 2f) ? 1f : 0f, (mousePosition.y > (float)Screen.height / 2f) ? 1f : 0f);
				this.SetPivot(UITooltip.VectorPivotToCorner(pivot));
				return;
			}
			if (this.m_Anchoring == UITooltip.Anchoring.LeftOrRight)
			{
				Vector2 pivot2 = new Vector2((mousePosition.x > (float)Screen.width / 2f) ? 1f : 0f, 0.5f);
				this.SetPivot(pivot2);
				return;
			}
			if (this.m_Anchoring == UITooltip.Anchoring.TopOrBottom)
			{
				Vector2 pivot3 = new Vector2(0.5f, (mousePosition.y > (float)Screen.height / 2f) ? 1f : 0f);
				this.SetPivot(pivot3);
			}
		}

		// Token: 0x06002391 RID: 9105 RVA: 0x000AED34 File Offset: 0x000ACF34
		protected void SetPivot(Vector2 pivot)
		{
			this.m_Rect.pivot = pivot;
			this.m_CurrentAnchor = UITooltip.VectorPivotToAnchor(pivot);
			this.UpdateAnchorGraphicPosition();
		}

		// Token: 0x06002392 RID: 9106 RVA: 0x000AED54 File Offset: 0x000ACF54
		protected void SetPivot(UITooltip.Corner point)
		{
			switch (point)
			{
			case UITooltip.Corner.BottomLeft:
				this.m_Rect.pivot = new Vector2(0f, 0f);
				break;
			case UITooltip.Corner.TopLeft:
				this.m_Rect.pivot = new Vector2(0f, 1f);
				break;
			case UITooltip.Corner.TopRight:
				this.m_Rect.pivot = new Vector2(1f, 1f);
				break;
			case UITooltip.Corner.BottomRight:
				this.m_Rect.pivot = new Vector2(1f, 0f);
				break;
			}
			this.m_CurrentAnchor = UITooltip.VectorPivotToAnchor(this.m_Rect.pivot);
			this.UpdateAnchorGraphicPosition();
		}

		// Token: 0x06002393 RID: 9107 RVA: 0x000AEE04 File Offset: 0x000AD004
		protected void UpdateAnchorGraphicPosition()
		{
			if (this.m_AnchorGraphic == null)
			{
				return;
			}
			RectTransform rectTransform = this.m_AnchorGraphic.transform as RectTransform;
			if (this.m_Anchoring == UITooltip.Anchoring.Corners)
			{
				rectTransform.pivot = Vector2.zero;
				rectTransform.anchorMax = this.m_Rect.pivot;
				rectTransform.anchorMin = this.m_Rect.pivot;
				rectTransform.anchoredPosition = new Vector2((this.m_Rect.pivot.x == 1f) ? (this.m_AnchorGraphicOffset.x * -1f) : this.m_AnchorGraphicOffset.x, (this.m_Rect.pivot.y == 1f) ? (this.m_AnchorGraphicOffset.y * -1f) : this.m_AnchorGraphicOffset.y);
				rectTransform.localScale = new Vector3((this.m_Rect.pivot.x == 0f) ? 1f : -1f, (this.m_Rect.pivot.y == 0f) ? 1f : -1f, rectTransform.localScale.z);
			}
			else if (this.m_Anchoring == UITooltip.Anchoring.LeftOrRight || this.m_Anchoring == UITooltip.Anchoring.TopOrBottom)
			{
				switch (this.m_CurrentAnchor)
				{
				case UITooltip.Anchor.Left:
					rectTransform.pivot = new Vector2(0f, 0.5f);
					rectTransform.anchorMax = new Vector2(0f, 0.5f);
					rectTransform.anchorMin = new Vector2(0f, 0.5f);
					rectTransform.anchoredPosition3D = new Vector3(this.m_AnchorGraphicOffset.x, this.m_AnchorGraphicOffset.y, rectTransform.localPosition.z);
					rectTransform.localScale = new Vector3(1f, 1f, rectTransform.localScale.z);
					break;
				case UITooltip.Anchor.Right:
					rectTransform.pivot = new Vector2(1f, 0.5f);
					rectTransform.anchorMax = new Vector2(1f, 0.5f);
					rectTransform.anchorMin = new Vector2(1f, 0.5f);
					rectTransform.anchoredPosition3D = new Vector3(this.m_AnchorGraphicOffset.x * -1f - rectTransform.rect.width, this.m_AnchorGraphicOffset.y, rectTransform.localPosition.z);
					rectTransform.localScale = new Vector3(-1f, 1f, rectTransform.localScale.z);
					break;
				case UITooltip.Anchor.Top:
					rectTransform.pivot = new Vector2(0.5f, 1f);
					rectTransform.anchorMax = new Vector2(0.5f, 1f);
					rectTransform.anchorMin = new Vector2(0.5f, 1f);
					rectTransform.anchoredPosition3D = new Vector3(this.m_AnchorGraphicOffset.x, this.m_AnchorGraphicOffset.y * -1f - rectTransform.rect.height, rectTransform.localPosition.z);
					rectTransform.localScale = new Vector3(1f, -1f, rectTransform.localScale.z);
					break;
				case UITooltip.Anchor.Bottom:
					rectTransform.pivot = new Vector2(0.5f, 0f);
					rectTransform.anchorMax = new Vector2(0.5f, 0f);
					rectTransform.anchorMin = new Vector2(0.5f, 0f);
					rectTransform.anchoredPosition3D = new Vector3(this.m_AnchorGraphicOffset.x, this.m_AnchorGraphicOffset.y, rectTransform.localPosition.z);
					rectTransform.localScale = new Vector3(1f, 1f, rectTransform.localScale.z);
					break;
				}
			}
			if (this.onAnchorEvent != null)
			{
				this.onAnchorEvent.Invoke(this.m_CurrentAnchor);
			}
		}

		// Token: 0x06002394 RID: 9108 RVA: 0x000AF1F6 File Offset: 0x000AD3F6
		protected virtual void Internal_Show()
		{
			this.EvaluateAndCreateTooltipLines();
			this.UpdatePositionAndPivot();
			UIUtility.BringToFront(base.gameObject);
			this.EvaluateAndTransitionToState(true, false);
		}

		// Token: 0x06002395 RID: 9109 RVA: 0x000AF217 File Offset: 0x000AD417
		protected virtual void Internal_Hide()
		{
			this.EvaluateAndTransitionToState(false, false);
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x000AF221 File Offset: 0x000AD421
		protected virtual void Internal_AnchorToRect(RectTransform targetRect)
		{
			this.m_AnchorToTarget = targetRect;
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000AF22A File Offset: 0x000AD42A
		protected void Internal_SetWidth(float width)
		{
			this.m_Rect.sizeDelta = new Vector2(width, this.m_Rect.sizeDelta.y);
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000AF24D File Offset: 0x000AD44D
		protected void Internal_SetHorizontalFitMode(ContentSizeFitter.FitMode mode)
		{
			this.m_SizeFitter.horizontalFit = mode;
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000AF25B File Offset: 0x000AD45B
		protected void Internal_OverrideOffset(Vector2 offset)
		{
			this.m_Offset = offset;
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000AF264 File Offset: 0x000AD464
		protected void Internal_OverrideAnchoredOffset(Vector2 offset)
		{
			this.m_AnchoredOffset = offset;
		}

		// Token: 0x0600239B RID: 9115 RVA: 0x000AF270 File Offset: 0x000AD470
		private void EvaluateAndTransitionToState(bool state, bool instant)
		{
			UITooltip.Transition transition = this.m_Transition;
			if (transition != UITooltip.Transition.None && transition == UITooltip.Transition.Fade)
			{
				this.StartAlphaTween(state ? 1f : 0f, instant ? 0f : this.m_TransitionDuration);
			}
			else
			{
				this.SetAlpha(state ? 1f : 0f);
				this.m_VisualState = (state ? UITooltip.VisualState.Shown : UITooltip.VisualState.Hidden);
			}
			if (this.m_Transition == UITooltip.Transition.None && !state)
			{
				this.InternalOnHide();
			}
		}

		// Token: 0x0600239C RID: 9116 RVA: 0x000AF2E5 File Offset: 0x000AD4E5
		public void SetAlpha(float alpha)
		{
			this.m_CanvasGroup.alpha = alpha;
		}

		// Token: 0x0600239D RID: 9117 RVA: 0x000AF2F4 File Offset: 0x000AD4F4
		public void StartAlphaTween(float targetAlpha, float duration)
		{
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = this.m_CanvasGroup.alpha,
				targetFloat = targetAlpha
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetAlpha));
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = true;
			info.easing = this.m_TransitionEasing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x0600239E RID: 9118 RVA: 0x000AF375 File Offset: 0x000AD575
		protected virtual void OnTweenFinished()
		{
			if (this.alpha == 0f)
			{
				this.m_VisualState = UITooltip.VisualState.Hidden;
				this.InternalOnHide();
				return;
			}
			this.m_VisualState = UITooltip.VisualState.Shown;
		}

		// Token: 0x0600239F RID: 9119 RVA: 0x000AF39C File Offset: 0x000AD59C
		private void InternalOnHide()
		{
			this.CleanupLines();
			this.m_AnchorToTarget = null;
			this.m_SizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			this.m_Rect.sizeDelta = new Vector2(this.m_DefaultWidth, this.m_Rect.sizeDelta.y);
			this.m_Offset = this.m_OriginalOffset;
			this.m_AnchoredOffset = this.m_OriginalAnchoredOffset;
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000AF400 File Offset: 0x000AD600
		private void EvaluateAndCreateTooltipLines()
		{
			if (this.m_LinesTemplate == null || this.m_LinesTemplate.lineList.Count == 0)
			{
				return;
			}
			foreach (UITooltipLines.Line line in this.m_LinesTemplate.lineList)
			{
				GameObject gameObject = this.CreateLine(line.padding);
				for (int i = 0; i < 2; i++)
				{
					string text = (i == 0) ? line.left : line.right;
					if (!string.IsNullOrEmpty(text))
					{
						this.CreateLineColumn(gameObject.transform, text, i == 0, line.style, line.customStyle);
					}
				}
			}
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000AF4C0 File Offset: 0x000AD6C0
		private GameObject CreateLine(RectOffset padding)
		{
			GameObject gameObject = new GameObject("Line", new Type[]
			{
				typeof(RectTransform)
			});
			(gameObject.transform as RectTransform).pivot = new Vector2(0f, 1f);
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.layer = base.gameObject.layer;
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.padding = padding;
			horizontalLayoutGroup.childControlHeight = true;
			horizontalLayoutGroup.childControlWidth = true;
			return gameObject;
		}

		// Token: 0x060023A2 RID: 9122 RVA: 0x000AF574 File Offset: 0x000AD774
		private void CreateLineColumn(Transform parent, string content, bool isLeft, UITooltipLines.LineStyle style, string customStyle)
		{
			GameObject gameObject = new GameObject("Column", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer)
			});
			gameObject.layer = base.gameObject.layer;
			gameObject.transform.SetParent(parent);
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			gameObject.transform.localPosition = Vector3.zero;
			(gameObject.transform as RectTransform).pivot = new Vector2(0f, 1f);
			Text text = gameObject.AddComponent<Text>();
			text.text = content;
			text.supportRichText = true;
			text.raycastTarget = false;
			UITooltipLineStyle uitooltipLineStyle = UITooltipManager.Instance.defaultLineStyle;
			switch (style)
			{
			case UITooltipLines.LineStyle.Title:
				uitooltipLineStyle = UITooltipManager.Instance.titleLineStyle;
				break;
			case UITooltipLines.LineStyle.Description:
				uitooltipLineStyle = UITooltipManager.Instance.descriptionLineStyle;
				break;
			case UITooltipLines.LineStyle.Custom:
				uitooltipLineStyle = UITooltipManager.Instance.GetCustomStyle(customStyle);
				break;
			}
			text.font = uitooltipLineStyle.TextFont;
			text.fontStyle = uitooltipLineStyle.TextFontStyle;
			text.fontSize = uitooltipLineStyle.TextFontSize;
			text.lineSpacing = uitooltipLineStyle.TextFontLineSpacing;
			text.color = uitooltipLineStyle.TextFontColor;
			if (uitooltipLineStyle.OverrideTextAlignment == OverrideTextAlignment.No)
			{
				text.alignment = (isLeft ? TextAnchor.LowerLeft : TextAnchor.LowerRight);
			}
			else
			{
				switch (uitooltipLineStyle.OverrideTextAlignment)
				{
				case OverrideTextAlignment.Left:
					text.alignment = TextAnchor.LowerLeft;
					break;
				case OverrideTextAlignment.Center:
					text.alignment = TextAnchor.LowerCenter;
					break;
				case OverrideTextAlignment.Right:
					text.alignment = TextAnchor.LowerRight;
					break;
				}
			}
			if (uitooltipLineStyle.TextEffects.Length != 0)
			{
				foreach (UITooltipTextEffect uitooltipTextEffect in uitooltipLineStyle.TextEffects)
				{
					if (uitooltipTextEffect.Effect == UITooltipTextEffectType.Shadow)
					{
						Shadow shadow = gameObject.AddComponent<Shadow>();
						shadow.effectColor = uitooltipTextEffect.EffectColor;
						shadow.effectDistance = uitooltipTextEffect.EffectDistance;
						shadow.useGraphicAlpha = uitooltipTextEffect.UseGraphicAlpha;
					}
					else if (uitooltipTextEffect.Effect == UITooltipTextEffectType.Outline)
					{
						Outline outline = gameObject.AddComponent<Outline>();
						outline.effectColor = uitooltipTextEffect.EffectColor;
						outline.effectDistance = uitooltipTextEffect.EffectDistance;
						outline.useGraphicAlpha = uitooltipTextEffect.UseGraphicAlpha;
					}
				}
			}
		}

		// Token: 0x060023A3 RID: 9123 RVA: 0x000AF7A4 File Offset: 0x000AD9A4
		protected virtual void CleanupLines()
		{
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if (!(transform.gameObject.GetComponent<LayoutElement>() != null) || !transform.gameObject.GetComponent<LayoutElement>().ignoreLayout)
				{
					UnityEngine.Object.Destroy(transform.gameObject);
				}
			}
			this.m_LinesTemplate = null;
		}

		// Token: 0x060023A4 RID: 9124 RVA: 0x000AF830 File Offset: 0x000ADA30
		protected void Internal_SetLines(UITooltipLines lines)
		{
			this.m_LinesTemplate = lines;
		}

		// Token: 0x060023A5 RID: 9125 RVA: 0x000AF839 File Offset: 0x000ADA39
		protected void Internal_AddLine(string a, RectOffset padding)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(a, padding);
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x000AF85B File Offset: 0x000ADA5B
		protected void Internal_AddLine(string a, UITooltipLines.LineStyle style)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(a, new RectOffset(), style);
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x000AF882 File Offset: 0x000ADA82
		protected void Internal_AddLine(string a, string customStyle)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(a, new RectOffset(), customStyle);
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x000AF8A9 File Offset: 0x000ADAA9
		protected void Internal_AddLine(string a, RectOffset padding, UITooltipLines.LineStyle style)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(a, padding, style);
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x000AF8CC File Offset: 0x000ADACC
		protected void Internal_AddLine(string a, RectOffset padding, string customStyle)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(a, padding, customStyle);
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x000AF8EF File Offset: 0x000ADAEF
		protected void Internal_AddLineColumn(string a)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddColumn(a);
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x000AF910 File Offset: 0x000ADB10
		protected void Internal_AddLineColumn(string a, UITooltipLines.LineStyle style)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddColumn(a, style);
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x000AF932 File Offset: 0x000ADB32
		protected void Internal_AddLineColumn(string a, string customStyle)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddColumn(a, customStyle);
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x000AF954 File Offset: 0x000ADB54
		protected virtual void Internal_AddTitle(string title)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(title, new RectOffset(0, 0, 0, 0), UITooltipLines.LineStyle.Title);
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x000AF97F File Offset: 0x000ADB7F
		protected virtual void Internal_AddDescription(string description)
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine(description, new RectOffset(0, 0, 0, 0), UITooltipLines.LineStyle.Description);
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x000AF9AA File Offset: 0x000ADBAA
		protected virtual void Internal_AddSpacer()
		{
			if (this.m_LinesTemplate == null)
			{
				this.m_LinesTemplate = new UITooltipLines();
			}
			this.m_LinesTemplate.AddLine("", new RectOffset(0, 0, UITooltipManager.Instance.spacerHeight, 0));
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x000AF9E1 File Offset: 0x000ADBE1
		public static void SetLines(UITooltipLines lines)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_SetLines(lines);
			}
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x000AF9FB File Offset: 0x000ADBFB
		public static void AddLine(string content)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLine(content, new RectOffset());
			}
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000AFA1A File Offset: 0x000ADC1A
		public static void AddLine(string content, UITooltipLines.LineStyle style)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLine(content, new RectOffset(), style);
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000AFA3A File Offset: 0x000ADC3A
		public static void AddLine(string content, string customStyle)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLine(content, new RectOffset(), customStyle);
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000AFA5A File Offset: 0x000ADC5A
		public static void AddLine(string content, RectOffset padding)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLine(content, padding);
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000AFA75 File Offset: 0x000ADC75
		public static void AddLine(string content, RectOffset padding, UITooltipLines.LineStyle style)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLine(content, padding, style);
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000AFA91 File Offset: 0x000ADC91
		public static void AddLine(string content, RectOffset padding, string customStyle)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLine(content, padding, customStyle);
			}
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000AFAAD File Offset: 0x000ADCAD
		public static void AddLineColumn(string content)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLineColumn(content);
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000AFAC7 File Offset: 0x000ADCC7
		public static void AddLineColumn(string content, UITooltipLines.LineStyle style)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLineColumn(content, style);
			}
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000AFAE2 File Offset: 0x000ADCE2
		public static void AddLineColumn(string content, string customStyle)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddLineColumn(content, customStyle);
			}
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x000AFAFD File Offset: 0x000ADCFD
		public static void AddSpacer()
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddSpacer();
			}
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x000AFB18 File Offset: 0x000ADD18
		public static void InstantiateIfNecessary(GameObject rel)
		{
			if (UITooltipManager.Instance == null || UITooltipManager.Instance.prefab == null)
			{
				return;
			}
			Canvas canvas = UIUtility.FindInParents<Canvas>(rel);
			if (canvas == null)
			{
				return;
			}
			if (UITooltip.mInstance != null)
			{
				Canvas canvas2 = UIUtility.FindInParents<Canvas>(UITooltip.mInstance.gameObject);
				if (canvas2 != null && canvas2.Equals(canvas))
				{
					return;
				}
				UnityEngine.Object.Destroy(UITooltip.mInstance.gameObject);
			}
			UnityEngine.Object.Instantiate<GameObject>(UITooltipManager.Instance.prefab, canvas.transform, false);
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x000AFBAC File Offset: 0x000ADDAC
		public static void AddTitle(string title)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddTitle(title);
			}
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x000AFBC6 File Offset: 0x000ADDC6
		public static void AddDescription(string description)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AddDescription(description);
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x000AFBE0 File Offset: 0x000ADDE0
		public static void Show()
		{
			if (UITooltip.mInstance != null && UITooltip.mInstance.IsActive())
			{
				UITooltip.mInstance.Internal_Show();
			}
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x000AFC05 File Offset: 0x000ADE05
		public static void Hide()
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_Hide();
			}
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x000AFC1E File Offset: 0x000ADE1E
		public static void AnchorToRect(RectTransform targetRect)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_AnchorToRect(targetRect);
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000AFC38 File Offset: 0x000ADE38
		public static void SetHorizontalFitMode(ContentSizeFitter.FitMode mode)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_SetHorizontalFitMode(mode);
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000AFC52 File Offset: 0x000ADE52
		public static void SetWidth(float width)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_SetWidth(width);
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000AFC6C File Offset: 0x000ADE6C
		public static void OverrideOffset(Vector2 offset)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_OverrideOffset(offset);
			}
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000AFC86 File Offset: 0x000ADE86
		public static void OverrideAnchoredOffset(Vector2 offset)
		{
			if (UITooltip.mInstance != null)
			{
				UITooltip.mInstance.Internal_OverrideAnchoredOffset(offset);
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000AFCA0 File Offset: 0x000ADEA0
		public static UITooltip.Corner VectorPivotToCorner(Vector2 pivot)
		{
			if (pivot.x == 0f && pivot.y == 0f)
			{
				return UITooltip.Corner.BottomLeft;
			}
			if (pivot.x == 0f && pivot.y == 1f)
			{
				return UITooltip.Corner.TopLeft;
			}
			if (pivot.x == 1f && pivot.y == 0f)
			{
				return UITooltip.Corner.BottomRight;
			}
			return UITooltip.Corner.TopRight;
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x000AFD04 File Offset: 0x000ADF04
		public static UITooltip.Anchor VectorPivotToAnchor(Vector2 pivot)
		{
			if (pivot.x == 0f && pivot.y == 0f)
			{
				return UITooltip.Anchor.BottomLeft;
			}
			if (pivot.x == 0f && pivot.y == 1f)
			{
				return UITooltip.Anchor.TopLeft;
			}
			if (pivot.x == 1f && pivot.y == 0f)
			{
				return UITooltip.Anchor.BottomRight;
			}
			if (pivot.x == 0.5f && pivot.y == 0f)
			{
				return UITooltip.Anchor.Bottom;
			}
			if (pivot.x == 0.5f && pivot.y == 1f)
			{
				return UITooltip.Anchor.Top;
			}
			if (pivot.x == 0f && pivot.y == 0.5f)
			{
				return UITooltip.Anchor.Left;
			}
			if (pivot.x == 1f && pivot.y == 0.5f)
			{
				return UITooltip.Anchor.Right;
			}
			return UITooltip.Anchor.TopRight;
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x000AFDD6 File Offset: 0x000ADFD6
		public static UITooltip.Corner GetOppositeCorner(UITooltip.Corner corner)
		{
			switch (corner)
			{
			case UITooltip.Corner.BottomLeft:
				return UITooltip.Corner.TopRight;
			case UITooltip.Corner.TopLeft:
				return UITooltip.Corner.BottomRight;
			case UITooltip.Corner.TopRight:
				return UITooltip.Corner.BottomLeft;
			case UITooltip.Corner.BottomRight:
				return UITooltip.Corner.TopLeft;
			default:
				return UITooltip.Corner.BottomLeft;
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x000AFDF9 File Offset: 0x000ADFF9
		public static UITooltip.Anchor GetOppositeAnchor(UITooltip.Anchor anchor)
		{
			switch (anchor)
			{
			case UITooltip.Anchor.BottomLeft:
				return UITooltip.Anchor.TopRight;
			case UITooltip.Anchor.BottomRight:
				return UITooltip.Anchor.TopLeft;
			case UITooltip.Anchor.TopLeft:
				return UITooltip.Anchor.BottomRight;
			case UITooltip.Anchor.TopRight:
				return UITooltip.Anchor.BottomLeft;
			case UITooltip.Anchor.Left:
				return UITooltip.Anchor.Right;
			case UITooltip.Anchor.Right:
				return UITooltip.Anchor.Left;
			case UITooltip.Anchor.Top:
				return UITooltip.Anchor.Bottom;
			case UITooltip.Anchor.Bottom:
				return UITooltip.Anchor.Top;
			default:
				return UITooltip.Anchor.BottomLeft;
			}
		}

		// Token: 0x04001CAB RID: 7339
		private static UITooltip mInstance;

		// Token: 0x04001CAC RID: 7340
		public const ContentSizeFitter.FitMode DefaultHorizontalFitMode = ContentSizeFitter.FitMode.Unconstrained;

		// Token: 0x04001CAD RID: 7341
		[SerializeField]
		[Tooltip("Used when no width is specified for the current tooltip display.")]
		private float m_DefaultWidth = 257f;

		// Token: 0x04001CAE RID: 7342
		[SerializeField]
		[Tooltip("Should the tooltip follow the mouse movement or anchor to the position where it was called.")]
		private bool m_followMouse;

		// Token: 0x04001CAF RID: 7343
		[SerializeField]
		[Tooltip("Tooltip offset from the pointer when not anchored to a rect.")]
		private Vector2 m_Offset = Vector2.zero;

		// Token: 0x04001CB0 RID: 7344
		[SerializeField]
		private UITooltip.Anchoring m_Anchoring;

		// Token: 0x04001CB1 RID: 7345
		[SerializeField]
		[Tooltip("Tooltip offset when anchored to a rect.")]
		private Vector2 m_AnchoredOffset = Vector2.zero;

		// Token: 0x04001CB2 RID: 7346
		[SerializeField]
		private Graphic m_AnchorGraphic;

		// Token: 0x04001CB3 RID: 7347
		[SerializeField]
		private Vector2 m_AnchorGraphicOffset = Vector2.zero;

		// Token: 0x04001CB4 RID: 7348
		[SerializeField]
		private UITooltip.Transition m_Transition;

		// Token: 0x04001CB5 RID: 7349
		[SerializeField]
		private TweenEasing m_TransitionEasing;

		// Token: 0x04001CB6 RID: 7350
		[SerializeField]
		private float m_TransitionDuration = 0.1f;

		// Token: 0x04001CB7 RID: 7351
		public UITooltip.AnchorEvent onAnchorEvent = new UITooltip.AnchorEvent();

		// Token: 0x04001CB8 RID: 7352
		private RectTransform m_Rect;

		// Token: 0x04001CB9 RID: 7353
		private CanvasGroup m_CanvasGroup;

		// Token: 0x04001CBA RID: 7354
		private ContentSizeFitter m_SizeFitter;

		// Token: 0x04001CBB RID: 7355
		private Canvas m_Canvas;

		// Token: 0x04001CBC RID: 7356
		private UITooltip.VisualState m_VisualState;

		// Token: 0x04001CBD RID: 7357
		private RectTransform m_AnchorToTarget;

		// Token: 0x04001CBE RID: 7358
		private UITooltip.Anchor m_CurrentAnchor;

		// Token: 0x04001CBF RID: 7359
		private UITooltipLines m_LinesTemplate;

		// Token: 0x04001CC0 RID: 7360
		private Vector2 m_OriginalOffset = Vector2.zero;

		// Token: 0x04001CC1 RID: 7361
		private Vector2 m_OriginalAnchoredOffset = Vector2.zero;

		// Token: 0x04001CC2 RID: 7362
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x02000647 RID: 1607
		public enum Transition
		{
			// Token: 0x04001CC4 RID: 7364
			None,
			// Token: 0x04001CC5 RID: 7365
			Fade
		}

		// Token: 0x02000648 RID: 1608
		public enum VisualState
		{
			// Token: 0x04001CC7 RID: 7367
			Shown,
			// Token: 0x04001CC8 RID: 7368
			Hidden
		}

		// Token: 0x02000649 RID: 1609
		public enum Corner
		{
			// Token: 0x04001CCA RID: 7370
			BottomLeft,
			// Token: 0x04001CCB RID: 7371
			TopLeft,
			// Token: 0x04001CCC RID: 7372
			TopRight,
			// Token: 0x04001CCD RID: 7373
			BottomRight
		}

		// Token: 0x0200064A RID: 1610
		public enum Anchoring
		{
			// Token: 0x04001CCF RID: 7375
			Corners,
			// Token: 0x04001CD0 RID: 7376
			LeftOrRight,
			// Token: 0x04001CD1 RID: 7377
			TopOrBottom
		}

		// Token: 0x0200064B RID: 1611
		public enum Anchor
		{
			// Token: 0x04001CD3 RID: 7379
			BottomLeft,
			// Token: 0x04001CD4 RID: 7380
			BottomRight,
			// Token: 0x04001CD5 RID: 7381
			TopLeft,
			// Token: 0x04001CD6 RID: 7382
			TopRight,
			// Token: 0x04001CD7 RID: 7383
			Left,
			// Token: 0x04001CD8 RID: 7384
			Right,
			// Token: 0x04001CD9 RID: 7385
			Top,
			// Token: 0x04001CDA RID: 7386
			Bottom
		}

		// Token: 0x0200064C RID: 1612
		[Serializable]
		public class AnchorEvent : UnityEvent<UITooltip.Anchor>
		{
		}
	}
}
