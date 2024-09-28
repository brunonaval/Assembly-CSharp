using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000068 RID: 104
	public static class SortedListExtensions
	{
		// Token: 0x06000320 RID: 800 RVA: 0x0000BFA0 File Offset: 0x0000A1A0
		public static void RemoveRange<T, U>(this SortedList<T, U> list, int amount)
		{
			int num = 0;
			while (num < amount && num < list.Count)
			{
				list.RemoveAt(0);
				num++;
			}
		}
	}
}
