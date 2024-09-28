using System;

namespace Mirror.RemoteCalls
{
	// Token: 0x02000092 RID: 146
	internal class Invoker
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x0000EC1C File Offset: 0x0000CE1C
		public bool AreEqual(Type componentType, RemoteCallType remoteCallType, RemoteCallDelegate invokeFunction)
		{
			return this.componentType == componentType && this.callType == remoteCallType && this.function == invokeFunction;
		}

		// Token: 0x040001A2 RID: 418
		public Type componentType;

		// Token: 0x040001A3 RID: 419
		public RemoteCallType callType;

		// Token: 0x040001A4 RID: 420
		public RemoteCallDelegate function;

		// Token: 0x040001A5 RID: 421
		public bool cmdRequiresAuthority;
	}
}
