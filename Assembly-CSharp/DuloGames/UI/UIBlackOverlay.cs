using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI
{
	// Token: 0x0200066F RID: 1647
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasGroup))]
	public class UIBlackOverlay : MonoBehaviour
	{
		// Token: 0x06002488 RID: 9352 RVA: 0x000B2B68 File Offset: 0x000B0D68
		protected UIBlackOverlay()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002489 RID: 9353 RVA: 0x000B2B8F File Offset: 0x000B0D8F
		protected void Awake()
		{
			this.m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
			this.m_CanvasGroup.interactable = false;
			this.m_CanvasGroup.blocksRaycasts = false;
		}

		// Token: 0x0600248A RID: 9354 RVA: 0x000B2BBA File Offset: 0x000B0DBA
		protected void Start()
		{
			this.m_CanvasGroup.interactable = false;
			this.Hide();
		}

		// Token: 0x0600248B RID: 9355 RVA: 0x000B2BCE File Offset: 0x000B0DCE
		protected void OnEnable()
		{
			if (!Application.isPlaying)
			{
				this.Hide();
			}
		}

		// Token: 0x0600248C RID: 9356 RVA: 0x000B2BDD File Offset: 0x000B0DDD
		public void Show()
		{
			this.SetAlpha(1f);
			this.m_CanvasGroup.blocksRaycasts = true;
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000B2BF6 File Offset: 0x000B0DF6
		public void Hide()
		{
			this.SetAlpha(0f);
			this.m_CanvasGroup.blocksRaycasts = false;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000A2602 File Offset: 0x000A0802
		public bool IsActive()
		{
			return base.enabled && base.gameObject.activeInHierarchy;
		}

		// Token: 0x0600248F RID: 9359 RVA: 0x000B2C0F File Offset: 0x000B0E0F
		public bool IsVisible()
		{
			return this.m_CanvasGroup.alpha > 0f;
		}

		// Token: 0x06002490 RID: 9360 RVA: 0x000B2C24 File Offset: 0x000B0E24
		public void OnTransitionBegin(UIWindow window, UIWindow.VisualState state, bool instant)
		{
			if (!this.IsActive() || window == null)
			{
				return;
			}
			if (state == UIWindow.VisualState.Hidden && !this.IsVisible())
			{
				return;
			}
			float duration = instant ? 0f : window.transitionDuration;
			TweenEasing transitionEasing = window.transitionEasing;
			if (state == UIWindow.VisualState.Shown)
			{
				this.m_WindowsCount++;
				if (this.IsVisible() && !this.m_Transitioning)
				{
					UIUtility.BringToFront(window.gameObject);
					return;
				}
				UIUtility.BringToFront(base.gameObject);
				UIUtility.BringToFront(window.gameObject);
				this.StartAlphaTween(1f, duration, transitionEasing);
				this.m_CanvasGroup.blocksRaycasts = true;
				return;
			}
			else
			{
				this.m_WindowsCount--;
				if (this.m_WindowsCount < 0)
				{
					this.m_WindowsCount = 0;
				}
				if (this.m_WindowsCount > 0)
				{
					return;
				}
				this.StartAlphaTween(0f, duration, transitionEasing);
				this.m_CanvasGroup.blocksRaycasts = false;
				return;
			}
		}

		// Token: 0x06002491 RID: 9361 RVA: 0x000B2D08 File Offset: 0x000B0F08
		private void StartAlphaTween(float targetAlpha, float duration, TweenEasing easing)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			if (this.m_Transitioning)
			{
				this.m_FloatTweenRunner.StopTween();
			}
			if (duration == 0f || !Application.isPlaying)
			{
				this.SetAlpha(targetAlpha);
				return;
			}
			this.m_Transitioning = true;
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = this.m_CanvasGroup.alpha,
				targetFloat = targetAlpha
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetAlpha));
			info.ignoreTimeScale = true;
			info.easing = easing;
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000B2DC3 File Offset: 0x000B0FC3
		public void SetAlpha(float alpha)
		{
			if (this.m_CanvasGroup != null)
			{
				this.m_CanvasGroup.alpha = alpha;
			}
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000B2DDF File Offset: 0x000B0FDF
		protected void OnTweenFinished()
		{
			this.m_Transitioning = false;
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000B2DE8 File Offset: 0x000B0FE8
		public static UIBlackOverlay GetOverlay(GameObject relativeGameObject)
		{
			Canvas canvas = UIUtility.FindInParents<Canvas>(relativeGameObject);
			if (canvas != null)
			{
				UIBlackOverlay componentInChildren = canvas.gameObject.GetComponentInChildren<UIBlackOverlay>();
				if (componentInChildren != null)
				{
					return componentInChildren;
				}
				if (UIBlackOverlayManager.Instance != null && UIBlackOverlayManager.Instance.prefab != null)
				{
					return UIBlackOverlayManager.Instance.Create(canvas.transform);
				}
			}
			return null;
		}

		// Token: 0x04001DB9 RID: 7609
		private CanvasGroup m_CanvasGroup;

		// Token: 0x04001DBA RID: 7610
		private int m_WindowsCount;

		// Token: 0x04001DBB RID: 7611
		private bool m_Transitioning;

		// Token: 0x04001DBC RID: 7612
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
