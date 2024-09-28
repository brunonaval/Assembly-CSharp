using System;
using System.Collections.Generic;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x02000672 RID: 1650
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Window", 58)]
	[RequireComponent(typeof(CanvasGroup))]
	public class UIWindow : MonoBehaviour, IEventSystemHandler, ISelectHandler, IPointerDownHandler
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000B2ED4 File Offset: 0x000B10D4
		// (set) Token: 0x0600249C RID: 9372 RVA: 0x000B2EDB File Offset: 0x000B10DB
		public static UIWindow FocusedWindow
		{
			get
			{
				return UIWindow.m_FucusedWindow;
			}
			private set
			{
				UIWindow.m_FucusedWindow = value;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000B2EE3 File Offset: 0x000B10E3
		// (set) Token: 0x0600249E RID: 9374 RVA: 0x000B2EEB File Offset: 0x000B10EB
		public UIWindowID ID
		{
			get
			{
				return this.m_WindowId;
			}
			set
			{
				this.m_WindowId = value;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000B2EF4 File Offset: 0x000B10F4
		// (set) Token: 0x060024A0 RID: 9376 RVA: 0x000B2EFC File Offset: 0x000B10FC
		public int CustomID
		{
			get
			{
				return this.m_CustomWindowId;
			}
			set
			{
				this.m_CustomWindowId = value;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x000B2F05 File Offset: 0x000B1105
		// (set) Token: 0x060024A2 RID: 9378 RVA: 0x000B2F0D File Offset: 0x000B110D
		public UIWindow.EscapeKeyAction escapeKeyAction
		{
			get
			{
				return this.m_EscapeKeyAction;
			}
			set
			{
				this.m_EscapeKeyAction = value;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x000B2F16 File Offset: 0x000B1116
		// (set) Token: 0x060024A4 RID: 9380 RVA: 0x000B2F20 File Offset: 0x000B1120
		public bool useBlackOverlay
		{
			get
			{
				return this.m_UseBlackOverlay;
			}
			set
			{
				this.m_UseBlackOverlay = value;
				if (Application.isPlaying && this.m_UseBlackOverlay && base.isActiveAndEnabled)
				{
					UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(base.gameObject);
					if (overlay != null)
					{
						if (value)
						{
							this.onTransitionBegin.AddListener(new UnityAction<UIWindow, UIWindow.VisualState, bool>(overlay.OnTransitionBegin));
							return;
						}
						this.onTransitionBegin.RemoveListener(new UnityAction<UIWindow, UIWindow.VisualState, bool>(overlay.OnTransitionBegin));
					}
				}
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x000B2F92 File Offset: 0x000B1192
		// (set) Token: 0x060024A6 RID: 9382 RVA: 0x000B2F9A File Offset: 0x000B119A
		public bool focusAllowReparent
		{
			get
			{
				return this.m_FocusAllowReparent;
			}
			set
			{
				this.m_FocusAllowReparent = value;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x000B2FA3 File Offset: 0x000B11A3
		// (set) Token: 0x060024A8 RID: 9384 RVA: 0x000B2FAB File Offset: 0x000B11AB
		public UIWindow.Transition transition
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

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x000B2FB4 File Offset: 0x000B11B4
		// (set) Token: 0x060024AA RID: 9386 RVA: 0x000B2FBC File Offset: 0x000B11BC
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

		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x000B2FC5 File Offset: 0x000B11C5
		// (set) Token: 0x060024AC RID: 9388 RVA: 0x000B2FCD File Offset: 0x000B11CD
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

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x000B2FD6 File Offset: 0x000B11D6
		public bool IsVisible
		{
			get
			{
				return this.m_CanvasGroup != null && this.m_CanvasGroup.alpha > 0f;
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x000B2FFB File Offset: 0x000B11FB
		public bool IsOpen
		{
			get
			{
				return this.m_CurrentVisualState == UIWindow.VisualState.Shown;
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060024AF RID: 9391 RVA: 0x000B3006 File Offset: 0x000B1206
		public bool IsFocused
		{
			get
			{
				return this.m_IsFocused;
			}
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x000B3010 File Offset: 0x000B1210
		protected UIWindow()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000B3087 File Offset: 0x000B1287
		protected virtual void Awake()
		{
			this.m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
			if (Application.isPlaying)
			{
				this.ApplyVisualState(this.m_StartingState);
			}
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000B30B0 File Offset: 0x000B12B0
		protected virtual void Start()
		{
			if (this.CustomID == 0)
			{
				this.CustomID = UIWindow.NextUnusedCustomID;
			}
			if (this.m_EscapeKeyAction != UIWindow.EscapeKeyAction.None && UnityEngine.Object.FindObjectOfType<UIWindowManager>() == null)
			{
				GameObject gameObject = new GameObject("Window Manager");
				gameObject.AddComponent<UIWindowManager>();
				gameObject.transform.SetAsFirstSibling();
			}
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000B3100 File Offset: 0x000B1300
		protected virtual void OnEnable()
		{
			if (Application.isPlaying && this.m_UseBlackOverlay)
			{
				UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(base.gameObject);
				if (overlay != null)
				{
					this.onTransitionBegin.AddListener(new UnityAction<UIWindow, UIWindow.VisualState, bool>(overlay.OnTransitionBegin));
				}
			}
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000B3148 File Offset: 0x000B1348
		protected virtual void OnDisable()
		{
			if (Application.isPlaying && this.m_UseBlackOverlay)
			{
				UIBlackOverlay overlay = UIBlackOverlay.GetOverlay(base.gameObject);
				if (overlay != null)
				{
					this.onTransitionBegin.RemoveListener(new UnityAction<UIWindow, UIWindow.VisualState, bool>(overlay.OnTransitionBegin));
				}
			}
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000A2602 File Offset: 0x000A0802
		protected virtual bool IsActive()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000B3190 File Offset: 0x000B1390
		public virtual void OnSelect(BaseEventData eventData)
		{
			this.Focus();
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000B3190 File Offset: 0x000B1390
		public virtual void OnPointerDown(PointerEventData eventData)
		{
			this.Focus();
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000B3198 File Offset: 0x000B1398
		public virtual void Focus()
		{
			if (this.m_IsFocused)
			{
				return;
			}
			this.m_IsFocused = true;
			UIWindow.OnBeforeFocusWindow(this);
			this.BringToFront();
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x000B31B6 File Offset: 0x000B13B6
		public void BringToFront()
		{
			UIUtility.BringToFront(base.gameObject, this.m_FocusAllowReparent);
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000B31C9 File Offset: 0x000B13C9
		public virtual void Toggle()
		{
			if (this.m_CurrentVisualState == UIWindow.VisualState.Shown)
			{
				this.Hide();
				return;
			}
			this.Show();
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000B31E0 File Offset: 0x000B13E0
		public virtual void Show()
		{
			this.Show(false);
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000B31E9 File Offset: 0x000B13E9
		public virtual void Show(bool instant)
		{
			if (!this.IsActive())
			{
				return;
			}
			this.Focus();
			if (this.m_CurrentVisualState == UIWindow.VisualState.Shown)
			{
				return;
			}
			this.EvaluateAndTransitionToVisualState(UIWindow.VisualState.Shown, instant);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000B320B File Offset: 0x000B140B
		public virtual void Hide()
		{
			this.Hide(false);
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000B3214 File Offset: 0x000B1414
		public virtual void Hide(bool instant)
		{
			if (!this.IsActive())
			{
				return;
			}
			if (this.m_CurrentVisualState == UIWindow.VisualState.Hidden)
			{
				return;
			}
			this.EvaluateAndTransitionToVisualState(UIWindow.VisualState.Hidden, instant);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000B3234 File Offset: 0x000B1434
		protected virtual void EvaluateAndTransitionToVisualState(UIWindow.VisualState state, bool instant)
		{
			float num = (state == UIWindow.VisualState.Shown) ? 1f : 0f;
			if (this.onTransitionBegin != null)
			{
				this.onTransitionBegin.Invoke(this, state, instant || this.m_Transition == UIWindow.Transition.Instant);
			}
			if (this.m_Transition == UIWindow.Transition.Fade)
			{
				float duration = instant ? 0f : this.m_TransitionDuration;
				this.StartAlphaTween(num, duration, true);
			}
			else
			{
				this.SetCanvasAlpha(num);
				if (this.onTransitionComplete != null)
				{
					this.onTransitionComplete.Invoke(this, state);
				}
			}
			this.m_CurrentVisualState = state;
			if (state == UIWindow.VisualState.Shown)
			{
				this.m_CanvasGroup.blocksRaycasts = true;
			}
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x000B32CC File Offset: 0x000B14CC
		public virtual void ApplyVisualState(UIWindow.VisualState state)
		{
			float canvasAlpha = (state == UIWindow.VisualState.Shown) ? 1f : 0f;
			this.SetCanvasAlpha(canvasAlpha);
			this.m_CurrentVisualState = state;
			if (state == UIWindow.VisualState.Shown)
			{
				this.m_CanvasGroup.blocksRaycasts = true;
				return;
			}
			this.m_CanvasGroup.blocksRaycasts = false;
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000B3314 File Offset: 0x000B1514
		public void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = this.m_CanvasGroup.alpha,
				targetFloat = targetAlpha
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetCanvasAlpha));
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = ignoreTimeScale;
			info.easing = this.m_TransitionEasing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000B33A4 File Offset: 0x000B15A4
		public void SetCanvasAlpha(float alpha)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			this.m_CanvasGroup.alpha = alpha;
			if (alpha == 0f)
			{
				this.m_CanvasGroup.blocksRaycasts = false;
			}
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000B33D5 File Offset: 0x000B15D5
		protected virtual void OnTweenFinished()
		{
			if (this.onTransitionComplete != null)
			{
				this.onTransitionComplete.Invoke(this, this.m_CurrentVisualState);
			}
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x000B33F4 File Offset: 0x000B15F4
		public static List<UIWindow> GetWindows()
		{
			List<UIWindow> list = new List<UIWindow>();
			foreach (UIWindow uiwindow in Resources.FindObjectsOfTypeAll<UIWindow>())
			{
				if (uiwindow.gameObject.activeInHierarchy)
				{
					list.Add(uiwindow);
				}
			}
			return list;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000B3434 File Offset: 0x000B1634
		public static int SortByCustomWindowID(UIWindow w1, UIWindow w2)
		{
			return w1.CustomID.CompareTo(w2.CustomID);
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x000B3458 File Offset: 0x000B1658
		public static int NextUnusedCustomID
		{
			get
			{
				List<UIWindow> windows = UIWindow.GetWindows();
				if (UIWindow.GetWindows().Count > 0)
				{
					windows.Sort(new Comparison<UIWindow>(UIWindow.SortByCustomWindowID));
					return windows[windows.Count - 1].CustomID + 1;
				}
				return 0;
			}
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000B34A4 File Offset: 0x000B16A4
		public static UIWindow GetWindow(UIWindowID id)
		{
			foreach (UIWindow uiwindow in UIWindow.GetWindows())
			{
				if (uiwindow.ID == id)
				{
					return uiwindow;
				}
			}
			return null;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x000B3500 File Offset: 0x000B1700
		public static UIWindow GetWindowByCustomID(int customId)
		{
			foreach (UIWindow uiwindow in UIWindow.GetWindows())
			{
				if (uiwindow.CustomID == customId)
				{
					return uiwindow;
				}
			}
			return null;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x000B355C File Offset: 0x000B175C
		public static void FocusWindow(UIWindowID id)
		{
			if (UIWindow.GetWindow(id) != null)
			{
				UIWindow.GetWindow(id).Focus();
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x000B3577 File Offset: 0x000B1777
		protected static void OnBeforeFocusWindow(UIWindow window)
		{
			if (UIWindow.m_FucusedWindow != null)
			{
				UIWindow.m_FucusedWindow.m_IsFocused = false;
			}
			UIWindow.m_FucusedWindow = window;
		}

		// Token: 0x04001DC1 RID: 7617
		protected static UIWindow m_FucusedWindow;

		// Token: 0x04001DC2 RID: 7618
		[SerializeField]
		private UIWindowID m_WindowId;

		// Token: 0x04001DC3 RID: 7619
		[SerializeField]
		private int m_CustomWindowId;

		// Token: 0x04001DC4 RID: 7620
		[SerializeField]
		private UIWindow.VisualState m_StartingState = UIWindow.VisualState.Hidden;

		// Token: 0x04001DC5 RID: 7621
		[SerializeField]
		private UIWindow.EscapeKeyAction m_EscapeKeyAction = UIWindow.EscapeKeyAction.Hide;

		// Token: 0x04001DC6 RID: 7622
		[SerializeField]
		private bool m_UseBlackOverlay;

		// Token: 0x04001DC7 RID: 7623
		[SerializeField]
		private bool m_FocusAllowReparent = true;

		// Token: 0x04001DC8 RID: 7624
		[SerializeField]
		private UIWindow.Transition m_Transition;

		// Token: 0x04001DC9 RID: 7625
		[SerializeField]
		private TweenEasing m_TransitionEasing = TweenEasing.InOutQuint;

		// Token: 0x04001DCA RID: 7626
		[SerializeField]
		private float m_TransitionDuration = 0.1f;

		// Token: 0x04001DCB RID: 7627
		protected bool m_IsFocused;

		// Token: 0x04001DCC RID: 7628
		private UIWindow.VisualState m_CurrentVisualState = UIWindow.VisualState.Hidden;

		// Token: 0x04001DCD RID: 7629
		private CanvasGroup m_CanvasGroup;

		// Token: 0x04001DCE RID: 7630
		public UIWindow.TransitionBeginEvent onTransitionBegin = new UIWindow.TransitionBeginEvent();

		// Token: 0x04001DCF RID: 7631
		public UIWindow.TransitionCompleteEvent onTransitionComplete = new UIWindow.TransitionCompleteEvent();

		// Token: 0x04001DD0 RID: 7632
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x02000673 RID: 1651
		public enum Transition
		{
			// Token: 0x04001DD2 RID: 7634
			Instant,
			// Token: 0x04001DD3 RID: 7635
			Fade
		}

		// Token: 0x02000674 RID: 1652
		public enum VisualState
		{
			// Token: 0x04001DD5 RID: 7637
			Shown,
			// Token: 0x04001DD6 RID: 7638
			Hidden
		}

		// Token: 0x02000675 RID: 1653
		public enum EscapeKeyAction
		{
			// Token: 0x04001DD8 RID: 7640
			None,
			// Token: 0x04001DD9 RID: 7641
			Hide,
			// Token: 0x04001DDA RID: 7642
			HideIfFocused,
			// Token: 0x04001DDB RID: 7643
			Toggle
		}

		// Token: 0x02000676 RID: 1654
		[Serializable]
		public class TransitionBeginEvent : UnityEvent<UIWindow, UIWindow.VisualState, bool>
		{
		}

		// Token: 0x02000677 RID: 1655
		[Serializable]
		public class TransitionCompleteEvent : UnityEvent<UIWindow, UIWindow.VisualState>
		{
		}
	}
}
