using System;
using System.Collections.Generic;

namespace kcp2k
{
	// Token: 0x02000011 RID: 17
	public class Pool<T>
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00004DA4 File Offset: 0x00002FA4
		public Pool(Func<T> objectGenerator, Action<T> objectResetter, int initialCapacity)
		{
			this.objectGenerator = objectGenerator;
			this.objectResetter = objectResetter;
			for (int i = 0; i < initialCapacity; i++)
			{
				this.objects.Push(objectGenerator());
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004DED File Offset: 0x00002FED
		public T Take()
		{
			if (this.objects.Count <= 0)
			{
				return this.objectGenerator();
			}
			return this.objects.Pop();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004E14 File Offset: 0x00003014
		public void Return(T item)
		{
			this.objectResetter(item);
			this.objects.Push(item);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004E2E File Offset: 0x0000302E
		public void Clear()
		{
			this.objects.Clear();
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004E3B File Offset: 0x0000303B
		public int Count
		{
			get
			{
				return this.objects.Count;
			}
		}

		// Token: 0x04000093 RID: 147
		private readonly Stack<T> objects = new Stack<!0>();

		// Token: 0x04000094 RID: 148
		private readonly Func<T> objectGenerator;

		// Token: 0x04000095 RID: 149
		private readonly Action<T> objectResetter;
	}
}
