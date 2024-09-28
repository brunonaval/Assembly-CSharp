using System;
using UnityEngine;

namespace GooglePlayGames.OurUtils
{
	// Token: 0x02000692 RID: 1682
	public static class Misc
	{
		// Token: 0x0600254A RID: 9546 RVA: 0x000B4778 File Offset: 0x000B2978
		public static bool BuffersAreIdentical(byte[] a, byte[] b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000B47B8 File Offset: 0x000B29B8
		public static byte[] GetSubsetBytes(byte[] array, int offset, int length)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (offset < 0 || offset >= array.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (length < 0 || array.Length - offset < length)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			if (offset == 0 && length == array.Length)
			{
				return array;
			}
			byte[] array2 = new byte[length];
			Array.Copy(array, offset, array2, 0, length);
			return array2;
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000B481C File Offset: 0x000B2A1C
		public static T CheckNotNull<T>(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			return value;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000B482D File Offset: 0x000B2A2D
		public static T CheckNotNull<T>(T value, string paramName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(paramName);
			}
			return value;
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000B483F File Offset: 0x000B2A3F
		public static bool IsApiException(AndroidJavaObject exception)
		{
			return exception.Call<AndroidJavaObject>("getClass", Array.Empty<object>()).Call<string>("getName", Array.Empty<object>()) == "com.google.android.gms.common.api.ApiException";
		}
	}
}
