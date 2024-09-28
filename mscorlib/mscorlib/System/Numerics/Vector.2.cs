﻿using System;
using System.Runtime.CompilerServices;

namespace System.Numerics
{
	// Token: 0x0200094F RID: 2383
	[Intrinsic]
	public static class Vector
	{
		// Token: 0x060053B5 RID: 21429 RVA: 0x001180AC File Offset: 0x001162AC
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static void Widen(Vector<byte> source, out Vector<ushort> low, out Vector<ushort> high)
		{
			int count = Vector<byte>.Count;
			ushort* ptr = stackalloc ushort[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ushort)source[i];
			}
			ushort* ptr2 = stackalloc ushort[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (ushort)source[j + count / 2];
			}
			low = new Vector<ushort>((void*)ptr);
			high = new Vector<ushort>((void*)ptr2);
		}

		// Token: 0x060053B6 RID: 21430 RVA: 0x00118130 File Offset: 0x00116330
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static void Widen(Vector<ushort> source, out Vector<uint> low, out Vector<uint> high)
		{
			int count = Vector<ushort>.Count;
			uint* ptr = stackalloc uint[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (uint)source[i];
			}
			uint* ptr2 = stackalloc uint[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (uint)source[j + count / 2];
			}
			low = new Vector<uint>((void*)ptr);
			high = new Vector<uint>((void*)ptr2);
		}

