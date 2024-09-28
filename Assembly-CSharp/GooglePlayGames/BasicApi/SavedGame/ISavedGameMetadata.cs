using System;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006B0 RID: 1712
	public interface ISavedGameMetadata
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600258D RID: 9613
		bool IsOpen { get; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600258E RID: 9614
		string Filename { get; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600258F RID: 9615
		string Description { get; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06002590 RID: 9616
		string CoverImageURL { get; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06002591 RID: 9617
		TimeSpan TotalTimePlayed { get; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06002592 RID: 9618
		DateTime LastModifiedTimestamp { get; }
	}
}
