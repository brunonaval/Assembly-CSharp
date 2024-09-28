using System;
using System.Net;
using System.Net.Sockets;

namespace kcp2k
{
	// Token: 0x02000005 RID: 5
	public static class Extensions
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002217 File Offset: 0x00000417
		public static string ToHexString(this ArraySegment<byte> segment)
		{
			return BitConverter.ToString(segment.Array, segment.Offset, segment.Count);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002234 File Offset: 0x00000434
		public static bool SendToNonBlocking(this Socket socket, ArraySegment<byte> data, EndPoint remoteEP)
		{
			bool result;
			try
			{
				if (!socket.Poll(0, SelectMode.SelectWrite))
				{
					result = false;
				}
				else
				{
					socket.SendTo(data.Array, data.Offset, data.Count, SocketFlags.None, remoteEP);
					result = true;
				}
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode != SocketError.WouldBlock)
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002294 File Offset: 0x00000494
		public static bool SendNonBlocking(this Socket socket, ArraySegment<byte> data)
		{
			bool result;
			try
			{
				if (!socket.Poll(0, SelectMode.SelectWrite))
				{
					result = false;
				}
				else
				{
					socket.Send(data.Array, data.Offset, data.Count, SocketFlags.None);
					result = true;
				}
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode != SocketError.WouldBlock)
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022F4 File Offset: 0x000004F4
		public static bool ReceiveFromNonBlocking(this Socket socket, byte[] recvBuffer, out ArraySegment<byte> data, ref EndPoint remoteEP)
		{
			data = default(ArraySegment<byte>);
			bool result;
			try
			{
				if (!socket.Poll(0, SelectMode.SelectRead))
				{
					result = false;
				}
				else
				{
					int count = socket.ReceiveFrom(recvBuffer, 0, recvBuffer.Length, SocketFlags.None, ref remoteEP);
					data = new ArraySegment<byte>(recvBuffer, 0, count);
					result = true;
				}
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode != SocketError.WouldBlock)
				{
					throw;
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000235C File Offset: 0x0000055C
		public static bool ReceiveNonBlocking(this Socket socket, byte[] recvBuffer, out ArraySegment<byte> data)
		{
			data = default(ArraySegment<byte>);
			bool result;
			try
			{
				if (!socket.Poll(0, SelectMode.SelectRead))
				{
					result = false;
				}
				else
				{
					int count = socket.Receive(recvBuffer, 0, recvBuffer.Length, SocketFlags.None);
					data = new ArraySegment<byte>(recvBuffer, 0, count);
					result = true;
				}
			}
			catch (SocketException ex)
			{
				if (ex.SocketErrorCode != SocketError.WouldBlock)
				{
					throw;
				}
				result = false;
			}
			return result;
		}
	}
}
