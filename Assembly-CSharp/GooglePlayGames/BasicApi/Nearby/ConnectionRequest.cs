using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006B4 RID: 1716
	public struct ConnectionRequest
	{
		// Token: 0x060025A2 RID: 9634 RVA: 0x000B4FBD File Offset: 0x000B31BD
		public ConnectionRequest(string remoteEndpointId, string remoteEndpointName, string serviceId, byte[] payload)
		{
			Logger.d("Constructing ConnectionRequest");
			this.mRemoteEndpoint = new EndpointDetails(remoteEndpointId, remoteEndpointName, serviceId);
			this.mPayload = Misc.CheckNotNull<byte[]>(payload);
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x000B4FE4 File Offset: 0x000B31E4
		public EndpointDetails RemoteEndpoint
		{
			get
			{
				return this.mRemoteEndpoint;
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x060025A4 RID: 9636 RVA: 0x000B4FEC File Offset: 0x000B31EC
		public byte[] Payload
		{
			get
			{
				return this.mPayload;
			}
		}

		// Token: 0x04001EC8 RID: 7880
		private readonly EndpointDetails mRemoteEndpoint;

		// Token: 0x04001EC9 RID: 7881
		private readonly byte[] mPayload;
	}
}
