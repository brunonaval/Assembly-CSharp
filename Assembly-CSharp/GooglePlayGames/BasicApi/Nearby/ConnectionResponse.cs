using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006B5 RID: 1717
	public struct ConnectionResponse
	{
		// Token: 0x060025A5 RID: 9637 RVA: 0x000B4FF4 File Offset: 0x000B31F4
		private ConnectionResponse(long localClientId, string remoteEndpointId, ConnectionResponse.Status code, byte[] payload)
		{
			this.mLocalClientId = localClientId;
			this.mRemoteEndpointId = Misc.CheckNotNull<string>(remoteEndpointId);
			this.mResponseStatus = code;
			this.mPayload = Misc.CheckNotNull<byte[]>(payload);
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x000B501D File Offset: 0x000B321D
		public long LocalClientId
		{
			get
			{
				return this.mLocalClientId;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x000B5025 File Offset: 0x000B3225
		public string RemoteEndpointId
		{
			get
			{
				return this.mRemoteEndpointId;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000B502D File Offset: 0x000B322D
		public ConnectionResponse.Status ResponseStatus
		{
			get
			{
				return this.mResponseStatus;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000B5035 File Offset: 0x000B3235
		public byte[] Payload
		{
			get
			{
				return this.mPayload;
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000B503D File Offset: 0x000B323D
		public static ConnectionResponse Rejected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.Rejected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x000B504C File Offset: 0x000B324C
		public static ConnectionResponse NetworkNotConnected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorNetworkNotConnected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000B505B File Offset: 0x000B325B
		public static ConnectionResponse InternalError(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorInternal, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x000B506A File Offset: 0x000B326A
		public static ConnectionResponse EndpointNotConnected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorEndpointNotConnected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x000B5079 File Offset: 0x000B3279
		public static ConnectionResponse Accepted(long localClientId, string remoteEndpointId, byte[] payload)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.Accepted, payload);
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x000B5084 File Offset: 0x000B3284
		public static ConnectionResponse AlreadyConnected(long localClientId, string remoteEndpointId)
		{
			return new ConnectionResponse(localClientId, remoteEndpointId, ConnectionResponse.Status.ErrorAlreadyConnected, ConnectionResponse.EmptyPayload);
		}

		// Token: 0x04001ECA RID: 7882
		private static readonly byte[] EmptyPayload = new byte[0];

		// Token: 0x04001ECB RID: 7883
		private readonly long mLocalClientId;

		// Token: 0x04001ECC RID: 7884
		private readonly string mRemoteEndpointId;

		// Token: 0x04001ECD RID: 7885
		private readonly ConnectionResponse.Status mResponseStatus;

		// Token: 0x04001ECE RID: 7886
		private readonly byte[] mPayload;

		// Token: 0x020006B6 RID: 1718
		public enum Status
		{
			// Token: 0x04001ED0 RID: 7888
			Accepted,
			// Token: 0x04001ED1 RID: 7889
			Rejected,
			// Token: 0x04001ED2 RID: 7890
			ErrorInternal,
			// Token: 0x04001ED3 RID: 7891
			ErrorNetworkNotConnected,
			// Token: 0x04001ED4 RID: 7892
			ErrorEndpointNotConnected,
			// Token: 0x04001ED5 RID: 7893
			ErrorAlreadyConnected
		}
	}
}
