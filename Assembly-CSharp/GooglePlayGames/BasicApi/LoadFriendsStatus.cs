using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x0200069C RID: 1692
	public enum LoadFriendsStatus
	{
		// Token: 0x04001E6C RID: 7788
		Unknown,
		// Token: 0x04001E6D RID: 7789
		Completed,
		// Token: 0x04001E6E RID: 7790
		LoadMore,
		// Token: 0x04001E6F RID: 7791
		ResolutionRequired = -3,
		// Token: 0x04001E70 RID: 7792
		InternalError = -4,
		// Token: 0x04001E71 RID: 7793
		NotAuthorized = -5,
		// Token: 0x04001E72 RID: 7794
		NetworkError = -6
	}
}
