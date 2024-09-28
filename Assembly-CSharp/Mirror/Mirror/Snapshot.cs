using System;

namespace Mirror
{
	// Token: 0x02000067 RID: 103
	public interface Snapshot
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600031C RID: 796
		// (set) Token: 0x0600031D RID: 797
		double remoteTime { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600031E RID: 798
		// (set) Token: 0x0600031F RID: 799
		double localTime { get; set; }
	}
}
