using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000082 RID: 130
	public static class Extensions
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x0000E389 File Offset: 0x0000C589
		public static string ToHexString(this ArraySegment<byte> segment)
		{
			return BitConverter.ToString(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
		public static int GetStableHashCode(this string text)
		{
			int num = 23;
			foreach (char c in text)
			{
				num = num * 31 + (int)c;
			}
			return num;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000E3DB File Offset: 0x0000C5DB
		internal static string GetMethodName(this Delegate func)
		{
			return func.Method.Name;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000E3E8 File Offset: 0x0000C5E8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyTo<T>(this IEnumerable<T> source, List<T> destination)
		{
			destination.AddRange(source);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0000E3F1 File Offset: 0x0000C5F1
		public static bool TryDequeue<T>(this Queue<T> source, out T element)
		{
			if (source.Count > 0)
			{
				element = source.Dequeue();
				return true;
			}
			element = default(T);
			return false;
		}
	}
}
