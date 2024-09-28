using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006AD RID: 1709
	// (Invoke) Token: 0x06002581 RID: 9601
	public delegate void ConflictCallback(IConflictResolver resolver, ISavedGameMetadata original, byte[] originalData, ISavedGameMetadata unmerged, byte[] unmergedData);
}
