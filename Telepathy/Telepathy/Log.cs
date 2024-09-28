using System;

namespace Telepathy
{
	// Token: 0x02000008 RID: 8
	public static class Log
	{
		// Token: 0x04000015 RID: 21
		public static Action<string> Info = new Action<string>(Console.WriteLine);

		// Token: 0x04000016 RID: 22
		public static Action<string> Warning = new Action<string>(Console.WriteLine);

		// Token: 0x04000017 RID: 23
		public static Action<string> Error = new Action<string>(Console.Error.WriteLine);
	}
}
