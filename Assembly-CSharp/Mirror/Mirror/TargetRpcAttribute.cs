using System;

namespace Mirror
{
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.Method)]
	public class TargetRpcAttribute : Attribute
	{
		// Token: 0x0400000B RID: 11
		public int channel;
	}
}
