using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200007F RID: 127
	public static class Compression
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x0000D9A8 File Offset: 0x0000BBA8
		public static bool ScaleToLong(float value, float precision, out long result)
		{
			if (precision == 0f)
			{
				throw new DivideByZeroException("ScaleToLong: precision=0 would cause null division. If rounding isn't wanted, don't call this function.");
			}
			bool result2;
			try
			{
				result = Convert.ToInt64(value / precision);
				result2 = true;
			}
			catch (OverflowException)
			{
				result = ((value > 0f) ? long.MaxValue : long.MinValue);
				result2 = false;
			}
			return result2;
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000DA0C File Offset: 0x0000BC0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ScaleToLong(Vector3 value, float precision, out long x, out long y, out long z)
		{
			return true & Compression.ScaleToLong(value.x, precision, out x) & Compression.ScaleToLong(value.y, precision, out y) & Compression.ScaleToLong(value.z, precision, out z);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000DA3A File Offset: 0x0000BC3A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool ScaleToLong(Vector3 value, float precision, out Vector3Long quantized)
		{
			quantized = Vector3Long.zero;
			return Compression.ScaleToLong(value, precision, out quantized.x, out quantized.y, out quantized.z);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000DA60 File Offset: 0x0000BC60
		public static float ScaleToFloat(long value, float precision)
		{
			if (precision == 0f)
			{
				throw new DivideByZeroException("ScaleToLong: precision=0 would cause null division. If rounding isn't wanted, don't call this function.");
			}
			return (float)value * precision;
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ScaleToFloat(long x, long y, long z, float precision)
		{
			Vector3 result;
			result.x = Compression.ScaleToFloat(x, precision);
			result.y = Compression.ScaleToFloat(y, precision);
			result.z = Compression.ScaleToFloat(z, precision);
			return result;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000DAB4 File Offset: 0x0000BCB4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ScaleToFloat(Vector3Long value, float precision)
		{
			return Compression.ScaleToFloat(value.x, value.y, value.z, precision);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000DAD0 File Offset: 0x0000BCD0
		public static ushort ScaleFloatToUShort(float value, float minValue, float maxValue, ushort minTarget, ushort maxTarget)
		{
			int num = (int)(maxTarget - minTarget);
			float num2 = maxValue - minValue;
			float num3 = value - minValue;
			return minTarget + (ushort)(num3 / num2 * (float)num);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		public static float ScaleUShortToFloat(ushort value, ushort minValue, ushort maxValue, float minTarget, float maxTarget)
		{
			float num = maxTarget - minTarget;
			ushort num2 = maxValue - minValue;
			ushort num3 = value - minValue;
			return minTarget + (float)num3 / (float)num2 * num;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		public static int LargestAbsoluteComponentIndex(Vector4 value, out float largestAbs, out Vector3 withoutLargest)
		{
			Vector4 vector = new Vector4(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z), Mathf.Abs(value.w));
			largestAbs = vector.x;
			withoutLargest = new Vector3(value.y, value.z, value.w);
			int result = 0;
			if (vector.y > largestAbs)
			{
				result = 1;
				largestAbs = vector.y;
				withoutLargest = new Vector3(value.x, value.z, value.w);
			}
			if (vector.z > largestAbs)
			{
				result = 2;
				largestAbs = vector.z;
				withoutLargest = new Vector3(value.x, value.y, value.w);
			}
			if (vector.w > largestAbs)
			{
				result = 3;
				largestAbs = vector.w;
				withoutLargest = new Vector3(value.x, value.y, value.z);
			}
			return result;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000DC18 File Offset: 0x0000BE18
		public static uint CompressQuaternion(Quaternion q)
		{
			float num2;
			Vector3 vector;
			int num = Compression.LargestAbsoluteComponentIndex(new Vector4(q.x, q.y, q.z, q.w), out num2, out vector);
			if (q[num] < 0f)
			{
				vector = -vector;
			}
			ushort num3 = Compression.ScaleFloatToUShort(vector.x, -0.707107f, 0.707107f, 0, 1023);
			ushort num4 = Compression.ScaleFloatToUShort(vector.y, -0.707107f, 0.707107f, 0, 1023);
			ushort num5 = Compression.ScaleFloatToUShort(vector.z, -0.707107f, 0.707107f, 0, 1023);
			return (uint)(num << 30 | (int)num3 << 20 | (int)num4 << 10 | (int)num5);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000DCC8 File Offset: 0x0000BEC8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Quaternion QuaternionNormalizeSafe(Quaternion value)
		{
			Vector4 vector = new Vector4(value.x, value.y, value.z, value.w);
			if (Vector4.Dot(vector, vector) <= 1.1754944E-38f)
			{
				return Quaternion.identity;
			}
			return value.normalized;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000DD04 File Offset: 0x0000BF04
		public static Quaternion DecompressQuaternion(uint data)
		{
			ushort value = (ushort)(data & 1023U);
			ushort value2 = (ushort)(data >> 10 & 1023U);
			ushort value3 = (ushort)(data >> 20 & 1023U);
			int num = (int)(data >> 30);
			float num2 = Compression.ScaleUShortToFloat(value3, 0, 1023, -0.707107f, 0.707107f);
			float num3 = Compression.ScaleUShortToFloat(value2, 0, 1023, -0.707107f, 0.707107f);
			float num4 = Compression.ScaleUShortToFloat(value, 0, 1023, -0.707107f, 0.707107f);
			float num5 = Mathf.Sqrt(1f - num2 * num2 - num3 * num3 - num4 * num4);
			Vector4 vector;
			switch (num)
			{
			case 0:
				vector = new Vector4(num5, num2, num3, num4);
				break;
			case 1:
				vector = new Vector4(num2, num5, num3, num4);
				break;
			case 2:
				vector = new Vector4(num2, num3, num5, num4);
				break;
			default:
				vector = new Vector4(num2, num3, num4, num5);
				break;
			}
			return Compression.QuaternionNormalizeSafe(new Quaternion(vector.x, vector.y, vector.z, vector.w));
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000DE00 File Offset: 0x0000C000
		public static int VarUIntSize(ulong value)
		{
			if (value <= 240UL)
			{
				return 1;
			}
			if (value <= 2287UL)
			{
				return 2;
			}
			if (value <= 67823UL)
			{
				return 3;
			}
			if (value <= 16777215UL)
			{
				return 4;
			}
			if (value <= (ulong)-1)
			{
				return 5;
			}
			if (value <= 1099511627775UL)
			{
				return 6;
			}
			if (value <= 281474976710655UL)
			{
				return 7;
			}
			if (value <= 72057594037927935UL)
			{
				return 8;
			}
			return 9;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000DE6C File Offset: 0x0000C06C
		public static int VarIntSize(long value)
		{
			return Compression.VarUIntSize((ulong)(value >> 63 ^ value << 1));
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000DE7C File Offset: 0x0000C07C
		public static void CompressVarUInt(NetworkWriter writer, ulong value)
		{
			if (value <= 240UL)
			{
				byte value2 = (byte)value;
				writer.WriteByte(value2);
				return;
			}
			if (value <= 2287UL)
			{
				byte b = (byte)((value - 240UL >> 8) + 241UL);
				byte b2 = (byte)(value - 240UL & 255UL);
				writer.WriteUShort((ushort)((int)b2 << 8 | (int)b));
				return;
			}
			if (value <= 67823UL)
			{
				byte value3 = 249;
				byte b3 = (byte)(value - 2288UL >> 8);
				byte b4 = (byte)(value - 2288UL & 255UL);
				writer.WriteByte(value3);
				writer.WriteUShort((ushort)((int)b4 << 8 | (int)b3));
				return;
			}
			if (value <= 16777215UL)
			{
				byte b5 = 250;
				uint num = (uint)((uint)value << 8);
				writer.WriteUInt(num | (uint)b5);
				return;
			}
			if (value <= (ulong)-1)
			{
				byte value4 = 251;
				uint value5 = (uint)value;
				writer.WriteByte(value4);
				writer.WriteUInt(value5);
				return;
			}
			if (value <= 1099511627775UL)
			{
				byte b6 = 252;
				byte b7 = (byte)(value & 255UL);
				uint value6 = (uint)(value >> 8);
				writer.WriteUShort((ushort)((int)b7 << 8 | (int)b6));
				writer.WriteUInt(value6);
				return;
			}
			if (value <= 281474976710655UL)
			{
				byte value7 = 253;
				byte b8 = (byte)(value & 255UL);
				byte b9 = (byte)(value >> 8 & 255UL);
				uint value8 = (uint)(value >> 16);
				writer.WriteByte(value7);
				writer.WriteUShort((ushort)((int)b9 << 8 | (int)b8));
				writer.WriteUInt(value8);
				return;
			}
			if (value <= 72057594037927935UL)
			{
				byte b10 = 254;
				ulong num2 = value << 8;
				writer.WriteULong(num2 | (ulong)b10);
				return;
			}
			writer.WriteByte(byte.MaxValue);
			writer.WriteULong(value);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000E024 File Offset: 0x0000C224
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CompressVarInt(NetworkWriter writer, long i)
		{
			ulong value = (ulong)(i >> 63 ^ i << 1);
			Compression.CompressVarUInt(writer, value);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000E044 File Offset: 0x0000C244
		public static ulong DecompressVarUInt(NetworkReader reader)
		{
			byte b = reader.ReadByte();
			if (b < 241)
			{
				return (ulong)b;
			}
			byte b2 = reader.ReadByte();
			if (b <= 248)
			{
				return 240UL + ((ulong)b - 241UL << 8) + (ulong)b2;
			}
			byte b3 = reader.ReadByte();
			if (b == 249)
			{
				return 2288UL + ((ulong)b2 << 8) + (ulong)b3;
			}
			byte b4 = reader.ReadByte();
			if (b == 250)
			{
				return (ulong)b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16);
			}
			byte b5 = reader.ReadByte();
			if (b == 251)
			{
				return (ulong)b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24);
			}
			byte b6 = reader.ReadByte();
			if (b == 252)
			{
				return (ulong)b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32);
			}
			byte b7 = reader.ReadByte();
			if (b == 253)
			{
				return (ulong)b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32) + ((ulong)b7 << 40);
			}
			byte b8 = reader.ReadByte();
			if (b == 254)
			{
				return (ulong)b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32) + ((ulong)b7 << 40) + ((ulong)b8 << 48);
			}
			byte b9 = reader.ReadByte();
			if (b == 255)
			{
				return (ulong)b2 + ((ulong)b3 << 8) + ((ulong)b4 << 16) + ((ulong)b5 << 24) + ((ulong)b6 << 32) + ((ulong)b7 << 40) + ((ulong)b8 << 48) + ((ulong)b9 << 56);
			}
			throw new IndexOutOfRangeException(string.Format("DecompressVarInt failure: {0}", b));
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long DecompressVarInt(NetworkReader reader)
		{
			ulong num = Compression.DecompressVarUInt(reader);
			return (long)(num >> 1 ^ -(long)(num & 1UL));
		}

		// Token: 0x04000170 RID: 368
		private const float QuaternionMinRange = -0.707107f;

		// Token: 0x04000171 RID: 369
		private const float QuaternionMaxRange = 0.707107f;

		// Token: 0x04000172 RID: 370
		private const ushort TenBitsMax = 1023;
	}
}
