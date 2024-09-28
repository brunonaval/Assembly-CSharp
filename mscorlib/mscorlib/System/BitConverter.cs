﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Converts base data types to an array of bytes, and an array of bytes to base data types.</summary>
	// Token: 0x02000100 RID: 256
	public static class BitConverter
	{
		/// <summary>Returns the specified Boolean value as a byte array.</summary>
		/// <param name="value">A Boolean value.</param>
		/// <returns>A byte array with length 1.</returns>
		// Token: 0x06000765 RID: 1893 RVA: 0x0000A9B9 File Offset: 0x00008BB9
		public static byte[] GetBytes(bool value)
		{
			return new byte[]
			{
				value ? 1 : 0
			};
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x000219D6 File Offset: 0x0001FBD6
		public static bool TryWriteBytes(Span<byte> destination, bool value)
		{
			if (destination.Length < 1)
			{
				return false;
			}
			Unsafe.WriteUnaligned<byte>(MemoryMarshal.GetReference<byte>(destination), value ? 1 : 0);
			return true;
		}

		/// <summary>Returns the specified Unicode character value as an array of bytes.</summary>
		/// <param name="value">A character to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		// Token: 0x06000767 RID: 1895 RVA: 0x000219F7 File Offset: 0x0001FBF7
		public unsafe static byte[] GetBytes(char value)
		{
			byte[] array = new byte[2];
			*Unsafe.As<byte, char>(ref array[0]) = value;
			return array;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00021A0D File Offset: 0x0001FC0D
		public static bool TryWriteBytes(Span<byte> destination, char value)
		{
			if (destination.Length < 2)
			{
				return false;
			}
			Unsafe.WriteUnaligned<char>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified 16-bit signed integer value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		// Token: 0x06000769 RID: 1897 RVA: 0x00021A28 File Offset: 0x0001FC28
		public unsafe static byte[] GetBytes(short value)
		{
			byte[] array = new byte[2];
			*Unsafe.As<byte, short>(ref array[0]) = value;
			return array;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00021A3E File Offset: 0x0001FC3E
		public static bool TryWriteBytes(Span<byte> destination, short value)
		{
			if (destination.Length < 2)
			{
				return false;
			}
			Unsafe.WriteUnaligned<short>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified 32-bit signed integer value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		// Token: 0x0600076B RID: 1899 RVA: 0x00021A59 File Offset: 0x0001FC59
		public unsafe static byte[] GetBytes(int value)
		{
			byte[] array = new byte[4];
			*Unsafe.As<byte, int>(ref array[0]) = value;
			return array;
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00021A6F File Offset: 0x0001FC6F
		public static bool TryWriteBytes(Span<byte> destination, int value)
		{
			if (destination.Length < 4)
			{
				return false;
			}
			Unsafe.WriteUnaligned<int>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified 64-bit signed integer value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		// Token: 0x0600076D RID: 1901 RVA: 0x00021A8A File Offset: 0x0001FC8A
		public unsafe static byte[] GetBytes(long value)
		{
			byte[] array = new byte[8];
			*Unsafe.As<byte, long>(ref array[0]) = value;
			return array;
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00021AA0 File Offset: 0x0001FCA0
		public static bool TryWriteBytes(Span<byte> destination, long value)
		{
			if (destination.Length < 8)
			{
				return false;
			}
			Unsafe.WriteUnaligned<long>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified 16-bit unsigned integer value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 2.</returns>
		// Token: 0x0600076F RID: 1903 RVA: 0x00021ABB File Offset: 0x0001FCBB
		[CLSCompliant(false)]
		public unsafe static byte[] GetBytes(ushort value)
		{
			byte[] array = new byte[2];
			*Unsafe.As<byte, ushort>(ref array[0]) = value;
			return array;
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x00021AD1 File Offset: 0x0001FCD1
		[CLSCompliant(false)]
		public static bool TryWriteBytes(Span<byte> destination, ushort value)
		{
			if (destination.Length < 2)
			{
				return false;
			}
			Unsafe.WriteUnaligned<ushort>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified 32-bit unsigned integer value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		// Token: 0x06000771 RID: 1905 RVA: 0x00021AEC File Offset: 0x0001FCEC
		[CLSCompliant(false)]
		public unsafe static byte[] GetBytes(uint value)
		{
			byte[] array = new byte[4];
			*Unsafe.As<byte, uint>(ref array[0]) = value;
			return array;
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00021B02 File Offset: 0x0001FD02
		[CLSCompliant(false)]
		public static bool TryWriteBytes(Span<byte> destination, uint value)
		{
			if (destination.Length < 4)
			{
				return false;
			}
			Unsafe.WriteUnaligned<uint>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified 64-bit unsigned integer value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		// Token: 0x06000773 RID: 1907 RVA: 0x00021B1D File Offset: 0x0001FD1D
		[CLSCompliant(false)]
		public unsafe static byte[] GetBytes(ulong value)
		{
			byte[] array = new byte[8];
			*Unsafe.As<byte, ulong>(ref array[0]) = value;
			return array;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00021B33 File Offset: 0x0001FD33
		[CLSCompliant(false)]
		public static bool TryWriteBytes(Span<byte> destination, ulong value)
		{
			if (destination.Length < 8)
			{
				return false;
			}
			Unsafe.WriteUnaligned<ulong>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified single-precision floating point value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 4.</returns>
		// Token: 0x06000775 RID: 1909 RVA: 0x00021B4E File Offset: 0x0001FD4E
		public unsafe static byte[] GetBytes(float value)
		{
			byte[] array = new byte[4];
			*Unsafe.As<byte, float>(ref array[0]) = value;
			return array;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00021B64 File Offset: 0x0001FD64
		public static bool TryWriteBytes(Span<byte> destination, float value)
		{
			if (destination.Length < 4)
			{
				return false;
			}
			Unsafe.WriteUnaligned<float>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns the specified double-precision floating point value as an array of bytes.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>An array of bytes with length 8.</returns>
		// Token: 0x06000777 RID: 1911 RVA: 0x00021B7F File Offset: 0x0001FD7F
		public unsafe static byte[] GetBytes(double value)
		{
			byte[] array = new byte[8];
			*Unsafe.As<byte, double>(ref array[0]) = value;
			return array;
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x00021B95 File Offset: 0x0001FD95
		public static bool TryWriteBytes(Span<byte> destination, double value)
		{
			if (destination.Length < 8)
			{
				return false;
			}
			Unsafe.WriteUnaligned<double>(MemoryMarshal.GetReference<byte>(destination), value);
			return true;
		}

		/// <summary>Returns a Unicode character converted from two bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A character formed by two bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> equals the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x06000779 RID: 1913 RVA: 0x00021BB0 File Offset: 0x0001FDB0
		public static char ToChar(byte[] value, int startIndex)
		{
			return (char)BitConverter.ToInt16(value, startIndex);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00021BBA File Offset: 0x0001FDBA
		public static char ToChar(ReadOnlySpan<byte> value)
		{
			if (value.Length < 2)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<char>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a 16-bit signed integer converted from two bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A 16-bit signed integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> equals the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x0600077B RID: 1915 RVA: 0x00021BD8 File Offset: 0x0001FDD8
		public static short ToInt16(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if (startIndex >= value.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 2)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall, ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<short>(ref value[startIndex]);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00021C0F File Offset: 0x0001FE0F
		public static short ToInt16(ReadOnlySpan<byte> value)
		{
			if (value.Length < 2)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<short>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a 32-bit signed integer converted from four bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A 32-bit signed integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 3, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x0600077D RID: 1917 RVA: 0x00021C2D File Offset: 0x0001FE2D
		public static int ToInt32(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if (startIndex >= value.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 4)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall, ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<int>(ref value[startIndex]);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00021C64 File Offset: 0x0001FE64
		public static int ToInt32(ReadOnlySpan<byte> value)
		{
			if (value.Length < 4)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<int>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a 64-bit signed integer converted from eight bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A 64-bit signed integer formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 7, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x0600077F RID: 1919 RVA: 0x00021C82 File Offset: 0x0001FE82
		public static long ToInt64(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if (startIndex >= value.Length)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 8)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall, ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<long>(ref value[startIndex]);
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00021CB9 File Offset: 0x0001FEB9
		public static long ToInt64(ReadOnlySpan<byte> value)
		{
			if (value.Length < 8)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<long>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a 16-bit unsigned integer converted from two bytes at a specified position in a byte array.</summary>
		/// <param name="value">The array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A 16-bit unsigned integer formed by two bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> equals the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x06000781 RID: 1921 RVA: 0x00021BB0 File Offset: 0x0001FDB0
		[CLSCompliant(false)]
		public static ushort ToUInt16(byte[] value, int startIndex)
		{
			return (ushort)BitConverter.ToInt16(value, startIndex);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00021CD7 File Offset: 0x0001FED7
		[CLSCompliant(false)]
		public static ushort ToUInt16(ReadOnlySpan<byte> value)
		{
			if (value.Length < 2)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<ushort>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a 32-bit unsigned integer converted from four bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A 32-bit unsigned integer formed by four bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 3, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x06000783 RID: 1923 RVA: 0x00021CF5 File Offset: 0x0001FEF5
		[CLSCompliant(false)]
		public static uint ToUInt32(byte[] value, int startIndex)
		{
			return (uint)BitConverter.ToInt32(value, startIndex);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00021CFE File Offset: 0x0001FEFE
		[CLSCompliant(false)]
		public static uint ToUInt32(ReadOnlySpan<byte> value)
		{
			if (value.Length < 4)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<uint>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a 64-bit unsigned integer converted from eight bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A 64-bit unsigned integer formed by the eight bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 7, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x06000785 RID: 1925 RVA: 0x00021D1C File Offset: 0x0001FF1C
		[CLSCompliant(false)]
		public static ulong ToUInt64(byte[] value, int startIndex)
		{
			return (ulong)BitConverter.ToInt64(value, startIndex);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00021D25 File Offset: 0x0001FF25
		[CLSCompliant(false)]
		public static ulong ToUInt64(ReadOnlySpan<byte> value)
		{
			if (value.Length < 8)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<ulong>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a single-precision floating point number converted from four bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A single-precision floating point number formed by four bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 3, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x06000787 RID: 1927 RVA: 0x00021D43 File Offset: 0x0001FF43
		public static float ToSingle(byte[] value, int startIndex)
		{
			return BitConverter.Int32BitsToSingle(BitConverter.ToInt32(value, startIndex));
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00021D51 File Offset: 0x0001FF51
		public static float ToSingle(ReadOnlySpan<byte> value)
		{
			if (value.Length < 4)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<float>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Returns a double-precision floating point number converted from eight bytes at a specified position in a byte array.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A double precision floating point number formed by eight bytes beginning at <paramref name="startIndex" />.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="startIndex" /> is greater than or equal to the length of <paramref name="value" /> minus 7, and is less than or equal to the length of <paramref name="value" /> minus 1.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x06000789 RID: 1929 RVA: 0x00021D6F File Offset: 0x0001FF6F
		public static double ToDouble(byte[] value, int startIndex)
		{
			return BitConverter.Int64BitsToDouble(BitConverter.ToInt64(value, startIndex));
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00021D7D File Offset: 0x0001FF7D
		public static double ToDouble(ReadOnlySpan<byte> value)
		{
			if (value.Length < 8)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<double>(MemoryMarshal.GetReference<byte>(value));
		}

		/// <summary>Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <param name="length">The number of array elements in <paramref name="value" /> to convert.</param>
		/// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of <paramref name="value" />; for example, "7F-2C-4A-00".</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> or <paramref name="length" /> is less than zero.  
		/// -or-  
		/// <paramref name="startIndex" /> is greater than zero and is greater than or equal to the length of <paramref name="value" />.</exception>
		/// <exception cref="T:System.ArgumentException">The combination of <paramref name="startIndex" /> and <paramref name="length" /> does not specify a position within <paramref name="value" />; that is, the <paramref name="startIndex" /> parameter is greater than the length of <paramref name="value" /> minus the <paramref name="length" /> parameter.</exception>
		// Token: 0x0600078B RID: 1931 RVA: 0x00021D9C File Offset: 0x0001FF9C
		public unsafe static string ToString(byte[] value, int startIndex, int length)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if (startIndex < 0 || (startIndex >= value.Length && startIndex > 0))
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Value must be positive.");
			}
			if (startIndex > value.Length - length)
			{
				ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_ArrayPlusOffTooSmall, ExceptionArgument.value);
			}
			if (length == 0)
			{
				return string.Empty;
			}
			if (length > 715827882)
			{
				throw new ArgumentOutOfRangeException("length", SR.Format("The specified length exceeds the maximum value of {0}.", 715827882));
			}
			return string.Create<ValueTuple<byte[], int, int>>(length * 3 - 1, new ValueTuple<byte[], int, int>(value, startIndex, length), delegate(Span<char> dst, [TupleElementNames(new string[]
			{
				"value",
				"startIndex",
				"length"
			})] ValueTuple<byte[], int, int> state)
			{
				ReadOnlySpan<byte> readOnlySpan = new ReadOnlySpan<byte>(state.Item1, state.Item2, state.Item3);
				int i = 0;
				int num = 0;
				byte b = *readOnlySpan[i++];
				*dst[num++] = "0123456789ABCDEF"[b >> 4];
				*dst[num++] = "0123456789ABCDEF"[(int)(b & 15)];
				while (i < readOnlySpan.Length)
				{
					b = *readOnlySpan[i++];
					*dst[num++] = '-';
					*dst[num++] = "0123456789ABCDEF"[b >> 4];
					*dst[num++] = "0123456789ABCDEF"[(int)(b & 15)];
				}
			});
		}

		/// <summary>Converts the numeric value of each element of a specified array of bytes to its equivalent hexadecimal string representation.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in <paramref name="value" />; for example, "7F-2C-4A-00".</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		// Token: 0x0600078C RID: 1932 RVA: 0x00021E4F File Offset: 0x0002004F
		public static string ToString(byte[] value)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			return BitConverter.ToString(value, 0, value.Length);
		}

		/// <summary>Converts the numeric value of each element of a specified subarray of bytes to its equivalent hexadecimal string representation.</summary>
		/// <param name="value">An array of bytes.</param>
		/// <param name="startIndex">The starting position within <paramref name="value" />.</param>
		/// <returns>A string of hexadecimal pairs separated by hyphens, where each pair represents the corresponding element in a subarray of <paramref name="value" />; for example, "7F-2C-4A-00".</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x0600078D RID: 1933 RVA: 0x00021E65 File Offset: 0x00020065
		public static string ToString(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			return BitConverter.ToString(value, startIndex, value.Length - startIndex);
		}

		/// <summary>Returns a Boolean value converted from the byte at a specified position in a byte array.</summary>
		/// <param name="value">A byte array.</param>
		/// <param name="startIndex">The index of the byte within <paramref name="value" />.</param>
		/// <returns>
		///   <see langword="true" /> if the byte at <paramref name="startIndex" /> in <paramref name="value" /> is nonzero; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="startIndex" /> is less than zero or greater than the length of <paramref name="value" /> minus 1.</exception>
		// Token: 0x0600078E RID: 1934 RVA: 0x00021E7D File Offset: 0x0002007D
		public static bool ToBoolean(byte[] value, int startIndex)
		{
			if (value == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.value);
			}
			if (startIndex < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			if (startIndex > value.Length - 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.startIndex, ExceptionResource.ArgumentOutOfRange_Index);
			}
			return value[startIndex] > 0;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00021EAD File Offset: 0x000200AD
		public static bool ToBoolean(ReadOnlySpan<byte> value)
		{
			if (value.Length < 1)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value);
			}
			return Unsafe.ReadUnaligned<byte>(MemoryMarshal.GetReference<byte>(value)) > 0;
		}

		/// <summary>Converts the specified double-precision floating point number to a 64-bit signed integer.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A 64-bit signed integer whose value is equivalent to <paramref name="value" />.</returns>
		// Token: 0x06000790 RID: 1936 RVA: 0x00021ECE File Offset: 0x000200CE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static long DoubleToInt64Bits(double value)
		{
			return *(long*)(&value);
		}

		/// <summary>Converts the specified 64-bit signed integer to a double-precision floating point number.</summary>
		/// <param name="value">The number to convert.</param>
		/// <returns>A double-precision floating point number whose value is equivalent to <paramref name="value" />.</returns>
		// Token: 0x06000791 RID: 1937 RVA: 0x00021ED4 File Offset: 0x000200D4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static double Int64BitsToDouble(long value)
		{
			return *(double*)(&value);
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00021EDA File Offset: 0x000200DA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static int SingleToInt32Bits(float value)
		{
			return *(int*)(&value);
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00021EE0 File Offset: 0x000200E0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe static float Int32BitsToSingle(int value)
		{
			return *(float*)(&value);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00021EE8 File Offset: 0x000200E8
		unsafe static BitConverter()
		{
			ushort num = 4660;
			byte* ptr = (byte*)(&num);
			BitConverter.IsLittleEndian = (*ptr == 52);
		}

		/// <summary>Indicates the byte order ("endianness") in which data is stored in this computer architecture.</summary>
		// Token: 0x0400106A RID: 4202
		[Intrinsic]
		public static readonly bool IsLittleEndian;
	}
}
