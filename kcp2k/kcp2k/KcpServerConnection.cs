using System;
using System.Net;

namespace kcp2k
{
	// Token: 0x0200000D RID: 13
	public struct KcpServerConnection
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000038EB File Offset: 0x00001AEB
		public KcpServerConnection(EndPoint remoteEndPoint)
		{
			this.peer = null;
			this.remoteEndPoint = remoteEndPoint;
		}

		// Token: 0x0400004F RID: 79
		public KcpPeer peer;

		// Token: 0x04000050 RID: 80
		public readonly EndPoint remoteEndPoint;
	}
}
