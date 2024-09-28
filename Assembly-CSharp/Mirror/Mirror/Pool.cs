using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000084 RID: 132
	public class Pool<T>
	{
		// Token: 0x060003DB RID: 987 RVA: 0x0000E460 File Offset: 0x0000C660
		public Pool(Func<T> objectGenerator, int initialCapacity)
		{
			this.objectGenerator = objectGenerator;
			for (int i = 0; i < initialCapacity; i++)
			{
				this.objects.Push(objectGenerator());
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000E4A2 File Offset: 0x0000C6A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Get()
		{
			if (this.objects.Count <= 0)
			{
				return this.objectGenerator();
			}
			return this.objects.Pop();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000E4C9 File Offset: 0x0000C6C9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Return(T item)
		{
			this.objects.Push(item);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000E4D7 File Offset: 0x0000C6D7
		public int Count
		{
			get
			{
				return this.objects.Count;
			}
		}

		// Token: 0x04000178 RID: 376
		private readonly Stack<T> objects = new Stack<T>();

		// Token: 0x04000179 RID: 377
		private readonly Func<T> objectGenerator;
	}
}
