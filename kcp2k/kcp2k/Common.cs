using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace kcp2k
{
	// Token: 0x02000003 RID: 3
	public static class Common
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		public static bool ResolveHostname(string hostname, out IPAddress[] addresses)
		{
			bool result;
			try
			{
				addresses = Dns.GetHostAddresses(hostname);
				result = (addresses.Length >= 1);
			}
			catch (SocketException arg)
			{
				Log.Info(string.Format("Failed to resolve host: {0} reason: {1}", hostname, arg));
				addresses = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002114 File Offset: 0x00000314
		public static void ConfigureSocketBuffers(Socket socket, int recvBufferSize, int sendBufferSize)
		{
			int receiveBufferSize = socket.ReceiveBufferSize;
			int sendBufferSize2 = socket.SendBufferSize;
			try
			{
				socket.ReceiveBufferSize = recvBufferSize;
				socket.SendBufferSize = sendBufferSize;
			}
			catch (SocketException)
			{
				Log.Warning(string.Format("Kcp: failed to set Socket RecvBufSize = {0} SendBufSize = {1}", recvBufferSize, sendBufferSize));
			}
			Log.Info(string.Format("Kcp: RecvBuf = {0}=>{1} ({2}x) SendBuf = {3}=>{4} ({5}x)", new object[]
			{
				receiveBufferSize,
				socket.ReceiveBufferSize,
				socket.ReceiveBufferSize / receiveBufferSize,
				sendBufferSize2,
				socket.SendBufferSize,
				socket.SendBufferSize / sendBufferSize2
			}));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021DC File Offset: 0x000003DC
		public static int ConnectionHash(EndPoint endPoint)
		{
			return endPoint.GetHashCode();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021E4 File Offset: 0x000003E4
		public static uint GenerateCookie()
		{
			Common.cryptoRandom.GetBytes(Common.cryptoRandomBuffer);
			return BitConverter.ToUInt32(Common.cryptoRandomBuffer, 0);
		}

		// Token: 0x04000001 RID: 1
		private static readonly RNGCryptoServiceProvider cryptoRandom = new RNGCryptoServiceProvider();

		// Token: 0x04000002 RID: 2
		private static readonly byte[] cryptoRandomBuffer = new byte[4];
	}
}
