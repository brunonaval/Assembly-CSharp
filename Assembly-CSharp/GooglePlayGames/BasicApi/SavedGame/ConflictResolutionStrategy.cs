using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006AA RID: 1706
	public enum ConflictResolutionStrategy
	{
		// Token: 0x04001EA8 RID: 7848
		UseLongestPlaytime,
		// Token: 0x04001EA9 RID: 7849
		UseOriginal,
		// Token: 0x04001EAA RID: 7850
		UseUnmerged,
		// Token: 0x04001EAB RID: 7851
		UseManual,
		// Token: 0x04001EAC RID: 7852
		UseLastKnownGood,
		// Token: 0x04001EAD RID: 7853
		UseMostRecentlySaved
	}
}
