using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000058 RID: 88
	public static class NetworkReaderPool
	{
		// Token: 0x06000252 RID: 594 RVA: 0x000098F6 File Offset: 0x00007AF6
		public static NetworkReaderPooled Get(byte[] bytes)
		{
			NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Pool.Get();
			networkReaderPooled.SetBuffer(bytes);
			return networkReaderPooled;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000990E File Offset: 0x00007B0E
		public static NetworkReaderPooled Get(ArraySegment<byte> segment)
		{
			NetworkReaderPooled networkReaderPooled = NetworkReaderPool.Pool.Get();
			networkReaderPooled.SetBuffer(segment);
			return networkReaderPooled;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00009921 File Offset: 0x00007B21
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Return(NetworkReaderPooled reader)
		{
			NetworkReaderPool.Pool.Return(reader);
		}

		// Token: 0x0400010D RID: 269
		private static readonly Pool<NetworkReaderPooled> Pool = new Pool<NetworkReaderPooled>(() => new NetworkReaderPooled(new byte[0]), 1000);
	}
}
