using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005B2 RID: 1458
	public class Test_UIProgressBar : MonoBehaviour
	{
		// Token: 0x06002017 RID: 8215 RVA: 0x000A0EA0 File Offset: 0x0009F0A0
		protected Test_UIProgressBar()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x000A0EF8 File Offset: 0x0009F0F8
		protected void OnEnable()
		{
			if (this.bar == null)
			{
				return;
			}
			this.StartTween(0f, this.bar.fillAmount * this.Duration);
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x000A0F28 File Offset: 0x0009F128
		protected void SetFillAmount(float amount)
		{
			if (this.bar == null)
			{
				return;
			}
			this.bar.fillAmount = amount;
			if (this.m_Text != null)
			{
				if (this.m_TextVariant == Test_UIProgressBar.TextVariant.Percent)
				{
					this.m_Text.text = Mathf.RoundToInt(amount * 100f).ToString() + "%";
					return;
				}
				if (this.m_TextVariant == Test_UIProgressBar.TextVariant.Value)
				{
					this.m_Text.text = ((float)this.m_TextValue * amount).ToString(this.m_TextValueFormat);
					return;
				}
				if (this.m_TextVariant == Test_UIProgressBar.TextVariant.ValueMax)
				{
					this.m_Text.text = ((float)this.m_TextValue * amount).ToString(this.m_TextValueFormat) + "/" + this.m_TextValue.ToString();
				}
			}
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x000A0FFF File Offset: 0x0009F1FF
		protected void OnTweenFinished()
		{
			if (this.bar == null)
			{
				return;
			}
			this.StartTween((this.bar.fillAmount == 0f) ? 1f : 0f, this.Duration);
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x000A103C File Offset: 0x0009F23C
		protected void StartTween(float targetFloat, float duration)
		{
			if (this.bar == null)
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = this.bar.fillAmount,
				targetFloat = targetFloat
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetFillAmount));
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = true;
			info.easing = this.Easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x040019E4 RID: 6628
		public UIProgressBar bar;

		// Token: 0x040019E5 RID: 6629
		public float Duration = 5f;

		// Token: 0x040019E6 RID: 6630
		public TweenEasing Easing = TweenEasing.InOutQuint;

		// Token: 0x040019E7 RID: 6631
		public Text m_Text;

		// Token: 0x040019E8 RID: 6632
		public Test_UIProgressBar.TextVariant m_TextVariant;

		// Token: 0x040019E9 RID: 6633
		public int m_TextValue = 100;

		// Token: 0x040019EA RID: 6634
		public string m_TextValueFormat = "0";

		// Token: 0x040019EB RID: 6635
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x020005B3 RID: 1459
		public enum TextVariant
		{
			// Token: 0x040019ED RID: 6637
			Percent,
			// Token: 0x040019EE RID: 6638
			Value,
			// Token: 0x040019EF RID: 6639
			ValueMax
		}
	}
}
