using System;
using DuloGames.UI.Tweens;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI
{
	// Token: 0x020005AA RID: 1450
	public class Test_FadeInOut : MonoBehaviour
	{
		// Token: 0x06001FFB RID: 8187 RVA: 0x000A0842 File Offset: 0x0009EA42
		protected Test_FadeInOut()
		{
			if (this.m_FloatTweenRunner == null)
			{
				this.m_FloatTweenRunner = new TweenRunner<FloatTween>();
			}
			this.m_FloatTweenRunner.Init(this);
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000A087C File Offset: 0x0009EA7C
		protected void OnEnable()
		{
			this.m_Group = base.GetComponent<CanvasGroup>();
			if (this.m_Group == null)
			{
				this.m_Group = base.gameObject.AddComponent<CanvasGroup>();
			}
			this.StartAlphaTween(0f, this.m_Duration, true);
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000A08BC File Offset: 0x0009EABC
		private void StartAlphaTween(float targetAlpha, float duration, bool ignoreTimeScale)
		{
			if (this.m_Group == null)
			{
				return;
			}
			float alpha = this.m_Group.alpha;
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
			info.AddOnChangedCallback(new UnityAction<float>(this.SetAlpha));
			info.AddOnFinishCallback(new UnityAction(this.OnTweenFinished));
			info.ignoreTimeScale = ignoreTimeScale;
			info.easing = this.m_Easing;
			this.m_FloatTweenRunner.StartTween(info);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000A0959 File Offset: 0x0009EB59
		private void SetAlpha(float alpha)
		{
			if (this.m_Group == null)
			{
				return;
			}
			this.m_Group.alpha = alpha;
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000A0976 File Offset: 0x0009EB76
		protected virtual void OnTweenFinished()
		{
			if (this.m_Group == null)
			{
				return;
			}
			this.StartAlphaTween((this.m_Group.alpha == 1f) ? 0f : 1f, this.m_Duration, true);
		}

		// Token: 0x040019CA RID: 6602
		[SerializeField]
		private float m_Duration = 4f;

		// Token: 0x040019CB RID: 6603
		[SerializeField]
		private TweenEasing m_Easing = TweenEasing.InOutQuint;

		// Token: 0x040019CC RID: 6604
		private CanvasGroup m_Group;

		// Token: 0x040019CD RID: 6605
		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;
	}
}
