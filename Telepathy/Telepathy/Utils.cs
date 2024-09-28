using System;

namespace Telepathy
{
	// Token: 0x0200000F RID: 15
	public static class Utils
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003628 File Offset: 0x00001828
		public static void IntToBytesBigEndianNonAlloc(int value, byte[] bytes, int offset = 0)
		{
			bytes[offset] = (byte)(value >> 24);
			bytes[offset + 1] = (byte)(value >> 16);
			bytes[offset + 2] = (byte)(value >> 8);
			bytes[offset + 3] = (byte)value;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000364C File Offset: 0x0000184C
		public static int BytesToIntBigEndian(byte[] bytes)
		{
			return (int)bytes[0] << 24 | (int)bytes[1] << 16 | (int)bytes[2] << 8 | (int)bytes[3];
		}
	}
}
