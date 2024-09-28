using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200084B RID: 2123
	internal static class JitHelpers
	{
		// Token: 0x060046CA RID: 18122 RVA: 0x000E71B1 File Offset: 0x000E53B1
		internal static T UnsafeCast<T>(object o) where T : class
		{
			return Array.UnsafeMov<object, T>(o);
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x000E71B9 File Offset: 0x000E53B9
		internal static int UnsafeEnumCast<T>(T val) where T : struct
		{
			return Array.UnsafeMov<T, int>(val);
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x000E71C1 File Offset: 0x000E53C1
		internal static long UnsafeEnumCastLong<T>(T val) where T : struct
		{
			return Array.UnsafeMov<T, long>(val);
		}
	}
}
