using System;

namespace Mirror
{
	// Token: 0x0200002D RID: 45
	public struct NetworkPingMessage : NetworkMessage
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00002F97 File Offset: 0x00001197
		public NetworkPingMessage(double value)
		{
			this.localTime = value;
		}

		// Token: 0x04000049 RID: 73
		public double localTime;
	}
}
