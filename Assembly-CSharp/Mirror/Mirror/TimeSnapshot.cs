using System;

namespace Mirror
{
	// Token: 0x0200006B RID: 107
	public struct TimeSnapshot : Snapshot
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600032B RID: 811 RVA: 0x0000C316 File Offset: 0x0000A516
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0000C31E File Offset: 0x0000A51E
		public double remoteTime { readonly get; set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000C327 File Offset: 0x0000A527
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000C32F File Offset: 0x0000A52F
		public double localTime { readonly get; set; }

		// Token: 0x0600032F RID: 815 RVA: 0x0000C338 File Offset: 0x0000A538
		public TimeSnapshot(double remoteTime, double localTime)
		{
			this.remoteTime = remoteTime;
			this.localTime = localTime;
		}
	}
}
