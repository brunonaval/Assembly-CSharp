using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
	// Token: 0x02000AF0 RID: 2800
	public static class SequenceReaderExtensions
	{
		// Token: 0x060063BB RID: 25531 RVA: 0x0014E170 File Offset: 0x0014C370
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool TryRead<[IsUnmanaged] T>(this SequenceReader<byte> reader, out T value) where T : struct, ValueType
		{
			ReadOnlySpan<byte> unreadSpan = reader.UnreadSpan;
			if (unreadSpan.Length < sizeof(T))
			{
				return SequenceReaderExtensions.TryReadMultisegment<T>(ref reader, out value);
			}
			value = Unsafe.ReadUnaligned<T>(MemoryMarshal.GetReference<byte>(unreadSpan));
			reader.Advance((long)sizeof(T));
			return true;
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x0014E1BC File Offset: 0x0014C3BC
		private unsafe static bool TryReadMultisegment<[IsUnmanaged] T>(ref SequenceReader<byte> reader, out T value) where T : struct, ValueType
		{
			T t = default(T);
			Span<byte> span = new Span<byte>((void*)(&t), sizeof(T));
			if (!reader.TryCopyTo(span))
			{
				value = default(T);
				return false;
			}
			value = Unsafe.ReadUnaligned<T>(MemoryMarshal.GetReference<byte>(span));
			reader.Advance((long)sizeof(T));
			return true;
		}

		// Token: 0x060063BD RID: 25533 RVA: 0x0014E212 File Offset: 0x0014C412
		public static bool TryReadLittleEndian(this SequenceReader<byte> reader, out short value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return ref reader.TryRead(out value);
			}
			return SequenceReaderExtensions.TryReadReverseEndianness(ref reader, out value);
		}

		// Token: 0x060063BE RID: 25534 RVA: 0x0014E22A File Offset: 0x0014C42A
		public static bool TryReadBigEndian(this SequenceReader<byte> reader, out short value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return ref reader.TryRead(out value);
			}
			return SequenceReaderExtensions.TryReadReverseEndianness(ref reader, out value);
		}

		// Token: 0x060063BF RID: 25535 RVA: 0x0014E242 File Offset: 0x0014C442
		private static bool TryReadReverseEndianness(ref SequenceReader<byte> reader, out short value)
		{
			if (ref reader.TryRead(out value))
			{
				value = BinaryPrimitives.ReverseEndianness(value);
				return true;
			}
			return false;
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x0014E259 File Offset: 0x0014C459
		public static bool TryReadLittleEndian(this SequenceReader<byte> reader, out int value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return ref reader.TryRead(out value);
			}
			return SequenceReaderExtensions.TryReadReverseEndianness(ref reader, out value);
		}

		// Token: 0x060063C1 RID: 25537 RVA: 0x0014E271 File Offset: 0x0014C471
		public static bool TryReadBigEndian(this SequenceReader<byte> reader, out int value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return ref reader.TryRead(out value);
			}
			return SequenceReaderExtensions.TryReadReverseEndianness(ref reader, out value);
		}

		// Token: 0x060063C2 RID: 25538 RVA: 0x0014E289 File Offset: 0x0014C489
		private static bool TryReadReverseEndianness(ref SequenceReader<byte> reader, out int value)
		{
			if (ref reader.TryRead(out value))
			{
				value = BinaryPrimitives.ReverseEndianness(value);
				return true;
			}
			return false;
		}

		// Token: 0x060063C3 RID: 25539 RVA: 0x0014E2A0 File Offset: 0x0014C4A0
		public static bool TryReadLittleEndian(this SequenceReader<byte> reader, out long value)
		{
			if (BitConverter.IsLittleEndian)
			{
				return ref reader.TryRead(out value);
			}
			return SequenceReaderExtensions.TryReadReverseEndianness(ref reader, out value);
		}

		// Token: 0x060063C4 RID: 25540 RVA: 0x0014E2B8 File Offset: 0x0014C4B8
		public static bool TryReadBigEndian(this SequenceReader<byte> reader, out long value)
		{
			if (!BitConverter.IsLittleEndian)
			{
				return ref reader.TryRead(out value);
			}
			return SequenceReaderExtensions.TryReadReverseEndianness(ref reader, out value);
		}

		// Token: 0x060063C5 RID: 25541 RVA: 0x0014E2D0 File Offset: 0x0014C4D0
		private static bool TryReadReverseEndianness(ref SequenceReader<byte> reader, out long value)
		{
			if (ref reader.TryRead(out value))
			{
				value = BinaryPrimitives.ReverseEndianness(value);
				return true;
			}
			return false;
		}
	}
}
