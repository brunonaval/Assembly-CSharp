using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x0200007E RID: 126
	public static class AccurateInterval
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x0000D984 File Offset: 0x0000BB84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Elapsed(double time, double interval, ref double lastTime)
		{
			if (time < lastTime + interval)
			{
				return false;
			}
			long num = (long)(time / interval);
			lastTime = (double)num * interval;
			return true;
		}
	}
}
