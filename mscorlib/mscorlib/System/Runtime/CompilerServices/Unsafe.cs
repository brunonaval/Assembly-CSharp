using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x02000855 RID: 2133
	internal static class Unsafe
	{
		// Token: 0x06004711 RID: 18193 RVA: 0x000E7ECE File Offset: 0x000E60CE
		public static ref T Add<T>(ref T source, int elementOffset)
		{
			return ref source + (IntPtr)elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x000E7EDB File Offset: 0x000E60DB
		public static ref T Add<T>(ref T source, IntPtr elementOffset)
		{
			return ref source + elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x000E7ECE File Offset: 0x000E60CE
		public unsafe static void* Add<T>(void* source, int elementOffset)
		{
			return (void*)((byte*)source + (IntPtr)elementOffset * (IntPtr)sizeof(T));
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x000E7EE7 File Offset: 0x000E60E7
		public static ref T AddByteOffset<T>(ref T source, IntPtr byteOffset)
		{
			return ref source + byteOffset;
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0002842A File Offset: 0x0002662A
		public static bool AreSame<T>(ref T left, ref T right)
		{
			return ref left == ref right;
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0000270D File Offset: 0x0000090D
		public static T As<T>(object o) where T : class
		{
			return o;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0000270D File Offset: 0x0000090D
		public static ref TTo As<TFrom, TTo>(ref TFrom source)
		{
			return ref source;
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x000E7EEC File Offset: 0x000E60EC
		public unsafe static void* AsPointer<T>(ref T value)
		{
			return (void*)(&value);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0000270D File Offset: 0x0000090D
		public unsafe static ref T AsRef<T>(void* source)
		{
			return ref *(T*)source;
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0000270D File Offset: 0x0000090D
		public static ref T AsRef<T>(in T source)
		{
			return ref source;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x000E7EF0 File Offset: 0x000E60F0
		public static IntPtr ByteOffset<T>(ref T origin, ref T target)
		{
			return ref target - ref origin;
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x000E7EF5 File Offset: 0x000E60F5
		public static void CopyBlock(ref byte destination, ref byte source, uint byteCount)
		{
			cpblk(ref destination, ref source, byteCount);
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x000E7EFC File Offset: 0x000E60FC
		public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount)
		{
			initblk(ref startAddress, value, byteCount);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x000E7EFC File Offset: 0x000E60FC
		public unsafe static void InitBlockUnaligned(void* startAddress, byte value, uint byteCount)
		{
			initblk(startAddress, value, byteCount);
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x000E7F06 File Offset: 0x000E6106
		public unsafe static T Read<T>(void* source)
		{
			return *(T*)source;
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x000E7F0E File Offset: 0x000E610E
		public unsafe static T ReadUnaligned<T>(void* source)
		{
			return *(T*)source;
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x000E7F0E File Offset: 0x000E610E
		public static T ReadUnaligned<T>(ref byte source)
		{
			return source;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x000E7F19 File Offset: 0x000E6119
		public static int SizeOf<T>()
		{
			return sizeof(T);
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x000E7F21 File Offset: 0x000E6121
		public static ref T Subtract<T>(ref T source, int elementOffset)
		{
			return ref source - (IntPtr)elementOffset * (IntPtr)sizeof(T);
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x000E7F2E File Offset: 0x000E612E
		public static void WriteUnaligned<T>(ref byte destination, T value)
		{
			destination = value;
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x000E7F2E File Offset: 0x000E612E
		public unsafe static void WriteUnaligned<T>(void* destination, T value)
		{
			*(T*)destination = value;
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x000E7F3A File Offset: 0x000E613A
		public static bool IsAddressGreaterThan<T>(ref T left, ref T right)
		{
			return ref left != ref right;
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x000E7F40 File Offset: 0x000E6140
		public static bool IsAddressLessThan<T>(ref T left, ref T right)
		{
			return ref left < ref right;
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x000E7F46 File Offset: 0x000E6146
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static ref T AddByteOffset<T>(ref T source, ulong byteOffset)
		{
			return Unsafe.AddByteOffset<T>(ref source, (IntPtr)byteOffset);
		}
	}
}
