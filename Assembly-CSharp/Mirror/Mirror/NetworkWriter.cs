using System;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000060 RID: 96
	public class NetworkWriter
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000B5A1 File Offset: 0x000097A1
		public int Capacity
		{
			get
			{
				return this.buffer.Length;
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000B5AB File Offset: 0x000097AB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset()
		{
			this.Position = 0;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000B5B4 File Offset: 0x000097B4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void EnsureCapacity(int value)
		{
			if (this.buffer.Length < value)
			{
				int newSize = Math.Max(value, this.buffer.Length * 2);
				Array.Resize<byte>(ref this.buffer, newSize);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000B5EC File Offset: 0x000097EC
		public byte[] ToArray()
		{
			byte[] array = new byte[this.Position];
			Array.ConstrainedCopy(this.buffer, 0, array, 0, this.Position);
			return array;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000B61A File Offset: 0x0000981A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArraySegment<byte> ToArraySegment()
		{
			return new ArraySegment<byte>(this.buffer, 0, this.Position);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000B62E File Offset: 0x0000982E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ArraySegment<byte>(NetworkWriter w)
		{
			return w.ToArraySegment();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000B638 File Offset: 0x00009838
		internal unsafe void WriteBlittable<[IsUnmanaged] T>(T value) where T : struct, ValueType
		{
			int num = sizeof(T);
			this.EnsureCapacity(this.Position + num);
			fixed (byte* ptr = &this.buffer[this.Position])
			{
				*(T*)ptr = value;
			}
			this.Position += num;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000B685 File Offset: 0x00009885
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void WriteBlittableNullable<[IsUnmanaged] T>(T? value) where T : struct, ValueType
		{
			this.WriteByte((value != null) ? 1 : 0);
			if (value != null)
			{
				this.WriteBlittable<T>(value.Value);
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B6B1 File Offset: 0x000098B1
		public void WriteByte(byte value)
		{
			this.WriteBlittable<byte>(value);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B6BA File Offset: 0x000098BA
		public void WriteBytes(byte[] array, int offset, int count)
		{
			this.EnsureCapacity(this.Position + count);
			Array.ConstrainedCopy(array, offset, this.buffer, this.Position, count);
			this.Position += count;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000B6EC File Offset: 0x000098EC
		public unsafe bool WriteBytes(byte* ptr, int offset, int size)
		{
			this.EnsureCapacity(this.Position + size);
			fixed (byte* ptr2 = &this.buffer[this.Position])
			{
				UnsafeUtility.MemCpy((void*)ptr2, (void*)(ptr + offset), (long)size);
			}
			this.Position += size;
			return true;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000B738 File Offset: 0x00009938
		public void Write<T>(T value)
		{
			Action<NetworkWriter, T> write = Writer<T>.write;
			if (write == null)
			{
				Debug.LogError(string.Format("No writer found for {0}. This happens either if you are missing a NetworkWriter extension for your custom type, or if weaving failed. Try to reimport a script to weave again.", typeof(T)));
				return;
			}
			write(this, value);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000B770 File Offset: 0x00009970
		public override string ToString()
		{
			return string.Format("[{0} @ {1}/{2}]", this.ToArraySegment().ToHexString(), this.Position, this.Capacity);
		}

		// Token: 0x0400012F RID: 303
		public const ushort MaxStringLength = 65534;

		// Token: 0x04000130 RID: 304
		public const int DefaultCapacity = 1500;

		// Token: 0x04000131 RID: 305
		internal byte[] buffer = new byte[1500];

		// Token: 0x04000132 RID: 306
		public int Position;

		// Token: 0x04000133 RID: 307
		internal readonly UTF8Encoding encoding = new UTF8Encoding(false, true);
	}
}
