using System;
using System.Runtime.CompilerServices;

namespace kcp2k
{
	// Token: 0x02000013 RID: 19
	public static class Utils
	{
		// Token: 0x0600006B RID: 107 RVA: 0x00004F6B File Offset: 0x0000316B
		public static int Clamp(int value, int min, int max)
		{
			if (value < min)
			{
				return min;
			}
			if (value > max)
			{
				return max;
			}
			return value;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004F7A File Offset: 0x0000317A
		public static int Encode8u(byte[] p, int offset, byte value)
		{
			p[offset] = value;
			return 1;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004F81 File Offset: 0x00003181
		public static int Decode8u(byte[] p, int offset, out byte value)
		{
			value = p[offset];
			return 1;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004F89 File Offset: 0x00003189
		public static int Encode16U(byte[] p, int offset, ushort value)
		{
			p[offset] = (byte)value;
			p[1 + offset] = (byte)(value >> 8);
			return 2;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004F9C File Offset: 0x0000319C
		public static int Decode16U(byte[] p, int offset, out ushort value)
		{
			ushort num = 0;
			num |= (ushort)p[offset];
			num |= (ushort)(p[1 + offset] << 8);
			value = num;
			return 2;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004FC2 File Offset: 0x000031C2
		public static int Encode32U(byte[] p, int offset, uint value)
		{
			p[offset] = (byte)value;
			p[1 + offset] = (byte)(value >> 8);
			p[2 + offset] = (byte)(value >> 16);
			p[3 + offset] = (byte)(value >> 24);
			return 4;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004FE8 File Offset: 0x000031E8
		public static int Decode32U(byte[] p, int offset, out uint value)
		{
			uint num = 0U;
			num |= (uint)p[offset];
			num |= (uint)((uint)p[1 + offset] << 8);
			num |= (uint)((uint)p[2 + offset] << 16);
			num |= (uint)((uint)p[3 + offset] << 24);
			value = num;
			return 4;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005021 File Offset: 0x00003221
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int TimeDiff(uint later, uint earlier)
		{
			return (int)(later - earlier);
		}
	}
}
