using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000696 RID: 1686
	public enum ResponseStatus
	{
		// Token: 0x04001E49 RID: 7753
		Success = 1,
		// Token: 0x04001E4A RID: 7754
		SuccessWithStale,
		// Token: 0x04001E4B RID: 7755
		LicenseCheckFailed = -1,
		// Token: 0x04001E4C RID: 7756
		InternalError = -2,
		// Token: 0x04001E4D RID: 7757
		NotAuthorized = -3,
		// Token: 0x04001E4E RID: 7758
		VersionUpdateRequired = -4,
		// Token: 0x04001E4F RID: 7759
		Timeout = -5,
		// Token: 0x04001E50 RID: 7760
		ResolutionRequired = -6
	}
}
