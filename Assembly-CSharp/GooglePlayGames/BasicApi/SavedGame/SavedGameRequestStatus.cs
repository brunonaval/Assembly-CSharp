using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006AB RID: 1707
	public enum SavedGameRequestStatus
	{
		// Token: 0x04001EAF RID: 7855
		Success = 1,
		// Token: 0x04001EB0 RID: 7856
		TimeoutError = -1,
		// Token: 0x04001EB1 RID: 7857
		InternalError = -2,
		// Token: 0x04001EB2 RID: 7858
		AuthenticationError = -3,
		// Token: 0x04001EB3 RID: 7859
		BadInputError = -4
	}
}
