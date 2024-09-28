using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
	// Token: 0x020003BA RID: 954
	internal ref struct ValueStringBuilder
	{
		// Token: 0x0600277D RID: 10109 RVA: 0x0008FE12 File Offset: 0x0008E012
		public ValueStringBuilder(Span<char> initialBuffer)
		{
			this._arrayToReturnToPool = null;
			this._chars = initialBuffer;
			this._pos = 0;
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600277E RID: 10110 RVA: 0x0008FE29 File Offset: 0x0008E029
		// (set) Token: 0x0600277F RID: 10111 RVA: 0x0008FE31 File Offset: 0x0008E031
		public int Length
		{
			get
			{
				return this._pos;
			}
			set
			{
				this._pos = value;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x0008FE3A File Offset: 0x0008E03A
		public int Capacity
		{
			get
			{
				return this._chars.Length;
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x0008FE47 File Offset: 0x0008E047
		public void EnsureCapacity(int capacity)
		{
			if (capacity > this._chars.Length)
			{
				this.Grow(capacity - this._chars.Length);
			}
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x0008FE6A File Offset: 0x0008E06A
		public unsafe ref char GetPinnableReference(bool terminate = false)
		{
			if (terminate)
			{
				this.EnsureCapacity(this.Length + 1);
				*this._chars[this.Length] = '\0';
			}
			return MemoryMarshal.GetReference<char>(this._chars);
		}

		// Token: 0x170004CA RID: 1226
		public char this[int index]
		{
			get
			{
				return this._chars[index];
			}
		}

		// Token: 0x06002784 RID: 10116 RVA: 0x0008FEA9 File Offset: 0x0008E0A9
		public override string ToString()
		{
			string result = new string(this._chars.Slice(0, this._pos));
			this.Dispose();
			return result;
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002785 RID: 10117 RVA: 0x0008FECD File Offset: 0x0008E0CD
		public Span<char> RawChars
		{
			get
			{
				return this._chars;
			}
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x0008FED5 File Offset: 0x0008E0D5
		public unsafe ReadOnlySpan<char> AsSpan(bool terminate)
		{
			if (terminate)
			{
				this.EnsureCapacity(this.Length + 1);
				*this._chars[this.Length] = '\0';
			}
			return this._chars.Slice(0, this._pos);
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x0008FF12 File Offset: 0x0008E112
		public ReadOnlySpan<char> AsSpan()
		{
			return this._chars.Slice(0, this._pos);
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x0008FF2B File Offset: 0x0008E12B
		public ReadOnlySpan<char> AsSpan(int start)
		{
			return this._chars.Slice(start, this._pos - start);
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x0008FF46 File Offset: 0x0008E146
		public ReadOnlySpan<char> AsSpan(int start, int length)
		{
			return this._chars.Slice(start, length);
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x0008FF5C File Offset: 0x0008E15C
		public bool TryCopyTo(Span<char> destination, out int charsWritten)
		{
			if (this._chars.Slice(0, this._pos).TryCopyTo(destination))
			{
				charsWritten = this._pos;
				this.Dispose();
				return true;
			}
			charsWritten = 0;
			this.Dispose();
			return false;
		}

		// Token: 0x0600278B RID: 10123 RVA: 0x0008FFA0 File Offset: 0x0008E1A0
		public void Insert(int index, char value, int count)
		{
			if (this._pos > this._chars.Length - count)
			{
				this.Grow(count);
			}
			int length = this._pos - index;
			this._chars.Slice(index, length).CopyTo(this._chars.Slice(index + count));
			this._chars.Slice(index, count).Fill(value);
			this._pos += count;
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x0009001C File Offset: 0x0008E21C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Append(char c)
		{
			int pos = this._pos;
			if (pos < this._chars.Length)
			{
				*this._chars[pos] = c;
				this._pos = pos + 1;
				return;
			}
			this.GrowAndAppend(c);
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x00090060 File Offset: 0x0008E260
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Append(string s)
		{
			int pos = this._pos;
			if (s.Length == 1 && pos < this._chars.Length)
			{
				*this._chars[pos] = s[0];
				this._pos = pos + 1;
				return;
			}
			this.AppendSlow(s);
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000900B0 File Offset: 0x0008E2B0
		private void AppendSlow(string s)
		{
			int pos = this._pos;
			if (pos > this._chars.Length - s.Length)
			{
				this.Grow(s.Length);
			}
			s.AsSpan().CopyTo(this._chars.Slice(pos));
			this._pos += s.Length;
		}

		// Token: 0x0600278F RID: 10127 RVA: 0x00090114 File Offset: 0x0008E314
		public unsafe void Append(char c, int count)
		{
			if (this._pos > this._chars.Length - count)
			{
				this.Grow(count);
			}
			Span<char> span = this._chars.Slice(this._pos, count);
			for (int i = 0; i < span.Length; i++)
			{
				*span[i] = c;
			}
			this._pos += count;
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x0009017C File Offset: 0x0008E37C
		public unsafe void Append(char* value, int length)
		{
			if (this._pos > this._chars.Length - length)
			{
				this.Grow(length);
			}
			Span<char> span = this._chars.Slice(this._pos, length);
			for (int i = 0; i < span.Length; i++)
			{
				*span[i] = *(value++);
			}
			this._pos += length;
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000901E8 File Offset: 0x0008E3E8
		public void Append(ReadOnlySpan<char> value)
		{
			if (this._pos > this._chars.Length - value.Length)
			{
				this.Grow(value.Length);
			}
			value.CopyTo(this._chars.Slice(this._pos));
			this._pos += value.Length;
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x0009024C File Offset: 0x0008E44C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Span<char> AppendSpan(int length)
		{
			int pos = this._pos;
			if (pos > this._chars.Length - length)
			{
				this.Grow(length);
			}
			this._pos = pos + length;
			return this._chars.Slice(pos, length);
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x0009028D File Offset: 0x0008E48D
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void GrowAndAppend(char c)
		{
			this.Grow(1);
			this.Append(c);
		}

		// Token: 0x06002794 RID: 10132 RVA: 0x000902A0 File Offset: 0x0008E4A0
		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Grow(int requiredAdditionalCapacity)
		{
			char[] array = ArrayPool<char>.Shared.Rent(Math.Max(this._pos + requiredAdditionalCapacity, this._chars.Length * 2));
			this._chars.CopyTo(array);
			char[] arrayToReturnToPool = this._arrayToReturnToPool;
			this._chars = (this._arrayToReturnToPool = array);
			if (arrayToReturnToPool != null)
			{
				ArrayPool<char>.Shared.Return(arrayToReturnToPool, false);
			}
		}

		// Token: 0x06002795 RID: 10133 RVA: 0x00090310 File Offset: 0x0008E510
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			char[] arrayToReturnToPool = this._arrayToReturnToPool;
			this = default(ValueStringBuilder);
			if (arrayToReturnToPool != null)
			{
				ArrayPool<char>.Shared.Return(arrayToReturnToPool, false);
			}
		}

		// Token: 0x04001E0F RID: 7695
		private char[] _arrayToReturnToPool;

		// Token: 0x04001E10 RID: 7696
		private Span<char> _chars;

		// Token: 0x04001E11 RID: 7697
		private int _pos;
	}
}
