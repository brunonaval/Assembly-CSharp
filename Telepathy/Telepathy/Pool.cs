using System;
using System.Collections.Generic;

namespace Telepathy
{
	// Token: 0x0200000C RID: 12
	public class Pool<T>
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002D49 File Offset: 0x00000F49
		public Pool(Func<T> objectGenerator)
		{
			this.objectGenerator = objectGenerator;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002D63 File Offset: 0x00000F63
		public T Take()
		{
			if (this.objects.Count <= 0)
			{
				return this.objectGenerator();
			}
			return this.objects.Pop();
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002D8A File Offset: 0x00000F8A
		public void Return(T item)
		{
			this.objects.Push(item);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002D98 File Offset: 0x00000F98
		public void Clear()
		{
			this.objects.Clear();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002DA5 File Offset: 0x00000FA5
		public int Count()
		{
			return this.objects.Count;
		}

		// Token: 0x0400001D RID: 29
		private readonly Stack<T> objects = new Stack<T>();

		// Token: 0x0400001E RID: 30
		private readonly Func<T> objectGenerator;
	}
}
