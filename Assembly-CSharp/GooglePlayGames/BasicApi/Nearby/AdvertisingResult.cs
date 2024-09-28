using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006B3 RID: 1715
	public struct AdvertisingResult
	{
		// Token: 0x0600259E RID: 9630 RVA: 0x000B4F8D File Offset: 0x000B318D
		public AdvertisingResult(ResponseStatus status, string localEndpointName)
		{
			this.mStatus = status;
			this.mLocalEndpointName = Misc.CheckNotNull<string>(localEndpointName);
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x000B4FA2 File Offset: 0x000B31A2
		public bool Succeeded
		{
			get
			{
				return this.mStatus == ResponseStatus.Success;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x000B4FAD File Offset: 0x000B31AD
		public ResponseStatus Status
		{
			get
			{
				return this.mStatus;
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x000B4FB5 File Offset: 0x000B31B5
		public string LocalEndpointName
		{
			get
			{
				return this.mLocalEndpointName;
			}
		}

		// Token: 0x04001EC6 RID: 7878
		private readonly ResponseStatus mStatus;

		// Token: 0x04001EC7 RID: 7879
		private readonly string mLocalEndpointName;
	}
}
