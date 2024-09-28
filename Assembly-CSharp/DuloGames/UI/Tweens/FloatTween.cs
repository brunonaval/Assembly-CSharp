using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI.Tweens
{
	// Token: 0x0200067E RID: 1662
	public struct FloatTween : ITweenValue
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060024E9 RID: 9449 RVA: 0x000B38C3 File Offset: 0x000B1AC3
		// (set) Token: 0x060024EA RID: 9450 RVA: 0x000B38CB File Offset: 0x000B1ACB
		public float startFloat
		{
			get
			{
				return this.m_StartFloat;
			}
			set
			{
				this.m_StartFloat = value;
			}
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060024EB RID: 9451 RVA: 0x000B38D4 File Offset: 0x000B1AD4
		// (set) Token: 0x060024EC RID: 9452 RVA: 0x000B38DC File Offset: 0x000B1ADC
		public float targetFloat
		{
			get
			{
				return this.m_TargetFloat;
			}
			set
			{
				this.m_TargetFloat = value;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060024ED RID: 9453 RVA: 0x000B38E5 File Offset: 0x000B1AE5
		// (set) Token: 0x060024EE RID: 9454 RVA: 0x000B38ED File Offset: 0x000B1AED
		public float duration
		{
			get
			{
				return this.m_Duration;
			}
			set
			{
				this.m_Duration = value;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060024EF RID: 9455 RVA: 0x000B38F6 File Offset: 0x000B1AF6
		// (set) Token: 0x060024F0 RID: 9456 RVA: 0x000B38FE File Offset: 0x000B1AFE
		public bool ignoreTimeScale
		{
			get
			{
				return this.m_IgnoreTimeScale;
			}
			set
			{
				this.m_IgnoreTimeScale = value;
			}
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060024F1 RID: 9457 RVA: 0x000B3907 File Offset: 0x000B1B07
		// (set) Token: 0x060024F2 RID: 9458 RVA: 0x000B390F File Offset: 0x000B1B0F
		public TweenEasing easing
		{
			get
			{
				return this.m_Easing;
			}
			set
			{
				this.m_Easing = value;
			}
		}

		// Token: 0x060024F3 RID: 9459 RVA: 0x000B3918 File Offset: 0x000B1B18
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			this.m_Target.Invoke(Mathf.Lerp(this.m_StartFloat, this.m_TargetFloat, floatPercentage));
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x000B3940 File Offset: 0x000B1B40
		public void AddOnChangedCallback(UnityAction<float> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new FloatTween.FloatTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x000B3961 File Offset: 0x000B1B61
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new FloatTween.FloatFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x060024F6 RID: 9462 RVA: 0x000B38F6 File Offset: 0x000B1AF6
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x060024F7 RID: 9463 RVA: 0x000B38E5 File Offset: 0x000B1AE5
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x060024F8 RID: 9464 RVA: 0x000B3982 File Offset: 0x000B1B82
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x060024F9 RID: 9465 RVA: 0x000B398D File Offset: 0x000B1B8D
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x04001DF7 RID: 7671
		private float m_StartFloat;

		// Token: 0x04001DF8 RID: 7672
		private float m_TargetFloat;

		// Token: 0x04001DF9 RID: 7673
		private float m_Duration;

		// Token: 0x04001DFA RID: 7674
		private bool m_IgnoreTimeScale;

		// Token: 0x04001DFB RID: 7675
		private TweenEasing m_Easing;

		// Token: 0x04001DFC RID: 7676
		private FloatTween.FloatTweenCallback m_Target;

		// Token: 0x04001DFD RID: 7677
		private FloatTween.FloatFinishCallback m_Finish;

		// Token: 0x0200067F RID: 1663
		public class FloatTweenCallback : UnityEvent<float>
		{
		}

		// Token: 0x02000680 RID: 1664
		public class FloatFinishCallback : UnityEvent
		{
		}
	}
}
