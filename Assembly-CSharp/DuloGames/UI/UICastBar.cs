using System;
using System.Collections;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005BE RID: 1470
	[ExecuteInEditMode]
	[RequireComponent(typeof(CanvasGroup))]
	public class UICastBar : MonoBehaviour
	{
		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06002048 RID: 8264 RVA: 0x000A1B8B File Offset: 0x0009FD8B
		public bool IsCasting
		{
			get
			{
				return this.m_IsCasting;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06002049 RID: 8265 RVA: 0x000A1B93 File Offset: 0x0009FD93
		public CanvasGroup canvasGroup
		{
			get
			{
				return this.m_CanvasGroup;
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000A1B9C File Offset: 0x0009FD9C
		protected UICastBar()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000A1C3B File Offset: 0x0009FE3B
		protected virtual void Awake()
		{
			this.m_CanvasGroup = base.gameObject.GetComponent<CanvasGroup>();
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000A1C4E File Offset: 0x0009FE4E
		protected virtual void Start()
		{
			if (base.isActiveAndEnabled)
			{
				this.ApplyColorStage(this.m_NormalColors);
				if (Application.isPlaying)
				{
					if (this.m_IconFrame != null)
					{
						this.m_IconFrame.SetActive(false);
					}
					this.Hide(true);
				}
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000A1C8C File Offset: 0x0009FE8C
		public virtual void ApplyColorStage(UICastBar.ColorStage stage)
		{
			if (this.m_FillImage != null)
			{
				this.m_FillImage.canvasRenderer.SetColor(stage.fillColor);
			}
			if (this.m_TitleLabel != null)
			{
				this.m_TitleLabel.canvasRenderer.SetColor(stage.titleColor);
			}
			if (this.m_TimeLabel != null)
			{
				this.m_TimeLabel.canvasRenderer.SetColor(stage.timeColor);
			}
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000A1D05 File Offset: 0x0009FF05
		public void Show()
		{
			this.Show(false);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000A1D10 File Offset: 0x0009FF10
		public virtual void Show(bool instant)
		{
			if (this.m_BrindToFront)
			{
				UIUtility.BringToFront(base.gameObject);
			}
			if (instant || this.m_InTransition == UICastBar.Transition.Instant)
			{
				this.m_CanvasGroup.alpha = 1f;
				return;
			}
			this.StartAlphaTween(1f, this.m_InTransitionDuration, true);
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000A1D5E File Offset: 0x0009FF5E
		public void Hide()
		{
			this.Hide(false);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000A1D67 File Offset: 0x0009FF67
		public virtual void Hide(bool instant)
		{
			if (instant || this.m_OutTransition == UICastBar.Transition.Instant)
			{
				this.m_CanvasGroup.alpha = 0f;
				this.OnHideTweenFinished();
				return;
			}
			this.StartAlphaTween(0f, this.m_OutTransitionDuration, true);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000A1DA0 File Offset: 0x0009FFA0
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
			info.AddOnFinishCallback(new UnityAction(this.OnHideTweenFinished));
			info.ignoreTimeScale = ignoreTimeScale;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000A1E23 File Offset: 0x000A0023
		protected void SetCanvasAlpha(float alpha)
		{
			if (this.m_CanvasGroup == null)
			{
				return;
			}
			this.m_CanvasGroup.alpha = alpha;
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000A1E40 File Offset: 0x000A0040
		protected virtual void OnHideTweenFinished()
		{
			if (this.m_IconFrame != null)
			{
				this.m_IconFrame.SetActive(false);
			}
			if (this.m_IconImage != null)
			{
				this.m_IconImage.sprite = null;
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000A1E76 File Offset: 0x000A0076
		public void SetFillAmount(float amount)
		{
			if (this.m_ProgressBar != null)
			{
				this.m_ProgressBar.fillAmount = amount;
			}
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x000A1E92 File Offset: 0x000A0092
		private IEnumerator AnimateCast()
		{
			if (this.m_TimeLabel != null)
			{
				this.m_TimeLabel.text = ((this.m_DisplayTime == UICastBar.DisplayTime.Elapsed) ? 0f.ToString(this.m_TimeFormat) : this.currentCastDuration.ToString(this.m_TimeFormat));
			}
			float startTime = (this.currentCastEndTime > 0f) ? (this.currentCastEndTime - this.currentCastDuration) : Time.time;
			while (Time.time < startTime + this.currentCastDuration)
			{
				float num = startTime + this.currentCastDuration - Time.time;
				float num2 = this.currentCastDuration - num;
				float fillAmount = num2 / this.currentCastDuration;
				if (this.m_TimeLabel != null)
				{
					this.m_TimeLabel.text = ((this.m_DisplayTime == UICastBar.DisplayTime.Elapsed) ? num2.ToString(this.m_TimeFormat) : num.ToString(this.m_TimeFormat));
				}
				this.SetFillAmount(fillAmount);
				yield return 0;
			}
			this.SetFillAmount(1f);
			if (this.m_TimeLabel != null)
			{
				this.m_TimeLabel.text = ((this.m_DisplayTime == UICastBar.DisplayTime.Elapsed) ? this.currentCastDuration.ToString(this.m_TimeFormat) : 0f.ToString(this.m_TimeFormat));
			}
			this.OnFinishedCasting();
			base.StartCoroutine("DelayHide");
			yield break;
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000A1EA1 File Offset: 0x000A00A1
		private IEnumerator DelayHide()
		{
			yield return new WaitForSeconds(this.m_HideDelay);
			this.Hide();
			yield break;
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000A1EB0 File Offset: 0x000A00B0
		public virtual void StartCasting(UISpellInfo spellInfo, float duration, float endTime)
		{
			if (this.m_IsCasting)
			{
				return;
			}
			base.StopCoroutine("AnimateCast");
			base.StopCoroutine("DelayHide");
			this.ApplyColorStage(this.m_NormalColors);
			this.SetFillAmount(0f);
			if (this.m_TitleLabel != null)
			{
				this.m_TitleLabel.text = spellInfo.Name;
			}
			if (this.m_FullTimeLabel != null)
			{
				this.m_FullTimeLabel.text = spellInfo.CastTime.ToString(this.m_FullTimeFormat);
			}
			if (this.m_UseSpellIcon && spellInfo.Icon != null)
			{
				if (this.m_IconImage != null)
				{
					this.m_IconImage.sprite = spellInfo.Icon;
				}
				if (this.m_IconFrame != null)
				{
					this.m_IconFrame.SetActive(true);
				}
			}
			this.currentCastDuration = duration;
			this.currentCastEndTime = endTime;
			this.m_IsCasting = true;
			this.Show();
			base.StartCoroutine("AnimateCast");
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000A1FB2 File Offset: 0x000A01B2
		public virtual void Interrupt()
		{
			if (this.m_IsCasting)
			{
				base.StopCoroutine("AnimateCast");
				this.m_IsCasting = false;
				this.ApplyColorStage(this.m_OnInterruptColors);
				base.StartCoroutine("DelayHide");
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000A1FE6 File Offset: 0x000A01E6
		protected void OnFinishedCasting()
		{
			this.m_IsCasting = false;
			this.ApplyColorStage(this.m_OnFinishColors);
		}

		// Token: 0x04001A1D RID: 6685
		[SerializeField]
		private UIProgressBar m_ProgressBar;

		// Token: 0x04001A1E RID: 6686
		[SerializeField]
		private Text m_TitleLabel;

		// Token: 0x04001A1F RID: 6687
		[SerializeField]
		private Text m_TimeLabel;

		// Token: 0x04001A20 RID: 6688
		[SerializeField]
		private UICastBar.DisplayTime m_DisplayTime = UICastBar.DisplayTime.Remaining;

		// Token: 0x04001A21 RID: 6689
		[SerializeField]
		private string m_TimeFormat = "0.0 sec";

		// Token: 0x04001A22 RID: 6690
		[SerializeField]
		private Text m_FullTimeLabel;

		// Token: 0x04001A23 RID: 6691
		[SerializeField]
		private string m_FullTimeFormat = "0.0 sec";

		// Token: 0x04001A24 RID: 6692
		[SerializeField]
		private bool m_UseSpellIcon;

		// Token: 0x04001A25 RID: 6693
		[SerializeField]
		private GameObject m_IconFrame;

		// Token: 0x04001A26 RID: 6694
		[SerializeField]
		private Image m_IconImage;

		// Token: 0x04001A27 RID: 6695
		[SerializeField]
		private Image m_FillImage;

		// Token: 0x04001A28 RID: 6696
		[SerializeField]
		private UICastBar.ColorStage m_NormalColors = new UICastBar.ColorStage();

		// Token: 0x04001A29 RID: 6697
		[SerializeField]
		private UICastBar.ColorStage m_OnInterruptColors = new UICastBar.ColorStage();

		// Token: 0x04001A2A RID: 6698
		[SerializeField]
		private UICastBar.ColorStage m_OnFinishColors = new UICastBar.ColorStage();

		// Token: 0x04001A2B RID: 6699
		[SerializeField]
		private UICastBar.Transition m_InTransition;

		// Token: 0x04001A2C RID: 6700
		[SerializeField]
		private float m_InTransitionDuration = 0.1f;

		// Token: 0x04001A2D RID: 6701
		[SerializeField]
		private bool m_BrindToFront = true;

		// Token: 0x04001A2E RID: 6702
		[SerializeField]
		private UICastBar.Transition m_OutTransition = UICastBar.Transition.Fade;

		// Token: 0x04001A2F RID: 6703
		[SerializeField]
		private float m_OutTransitionDuration = 0.1f;

		// Token: 0x04001A30 RID: 6704
		[SerializeField]
		private float m_HideDelay = 0.3f;

		// Token: 0x04001A31 RID: 6705
		private bool m_IsCasting;

		// Token: 0x04001A32 RID: 6706
		private float currentCastDuration;

		// Token: 0x04001A33 RID: 6707
		private float currentCastEndTime;

		// Token: 0x04001A34 RID: 6708
		private CanvasGroup m_CanvasGroup;

		// Token: 0x04001A35 RID: 6709
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x020005BF RID: 1471
		[Serializable]
		public enum DisplayTime
		{
			// Token: 0x04001A37 RID: 6711
			Elapsed,
			// Token: 0x04001A38 RID: 6712
			Remaining
		}

		// Token: 0x020005C0 RID: 1472
		[Serializable]
		public enum Transition
		{
			// Token: 0x04001A3A RID: 6714
			Instant,
			// Token: 0x04001A3B RID: 6715
			Fade
		}

		// Token: 0x020005C1 RID: 1473
		[Serializable]
		public class ColorStage
		{
			// Token: 0x04001A3C RID: 6716
			public Color fillColor = Color.white;

			// Token: 0x04001A3D RID: 6717
			public Color titleColor = Color.white;

			// Token: 0x04001A3E RID: 6718
			public Color timeColor = Color.white;
		}
	}
}
