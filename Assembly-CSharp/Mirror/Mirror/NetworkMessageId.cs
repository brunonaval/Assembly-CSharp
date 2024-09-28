using System;

namespace Mirror
{
	// Token: 0x02000051 RID: 81
	public static class NetworkMessageId<T> where T : struct, NetworkMessage
	{
		// Token: 0x04000102 RID: 258
		public static readonly ushort Id = (ushort)typeof(T).FullName.GetStableHashCode();
	}
}
