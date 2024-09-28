using System;

namespace Telepathy
{
	// Token: 0x02000005 RID: 5
	public abstract class Common
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000025CC File Offset: 0x000007CC
		protected Common(int MaxMessageSize)
		{
			this.MaxMessageSize = MaxMessageSize;
		}

		// Token: 0x0400000A RID: 10
		public bool NoDelay = true;

		// Token: 0x0400000B RID: 11
		public readonly int MaxMessageSize;

		// Token: 0x0400000C RID: 12
		public int SendTimeout = 5000;

		// Token: 0x0400000D RID: 13
		public int ReceiveTimeout;
	}
}
