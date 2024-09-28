using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200005F RID: 95
	public static class NetworkTime
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000B412 File Offset: 0x00009612
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0000B419 File Offset: 0x00009619
		[Obsolete("NetworkTime.PingFrequency was renamed to PingInterval, because we use it as seconds, not as Hz. Please rename all usages, but keep using it just as before.")]
		public static float PingFrequency
		{
			get
			{
				return NetworkTime.PingInterval;
			}
			set
			{
				NetworkTime.PingInterval = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x0000B421 File Offset: 0x00009621
		public static double localTime
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Time.unscaledTimeAsDouble;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000B428 File Offset: 0x00009628
		public static double time
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (!NetworkServer.active)
				{
					return NetworkClient.localTimeline;
				}
				return NetworkTime.localTime;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000B43C File Offset: 0x0000963C
		public static double offset
		{
			get
			{
				return NetworkTime.localTime - NetworkTime.time;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000B449 File Offset: 0x00009649
		public static double rtt
		{
			get
			{
				return NetworkTime._rtt.Value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000B455 File Offset: 0x00009655
		public static double rttVariance
		{
			get
			{
				return NetworkTime._rtt.Variance;
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000B461 File Offset: 0x00009661
		[RuntimeInitializeOnLoadMethod]
		public static void ResetStatics()
		{
			NetworkTime.PingInterval = 2f;
			NetworkTime.PingWindowSize = 6;
			NetworkTime.lastPingTime = 0.0;
			NetworkTime._rtt = new ExponentialMovingAverage(NetworkTime.PingWindowSize);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000B490 File Offset: 0x00009690
		internal static void UpdateClient()
		{
			if (NetworkTime.localTime >= NetworkTime.lastPingTime + (double)NetworkTime.PingInterval)
			{
				NetworkClient.Send<NetworkPingMessage>(new NetworkPingMessage(NetworkTime.localTime), 1);
				NetworkTime.lastPingTime = NetworkTime.localTime;
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000B4C0 File Offset: 0x000096C0
		internal static void OnServerPing(NetworkConnectionToClient conn, NetworkPingMessage message)
		{
			NetworkPongMessage message2 = new NetworkPongMessage
			{
				localTime = message.localTime
			};
			conn.Send<NetworkPongMessage>(message2, 1);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000B4EC File Offset: 0x000096EC
		internal static void OnClientPong(NetworkPongMessage message)
		{
			if (message.localTime > NetworkTime.localTime)
			{
				return;
			}
			double newValue = NetworkTime.localTime - message.localTime;
			NetworkTime._rtt.Add(newValue);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000B520 File Offset: 0x00009720
		internal static void OnClientPing(NetworkPingMessage message)
		{
			NetworkClient.Send<NetworkPongMessage>(new NetworkPongMessage
			{
				localTime = message.localTime
			}, 1);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000B54C File Offset: 0x0000974C
		internal static void OnServerPong(NetworkConnectionToClient conn, NetworkPongMessage message)
		{
			if (message.localTime > NetworkTime.localTime)
			{
				return;
			}
			double newValue = NetworkTime.localTime - message.localTime;
			conn._rtt.Add(newValue);
		}

		// Token: 0x0400012B RID: 299
		public static float PingInterval = 2f;

		// Token: 0x0400012C RID: 300
		public static int PingWindowSize = 6;

		// Token: 0x0400012D RID: 301
		private static double lastPingTime;

		// Token: 0x0400012E RID: 302
		private static ExponentialMovingAverage _rtt = new ExponentialMovingAverage(NetworkTime.PingWindowSize);
	}
}
