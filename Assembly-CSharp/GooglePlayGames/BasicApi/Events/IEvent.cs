using System;

namespace GooglePlayGames.BasicApi.Events
{
	// Token: 0x020006BE RID: 1726
	public interface IEvent
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060025C3 RID: 9667
		string Id { get; }

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060025C4 RID: 9668
		string Name { get; }

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060025C5 RID: 9669
		string Description { get; }

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060025C6 RID: 9670
		string ImageUrl { get; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060025C7 RID: 9671
		ulong CurrentCount { get; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060025C8 RID: 9672
		EventVisibility Visibility { get; }
	}
}
