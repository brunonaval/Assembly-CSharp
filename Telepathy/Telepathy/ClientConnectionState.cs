using System;
using System.Net.Sockets;
using System.Threading;

namespace Telepathy
{
	// Token: 0x02000003 RID: 3
	internal class ClientConnectionState : ConnectionState
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public bool Connected
		{
			get
			{
				return this.client != null && this.client.Client != null && this.client.Client.Connected;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020E9 File Offset: 0x000002E9
		public ClientConnectionState(int MaxMessageSize) : base(new TcpClient(), MaxMessageSize)
		{
			this.receivePipe = new MagnificentReceivePipe(MaxMessageSize);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002103 File Offset: 0x00000303
		public void Dispose()
		{
			this.client.Close();
			Thread thread = this.receiveThread;
			if (thread != null)
			{
				thread.Interrupt();
			}
			this.Connecting = false;
			this.sendPipe.Clear();
			this.client = null;
		}

		// Token: 0x04000001 RID: 1
		public Thread receiveThread;

		// Token: 0x04000002 RID: 2
		public volatile bool Connecting;

		// Token: 0x04000003 RID: 3
		public readonly MagnificentReceivePipe receivePipe;
	}
}
