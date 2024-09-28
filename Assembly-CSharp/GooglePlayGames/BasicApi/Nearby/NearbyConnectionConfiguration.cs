using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.Nearby
{
	// Token: 0x020006BB RID: 1723
	public struct NearbyConnectionConfiguration
	{
		// Token: 0x060025B9 RID: 9657 RVA: 0x000B50DE File Offset: 0x000B32DE
		public NearbyConnectionConfiguration(Action<InitializationStatus> callback, long localClientId)
		{
			this.mInitializationCallback = Misc.CheckNotNull<Action<InitializationStatus>>(callback);
			this.mLocalClientId = localClientId;
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x000B50F3 File Offset: 0x000B32F3
		public long LocalClientId
		{
			get
			{
				return this.mLocalClientId;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x000B50FB File Offset: 0x000B32FB
		public Action<InitializationStatus> InitializationCallback
		{
			get
			{
				return this.mInitializationCallback;
			}
		}

		// Token: 0x04001EDD RID: 7901
		public const int MaxUnreliableMessagePayloadLength = 1168;

		// Token: 0x04001EDE RID: 7902
		public const int MaxReliableMessagePayloadLength = 4096;

		// Token: 0x04001EDF RID: 7903
		private readonly Action<InitializationStatus> mInitializationCallback;

		// Token: 0x04001EE0 RID: 7904
		private readonly long mLocalClientId;
	}
}
