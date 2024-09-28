using System;
using System.IO;
using System.Net.Sockets;

namespace Telepathy
{
	// Token: 0x0200000B RID: 11
	public static class NetworkStreamExtensions
	{
		// Token: 0x06000020 RID: 32 RVA: 0x00002CDC File Offset: 0x00000EDC
		public static int ReadSafely(this NetworkStream stream, byte[] buffer, int offset, int size)
		{
			int result;
			try
			{
				result = stream.Read(buffer, offset, size);
			}
			catch (IOException)
			{
				result = 0;
			}
			catch (ObjectDisposedException)
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D1C File Offset: 0x00000F1C
		public static bool ReadExactly(this NetworkStream stream, byte[] buffer, int amount)
		{
			int num;
			for (int i = 0; i < amount; i += num)
			{
				int size = amount - i;
				num = stream.ReadSafely(buffer, i, size);
				if (num == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
