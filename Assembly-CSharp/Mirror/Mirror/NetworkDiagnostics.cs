using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000041 RID: 65
	public static class NetworkDiagnostics
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000140 RID: 320 RVA: 0x00006828 File Offset: 0x00004A28
		// (remove) Token: 0x06000141 RID: 321 RVA: 0x0000685C File Offset: 0x00004A5C
		public static event Action<NetworkDiagnostics.MessageInfo> OutMessageEvent;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000142 RID: 322 RVA: 0x00006890 File Offset: 0x00004A90
		// (remove) Token: 0x06000143 RID: 323 RVA: 0x000068C4 File Offset: 0x00004AC4
		public static event Action<NetworkDiagnostics.MessageInfo> InMessageEvent;

		// Token: 0x06000144 RID: 324 RVA: 0x000068F7 File Offset: 0x00004AF7
		[RuntimeInitializeOnLoadMethod]
		private static void ResetStatics()
		{
			NetworkDiagnostics.InMessageEvent = null;
			NetworkDiagnostics.OutMessageEvent = null;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00006908 File Offset: 0x00004B08
		internal static void OnSend<T>(T message, int channel, int bytes, int count) where T : struct, NetworkMessage
		{
			if (count > 0 && NetworkDiagnostics.OutMessageEvent != null)
			{
				NetworkDiagnostics.MessageInfo obj = new NetworkDiagnostics.MessageInfo(message, channel, bytes, count);
				Action<NetworkDiagnostics.MessageInfo> outMessageEvent = NetworkDiagnostics.OutMessageEvent;
				if (outMessageEvent == null)
				{
					return;
				}
				outMessageEvent(obj);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00006940 File Offset: 0x00004B40
		internal static void OnReceive<T>(T message, int channel, int bytes) where T : struct, NetworkMessage
		{
			if (NetworkDiagnostics.InMessageEvent != null)
			{
				NetworkDiagnostics.MessageInfo obj = new NetworkDiagnostics.MessageInfo(message, channel, bytes, 1);
				Action<NetworkDiagnostics.MessageInfo> inMessageEvent = NetworkDiagnostics.InMessageEvent;
				if (inMessageEvent == null)
				{
					return;
				}
				inMessageEvent(obj);
			}
		}

		// Token: 0x02000042 RID: 66
		public readonly struct MessageInfo
		{
			// Token: 0x06000147 RID: 327 RVA: 0x00006974 File Offset: 0x00004B74
			internal MessageInfo(NetworkMessage message, int channel, int bytes, int count)
			{
				this.message = message;
				this.channel = channel;
				this.bytes = bytes;
				this.count = count;
			}

			// Token: 0x040000A8 RID: 168
			public readonly NetworkMessage message;

			// Token: 0x040000A9 RID: 169
			public readonly int channel;

			// Token: 0x040000AA RID: 170
			public readonly int bytes;

			// Token: 0x040000AB RID: 171
			public readonly int count;
		}
	}
}
