﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
	// Token: 0x02000AF4 RID: 2804
	public static class Base64
	{
		// Token: 0x0600640C RID: 25612 RVA: 0x0014EC98 File Offset: 0x0014CE98
		public unsafe static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> bytes, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
		{
			ref byte reference = ref MemoryMarshal.GetReference<byte>(utf8);
			ref byte reference2 = ref MemoryMarshal.GetReference<byte>(bytes);
			int num = utf8.Length & -4;
			int length = bytes.Length;
			int i = 0;
			int num2 = 0;
			if (utf8.Length != 0)
			{
				ref sbyte ptr = ref Base64.s_decodingMap[0];
				int num3 = isFinalBlock ? 4 : 0;
				int num4;
				if (length >= Base64.GetMaxDecodedFromUtf8Length(num))
				{
					num4 = num - num3;
				}
				else
				{
					num4 = length / 3 * 4;
				}
				while (i < num4)
				{
					int num5 = Base64.Decode(Unsafe.Add<byte>(ref reference, i), ref ptr);
					if (num5 < 0)
					{
						IL_22B:
						bytesConsumed = i;
						bytesWritten = num2;
						return OperationStatus.InvalidData;
					}
					Base64.WriteThreeLowOrderBytes(Unsafe.Add<byte>(ref reference2, num2), num5);
					num2 += 3;
					i += 4;
				}
				if (num4 == num - num3)
				{
					if (i == num)
					{
						if (isFinalBlock)
						{
							goto IL_22B;
						}
						bytesConsumed = i;
						bytesWritten = num2;
						return OperationStatus.NeedMoreData;
					}
					else
					{
						int num6 = (int)(*Unsafe.Add<byte>(ref reference, num - 4));
						int num7 = (int)(*Unsafe.Add<byte>(ref reference, num - 3));
						int num8 = (int)(*Unsafe.Add<byte>(ref reference, num - 2));
						int num9 = (int)(*Unsafe.Add<byte>(ref reference, num - 1));
						num6 = (int)(*Unsafe.Add<sbyte>(ref ptr, num6));
						num7 = (int)(*Unsafe.Add<sbyte>(ref ptr, num7));
						num6 <<= 18;
						num7 <<= 12;
						num6 |= num7;
						if (num9 != 61)
						{
							num8 = (int)(*Unsafe.Add<sbyte>(ref ptr, num8));
							num9 = (int)(*Unsafe.Add<sbyte>(ref ptr, num9));
							num8 <<= 6;
							num6 |= num9;
							num6 |= num8;
							if (num6 < 0)
							{
								goto IL_22B;
							}
							if (num2 > length - 3)
							{
								goto IL_205;
							}
							Base64.WriteThreeLowOrderBytes(Unsafe.Add<byte>(ref reference2, num2), num6);
							num2 += 3;
						}
						else if (num8 != 61)
						{
							num8 = (int)(*Unsafe.Add<sbyte>(ref ptr, num8));
							num8 <<= 6;
							num6 |= num8;
							if (num6 < 0)
							{
								goto IL_22B;
							}
							if (num2 > length - 2)
							{
								goto IL_205;
							}
							*Unsafe.Add<byte>(ref reference2, num2) = (byte)(num6 >> 16);
							*Unsafe.Add<byte>(ref reference2, num2 + 1) = (byte)(num6 >> 8);
							num2 += 2;
						}
						else
						{
							if (num6 < 0)
							{
								goto IL_22B;
							}
							if (num2 > length - 1)
							{
								goto IL_205;
							}
							*Unsafe.Add<byte>(ref reference2, num2) = (byte)(num6 >> 16);
							num2++;
						}
						i += 4;
						if (num == utf8.Length)
						{
							goto IL_1FB;
						}
						goto IL_22B;
					}
				}
				IL_205:
				if (num == utf8.Length || !isFinalBlock)
				{
					bytesConsumed = i;
					bytesWritten = num2;
					return OperationStatus.DestinationTooSmall;
				}
				goto IL_22B;
			}
			IL_1FB:
			bytesConsumed = i;
			bytesWritten = num2;
			return OperationStatus.Done;
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x0014EED9 File Offset: 0x0014D0D9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetMaxDecodedFromUtf8Length(int length)
		{
			if (length < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			return (length >> 2) * 3;
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x0014EEEC File Offset: 0x0014D0EC
		public unsafe static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, out int bytesWritten)
		{
			int length = buffer.Length;
			int i = 0;
			int num = 0;
			if (length == (length >> 2) * 4)
			{
				if (length != 0)
				{
					ref byte reference = ref MemoryMarshal.GetReference<byte>(buffer);
					ref sbyte ptr = ref Base64.s_decodingMap[0];
					while (i < length - 4)
					{
						int num2 = Base64.Decode(Unsafe.Add<byte>(ref reference, i), ref ptr);
						if (num2 < 0)
						{
							goto IL_172;
						}
						Base64.WriteThreeLowOrderBytes(Unsafe.Add<byte>(ref reference, num), num2);
						num += 3;
						i += 4;
					}
					int num3 = (int)(*Unsafe.Add<byte>(ref reference, length - 4));
					int num4 = (int)(*Unsafe.Add<byte>(ref reference, length - 3));
					int num5 = (int)(*Unsafe.Add<byte>(ref reference, length - 2));
					int num6 = (int)(*Unsafe.Add<byte>(ref reference, length - 1));
					num3 = (int)(*Unsafe.Add<sbyte>(ref ptr, num3));
					num4 = (int)(*Unsafe.Add<sbyte>(ref ptr, num4));
					num3 <<= 18;
					num4 <<= 12;
					num3 |= num4;
					if (num6 != 61)
					{
						num5 = (int)(*Unsafe.Add<sbyte>(ref ptr, num5));
						num6 = (int)(*Unsafe.Add<sbyte>(ref ptr, num6));
						num5 <<= 6;
						num3 |= num6;
						num3 |= num5;
						if (num3 < 0)
						{
							goto IL_172;
						}
						Base64.WriteThreeLowOrderBytes(Unsafe.Add<byte>(ref reference, num), num3);
						num += 3;
					}
					else if (num5 != 61)
					{
						num5 = (int)(*Unsafe.Add<sbyte>(ref ptr, num5));
						num5 <<= 6;
						num3 |= num5;
						if (num3 < 0)
						{
							goto IL_172;
						}
						*Unsafe.Add<byte>(ref reference, num) = (byte)(num3 >> 16);
						*Unsafe.Add<byte>(ref reference, num + 1) = (byte)(num3 >> 8);
						num += 2;
					}
					else
					{
						if (num3 < 0)
						{
							goto IL_172;
						}
						*Unsafe.Add<byte>(ref reference, num) = (byte)(num3 >> 16);
						num++;
					}
				}
				bytesWritten = num;
				return OperationStatus.Done;
			}
			IL_172:
			bytesWritten = num;
			return OperationStatus.InvalidData;
		}

		// Token: 0x0600640F RID: 25615 RVA: 0x0014F070 File Offset: 0x0014D270
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static int Decode(ref byte encodedBytes, ref sbyte decodingMap)
		{
			int num = (int)encodedBytes;
			int num2 = (int)(*Unsafe.Add<byte>(ref encodedBytes, 1));
			int num3 = (int)(*Unsafe.Add<byte>(ref encodedBytes, 2));
			int num4 = (int)(*Unsafe.Add<byte>(ref encodedBytes, 3));
			num = (int)(*Unsafe.Add<sbyte>(ref decodingMap, num));
			num2 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, num2));
			num3 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, num3));
			num4 = (int)(*Unsafe.Add<sbyte>(ref decodingMap, num4));
			num <<= 18;
			num2 <<= 12;
			num3 <<= 6;
			num |= num4;
			num2 |= num3;
			return num | num2;
		}

		// Token: 0x06006410 RID: 25616 RVA: 0x00023545 File Offset: 0x00021745
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static void WriteThreeLowOrderBytes(ref byte destination, int value)
		{
			destination = (byte)(value >> 16);
			*Unsafe.Add<byte>(ref destination, 1) = (byte)(value >> 8);
			*Unsafe.Add<byte>(ref destination, 2) = (byte)value;
		}

		// Token: 0x06006411 RID: 25617 RVA: 0x0014F0DC File Offset: 0x0014D2DC
		public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int bytesConsumed, out int bytesWritten, bool isFinalBlock = true)
		{
			ref byte reference = ref MemoryMarshal.GetReference<byte>(bytes);
			ref byte reference2 = ref MemoryMarshal.GetReference<byte>(utf8);
			int length = bytes.Length;
			int length2 = utf8.Length;
			int num;
			if (length <= 1610612733 && length2 >= Base64.GetMaxEncodedToUtf8Length(length))
			{
				num = length - 2;
			}
			else
			{
				num = (length2 >> 2) * 3 - 2;
			}
			int i = 0;
			int num2 = 0;
			ref byte encodingMap = ref Base64.s_encodingMap[0];
			while (i < num)
			{
				int value = Base64.Encode(Unsafe.Add<byte>(ref reference, i), ref encodingMap);
				Unsafe.WriteUnaligned<int>(Unsafe.Add<byte>(ref reference2, num2), value);
				num2 += 4;
				i += 3;
			}
			if (num != length - 2)
			{
				bytesConsumed = i;
				bytesWritten = num2;
				return OperationStatus.DestinationTooSmall;
			}
			if (isFinalBlock)
			{
				if (i == length - 1)
				{
					int value = Base64.EncodeAndPadTwo(Unsafe.Add<byte>(ref reference, i), ref encodingMap);
					Unsafe.WriteUnaligned<int>(Unsafe.Add<byte>(ref reference2, num2), value);
					num2 += 4;
					i++;
				}
				else if (i == length - 2)
				{
					int value = Base64.EncodeAndPadOne(Unsafe.Add<byte>(ref reference, i), ref encodingMap);
					Unsafe.WriteUnaligned<int>(Unsafe.Add<byte>(ref reference2, num2), value);
					num2 += 4;
					i += 2;
				}
				bytesConsumed = i;
				bytesWritten = num2;
				return OperationStatus.Done;
			}
			bytesConsumed = i;
			bytesWritten = num2;
			return OperationStatus.NeedMoreData;
		}

		// Token: 0x06006412 RID: 25618 RVA: 0x0014F208 File Offset: 0x0014D408
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetMaxEncodedToUtf8Length(int length)
		{
			if (length > 1610612733)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
			}
			return (length + 2) / 3 * 4;
		}

		// Token: 0x06006413 RID: 25619 RVA: 0x0014F220 File Offset: 0x0014D420
		public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer, int dataLength, out int bytesWritten)
		{
			int maxEncodedToUtf8Length = Base64.GetMaxEncodedToUtf8Length(dataLength);
			if (buffer.Length >= maxEncodedToUtf8Length)
			{
				int num = dataLength - dataLength / 3 * 3;
				int num2 = maxEncodedToUtf8Length - 4;
				int i = dataLength - num;
				ref byte encodingMap = ref Base64.s_encodingMap[0];
				ref byte reference = ref MemoryMarshal.GetReference<byte>(buffer);
				if (num != 0)
				{
					if (num == 1)
					{
						int value = Base64.EncodeAndPadTwo(Unsafe.Add<byte>(ref reference, i), ref encodingMap);
						Unsafe.WriteUnaligned<int>(Unsafe.Add<byte>(ref reference, num2), value);
						num2 -= 4;
					}
					else
					{
						int value = Base64.EncodeAndPadOne(Unsafe.Add<byte>(ref reference, i), ref encodingMap);
						Unsafe.WriteUnaligned<int>(Unsafe.Add<byte>(ref reference, num2), value);
						num2 -= 4;
					}
				}
				for (i -= 3; i >= 0; i -= 3)
				{
					int value = Base64.Encode(Unsafe.Add<byte>(ref reference, i), ref encodingMap);
					Unsafe.WriteUnaligned<int>(Unsafe.Add<byte>(ref reference, num2), value);
					num2 -= 4;
				}
				bytesWritten = maxEncodedToUtf8Length;
				return OperationStatus.Done;
			}
			bytesWritten = 0;
			return OperationStatus.DestinationTooSmall;
		}

		// Token: 0x06006414 RID: 25620 RVA: 0x0014F2F8 File Offset: 0x0014D4F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static int Encode(ref byte threeBytes, ref byte encodingMap)
		{
			int num = (int)threeBytes << 16 | (int)(*Unsafe.Add<byte>(ref threeBytes, 1)) << 8 | (int)(*Unsafe.Add<byte>(ref threeBytes, 2));
			int num2 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 18));
			int num3 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 12 & 63));
			int num4 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 6 & 63));
			int num5 = (int)(*Unsafe.Add<byte>(ref encodingMap, num & 63));
			return num2 | num3 << 8 | num4 << 16 | num5 << 24;
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x0014F364 File Offset: 0x0014D564
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static int EncodeAndPadOne(ref byte twoBytes, ref byte encodingMap)
		{
			int num = (int)twoBytes << 16 | (int)(*Unsafe.Add<byte>(ref twoBytes, 1)) << 8;
			int num2 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 18));
			int num3 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 12 & 63));
			int num4 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 6 & 63));
			return num2 | num3 << 8 | num4 << 16 | 1023410176;
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x0014F3BC File Offset: 0x0014D5BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private unsafe static int EncodeAndPadTwo(ref byte oneByte, ref byte encodingMap)
		{
			int num = (int)oneByte << 8;
			int num2 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 10));
			int num3 = (int)(*Unsafe.Add<byte>(ref encodingMap, num >> 4 & 63));
			return num2 | num3 << 8 | 3997696 | 1023410176;
		}

		// Token: 0x04003A96 RID: 14998
		private static readonly sbyte[] s_decodingMap = new sbyte[]
		{
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			62,
			-1,
			-1,
			-1,
			63,
			52,
			53,
			54,
			55,
			56,
			57,
			58,
			59,
			60,
			61,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1,
			-1
		};

		// Token: 0x04003A97 RID: 14999
		private static readonly byte[] s_encodingMap = new byte[]
		{
			65,
			66,
			67,
			68,
			69,
			70,
			71,
			72,
			73,
			74,
			75,
			76,
			77,
			78,
			79,
			80,
			81,
			82,
			83,
			84,
			85,
			86,
			87,
			88,
			89,
			90,
			97,
			98,
			99,
			100,
			101,
			102,
			103,
			104,
			105,
			106,
			107,
			108,
			109,
			110,
			111,
			112,
			113,
			114,
			115,
			116,
			117,
			118,
			119,
			120,
			121,
			122,
			48,
			49,
			50,
			51,
			52,
			53,
			54,
			55,
			56,
			57,
			43,
			47
		};

		// Token: 0x04003A98 RID: 15000
		private const byte EncodingPad = 61;

		// Token: 0x04003A99 RID: 15001
		private const int MaximumEncodeLength = 1610612733;
	}
}
