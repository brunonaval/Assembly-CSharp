using System;
using System.Text;

namespace System.Collections.Generic
{
	// Token: 0x02000AA1 RID: 2721
	public static class KeyValuePair
	{
		// Token: 0x0600613B RID: 24891 RVA: 0x001451ED File Offset: 0x001433ED
		public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
		{
			return new KeyValuePair<TKey, TValue>(key, value);
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x001451F8 File Offset: 0x001433F8
		internal static string PairToString(object key, object value)
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire(16);
			stringBuilder.Append('[');
			if (key != null)
			{
				stringBuilder.Append(key);
			}
			stringBuilder.Append(", ");
			if (value != null)
			{
				stringBuilder.Append(value);
			}
			stringBuilder.Append(']');
			return StringBuilderCache.GetStringAndRelease(stringBuilder);
		}
	}
}
