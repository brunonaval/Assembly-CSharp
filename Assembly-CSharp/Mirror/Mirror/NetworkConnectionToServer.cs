using System;
using System.Runtime.CompilerServices;

namespace Mirror
{
	// Token: 0x02000040 RID: 64
	public class NetworkConnectionToServer : NetworkConnection
	{
		// Token: 0x0600013D RID: 317 RVA: 0x000067F8 File Offset: 0x000049F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected override void SendToTransport(ArraySegment<byte> segment, int channelId = 0)
		{
			Transport.active.ClientSend(segment, channelId);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006806 File Offset: 0x00004A06
		public override void Disconnect()
		{
			this.isReady = false;
			NetworkClient.ready = false;
			Transport.active.ClientDisconnect();
		}
	}
}
