using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005C4 RID: 1476
	[AddComponentMenu("UI/Bars/Progress Bar")]
	public class UIProgressBar : MonoBehaviour, IUIProgressBar
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06002068 RID: 8296 RVA: 0x000A224F File Offset: 0x000A044F
		// (set) Token: 0x06002069 RID: 8297 RVA: 0x000A2257 File Offset: 0x000A0457
		public UIProgressBar.Type type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x0600206A RID: 8298 RVA: 0x000A2260 File Offset: 0x000A0460
		// (set) Token: 0x0600206B RID: 8299 RVA: 0x000A2268 File Offset: 0x000A0468
		public Image targetImage
		{
			get
			{
				return this.m_TargetImage;
			}
			set
			{
				this.m_TargetImage = value;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x0600206C RID: 8300 RVA: 0x000A2271 File Offset: 0x000A0471
		// (set) Token: 0x0600206D RID: 8301 RVA: 0x000A2279 File Offset: 0x000A0479
		public Sprite[] sprites
		{
			get
			{
				return this.m_Sprites;
			}
			set
			{
				this.m_Sprites = value;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x0600206E RID: 8302 RVA: 0x000A2282 File Offset: 0x000A0482
		// (set) Token: 0x0600206F RID: 8303 RVA: 0x000A228A File Offset: 0x000A048A
		public RectTransform targetTransform
		{
			get
			{
				return this.m_TargetTransform;
			}
			set
			{
				this.m_TargetTransform = value;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06002070 RID: 8304 RVA: 0x000A2293 File Offset: 0x000A0493
		// (set) Token: 0x06002071 RID: 8305 RVA: 0x000A229B File Offset: 0x000A049B
		public float minWidth
		{
			get
			{
				return this.m_MinWidth;
			}
			set
			{
				this.m_MinWidth = value;
				this.UpdateBarFill();
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06002072 RID: 8306 RVA: 0x000A22AA File Offset: 0x000A04AA
		// (set) Token: 0x06002073 RID: 8307 RVA: 0x000A22B2 File Offset: 0x000A04B2
		public float maxWidth
		{
			get
			{
				return this.m_MaxWidth;
			}
			set
			{
				this.m_MaxWidth = value;
				this.UpdateBarFill();
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06002074 RID: 8308 RVA: 0x000A22C1 File Offset: 0x000A04C1
		// (set) Token: 0x06002075 RID: 8309 RVA: 0x000A22C9 File Offset: 0x000A04C9
		public float fillAmount
		{
			get
			{
				return this.m_FillAmount;
			}
			set
			{
				if (this.m_FillAmount != Mathf.Clamp01(value))
				{
					this.m_FillAmount = Mathf.Clamp01(value);
					this.UpdateBarFill();
					this.onChange.Invoke(this.m_FillAmount);
				}
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06002076 RID: 8310 RVA: 0x000A22FC File Offset: 0x000A04FC
		// (set) Token: 0x06002077 RID: 8311 RVA: 0x000A2304 File Offset: 0x000A0504
		public int steps
		{
			get
			{
				return this.m_Steps;
			}
			set
			{
				this.m_Steps = value;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x000A2310 File Offset: 0x000A0510
		// (set) Token: 0x06002079 RID: 8313 RVA: 0x000A2344 File Offset: 0x000A0544
		public int currentStep
		{
			get
			{
				if (this.m_Steps == 0)
				{
					return 0;
				}
				float num = 1f / (float)(this.m_Steps - 1);
				return Mathf.RoundToInt(this.fillAmount / num);
			}
			set
			{
				if (this.m_Steps > 0)
				{
					float num = 1f / (float)(this.m_Steps - 1);
					this.fillAmount = (float)Mathf.Clamp(value, 0, this.m_Steps) * num;
				}
			}
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x000A2380 File Offset: 0x000A0580
		protected virtual void Start()
		{
			if (this.m_Type == UIProgressBar.Type.Resize && this.m_FillSizing == UIProgressBar.FillSizing.Parent && this.m_TargetTransform != null)
			{
				float height = this.m_TargetTransform.rect.height;
				this.m_TargetTransform.anchorMin = this.m_TargetTransform.pivot;
				this.m_TargetTransform.anchorMax = this.m_TargetTransform.pivot;
				this.m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			}
			this.UpdateBarFill();
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x000A23FF File Offset: 0x000A05FF
		protected virtual void OnRectTransformDimensionsChange()
		{
			this.UpdateBarFill();
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x000A2408 File Offset: 0x000A0608
		public void UpdateBarFill()
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.m_Type == UIProgressBar.Type.Filled && this.m_TargetImage == null)
			{
				return;
			}
			if (this.m_Type == UIProgressBar.Type.Resize && this.m_TargetTransform == null)
			{
				return;
			}
			if (this.m_Type == UIProgressBar.Type.Sprites && this.m_Sprites.Length == 0)
			{
				return;
			}
			float num = this.m_FillAmount;
			if (this.m_Steps > 0)
			{
				num = Mathf.Round(this.m_FillAmount * (float)(this.m_Steps - 1)) / (float)(this.m_Steps - 1);
			}
			if (this.m_Type == UIProgressBar.Type.Resize)
			{
				if (this.m_FillSizing == UIProgressBar.FillSizing.Fixed)
				{
					this.m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.m_MinWidth + (this.m_MaxWidth - this.m_MinWidth) * num);
					return;
				}
				this.m_TargetTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (this.m_TargetTransform.parent as RectTransform).rect.width * num);
				return;
			}
			else
			{
				if (this.m_Type != UIProgressBar.Type.Sprites)
				{
					this.m_TargetImage.fillAmount = num;
					return;
				}
				int num2 = Mathf.RoundToInt(num * (float)this.m_Sprites.Length) - 1;
				if (num2 > -1)
				{
					this.targetImage.overrideSprite = this.m_Sprites[num2];
					this.targetImage.canvasRenderer.SetAlpha(1f);
					return;
				}
				this.targetImage.overrideSprite = null;
				this.targetImage.canvasRenderer.SetAlpha(0f);
				return;
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x000A2568 File Offset: 0x000A0768
		public void AddFill()
		{
			if (this.m_Steps > 0)
			{
				this.currentStep++;
				return;
			}
			this.fillAmount += 0.1f;
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x000A2594 File Offset: 0x000A0794
		public void RemoveFill()
		{
			if (this.m_Steps > 0)
			{
				this.currentStep--;
				return;
			}
			this.fillAmount -= 0.1f;
		}

		// Token: 0x04001A46 RID: 6726
		[SerializeField]
		private UIProgressBar.Type m_Type;

		// Token: 0x04001A47 RID: 6727
		[SerializeField]
		private Image m_TargetImage;

		// Token: 0x04001A48 RID: 6728
		[SerializeField]
		private Sprite[] m_Sprites;

		// Token: 0x04001A49 RID: 6729
		[SerializeField]
		private RectTransform m_TargetTransform;

		// Token: 0x04001A4A RID: 6730
		[SerializeField]
		private UIProgressBar.FillSizing m_FillSizing;

		// Token: 0x04001A4B RID: 6731
		[SerializeField]
		private float m_MinWidth;

		// Token: 0x04001A4C RID: 6732
		[SerializeField]
		private float m_MaxWidth = 100f;

		// Token: 0x04001A4D RID: 6733
		[SerializeField]
		[Range(0f, 1f)]
		private float m_FillAmount = 1f;

		// Token: 0x04001A4E RID: 6734
		[SerializeField]
		private int m_Steps;

		// Token: 0x04001A4F RID: 6735
		public UIProgressBar.ChangeEvent onChange = new UIProgressBar.ChangeEvent();

		// Token: 0x020005C5 RID: 1477
		[Serializable]
		public class ChangeEvent : UnityEvent<float>
		{
		}

		// Token: 0x020005C6 RID: 1478
		public enum Type
		{
			// Token: 0x04001A51 RID: 6737
			Filled,
			// Token: 0x04001A52 RID: 6738
			Resize,
			// Token: 0x04001A53 RID: 6739
			Sprites
		}

		// Token: 0x020005C7 RID: 1479
		public enum FillSizing
		{
			// Token: 0x04001A55 RID: 6741
			Parent,
			// Token: 0x04001A56 RID: 6742
			Fixed
		}
	}
}
