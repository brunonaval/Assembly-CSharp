﻿using System;
using System.Collections.Generic;

namespace System.IO
{
	// Token: 0x02000B6E RID: 2926
	internal static class MonoLinqHelper
	{
		// Token: 0x06006A76 RID: 27254 RVA: 0x0016C7F1 File Offset: 0x0016A9F1
		public static T[] ToArray<T>(this IEnumerable<T> source)
		{
			return EnumerableHelpers.ToArray<T>(source);
		}
	}
}
