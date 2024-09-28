using System;

namespace GooglePlayGames.BasicApi.Events
{
	// Token: 0x020006BC RID: 1724
	internal class Event : IEvent
	{
		// Token: 0x060025BC RID: 9660 RVA: 0x000B5103 File Offset: 0x000B3303
		internal Event(string id, string name, string description, string imageUrl, ulong currentCount, EventVisibility visibility)
		{
			this.mId = id;
			this.mName = name;
			this.mDescription = description;
			this.mImageUrl = imageUrl;
			this.mCurrentCount = currentCount;
			this.mVisibility = visibility;
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x000B5138 File Offset: 0x000B3338
		public string Id
		{
			get
			{
				return this.mId;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x060025BE RID: 9662 RVA: 0x000B5140 File Offset: 0x000B3340
		public string Name
		{
			get
			{
				return this.mName;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x000B5148 File Offset: 0x000B3348
		public string Description
		{
			get
			{
				return this.mDescription;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x000B5150 File Offset: 0x000B3350
		public string ImageUrl
		{
			get
			{
				return this.mImageUrl;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x000B5158 File Offset: 0x000B3358
		public ulong CurrentCount
		{
			get
			{
				return this.mCurrentCount;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x000B5160 File Offset: 0x000B3360
		public EventVisibility Visibility
		{
			get
			{
				return this.mVisibility;
			}
		}

		// Token: 0x04001EE1 RID: 7905
		private string mId;

		// Token: 0x04001EE2 RID: 7906
		private string mName;

		// Token: 0x04001EE3 RID: 7907
		private string mDescription;

		// Token: 0x04001EE4 RID: 7908
		private string mImageUrl;

		// Token: 0x04001EE5 RID: 7909
		private ulong mCurrentCount;

		// Token: 0x04001EE6 RID: 7910
		private EventVisibility mVisibility;
	}
}
