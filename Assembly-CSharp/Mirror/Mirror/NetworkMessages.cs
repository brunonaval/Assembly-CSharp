using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000052 RID: 82
	public static class NetworkMessages
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00008D30 File Offset: 0x00006F30
		public static void LogTypes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("NetworkMessageIds:");
			foreach (KeyValuePair<ushort, Type> keyValuePair in NetworkMessages.Lookup)
			{
				stringBuilder.AppendLine(string.Format("  Id={0} = {1}", keyValuePair.Key, keyValuePair.Value));
			}
			Debug.Log(stringBuilder.ToString());
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00008DBC File Offset: 0x00006FBC
		public static int MaxContentSize(int channelId)
		{
			int maxPacketSize = Transport.active.GetMaxPacketSize(channelId);
			return maxPacketSize - 2 - Batcher.MaxMessageOverhead(maxPacketSize);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00008DDF File Offset: 0x00006FDF
		public static int MaxMessageSize(int channelId)
		{
			return NetworkMessages.MaxContentSize(channelId) + 2;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00008DE9 File Offset: 0x00006FE9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ushort GetId<T>() where T : struct, NetworkMessage
		{
			return NetworkMessageId<T>.Id;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00008DF0 File Offset: 0x00006FF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Pack<T>(T message, NetworkWriter writer) where T : struct, NetworkMessage
		{
			writer.WriteUShort(NetworkMessageId<T>.Id);
			writer.Write<T>(message);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00008E04 File Offset: 0x00007004
		public static bool UnpackId(NetworkReader reader, out ushort messageId)
		{
			bool result;
			try
			{
				messageId = reader.ReadUShort();
				result = true;
			}
			catch (EndOfStreamException)
			{
				messageId = 0;
				result = false;
			}
			return result;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00008E38 File Offset: 0x00007038
		internal static NetworkMessageDelegate WrapHandler<T, C>(Action<C, T, int> handler, bool requireAuthentication) where T : struct, NetworkMessage where C : NetworkConnection
		{
			return delegate(NetworkConnection conn, NetworkReader reader, int channelId)
			{
				T t = default(T);
				int position = reader.Position;
				try
				{
					if (requireAuthentication && !conn.isAuthenticated)
					{
						Debug.LogWarning(string.Format("Disconnecting connection: {0}. Received message {1} that required authentication, but the user has not authenticated yet", conn, typeof(T)));
						conn.Disconnect();
						return;
					}
					t = reader.Read<T>();
				}
				catch (Exception arg)
				{
					Debug.LogError(string.Format("Disconnecting connection: {0}. This can happen if the other side accidentally (or an attacker intentionally) sent invalid data. Reason: {1}", conn, arg));
					conn.Disconnect();
					return;
				}
				finally
				{
					int position2 = reader.Position;
					NetworkDiagnostics.OnReceive<T>(t, channelId, position2 - position);
				}
				try
				{
					handler((C)((object)conn), t, channelId);
				}
				catch (Exception ex)
				{
					Debug.LogError(string.Format("Disconnecting connId={0} to prevent exploits from an Exception in MessageHandler: {1} {2}\n{3}", new object[]
					{
						conn.connectionId,
						ex.GetType().Name,
						ex.Message,
						ex.StackTrace
					}));
					conn.Disconnect();
				}
			};
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00008E58 File Offset: 0x00007058
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static NetworkMessageDelegate WrapHandler<T, C>(Action<C, T> handler, bool requireAuthentication) where T : struct, NetworkMessage where C : NetworkConnection
		{
			NetworkMessages.<>c__DisplayClass9_0<T, C> CS$<>8__locals1 = new NetworkMessages.<>c__DisplayClass9_0<T, C>();
			CS$<>8__locals1.handler = handler;
			return NetworkMessages.WrapHandler<T, C>(new Action<C, T, int>(CS$<>8__locals1.<WrapHandler>g__Wrapped|0), requireAuthentication);
		}

		// Token: 0x04000103 RID: 259
		public const int IdSize = 2;

		// Token: 0x04000104 RID: 260
		public static readonly Dictionary<ushort, Type> Lookup = new Dictionary<ushort, Type>();
	}
}
