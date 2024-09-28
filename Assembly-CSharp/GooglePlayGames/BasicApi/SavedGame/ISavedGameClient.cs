using System;
using System.Collections.Generic;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006AE RID: 1710
	public interface ISavedGameClient
	{
		// Token: 0x06002584 RID: 9604
		void OpenWithAutomaticConflictResolution(string filename, DataSource source, ConflictResolutionStrategy resolutionStrategy, Action<SavedGameRequestStatus, ISavedGameMetadata> callback);

		// Token: 0x06002585 RID: 9605
		void OpenWithManualConflictResolution(string filename, DataSource source, bool prefetchDataOnConflict, ConflictCallback conflictCallback, Action<SavedGameRequestStatus, ISavedGameMetadata> completedCallback);

		// Token: 0x06002586 RID: 9606
		void ReadBinaryData(ISavedGameMetadata metadata, Action<SavedGameRequestStatus, byte[]> completedCallback);

		// Token: 0x06002587 RID: 9607
		void ShowSelectSavedGameUI(string uiTitle, uint maxDisplayedSavedGames, bool showCreateSaveUI, bool showDeleteSaveUI, Action<SelectUIStatus, ISavedGameMetadata> callback);

		// Token: 0x06002588 RID: 9608
		void CommitUpdate(ISavedGameMetadata metadata, SavedGameMetadataUpdate updateForMetadata, byte[] updatedBinaryData, Action<SavedGameRequestStatus, ISavedGameMetadata> callback);

		// Token: 0x06002589 RID: 9609
		void FetchAllSavedGames(DataSource source, Action<SavedGameRequestStatus, List<ISavedGameMetadata>> callback);

		// Token: 0x0600258A RID: 9610
		void Delete(ISavedGameMetadata metadata);
	}
}
