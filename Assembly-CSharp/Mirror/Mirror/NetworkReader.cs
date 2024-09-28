using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000055 RID: 85
	public class NetworkReader
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000202 RID: 514 RVA: 0x00008FA7 File Offset: 0x000071A7
		public int Remaining
		{
			get
			{
				return this.buffer.Count - this.Position;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000203 RID: 515 RVA: 0x00008FBB File Offset: 0x000071BB
		public int Capacity
		{
			get
			{
				return this.buffer.Count;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00008FC8 File Offset: 0x000071C8
		public NetworkReader(ArraySegment<byte> segment)
		{
			this.buffer = segment;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00008FE4 File Offset: 0x000071E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetBuffer(ArraySegment<byte> segment)
		{
			this.buffer = segment;
			this.Position = 0;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008FF4 File Offset: 0x000071F4
		internal unsafe T ReadBlittable<[IsUnmanaged] T>() where T : struct, ValueType
		{
			int num = sizeof(T);
			if (this.Remaining < num)
			{
				throw new EndOfStreamException(string.Format("ReadBlittable<{0}> not enough data in buffer to read {1} bytes: {2}", typeof(T), num, this.ToString()));
			}
			T result;
			fixed (byte* ptr = &this.buffer.Array[this.buffer.Offset + this.Position])
			{
				result = *(T*)ptr;
			}
			this.Position += num;
			return result;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00009074 File Offset: 0x00007274
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal T? ReadBlittableNullable<[IsUnmanaged] T>() where T : struct, ValueType
		{
			if (this.ReadByte() == 0)
			{
				return null;
			}
			return new T?(this.ReadBlittable<T>());
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000909E File Offset: 0x0000729E
		public byte ReadByte()
		{
			return this.ReadBlittable<byte>();
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000090A8 File Offset: 0x000072A8
		public byte[] ReadBytes(byte[] bytes, int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("ReadBytes requires count >= 0");
			}
			if (count > bytes.Length)
			{
				throw new EndOfStreamException(string.Format("ReadBytes can't read {0} + bytes because the passed byte[] only has length {1}", count, bytes.Length));
			}
			if (this.Remaining < count)
			{
				throw new EndOfStreamException(string.Format("ReadBytesSegment can't read {0} bytes because it would read past the end of the stream. {1}", count, this.ToString()));
			}
			Array.Copy(this.buffer.Array, this.buffer.Offset + this.Position, bytes, 0, count);
			this.Position += count;
			return bytes;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00009144 File Offset: 0x00007344
		public ArraySegment<byte> ReadBytesSegment(int count)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("ReadBytesSegment requires count >= 0");
			}
			if (this.Remaining < count)
			{
				throw new EndOfStreamException(string.Format("ReadBytesSegment can't read {0} bytes because it would read past the end of the stream. {1}", count, this.ToString()));
			}
			ArraySegment<byte> result = new ArraySegment<byte>(this.buffer.Array, this.buffer.Offset + this.Position, count);
			this.Position += count;
			return result;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000091B8 File Offset: 0x000073B8
		public T Read<T>()
		{
			Func<NetworkReader, T> read = Reader<T>.read;
			if (read == null)
			{
				Debug.LogError(string.Format("No reader found for {0}. Use a type supported by Mirror or define a custom reader extension for {1}.", typeof(T), typeof(T)));
				return default(T);
			}
			return read(this);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00009202 File Offset: 0x00007402
		public override string ToString()
		{
			return string.Format("[{0} @ {1}/{2}]", this.buffer.ToHexString(), this.Position, this.Capacity);
		}

		// Token: 0x04000108 RID: 264
		internal ArraySegment<byte> buffer;

		// Token: 0x04000109 RID: 265
		public int Position;

		// Token: 0x0400010A RID: 266
		internal readonly UTF8Encoding encoding = new UTF8Encoding(false, true);

		// Token: 0x0400010B RID: 267
		public const int AllocationLimit = 16777216;
	}
}
