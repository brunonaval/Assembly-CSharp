using System;

namespace kcp2k
{
	// Token: 0x0200000E RID: 14
	public static class Log
	{
		// Token: 0x04000051 RID: 81
		public static Action<string> Info = new Action<string>(Console.WriteLine);

		// Token: 0x04000052 RID: 82
		public static Action<string> Warning = new Action<string>(Console.WriteLine);

		// Token: 0x04000053 RID: 83
		public static Action<string> Error = new Action<string>(Console.Error.WriteLine);
	}
}
