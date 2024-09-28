using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006AF RID: 1711
	public interface IConflictResolver
	{
		// Token: 0x0600258B RID: 9611
		void ChooseMetadata(ISavedGameMetadata chosenMetadata);

		// Token: 0x0600258C RID: 9612
		void ResolveConflict(ISavedGameMetadata chosenMetadata, SavedGameMetadataUpdate metadataUpdate, byte[] updatedData);
	}
}
