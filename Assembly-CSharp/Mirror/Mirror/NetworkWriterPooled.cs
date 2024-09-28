using System;

namespace Mirror
{
	// Token: 0x02000065 RID: 101
	public class NetworkWriterPooled : NetworkWriter, IDisposable
	{
		// Token: 0x06000318 RID: 792 RVA: 0x0000BF8F File Offset: 0x0000A18F
		public void Dispose()
		{
			NetworkWriterPool.Return(this);
		}
	}
}
