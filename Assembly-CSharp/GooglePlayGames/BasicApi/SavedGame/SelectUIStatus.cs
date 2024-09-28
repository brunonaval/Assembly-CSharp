using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006AC RID: 1708
	public enum SelectUIStatus
	{
		// Token: 0x04001EB5 RID: 7861
		SavedGameSelected = 1,
		// Token: 0x04001EB6 RID: 7862
		UserClosedUI,
		// Token: 0x04001EB7 RID: 7863
		InternalError = -1,
		// Token: 0x04001EB8 RID: 7864
		TimeoutError = -2,
		// Token: 0x04001EB9 RID: 7865
		AuthenticationError = -3,
		// Token: 0x04001EBA RID: 7866
		BadInputError = -4,
		// Token: 0x04001EBB RID: 7867
		UiBusy = -5
	}
}
