using System;

namespace Mirror
{
	// Token: 0x02000017 RID: 23
	public interface Capture
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002E RID: 46
		// (set) Token: 0x0600002F RID: 47
		double timestamp { get; set; }

		// Token: 0x06000030 RID: 48
		void DrawGizmo();
	}
}
