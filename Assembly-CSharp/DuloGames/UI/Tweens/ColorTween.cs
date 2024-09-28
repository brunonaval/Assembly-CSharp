using System;
using UnityEngine;
using UnityEngine.Events;

namespace DuloGames.UI.Tweens
{
	// Token: 0x0200067A RID: 1658
	public struct ColorTween : ITweenValue
	{
		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x000B3757 File Offset: 0x000B1957
		// (set) Token: 0x060024D5 RID: 9429 RVA: 0x000B375F File Offset: 0x000B195F
		public Color startColor
		{
			get
			{
				return this.m_StartColor;
			}
			set
			{
				this.m_StartColor = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060024D6 RID: 9430 RVA: 0x000B3768 File Offset: 0x000B1968
		// (set) Token: 0x060024D7 RID: 9431 RVA: 0x000B3770 File Offset: 0x000B1970
		public Color targetColor
		{
			get
			{
				return this.m_TargetColor;
			}
			set
			{
				this.m_TargetColor = value;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060024D8 RID: 9432 RVA: 0x000B3779 File Offset: 0x000B1979
		// (set) Token: 0x060024D9 RID: 9433 RVA: 0x000B3781 File Offset: 0x000B1981
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

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060024DA RID: 9434 RVA: 0x000B378A File Offset: 0x000B198A
		// (set) Token: 0x060024DB RID: 9435 RVA: 0x000B3792 File Offset: 0x000B1992
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

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060024DC RID: 9436 RVA: 0x000B379B File Offset: 0x000B199B
		// (set) Token: 0x060024DD RID: 9437 RVA: 0x000B37A3 File Offset: 0x000B19A3
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

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060024DE RID: 9438 RVA: 0x000B37AC File Offset: 0x000B19AC
		// (set) Token: 0x060024DF RID: 9439 RVA: 0x000B37B4 File Offset: 0x000B19B4
		public ColorTween.ColorTweenMode tweenMode
		{
			get
			{
				return this.m_TweenMode;
			}
			set
			{
				this.m_TweenMode = value;
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000B37C0 File Offset: 0x000B19C0
		public void TweenValue(float floatPercentage)
		{
			if (!this.ValidTarget())
			{
				return;
			}
			Color arg = Color.Lerp(this.m_StartColor, this.m_TargetColor, floatPercentage);
			if (this.m_TweenMode == ColorTween.ColorTweenMode.Alpha)
			{
				arg.r = this.m_StartColor.r;
				arg.g = this.m_StartColor.g;
				arg.b = this.m_StartColor.b;
			}
			else if (this.m_TweenMode == ColorTween.ColorTweenMode.RGB)
			{
				arg.a = this.m_StartColor.a;
			}
			this.m_Target.Invoke(arg);
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x000B3851 File Offset: 0x000B1A51
		public void AddOnChangedCallback(UnityAction<Color> callback)
		{
			if (this.m_Target == null)
			{
				this.m_Target = new ColorTween.ColorTweenCallback();
			}
			this.m_Target.AddListener(callback);
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x000B3872 File Offset: 0x000B1A72
		public void AddOnFinishCallback(UnityAction callback)
		{
			if (this.m_Finish == null)
			{
				this.m_Finish = new ColorTween.ColorTweenFinishCallback();
			}
			this.m_Finish.AddListener(callback);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000B378A File Offset: 0x000B198A
		public bool GetIgnoreTimescale()
		{
			return this.m_IgnoreTimeScale;
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000B3779 File Offset: 0x000B1979
		public float GetDuration()
		{
			return this.m_Duration;
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000B3893 File Offset: 0x000B1A93
		public bool ValidTarget()
		{
			return this.m_Target != null;
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000B389E File Offset: 0x000B1A9E
		public void Finished()
		{
			if (this.m_Finish != null)
			{
				this.m_Finish.Invoke();
			}
		}

		// Token: 0x04001DEB RID: 7659
		private Color m_StartColor;

		// Token: 0x04001DEC RID: 7660
		private Color m_TargetColor;

		// Token: 0x04001DED RID: 7661
		private float m_Duration;

		// Token: 0x04001DEE RID: 7662
		private bool m_IgnoreTimeScale;

		// Token: 0x04001DEF RID: 7663
		private TweenEasing m_Easing;

		// Token: 0x04001DF0 RID: 7664
		private ColorTween.ColorTweenMode m_TweenMode;

		// Token: 0x04001DF1 RID: 7665
		private ColorTween.ColorTweenCallback m_Target;

		// Token: 0x04001DF2 RID: 7666
		private ColorTween.ColorTweenFinishCallback m_Finish;

		// Token: 0x0200067B RID: 1659
		public enum ColorTweenMode
		{
			// Token: 0x04001DF4 RID: 7668
			All,
			// Token: 0x04001DF5 RID: 7669
			RGB,
			// Token: 0x04001DF6 RID: 7670
			Alpha
		}

		// Token: 0x0200067C RID: 1660
		public class ColorTweenCallback : UnityEvent<Color>
		{
		}

		// Token: 0x0200067D RID: 1661
		public class ColorTweenFinishCallback : UnityEvent
		{
		}
	}
}
