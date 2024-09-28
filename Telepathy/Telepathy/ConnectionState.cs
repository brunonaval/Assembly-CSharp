using System;
using System.Net.Sockets;
using System.Threading;

namespace Telepathy
{
	// Token: 0x02000006 RID: 6
	public class ConnectionState
	{
		// Token: 0x06000010 RID: 16 RVA: 0x000025ED File Offset: 0x000007ED
		public ConnectionState(TcpClient client, int MaxMessageSize)
		{
			this.client = client;
			this.sendPipe = new MagnificentSendPipe(MaxMessageSize);
		}

		// Token: 0x0400000E RID: 14
		public TcpClient client;

		// Token: 0x0400000F RID: 15
		public readonly MagnificentSendPipe sendPipe;

		// Token: 0x04000010 RID: 16
		public ManualResetEvent sendPending = new ManualResetEvent(false);
	}
}
