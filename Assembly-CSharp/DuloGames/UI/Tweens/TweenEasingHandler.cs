using System;
using UnityEngine;

namespace DuloGames.UI.Tweens
{
	// Token: 0x02000683 RID: 1667
	internal class TweenEasingHandler
	{
		// Token: 0x06002502 RID: 9474 RVA: 0x000B39A4 File Offset: 0x000B1BA4
		public static float Apply(TweenEasing e, float t, float b, float c, float d)
		{
			switch (e)
			{
			case TweenEasing.Swing:
				return -c * (t /= d) * (t - 2f) + b;
			case TweenEasing.InQuad:
				return c * (t /= d) * t + b;
			case TweenEasing.OutQuad:
				return -c * (t /= d) * (t - 2f) + b;
			case TweenEasing.InOutQuad:
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * t * t + b;
				}
				return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
			case TweenEasing.InCubic:
				return c * (t /= d) * t * t + b;
			case TweenEasing.OutCubic:
				return c * ((t = t / d - 1f) * t * t + 1f) + b;
			case TweenEasing.InOutCubic:
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * t * t * t + b;
				}
				return c / 2f * ((t -= 2f) * t * t + 2f) + b;
			case TweenEasing.InQuart:
				return c * (t /= d) * t * t * t + b;
			case TweenEasing.OutQuart:
				return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
			case TweenEasing.InOutQuart:
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * t * t * t * t + b;
				}
				return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
			case TweenEasing.InQuint:
				return c * (t /= d) * t * t * t * t + b;
			case TweenEasing.OutQuint:
				return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
			case TweenEasing.InOutQuint:
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * t * t * t * t * t + b;
				}
				return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
			case TweenEasing.InSine:
				return -c * Mathf.Cos(t / d * 1.5707964f) + c + b;
			case TweenEasing.OutSine:
				return c * Mathf.Sin(t / d * 1.5707964f) + b;
			case TweenEasing.InOutSine:
				return -c / 2f * (Mathf.Cos(3.1415927f * t / d) - 1f) + b;
			case TweenEasing.InExpo:
				if (t != 0f)
				{
					return c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b;
				}
				return b;
			case TweenEasing.OutExpo:
				if (t != d)
				{
					return c * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b;
				}
				return b + c;
			case TweenEasing.InOutExpo:
				if (t == 0f)
				{
					return b;
				}
				if (t == d)
				{
					return b + c;
				}
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
				}
				return c / 2f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
			case TweenEasing.InCirc:
				return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
			case TweenEasing.OutCirc:
				return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
			case TweenEasing.InOutCirc:
				if ((t /= d / 2f) < 1f)
				{
					return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
				}
				return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
			case TweenEasing.InElastic:
			{
				float num = 0f;
				float num2 = c;
				if (t == 0f)
				{
					return b;
				}
				if ((t /= d) == 1f)
				{
					return b + c;
				}
				if (num == 0f)
				{
					num = d * 0.3f;
				}
				float num3;
				if (num2 < Mathf.Abs(c))
				{
					num2 = c;
					num3 = num / 4f;
				}
				else
				{
					num3 = num / 6.2831855f * Mathf.Asin(c / num2);
				}
				if (float.IsNaN(num3))
				{
					num3 = 0f;
				}
				return -(num2 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num3) * 6.2831855f / num)) + b;
			}
			case TweenEasing.OutElastic:
			{
				float num4 = 0f;
				float num5 = c;
				if (t == 0f)
				{
					return b;
				}
				if ((t /= d) == 1f)
				{
					return b + c;
				}
				if (num4 == 0f)
				{
					num4 = d * 0.3f;
				}
				float num6;
				if (num5 < Mathf.Abs(c))
				{
					num5 = c;
					num6 = num4 / 4f;
				}
				else
				{
					num6 = num4 / 6.2831855f * Mathf.Asin(c / num5);
				}
				if (float.IsNaN(num6))
				{
					num6 = 0f;
				}
				return num5 * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num6) * 6.2831855f / num4) + c + b;
			}
			case TweenEasing.InOutElastic:
			{
				float num7 = 0f;
				float num8 = c;
				if (t == 0f)
				{
					return b;
				}
				if ((t /= d / 2f) == 2f)
				{
					return b + c;
				}
				if (num7 == 0f)
				{
					num7 = d * 0.45000002f;
				}
				float num9;
				if (num8 < Mathf.Abs(c))
				{
					num8 = c;
					num9 = num7 / 4f;
				}
				else
				{
					num9 = num7 / 6.2831855f * Mathf.Asin(c / num8);
				}
				if (float.IsNaN(num9))
				{
					num9 = 0f;
				}
				if (t < 1f)
				{
					return -0.5f * (num8 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num9) * 6.2831855f / num7)) + b;
				}
				return num8 * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num9) * 6.2831855f / num7) * 0.5f + c + b;
			}
			case TweenEasing.InBack:
			{
				float num10 = 1.70158f;
				return c * (t /= d) * t * ((num10 + 1f) * t - num10) + b;
			}
			case TweenEasing.OutBack:
			{
				float num11 = 1.70158f;
				return c * ((t = t / d - 1f) * t * ((num11 + 1f) * t + num11) + 1f) + b;
			}
			case TweenEasing.InOutBack:
			{
				float num12 = 1.70158f;
				if ((t /= d / 2f) < 1f)
				{
					return c / 2f * (t * t * (((num12 *= 1.525f) + 1f) * t - num12)) + b;
				}
				return c / 2f * ((t -= 2f) * t * (((num12 *= 1.525f) + 1f) * t + num12) + 2f) + b;
			}
			case TweenEasing.InBounce:
				return c - TweenEasingHandler.Apply(TweenEasing.OutBounce, d - t, 0f, c, d) + b;
			case TweenEasing.OutBounce:
				if ((t /= d) < 0.36363637f)
				{
					return c * (7.5625f * t * t) + b;
				}
				if (t < 0.72727275f)
				{
					return c * (7.5625f * (t -= 0.54545456f) * t + 0.75f) + b;
				}
				if (t < 0.90909094f)
				{
					return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
				}
				return c * (7.5625f * (t -= 0.95454544f) * t + 0.984375f) + b;
			case TweenEasing.InOutBounce:
				if (t < d / 2f)
				{
					return TweenEasingHandler.Apply(TweenEasing.InBounce, t * 2f, 0f, c, d) * 0.5f + b;
				}
				return TweenEasingHandler.Apply(TweenEasing.OutBounce, t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
			}
			return c * t / d + b;
		}
	}
}
