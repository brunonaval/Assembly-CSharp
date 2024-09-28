using System;
using GooglePlayGames.OurUtils;

namespace GooglePlayGames.BasicApi.SavedGame
{
	// Token: 0x020006B1 RID: 1713
	public struct SavedGameMetadataUpdate
	{
		// Token: 0x06002593 RID: 9619 RVA: 0x000B4EAB File Offset: 0x000B30AB
		private SavedGameMetadataUpdate(SavedGameMetadataUpdate.Builder builder)
		{
			this.mDescriptionUpdated = builder.mDescriptionUpdated;
			this.mNewDescription = builder.mNewDescription;
			this.mCoverImageUpdated = builder.mCoverImageUpdated;
			this.mNewPngCoverImage = builder.mNewPngCoverImage;
			this.mNewPlayedTime = builder.mNewPlayedTime;
		}

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x000B4EE9 File Offset: 0x000B30E9
		public bool IsDescriptionUpdated
		{
			get
			{
				return this.mDescriptionUpdated;
			}
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06002595 RID: 9621 RVA: 0x000B4EF1 File Offset: 0x000B30F1
		public string UpdatedDescription
		{
			get
			{
				return this.mNewDescription;
			}
		}

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06002596 RID: 9622 RVA: 0x000B4EF9 File Offset: 0x000B30F9
		public bool IsCoverImageUpdated
		{
			get
			{
				return this.mCoverImageUpdated;
			}
		}

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x06002597 RID: 9623 RVA: 0x000B4F01 File Offset: 0x000B3101
		public byte[] UpdatedPngCoverImage
		{
			get
			{
				return this.mNewPngCoverImage;
			}
		}

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x06002598 RID: 9624 RVA: 0x000B4F09 File Offset: 0x000B3109
		public bool IsPlayedTimeUpdated
		{
			get
			{
				return this.mNewPlayedTime != null;
			}
		}

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x06002599 RID: 9625 RVA: 0x000B4F16 File Offset: 0x000B3116
		public TimeSpan? UpdatedPlayedTime
		{
			get
			{
				return this.mNewPlayedTime;
			}
		}

		// Token: 0x04001EBC RID: 7868
		private readonly bool mDescriptionUpdated;

		// Token: 0x04001EBD RID: 7869
		private readonly string mNewDescription;

		// Token: 0x04001EBE RID: 7870
		private readonly bool mCoverImageUpdated;

		// Token: 0x04001EBF RID: 7871
		private readonly byte[] mNewPngCoverImage;

		// Token: 0x04001EC0 RID: 7872
		private readonly TimeSpan? mNewPlayedTime;

		// Token: 0x020006B2 RID: 1714
		public struct Builder
		{
			// Token: 0x0600259A RID: 9626 RVA: 0x000B4F1E File Offset: 0x000B311E
			public SavedGameMetadataUpdate.Builder WithUpdatedDescription(string description)
			{
				this.mNewDescription = Misc.CheckNotNull<string>(description);
				this.mDescriptionUpdated = true;
				return this;
			}

			// Token: 0x0600259B RID: 9627 RVA: 0x000B4F39 File Offset: 0x000B3139
			public SavedGameMetadataUpdate.Builder WithUpdatedPngCoverImage(byte[] newPngCoverImage)
			{
				this.mCoverImageUpdated = true;
				this.mNewPngCoverImage = newPngCoverImage;
				return this;
			}

			// Token: 0x0600259C RID: 9628 RVA: 0x000B4F4F File Offset: 0x000B314F
			public SavedGameMetadataUpdate.Builder WithUpdatedPlayedTime(TimeSpan newPlayedTime)
			{
				if (newPlayedTime.TotalMilliseconds > 1.8446744073709552E+19)
				{
					throw new InvalidOperationException("Timespans longer than ulong.MaxValue milliseconds are not allowed");
				}
				this.mNewPlayedTime = new TimeSpan?(newPlayedTime);
				return this;
			}

			// Token: 0x0600259D RID: 9629 RVA: 0x000B4F80 File Offset: 0x000B3180
			public SavedGameMetadataUpdate Build()
			{
				return new SavedGameMetadataUpdate(this);
			}

			// Token: 0x04001EC1 RID: 7873
			internal bool mDescriptionUpdated;

			// Token: 0x04001EC2 RID: 7874
			internal string mNewDescription;

			// Token: 0x04001EC3 RID: 7875
			internal bool mCoverImageUpdated;

			// Token: 0x04001EC4 RID: 7876
			internal byte[] mNewPngCoverImage;

			// Token: 0x04001EC5 RID: 7877
			internal TimeSpan? mNewPlayedTime;
		}
	}
}
