using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x02000697 RID: 1687
	public enum UIStatus
	{
		// Token: 0x04001E52 RID: 7762
		Valid = 1,
		// Token: 0x04001E53 RID: 7763
		InternalError = -2,
		// Token: 0x04001E54 RID: 7764
		NotAuthorized = -3,
		// Token: 0x04001E55 RID: 7765
		VersionUpdateRequired = -4,
		// Token: 0x04001E56 RID: 7766
		Timeout = -5,
		// Token: 0x04001E57 RID: 7767
		UserClosedUI = -6,
		// Token: 0x04001E58 RID: 7768
		UiBusy = -12,
		// Token: 0x04001E59 RID: 7769
		NetworkError = -20
	}
}
