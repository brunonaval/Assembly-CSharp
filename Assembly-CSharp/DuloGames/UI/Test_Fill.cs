using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x020005AB RID: 1451
	public class Test_Fill : MonoBehaviour
	{
		// Token: 0x06002000 RID: 8192 RVA: 0x000A09B2 File Offset: 0x0009EBB2
		protected Test_Fill()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000A09EC File Offset: 0x0009EBEC
		protected void OnEnable()
		{
			if (this.imageComponent == null)
			{
				return;
			}
			this.StartTween(0f, this.imageComponent.fillAmount * this.Duration);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000A0A1A File Offset: 0x0009EC1A
		protected void SetFillAmount(float amount)
		{
			if (this.imageComponent == null)
			{
				return;
			}
			this.imageComponent.fillAmount = amount;
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x000A0A37 File Offset: 0x0009EC37
		protected void OnTweenFinished()
		{
			if (this.imageComponent == null)
			{
				return;
			}
			this.StartTween((this.imageComponent.fillAmount == 0f) ? 1f : 0f, this.Duration);
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x000A0A74 File Offset: 0x0009EC74
		protected void StartTween(float targetFloat, float duration)
		{
			if (this.imageComponent == null)
			{
				return;
			}
			FloatTween info = new FloatTween
			{
				duration = duration,
				startFloat = this.imageComponent.fillAmount,
				targetFloat = targetFloat
			};
			info.AddOnChangedCallback(new UnityAction<float>(this.SetFillAmount));
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = true;
			info.easing = this.Easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x040019CE RID: 6606
		[SerializeField]
		private Image imageComponent;

		// Token: 0x040019CF RID: 6607
		[SerializeField]
		private float Duration = 5f;

		// Token: 0x040019D0 RID: 6608
		[SerializeField]
		private TweenEasing Easing = TweenEasing.InOutQuint;

		// Token: 0x040019D1 RID: 6609
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