		// Token: 0x060053B7 RID: 21431 RVA: 0x001181B4 File Offset: 0x001163B4
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static void Widen(Vector<uint> source, out Vector<ulong> low, out Vector<ulong> high)
		{
			int count = Vector<uint>.Count;
			ulong* ptr = stackalloc ulong[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ulong)source[i];
			}
			ulong* ptr2 = stackalloc ulong[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (ulong)source[j + count / 2];
			}
			low = new Vector<ulong>((void*)ptr);
			high = new Vector<ulong>((void*)ptr2);
		}

		// Token: 0x060053B8 RID: 21432 RVA: 0x00118238 File Offset: 0x00116438
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static void Widen(Vector<sbyte> source, out Vector<short> low, out Vector<short> high)
		{
			int count = Vector<sbyte>.Count;
			short* ptr = stackalloc short[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (short)source[i];
			}
			short* ptr2 = stackalloc short[checked(unchecked((UIntPtr)(count / 2)) * 2)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (short)source[j + count / 2];
			}
			low = new Vector<short>((void*)ptr);
			high = new Vector<short>((void*)ptr2);
		}

		// Token: 0x060053B9 RID: 21433 RVA: 0x001182BC File Offset: 0x001164BC
		[Intrinsic]
		public unsafe static void Widen(Vector<short> source, out Vector<int> low, out Vector<int> high)
		{
			int count = Vector<short>.Count;
			int* ptr = stackalloc int[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (int)source[i];
			}
			int* ptr2 = stackalloc int[checked(unchecked((UIntPtr)(count / 2)) * 4)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (int)source[j + count / 2];
			}
			low = new Vector<int>((void*)ptr);
			high = new Vector<int>((void*)ptr2);
		}

		// Token: 0x060053BA RID: 21434 RVA: 0x00118340 File Offset: 0x00116540
		[Intrinsic]
		public unsafe static void Widen(Vector<int> source, out Vector<long> low, out Vector<long> high)
		{
			int count = Vector<int>.Count;
			long* ptr = stackalloc long[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (long)source[i];
			}
			long* ptr2 = stackalloc long[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (long)source[j + count / 2];
			}
			low = new Vector<long>((void*)ptr);
			high = new Vector<long>((void*)ptr2);
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x001183C4 File Offset: 0x001165C4
		[Intrinsic]
		public unsafe static void Widen(Vector<float> source, out Vector<double> low, out Vector<double> high)
		{
			int count = Vector<float>.Count;
			double* ptr = stackalloc double[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (double)source[i];
			}
			double* ptr2 = stackalloc double[checked(unchecked((UIntPtr)(count / 2)) * 8)];
			for (int j = 0; j < count / 2; j++)
			{
				ptr2[j] = (double)source[j + count / 2];
			}
			low = new Vector<double>((void*)ptr);
			high = new Vector<double>((void*)ptr2);
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x00118448 File Offset: 0x00116648
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static Vector<byte> Narrow(Vector<ushort> low, Vector<ushort> high)
		{
			int count = Vector<byte>.Count;
			byte* ptr = stackalloc byte[(UIntPtr)count];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (byte)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (byte)high[j];
			}
			return new Vector<byte>((void*)ptr);
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x001184A0 File Offset: 0x001166A0
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<ushort> Narrow(Vector<uint> low, Vector<uint> high)
		{
			int count = Vector<ushort>.Count;
			ushort* ptr = stackalloc ushort[checked(unchecked((UIntPtr)count) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (ushort)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (ushort)high[j];
			}
			return new Vector<ushort>((void*)ptr);
		}

		// Token: 0x060053BE RID: 21438 RVA: 0x00118500 File Offset: 0x00116700
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static Vector<uint> Narrow(Vector<ulong> low, Vector<ulong> high)
		{
			int count = Vector<uint>.Count;
			uint* ptr = stackalloc uint[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (uint)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (uint)high[j];
			}
			return new Vector<uint>((void*)ptr);
		}

		// Token: 0x060053BF RID: 21439 RVA: 0x00118560 File Offset: 0x00116760
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static Vector<sbyte> Narrow(Vector<short> low, Vector<short> high)
		{
			int count = Vector<sbyte>.Count;
			sbyte* ptr = stackalloc sbyte[(UIntPtr)count];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (sbyte)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (sbyte)high[j];
			}
			return new Vector<sbyte>((void*)ptr);
		}

		// Token: 0x060053C0 RID: 21440 RVA: 0x001185B8 File Offset: 0x001167B8
		[Intrinsic]
		public unsafe static Vector<short> Narrow(Vector<int> low, Vector<int> high)
		{
			int count = Vector<short>.Count;
			short* ptr = stackalloc short[checked(unchecked((UIntPtr)count) * 2)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (short)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (short)high[j];
			}
			return new Vector<short>((void*)ptr);
		}

		// Token: 0x060053C1 RID: 21441 RVA: 0x00118618 File Offset: 0x00116818
		[Intrinsic]
		public unsafe static Vector<int> Narrow(Vector<long> low, Vector<long> high)
		{
			int count = Vector<int>.Count;
			int* ptr = stackalloc int[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (int)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (int)high[j];
			}
			return new Vector<int>((void*)ptr);
		}

		// Token: 0x060053C2 RID: 21442 RVA: 0x00118678 File Offset: 0x00116878
		[Intrinsic]
		public unsafe static Vector<float> Narrow(Vector<double> low, Vector<double> high)
		{
			int count = Vector<float>.Count;
			float* ptr = stackalloc float[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count / 2; i++)
			{
				ptr[i] = (float)low[i];
			}
			for (int j = 0; j < count / 2; j++)
			{
				ptr[j + count / 2] = (float)high[j];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x060053C3 RID: 21443 RVA: 0x001186D8 File Offset: 0x001168D8
		[Intrinsic]
		public unsafe static Vector<float> ConvertToSingle(Vector<int> value)
		{
			int count = Vector<float>.Count;
			float* ptr = stackalloc float[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (float)value[i];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x00118714 File Offset: 0x00116914
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static Vector<float> ConvertToSingle(Vector<uint> value)
		{
			int count = Vector<float>.Count;
			float* ptr = stackalloc float[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = value[i];
			}
			return new Vector<float>((void*)ptr);
		}

		// Token: 0x060053C5 RID: 21445 RVA: 0x00118754 File Offset: 0x00116954
		[Intrinsic]
		public unsafe static Vector<double> ConvertToDouble(Vector<long> value)
		{
			int count = Vector<double>.Count;
			double* ptr = stackalloc double[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (double)value[i];
			}
			return new Vector<double>((void*)ptr);
		}

		// Token: 0x060053C6 RID: 21446 RVA: 0x00118790 File Offset: 0x00116990
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static Vector<double> ConvertToDouble(Vector<ulong> value)
		{
			int count = Vector<double>.Count;
			double* ptr = stackalloc double[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = value[i];
			}
			return new Vector<double>((void*)ptr);
		}

		// Token: 0x060053C7 RID: 21447 RVA: 0x001187D0 File Offset: 0x001169D0
		[Intrinsic]
		public unsafe static Vector<int> ConvertToInt32(Vector<float> value)
		{
			int count = Vector<int>.Count;
			int* ptr = stackalloc int[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (int)value[i];
			}
			return new Vector<int>((void*)ptr);
		}

		// Token: 0x060053C8 RID: 21448 RVA: 0x0011880C File Offset: 0x00116A0C
		[CLSCompliant(false)]
		[Intrinsic]
		public unsafe static Vector<uint> ConvertToUInt32(Vector<float> value)
		{
			int count = Vector<uint>.Count;
			uint* ptr = stackalloc uint[checked(unchecked((UIntPtr)count) * 4)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (uint)value[i];
			}
			return new Vector<uint>((void*)ptr);
		}

		// Token: 0x060053C9 RID: 21449 RVA: 0x00118848 File Offset: 0x00116A48
		[Intrinsic]
		public unsafe static Vector<long> ConvertToInt64(Vector<double> value)
		{
			int count = Vector<long>.Count;
			long* ptr = stackalloc long[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (long)value[i];
			}
			return new Vector<long>((void*)ptr);
		}

		// Token: 0x060053CA RID: 21450 RVA: 0x00118884 File Offset: 0x00116A84
		[Intrinsic]
		[CLSCompliant(false)]
		public unsafe static Vector<ulong> ConvertToUInt64(Vector<double> value)
		{
			int count = Vector<ulong>.Count;
			ulong* ptr = stackalloc ulong[checked(unchecked((UIntPtr)count) * 8)];
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (ulong)value[i];
			}
			return new Vector<ulong>((void*)ptr);
		}

		// Token: 0x060053CB RID: 21451 RVA: 0x001188C0 File Offset: 0x00116AC0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<float> ConditionalSelect(Vector<int> condition, Vector<float> left, Vector<float> right)
		{
			return Vector<float>.ConditionalSelect((Vector<float>)condition, left, right);
		}

		// Token: 0x060053CC RID: 21452 RVA: 0x001188CF File Offset: 0x00116ACF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<double> ConditionalSelect(Vector<long> condition, Vector<double> left, Vector<double> right)
		{
			return Vector<double>.ConditionalSelect((Vector<double>)condition, left, right);
		}

		// Token: 0x060053CD RID: 21453 RVA: 0x001188DE File Offset: 0x00116ADE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> ConditionalSelect<T>(Vector<T> condition, Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.ConditionalSelect(condition, left, right);
		}

		// Token: 0x060053CE RID: 21454 RVA: 0x001188E8 File Offset: 0x00116AE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Equals<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Equals(left, right);
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x001188F1 File Offset: 0x00116AF1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> Equals(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.Equals(left, right);
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x001188FF File Offset: 0x00116AFF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> Equals(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.Equals(left, right);
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x00118908 File Offset: 0x00116B08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> Equals(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.Equals(left, right);
		}

		// Token: 0x060053D2 RID: 21458 RVA: 0x00118916 File Offset: 0x00116B16
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> Equals(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.Equals(left, right);
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x0011891F File Offset: 0x00116B1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left == right;
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x00118928 File Offset: 0x00116B28
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool EqualsAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !Vector<T>.Equals(left, right).Equals(Vector<T>.Zero);
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x0011894C File Offset: 0x00116B4C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> LessThan<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.LessThan(left, right);
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x00118955 File Offset: 0x00116B55
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThan(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.LessThan(left, right);
		}

		// Token: 0x060053D7 RID: 21463 RVA: 0x00118963 File Offset: 0x00116B63
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThan(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.LessThan(left, right);
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x0011896C File Offset: 0x00116B6C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThan(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.LessThan(left, right);
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x0011897A File Offset: 0x00116B7A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThan(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.LessThan(left, right);
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x00118984 File Offset: 0x00116B84
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.LessThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x060053DB RID: 21467 RVA: 0x001189AC File Offset: 0x00116BAC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.LessThan(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x060053DC RID: 21468 RVA: 0x001189D5 File Offset: 0x00116BD5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> LessThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.LessThanOrEqual(left, right);
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x001189DE File Offset: 0x00116BDE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThanOrEqual(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.LessThanOrEqual(left, right);
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x001189EC File Offset: 0x00116BEC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> LessThanOrEqual(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.LessThanOrEqual(left, right);
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x001189F5 File Offset: 0x00116BF5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThanOrEqual(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.LessThanOrEqual(left, right);
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x001189FE File Offset: 0x00116BFE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> LessThanOrEqual(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.LessThanOrEqual(left, right);
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x00118A0C File Offset: 0x00116C0C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x00118A34 File Offset: 0x00116C34
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.LessThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x00118A5D File Offset: 0x00116C5D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> GreaterThan<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.GreaterThan(left, right);
		}

		// Token: 0x060053E4 RID: 21476 RVA: 0x00118A66 File Offset: 0x00116C66
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThan(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.GreaterThan(left, right);
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x00118A74 File Offset: 0x00116C74
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThan(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.GreaterThan(left, right);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x00118A7D File Offset: 0x00116C7D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThan(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.GreaterThan(left, right);
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x00118A8B File Offset: 0x00116C8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThan(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.GreaterThan(left, right);
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x00118A94 File Offset: 0x00116C94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x00118ABC File Offset: 0x00116CBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.GreaterThan(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x00118AE5 File Offset: 0x00116CE5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> GreaterThanOrEqual<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x060053EB RID: 21483 RVA: 0x00118AEE File Offset: 0x00116CEE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThanOrEqual(Vector<float> left, Vector<float> right)
		{
			return (Vector<int>)Vector<float>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x00118AFC File Offset: 0x00116CFC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> GreaterThanOrEqual(Vector<int> left, Vector<int> right)
		{
			return Vector<int>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x00118B05 File Offset: 0x00116D05
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThanOrEqual(Vector<long> left, Vector<long> right)
		{
			return Vector<long>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x00118B0E File Offset: 0x00116D0E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> GreaterThanOrEqual(Vector<double> left, Vector<double> right)
		{
			return (Vector<long>)Vector<double>.GreaterThanOrEqual(left, right);
		}

		// Token: 0x060053EF RID: 21487 RVA: 0x00118B1C File Offset: 0x00116D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqualAll<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return ((Vector<int>)Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.AllOnes);
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x00118B44 File Offset: 0x00116D44
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqualAny<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return !((Vector<int>)Vector<T>.GreaterThanOrEqual(left, right)).Equals(Vector<int>.Zero);
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x060053F1 RID: 21489 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public static bool IsHardwareAccelerated
		{
			[Intrinsic]
			get
			{
				return false;
			}
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x00118B6D File Offset: 0x00116D6D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Abs<T>(Vector<T> value) where T : struct
		{
			return Vector<T>.Abs(value);
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x00118B75 File Offset: 0x00116D75
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Min<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Min(left, right);
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x00118B7E File Offset: 0x00116D7E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Max<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.Max(left, right);
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00118B87 File Offset: 0x00116D87
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Dot<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return Vector<T>.DotProduct(left, right);
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x00118B90 File Offset: 0x00116D90
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> SquareRoot<T>(Vector<T> value) where T : struct
		{
			return Vector<T>.SquareRoot(value);
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x00118B98 File Offset: 0x00116D98
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Add<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left + right;
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x00118BA1 File Offset: 0x00116DA1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Subtract<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left - right;
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x00118BAA File Offset: 0x00116DAA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left * right;
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x00118BB3 File Offset: 0x00116DB3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(Vector<T> left, T right) where T : struct
		{
			return left * right;
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x00118BBC File Offset: 0x00116DBC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Multiply<T>(T left, Vector<T> right) where T : struct
		{
			return left * right;
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x00118BC5 File Offset: 0x00116DC5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Divide<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left / right;
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x00118BCE File Offset: 0x00116DCE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Negate<T>(Vector<T> value) where T : struct
		{
			return -value;
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x00118BD6 File Offset: 0x00116DD6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> BitwiseAnd<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left & right;
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00118BDF File Offset: 0x00116DDF
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> BitwiseOr<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left | right;
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x00118BE8 File Offset: 0x00116DE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> OnesComplement<T>(Vector<T> value) where T : struct
		{
			return ~value;
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x00118BF0 File Offset: 0x00116DF0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> Xor<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left ^ right;
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x00118BF9 File Offset: 0x00116DF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<T> AndNot<T>(Vector<T> left, Vector<T> right) where T : struct
		{
			return left & ~right;
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x00118C07 File Offset: 0x00116E07
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<byte> AsVectorByte<T>(Vector<T> value) where T : struct
		{
			return (Vector<byte>)value;
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x00118C0F File Offset: 0x00116E0F
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<sbyte> AsVectorSByte<T>(Vector<T> value) where T : struct
		{
			return (Vector<sbyte>)value;
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x00118C17 File Offset: 0x00116E17
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<ushort> AsVectorUInt16<T>(Vector<T> value) where T : struct
		{
			return (Vector<ushort>)value;
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x00118C1F File Offset: 0x00116E1F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<short> AsVectorInt16<T>(Vector<T> value) where T : struct
		{
			return (Vector<short>)value;
		}

		// Token: 0x06005407 RID: 21511 RVA: 0x00118C27 File Offset: 0x00116E27
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<uint> AsVectorUInt32<T>(Vector<T> value) where T : struct
		{
			return (Vector<uint>)value;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x00118C2F File Offset: 0x00116E2F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<int> AsVectorInt32<T>(Vector<T> value) where T : struct
		{
			return (Vector<int>)value;
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x00118C37 File Offset: 0x00116E37
		[CLSCompliant(false)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<ulong> AsVectorUInt64<T>(Vector<T> value) where T : struct
		{
			return (Vector<ulong>)value;
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x00118C3F File Offset: 0x00116E3F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<long> AsVectorInt64<T>(Vector<T> value) where T : struct
		{
			return (Vector<long>)value;
		}

		// Token: 0x0600540B RID: 21515 RVA: 0x00118C47 File Offset: 0x00116E47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<float> AsVectorSingle<T>(Vector<T> value) where T : struct
		{
			return (Vector<float>)value;
		}

		// Token: 0x0600540C RID: 21516 RVA: 0x00118C4F File Offset: 0x00116E4F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector<double> AsVectorDouble<T>(Vector<T> value) where T : struct
		{
			return (Vector<double>)value;
		}
	}
}
