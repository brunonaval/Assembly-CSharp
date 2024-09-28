using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006B7 RID: 1719
	public struct EndpointDetails
	{
		// Token: 0x060025B1 RID: 9649 RVA: 0x000B50A0 File Offset: 0x000B32A0
		public EndpointDetails(string endpointId, string name, string serviceId)
		{
			this.mEndpointId = Misc.CheckNotNull<string>(endpointId);
			this.mName = Misc.CheckNotNull<string>(name);
			this.mServiceId = Misc.CheckNotNull<string>(serviceId);
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000B50C6 File Offset: 0x000B32C6
		public string EndpointId
		{
			get
			{
				return this.mEndpointId;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x000B50CE File Offset: 0x000B32CE
		public string Name
		{
			get
			{
				return this.mName;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x060025B4 RID: 9652 RVA: 0x000B50D6 File Offset: 0x000B32D6
		public string ServiceId
		{
			get
			{
				return this.mServiceId;
			}
		}

		// Token: 0x04001ED6 RID: 7894
		private readonly string mEndpointId;

		// Token: 0x04001ED7 RID: 7895
		private readonly string mName;

		// Token: 0x04001ED8 RID: 7896
		private readonly string mServiceId;
	}
}
