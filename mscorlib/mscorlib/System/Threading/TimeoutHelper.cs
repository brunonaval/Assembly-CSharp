﻿using System;

namespace System.Threading
{
	// Token: 0x020002A6 RID: 678
	internal static class TimeoutHelper
	{
		// Token: 0x06001E08 RID: 7688 RVA: 0x0006F6E8 File Offset: 0x0006D8E8
		public static uint GetTime()
		{
			return (uint)Environment.TickCount;
		}

		// Token: 0x06001E09 RID: 7689 RVA: 0x0006F6F0 File Offset: 0x0006D8F0
		public static int UpdateTimeOut(uint startTime, int originalWaitMillisecondsTimeout)
		{
			uint num = TimeoutHelper.GetTime() - startTime;
			if (num > 2147483647U)
			{
				return 0;
			}
			int num2 = originalWaitMillisecondsTimeout - (int)num;
			if (num2 <= 0)
			{
				return 0;
			}
			return num2;
		}
	}
}
