using System;

namespace Mirror
{
	// Token: 0x0200005A RID: 90
	public class NetworkReaderPooled : NetworkReader, IDisposable
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00009968 File Offset: 0x00007B68
		internal NetworkReaderPooled(byte[] bytes) : base(bytes)
		{
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00009976 File Offset: 0x00007B76
		internal NetworkReaderPooled(ArraySegment<byte> segment) : base(segment)
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000997F File Offset: 0x00007B7F
		public void Dispose()
		{
			NetworkReaderPool.Return(this);
		}
	}
}
