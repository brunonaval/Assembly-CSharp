using System;

namespace System.Collections.Generic
{
	// Token: 0x02000AC6 RID: 2758
	[Serializable]
	internal class GenericEqualityComparer<T> : EqualityComparer<T> where T : IEquatable<T>
	{
		// Token: 0x06006291 RID: 25233 RVA: 0x00149F5C File Offset: 0x0014815C
		public override bool Equals(T x, T y)
		{
			if (x != null)
			{
				return y != null && x.Equals(y);
			}
			return y == null;
		}

		// Token: 0x06006292 RID: 25234 RVA: 0x00149F8A File Offset: 0x0014818A
		public override int GetHashCode(T obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		// Token: 0x06006293 RID: 25235 RVA: 0x00149FA4 File Offset: 0x001481A4
		internal override int IndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex + count;
			if (value == null)
			{
				for (int i = startIndex; i < num; i++)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j < num; j++)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06006294 RID: 25236 RVA: 0x0014A010 File Offset: 0x00148210
		internal override int LastIndexOf(T[] array, T value, int startIndex, int count)
		{
			int num = startIndex - count + 1;
			if (value == null)
			{
				for (int i = startIndex; i >= num; i--)
				{
					if (array[i] == null)
					{
						return i;
					}
				}
			}
			else
			{
				for (int j = startIndex; j >= num; j--)
				{
					if (array[j] != null && array[j].Equals(value))
					{
						return j;
					}
				}
			}
			return -1;
		}

		// Token: 0x06006295 RID: 25237 RVA: 0x0014A07E File Offset: 0x0014827E
		public override bool Equals(object obj)
		{
			return obj is GenericEqualityComparer<T>;
		}

		// Token: 0x06006296 RID: 25238 RVA: 0x00149C62 File Offset: 0x00147E62
		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}
}
