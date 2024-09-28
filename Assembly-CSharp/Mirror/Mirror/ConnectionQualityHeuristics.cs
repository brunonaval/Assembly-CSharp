using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000013 RID: 19
	public static class ConnectionQualityHeuristics
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000023D0 File Offset: 0x000005D0
		public static Color ColorCode(this ConnectionQuality quality)
		{
			switch (quality)
			{
			case ConnectionQuality.EXCELLENT:
				return Color.green;
			case ConnectionQuality.GOOD:
				return Color.yellow;
			case ConnectionQuality.FAIR:
				return new Color(1f, 0.647f, 0f);
			case ConnectionQuality.POOR:
				return Color.red;
			default:
				return Color.gray;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002424 File Offset: 0x00000624
		public static ConnectionQuality Simple(double rtt, double jitter)
		{
			if (rtt <= 0.1 && jitter <= 0.1)
			{
				return ConnectionQuality.EXCELLENT;
			}
			if (rtt <= 0.2 && jitter <= 0.2)
			{
				return ConnectionQuality.GOOD;
			}
			if (rtt <= 0.4 && jitter <= 0.5)
			{
				return ConnectionQuality.FAIR;
			}
			return ConnectionQuality.POOR;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002480 File Offset: 0x00000680
		public static ConnectionQuality Pragmatic(double targetBufferTime, double currentBufferTime)
		{
			double num = currentBufferTime / targetBufferTime;
			if (num <= 1.15)
			{
				return ConnectionQuality.EXCELLENT;
			}
			if (num <= 1.25)
			{
				return ConnectionQuality.GOOD;
			}
			if (num <= 1.5)
			{
				return ConnectionQuality.FAIR;
			}
			return ConnectionQuality.POOR;
		}
	}
}
