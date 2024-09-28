using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000083 RID: 131
	public static class Mathd
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x0000E412 File Offset: 0x0000C612
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Clamp(double value, double min, double max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0000E421 File Offset: 0x0000C621
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Clamp01(double value)
		{
			return Mathd.Clamp(value, 0.0, 1.0);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000E43B File Offset: 0x0000C63B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double InverseLerp(double a, double b, double value)
		{
			if (a == b)
			{
				return 0.0;
			}
			return Mathd.Clamp01((value - a) / (b - a));
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000E457 File Offset: 0x0000C657
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double LerpUnclamped(double a, double b, double t)
		{
			return a + (b - a) * t;
		}
	}
}
