using System;

namespace Mirror
{
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandAttribute : Attribute
	{
		// Token: 0x04000007 RID: 7
		public int channel;

		// Token: 0x04000008 RID: 8
		public bool requiresAuthority = true;
	}
}
