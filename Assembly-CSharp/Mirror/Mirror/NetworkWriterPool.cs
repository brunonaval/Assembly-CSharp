using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000063 RID: 99
	public static class NetworkWriterPool
	{
		// Token: 0x06000312 RID: 786 RVA: 0x0000BF3C File Offset: 0x0000A13C
		public static NetworkWriterPooled Get()
		{
			NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Pool.Get();
			networkWriterPooled.Reset();
			return networkWriterPooled;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000BF4E File Offset: 0x0000A14E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(NetworkWriterPooled writer)
		{
			NetworkWriterPool.Pool.Return(writer);
		}

		// Token: 0x04000135 RID: 309
		private static readonly Pool<NetworkWriterPooled> Pool = new Pool<NetworkWriterPooled>(() => new NetworkWriterPooled(), 1000);
	}
}
