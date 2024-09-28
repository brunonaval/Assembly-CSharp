using System;

namespace Mirror
{
	// Token: 0x02000008 RID: 8
	[AttributeUsage(AttributeTargets.Method)]
	public class ClientRpcAttribute : Attribute
	{
		// Token: 0x04000009 RID: 9
		public int channel;

		// Token: 0x0400000A RID: 10
		public bool includeOwner = true;
	}
}
