using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005AD RID: 1453
	public class Test_UIBulletBar : MonoBehaviour
	{
		// Token: 0x06002007 RID: 8199 RVA: 0x000A0B24 File Offset: 0x0009ED24
		protected Test_UIBulletBar()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x000A0B71 File Offset: 0x0009ED71
		protected void OnEnable()
		{
			if (this.bar == null)
			{
				return;
			}
			this.StartTween(0f, this.bar.fillAmount * this.Duration);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x000A0BA0 File Offset: 0x0009EDA0
		protected void SetFillAmount(float amount)
		{
			if (this.bar == null)
			{
				return;
			}
			this.bar.fillAmount = amount;
			if (this.m_Text != null)
			{
				if (this.m_TextVariant == Test_UIBulletBar.TextVariant.Percent)
				{
					this.m_Text.text = Mathf.RoundToInt(amount * 100f).ToString() + "%";
					return;
				}
				if (this.m_TextVariant == Test_UIBulletBar.TextVariant.Value)
				{
					this.m_Text.text = Mathf.RoundToInt((float)this.m_TextValue * amount).ToString();
					return;
				}
				if (this.m_TextVariant == Test_UIBulletBar.TextVariant.ValueMax)
				{
					this.m_Text.text = Mathf.RoundToInt((float)this.m_TextValue * amount).ToString() + "/" + this.m_TextValue.ToString();
				}
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x000A0C75 File Offset: 0x0009EE75
		protected void OnTweenFinished()
		{
			if (this.bar == null)
			{
				return;
			}
			this.StartTween((this.bar.fillAmount == 0f) ? 1f : 0f, this.Duration);
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x000A0CB0 File Offset: 0x0009EEB0
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

		// Token: 0x040019D4 RID: 6612
		public UIBulletBar bar;

		// Token: 0x040019D5 RID: 6613
		public float Duration = 5f;

		// Token: 0x040019D6 RID: 6614
		public TweenEasing Easing = TweenEasing.InOutQuint;

		// Token: 0x040019D7 RID: 6615
		public Text m_Text;

		// Token: 0x040019D8 RID: 6616
		public Test_UIBulletBar.TextVariant m_TextVariant;

		// Token: 0x040019D9 RID: 6617
		public int m_TextValue = 100;

		// Token: 0x040019DA RID: 6618
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		// Token: 0x020005AE RID: 1454
		public enum TextVariant
		{
			// Token: 0x040019DC RID: 6620
			Percent,
			// Token: 0x040019DD RID: 6621
			Value,
			// Token: 0x040019DE RID: 6622
			ValueMax
		}
	}
}
