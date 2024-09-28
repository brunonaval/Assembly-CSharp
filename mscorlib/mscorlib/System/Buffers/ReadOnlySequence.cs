﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers
{
	// Token: 0x02000AE5 RID: 2789
	[DebuggerTypeProxy(typeof(ReadOnlySequenceDebugView<>))]
	[DebuggerDisplay("{ToString(),raw}")]
	public readonly struct ReadOnlySequence<T>
	{
		// Token: 0x17001188 RID: 4488
		// (get) Token: 0x06006320 RID: 25376 RVA: 0x0014B65B File Offset: 0x0014985B
		public long Length
		{
			get
			{
				return this.GetLength();
			}
		}

		// Token: 0x17001189 RID: 4489
		// (get) Token: 0x06006321 RID: 25377 RVA: 0x0014B663 File Offset: 0x00149863
		public bool IsEmpty
		{
			get
			{
				return this.Length == 0L;
			}
		}

		// Token: 0x1700118A RID: 4490
		// (get) Token: 0x06006322 RID: 25378 RVA: 0x0014B66F File Offset: 0x0014986F
		public bool IsSingleSegment
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this._startObject == this._endObject;
			}
		}

		// Token: 0x1700118B RID: 4491
		// (get) Token: 0x06006323 RID: 25379 RVA: 0x0014B67F File Offset: 0x0014987F
		public ReadOnlyMemory<T> First
		{
			get
			{
				return this.GetFirstBuffer();
			}
		}

		// Token: 0x1700118C RID: 4492
		// (get) Token: 0x06006324 RID: 25380 RVA: 0x0014B687 File Offset: 0x00149887
		public ReadOnlySpan<T> FirstSpan
		{
			get
			{
				return this.GetFirstSpan();
			}
		}

		// Token: 0x1700118D RID: 4493
		// (get) Token: 0x06006325 RID: 25381 RVA: 0x0014B68F File Offset: 0x0014988F
		public SequencePosition Start
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new SequencePosition(this._startObject, this._startInteger);
			}
		}

		// Token: 0x1700118E RID: 4494
		// (get) Token: 0x06006326 RID: 25382 RVA: 0x0014B6A2 File Offset: 0x001498A2
		public SequencePosition End
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return new SequencePosition(this._endObject, this._endInteger);
			}
		}

		// Token: 0x06006327 RID: 25383 RVA: 0x0014B6B5 File Offset: 0x001498B5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlySequence(object startSegment, int startIndexAndFlags, object endSegment, int endIndexAndFlags)
		{
			this._startObject = startSegment;
			this._endObject = endSegment;
			this._startInteger = startIndexAndFlags;
			this._endInteger = endIndexAndFlags;
		}

		// Token: 0x06006328 RID: 25384 RVA: 0x0014B6D4 File Offset: 0x001498D4
		public ReadOnlySequence(ReadOnlySequenceSegment<T> startSegment, int startIndex, ReadOnlySequenceSegment<T> endSegment, int endIndex)
		{
			if (startSegment == null || endSegment == null || (startSegment != endSegment && startSegment.RunningIndex > endSegment.RunningIndex) || startSegment.Memory.Length < startIndex || endSegment.Memory.Length < endIndex || (startSegment == endSegment && endIndex < startIndex))
			{
				ThrowHelper.ThrowArgumentValidationException<T>(startSegment, startIndex, endSegment);
			}
			this._startObject = startSegment;
			this._endObject = endSegment;
			this._startInteger = ReadOnlySequence.SegmentToSequenceStart(startIndex);
			this._endInteger = ReadOnlySequence.SegmentToSequenceEnd(endIndex);
		}

		// Token: 0x06006329 RID: 25385 RVA: 0x0014B754 File Offset: 0x00149954
		public ReadOnlySequence(T[] array)
		{
			if (array == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.array);
			}
			this._startObject = array;
			this._endObject = array;
			this._startInteger = ReadOnlySequence.ArrayToSequenceStart(0);
			this._endInteger = ReadOnlySequence.ArrayToSequenceEnd(array.Length);
		}

		// Token: 0x0600632A RID: 25386 RVA: 0x0014B788 File Offset: 0x00149988
		public ReadOnlySequence(T[] array, int start, int length)
		{
			if (array == null || start > array.Length || length > array.Length - start)
			{
				ThrowHelper.ThrowArgumentValidationException(array, start);
			}
			this._startObject = array;
			this._endObject = array;
			this._startInteger = ReadOnlySequence.ArrayToSequenceStart(start);
			this._endInteger = ReadOnlySequence.ArrayToSequenceEnd(start + length);
		}

		// Token: 0x0600632B RID: 25387 RVA: 0x0014B7D8 File Offset: 0x001499D8
		public ReadOnlySequence(ReadOnlyMemory<T> memory)
		{
			MemoryManager<T> memoryManager;
			int startIndex;
			int num;
			if (MemoryMarshal.TryGetMemoryManager<T, MemoryManager<T>>(memory, out memoryManager, out startIndex, out num))
			{
				this._startObject = memoryManager;
				this._endObject = memoryManager;
				this._startInteger = ReadOnlySequence.MemoryManagerToSequenceStart(startIndex);
				this._endInteger = ReadOnlySequence.MemoryManagerToSequenceEnd(num);
				return;
			}
			ArraySegment<T> arraySegment;
			if (MemoryMarshal.TryGetArray<T>(memory, out arraySegment))
			{
				T[] array = arraySegment.Array;
				int offset = arraySegment.Offset;
				this._startObject = array;
				this._endObject = array;
				this._startInteger = ReadOnlySequence.ArrayToSequenceStart(offset);
				this._endInteger = ReadOnlySequence.ArrayToSequenceEnd(offset + arraySegment.Count);
				return;
			}
			if (typeof(T) == typeof(char))
			{
				string text;
				int num2;
				if (!MemoryMarshal.TryGetString((ReadOnlyMemory<char>)memory, out text, out num2, out num))
				{
					ThrowHelper.ThrowInvalidOperationException();
				}
				this._startObject = text;
				this._endObject = text;
				this._startInteger = ReadOnlySequence.StringToSequenceStart(num2);
				this._endInteger = ReadOnlySequence.StringToSequenceEnd(num2 + num);
				return;
			}
			ThrowHelper.ThrowInvalidOperationException();
			this._startObject = null;
			this._endObject = null;
			this._startInteger = 0;
			this._endInteger = 0;
		}

		// Token: 0x0600632C RID: 25388 RVA: 0x0014B8F0 File Offset: 0x00149AF0
		public ReadOnlySequence<T> Slice(long start, long length)
		{
			if (start < 0L || length < 0L)
			{
				ThrowHelper.ThrowStartOrEndArgumentValidationException(start);
			}
			int num = ReadOnlySequence<T>.GetIndex(this._startInteger);
			int index = ReadOnlySequence<T>.GetIndex(this._endInteger);
			object startObject = this._startObject;
			object endObject = this._endObject;
			SequencePosition sequencePosition;
			SequencePosition endPosition;
			if (startObject != endObject)
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)startObject;
				int num2 = readOnlySequenceSegment.Memory.Length - num;
				if ((long)num2 > start)
				{
					num += (int)start;
					sequencePosition = new SequencePosition(startObject, num);
					endPosition = ReadOnlySequence<T>.GetEndPosition(readOnlySequenceSegment, startObject, num, endObject, index, length);
				}
				else
				{
					if (num2 < 0)
					{
						ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					}
					sequencePosition = ReadOnlySequence<T>.SeekMultiSegment(readOnlySequenceSegment.Next, endObject, index, start - (long)num2, ExceptionArgument.start);
					int index2 = ReadOnlySequence<T>.GetIndex(sequencePosition);
					object @object = sequencePosition.GetObject();
					if (@object != endObject)
					{
						endPosition = ReadOnlySequence<T>.GetEndPosition((ReadOnlySequenceSegment<T>)@object, @object, index2, endObject, index, length);
					}
					else
					{
						if ((long)(index - index2) < length)
						{
							ThrowHelper.ThrowStartOrEndArgumentValidationException(0L);
						}
						endPosition = new SequencePosition(@object, index2 + (int)length);
					}
				}
			}
			else
			{
				if ((long)(index - num) < start)
				{
					ThrowHelper.ThrowStartOrEndArgumentValidationException(-1L);
				}
				num += (int)start;
				sequencePosition = new SequencePosition(startObject, num);
				if ((long)(index - num) < length)
				{
					ThrowHelper.ThrowStartOrEndArgumentValidationException(0L);
				}
				endPosition = new SequencePosition(startObject, num + (int)length);
			}
			return this.SliceImpl(sequencePosition, endPosition);
		}

		// Token: 0x0600632D RID: 25389 RVA: 0x0014BA38 File Offset: 0x00149C38
		public ReadOnlySequence<T> Slice(long start, SequencePosition end)
		{
			if (start < 0L)
			{
				ThrowHelper.ThrowStartOrEndArgumentValidationException(start);
			}
			uint index = (uint)ReadOnlySequence<T>.GetIndex(this._startInteger);
			object startObject = this._startObject;
			uint index2 = (uint)ReadOnlySequence<T>.GetIndex(this._endInteger);
			object endObject = this._endObject;
			uint num = (uint)ReadOnlySequence<T>.GetIndex(end);
			object obj = end.GetObject();
			if (obj == null)
			{
				obj = this._startObject;
				num = index;
			}
			if (startObject == endObject)
			{
				if (!ReadOnlySequence<T>.InRange(num, index, index2))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
				if ((ulong)(num - index) < (ulong)start)
				{
					ThrowHelper.ThrowStartOrEndArgumentValidationException(-1L);
				}
			}
			else
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)startObject;
				ulong num2 = (ulong)(readOnlySequenceSegment.RunningIndex + (long)((ulong)index));
				ulong num3 = (ulong)(((ReadOnlySequenceSegment<T>)obj).RunningIndex + (long)((ulong)num));
				if (!ReadOnlySequence<T>.InRange(num3, num2, (ulong)(((ReadOnlySequenceSegment<T>)endObject).RunningIndex + (long)((ulong)index2))))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
				if (num2 + (ulong)start > num3)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.start);
				}
				int num4 = readOnlySequenceSegment.Memory.Length - (int)index;
				if ((long)num4 <= start)
				{
					if (num4 < 0)
					{
						ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					}
					SequencePosition sequencePosition = ReadOnlySequence<T>.SeekMultiSegment(readOnlySequenceSegment.Next, obj, (int)num, start - (long)num4, ExceptionArgument.start);
					return this.SliceImpl(sequencePosition, end);
				}
			}
			SequencePosition sequencePosition2 = new SequencePosition(startObject, (int)(index + (uint)((int)start)));
			SequencePosition sequencePosition3 = new SequencePosition(obj, (int)num);
			return this.SliceImpl(sequencePosition2, sequencePosition3);
		}

		// Token: 0x0600632E RID: 25390 RVA: 0x0014BB80 File Offset: 0x00149D80
		public ReadOnlySequence<T> Slice(SequencePosition start, long length)
		{
			uint index = (uint)ReadOnlySequence<T>.GetIndex(this._startInteger);
			object startObject = this._startObject;
			uint index2 = (uint)ReadOnlySequence<T>.GetIndex(this._endInteger);
			object endObject = this._endObject;
			uint num = (uint)ReadOnlySequence<T>.GetIndex(start);
			object obj = start.GetObject();
			if (obj == null)
			{
				num = index;
				obj = this._startObject;
			}
			if (startObject == endObject)
			{
				if (!ReadOnlySequence<T>.InRange(num, index, index2))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
				if (length < 0L)
				{
					ThrowHelper.ThrowStartOrEndArgumentValidationException(0L);
				}
				if ((ulong)(index2 - num) < (ulong)length)
				{
					ThrowHelper.ThrowStartOrEndArgumentValidationException(0L);
				}
			}
			else
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)obj;
				long num2 = readOnlySequenceSegment.RunningIndex + (long)((ulong)num);
				ulong start2 = (ulong)(((ReadOnlySequenceSegment<T>)startObject).RunningIndex + (long)((ulong)index));
				ulong num3 = (ulong)(((ReadOnlySequenceSegment<T>)endObject).RunningIndex + (long)((ulong)index2));
				if (!ReadOnlySequence<T>.InRange((ulong)num2, start2, num3))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
				if (length < 0L)
				{
					ThrowHelper.ThrowStartOrEndArgumentValidationException(0L);
				}
				if (num2 + length > (long)num3)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
				}
				int num4 = readOnlySequenceSegment.Memory.Length - (int)num;
				if ((long)num4 < length)
				{
					if (num4 < 0)
					{
						ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					}
					SequencePosition sequencePosition = ReadOnlySequence<T>.SeekMultiSegment(readOnlySequenceSegment.Next, endObject, (int)index2, length - (long)num4, ExceptionArgument.length);
					return this.SliceImpl(start, sequencePosition);
				}
			}
			SequencePosition sequencePosition2 = new SequencePosition(obj, (int)num);
			SequencePosition sequencePosition3 = new SequencePosition(obj, (int)(num + (uint)((int)length)));
			return this.SliceImpl(sequencePosition2, sequencePosition3);
		}

		// Token: 0x0600632F RID: 25391 RVA: 0x0014BCD3 File Offset: 0x00149ED3
		public ReadOnlySequence<T> Slice(int start, int length)
		{
			return this.Slice((long)start, (long)length);
		}

		// Token: 0x06006330 RID: 25392 RVA: 0x0014BCDF File Offset: 0x00149EDF
		public ReadOnlySequence<T> Slice(int start, SequencePosition end)
		{
			return this.Slice((long)start, end);
		}

		// Token: 0x06006331 RID: 25393 RVA: 0x0014BCEA File Offset: 0x00149EEA
		public ReadOnlySequence<T> Slice(SequencePosition start, int length)
		{
			return this.Slice(start, (long)length);
		}

		// Token: 0x06006332 RID: 25394 RVA: 0x0014BCF5 File Offset: 0x00149EF5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySequence<T> Slice(SequencePosition start, SequencePosition end)
		{
			this.BoundsCheck((uint)ReadOnlySequence<T>.GetIndex(start), start.GetObject(), (uint)ReadOnlySequence<T>.GetIndex(end), end.GetObject());
			return this.SliceImpl(start, end);
		}

		// Token: 0x06006333 RID: 25395 RVA: 0x0014BD24 File Offset: 0x00149F24
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySequence<T> Slice(SequencePosition start)
		{
			bool flag = start.GetObject() != null;
			this.BoundsCheck(start, flag);
			SequencePosition sequencePosition = flag ? start : this.Start;
			return this.SliceImpl(sequencePosition);
		}

		// Token: 0x06006334 RID: 25396 RVA: 0x0014BD5C File Offset: 0x00149F5C
		public ReadOnlySequence<T> Slice(long start)
		{
			if (start < 0L)
			{
				ThrowHelper.ThrowStartOrEndArgumentValidationException(start);
			}
			if (start == 0L)
			{
				return this;
			}
			SequencePosition sequencePosition = this.Seek(start, ExceptionArgument.start);
			return this.SliceImpl(sequencePosition);
		}

		// Token: 0x06006335 RID: 25397 RVA: 0x0014BD90 File Offset: 0x00149F90
		public unsafe override string ToString()
		{
			if (typeof(T) == typeof(char))
			{
				ReadOnlySequence<T> readOnlySequence = this;
				ReadOnlySequence<char> readOnlySequence2 = *Unsafe.As<ReadOnlySequence<T>, ReadOnlySequence<char>>(ref readOnlySequence);
				string text;
				int startIndex;
				int length;
				if (SequenceMarshal.TryGetString(readOnlySequence2, out text, out startIndex, out length))
				{
					return text.Substring(startIndex, length);
				}
				if (this.Length < 2147483647L)
				{
					return string.Create<ReadOnlySequence<char>>((int)this.Length, readOnlySequence2, delegate(Span<char> span, ReadOnlySequence<char> sequence)
					{
						sequence.CopyTo(span);
					});
				}
			}
			return string.Format("System.Buffers.ReadOnlySequence<{0}>[{1}]", typeof(T).Name, this.Length);
		}

		// Token: 0x06006336 RID: 25398 RVA: 0x0014BE43 File Offset: 0x0014A043
		public ReadOnlySequence<T>.Enumerator GetEnumerator()
		{
			return new ReadOnlySequence<T>.Enumerator(ref this);
		}

		// Token: 0x06006337 RID: 25399 RVA: 0x0014BE4B File Offset: 0x0014A04B
		public SequencePosition GetPosition(long offset)
		{
			if (offset < 0L)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_OffsetOutOfRange();
			}
			return this.Seek(offset, ExceptionArgument.offset);
		}

		// Token: 0x06006338 RID: 25400 RVA: 0x0014BE60 File Offset: 0x0014A060
		public SequencePosition GetPosition(long offset, SequencePosition origin)
		{
			if (offset < 0L)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_OffsetOutOfRange();
			}
			return this.Seek(origin, offset);
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x0014BE78 File Offset: 0x0014A078
		public bool TryGet(ref SequencePosition position, out ReadOnlyMemory<T> memory, bool advance = true)
		{
			SequencePosition sequencePosition;
			bool result = this.TryGetBuffer(position, out memory, out sequencePosition);
			if (advance)
			{
				position = sequencePosition;
			}
			return result;
		}

		// Token: 0x0600633A RID: 25402 RVA: 0x0014BE9C File Offset: 0x0014A09C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal bool TryGetBuffer(in SequencePosition position, out ReadOnlyMemory<T> memory, out SequencePosition next)
		{
			object @object = position.GetObject();
			next = default(SequencePosition);
			if (@object == null)
			{
				memory = default(ReadOnlyMemory<T>);
				return false;
			}
			ReadOnlySequence<T>.SequenceType sequenceType = this.GetSequenceType();
			object endObject = this._endObject;
			int index = ReadOnlySequence<T>.GetIndex(position);
			int index2 = ReadOnlySequence<T>.GetIndex(this._endInteger);
			if (sequenceType == ReadOnlySequence<T>.SequenceType.MultiSegment)
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)@object;
				if (readOnlySequenceSegment != endObject)
				{
					ReadOnlySequenceSegment<T> next2 = readOnlySequenceSegment.Next;
					if (next2 == null)
					{
						ThrowHelper.ThrowInvalidOperationException_EndPositionNotReached();
					}
					next = new SequencePosition(next2, 0);
					memory = readOnlySequenceSegment.Memory.Slice(index);
				}
				else
				{
					memory = readOnlySequenceSegment.Memory.Slice(index, index2 - index);
				}
			}
			else
			{
				if (@object != endObject)
				{
					ThrowHelper.ThrowInvalidOperationException_EndPositionNotReached();
				}
				if (sequenceType == ReadOnlySequence<T>.SequenceType.Array)
				{
					memory = new ReadOnlyMemory<T>((T[])@object, index, index2 - index);
				}
				else if (typeof(T) == typeof(char) && sequenceType == ReadOnlySequence<T>.SequenceType.String)
				{
					memory = (ReadOnlyMemory<T>)((string)@object).AsMemory(index, index2 - index);
				}
				else
				{
					memory = ((MemoryManager<T>)@object).Memory.Slice(index, index2 - index);
				}
			}
			return true;
		}

		// Token: 0x0600633B RID: 25403 RVA: 0x0014BFDC File Offset: 0x0014A1DC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlyMemory<T> GetFirstBuffer()
		{
			object startObject = this._startObject;
			if (startObject == null)
			{
				return default(ReadOnlyMemory<T>);
			}
			int startInteger = this._startInteger;
			int endInteger = this._endInteger;
			bool flag = startObject != this._endObject;
			if ((startInteger | endInteger) < 0)
			{
				return this.GetFirstBufferSlow(startObject, flag);
			}
			ReadOnlyMemory<T> memory = ((ReadOnlySequenceSegment<T>)startObject).Memory;
			if (flag)
			{
				return memory.Slice(startInteger);
			}
			return memory.Slice(startInteger, endInteger - startInteger);
		}

		// Token: 0x0600633C RID: 25404 RVA: 0x0014C04C File Offset: 0x0014A24C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private ReadOnlyMemory<T> GetFirstBufferSlow(object startObject, bool isMultiSegment)
		{
			if (isMultiSegment)
			{
				ThrowHelper.ThrowInvalidOperationException_EndPositionNotReached();
			}
			int num = this._startInteger;
			int endInteger = this._endInteger;
			if (num >= 0)
			{
				return new ReadOnlyMemory<T>((T[])startObject, num, (endInteger & int.MaxValue) - num);
			}
			if (typeof(T) == typeof(char) && endInteger < 0)
			{
				return (ReadOnlyMemory<T>)((string)startObject).AsMemory(num & int.MaxValue, endInteger - num);
			}
			num &= int.MaxValue;
			return ((MemoryManager<T>)startObject).Memory.Slice(num, endInteger - num);
		}

		// Token: 0x0600633D RID: 25405 RVA: 0x0014C0EC File Offset: 0x0014A2EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlySpan<T> GetFirstSpan()
		{
			object startObject = this._startObject;
			if (startObject == null)
			{
				return default(ReadOnlySpan<T>);
			}
			int startInteger = this._startInteger;
			int endInteger = this._endInteger;
			bool flag = startObject != this._endObject;
			if ((startInteger | endInteger) < 0)
			{
				return this.GetFirstSpanSlow(startObject, flag);
			}
			ReadOnlySpan<T> span = ((ReadOnlySequenceSegment<T>)startObject).Memory.Span;
			if (flag)
			{
				return span.Slice(startInteger);
			}
			return span.Slice(startInteger, endInteger - startInteger);
		}

		// Token: 0x0600633E RID: 25406 RVA: 0x0014C168 File Offset: 0x0014A368
		[MethodImpl(MethodImplOptions.NoInlining)]
		private ReadOnlySpan<T> GetFirstSpanSlow(object startObject, bool isMultiSegment)
		{
			if (isMultiSegment)
			{
				ThrowHelper.ThrowInvalidOperationException_EndPositionNotReached();
			}
			int num = this._startInteger;
			int endInteger = this._endInteger;
			if (num >= 0)
			{
				return ((T[])startObject).Slice(num, (endInteger & int.MaxValue) - num);
			}
			if (typeof(T) == typeof(char) && endInteger < 0)
			{
				return ((ReadOnlyMemory<T>)((string)startObject).AsMemory()).Span.Slice(num & int.MaxValue, endInteger - num);
			}
			num &= int.MaxValue;
			return ((MemoryManager<T>)startObject).Memory.Span.Slice(num, endInteger - num);
		}

		// Token: 0x0600633F RID: 25407 RVA: 0x0014C22C File Offset: 0x0014A42C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal SequencePosition Seek(long offset, ExceptionArgument exceptionArgument = ExceptionArgument.offset)
		{
			object startObject = this._startObject;
			object endObject = this._endObject;
			int index = ReadOnlySequence<T>.GetIndex(this._startInteger);
			int index2 = ReadOnlySequence<T>.GetIndex(this._endInteger);
			if (startObject != endObject)
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)startObject;
				int num = readOnlySequenceSegment.Memory.Length - index;
				if ((long)num <= offset)
				{
					if (num < 0)
					{
						ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					}
					return ReadOnlySequence<T>.SeekMultiSegment(readOnlySequenceSegment.Next, endObject, index2, offset - (long)num, exceptionArgument);
				}
			}
			else if ((long)(index2 - index) < offset)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(exceptionArgument);
			}
			return new SequencePosition(startObject, index + (int)offset);
		}

		// Token: 0x06006340 RID: 25408 RVA: 0x0014C2BC File Offset: 0x0014A4BC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private SequencePosition Seek(in SequencePosition start, long offset)
		{
			object @object = start.GetObject();
			object endObject = this._endObject;
			int index = ReadOnlySequence<T>.GetIndex(start);
			int index2 = ReadOnlySequence<T>.GetIndex(this._endInteger);
			if (@object != endObject)
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)@object;
				int num = readOnlySequenceSegment.Memory.Length - index;
				if ((long)num <= offset)
				{
					if (num < 0)
					{
						ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					}
					return ReadOnlySequence<T>.SeekMultiSegment(readOnlySequenceSegment.Next, endObject, index2, offset - (long)num, ExceptionArgument.offset);
				}
			}
			else if ((long)(index2 - index) < offset)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.offset);
			}
			return new SequencePosition(@object, index + (int)offset);
		}

		// Token: 0x06006341 RID: 25409 RVA: 0x0014C348 File Offset: 0x0014A548
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static SequencePosition SeekMultiSegment(ReadOnlySequenceSegment<T> currentSegment, object endObject, int endIndex, long offset, ExceptionArgument argument)
		{
			while (currentSegment != null && currentSegment != endObject)
			{
				int length = currentSegment.Memory.Length;
				if ((long)length > offset)
				{
					IL_3A:
					return new SequencePosition(currentSegment, (int)offset);
				}
				offset -= (long)length;
				currentSegment = currentSegment.Next;
			}
			if (currentSegment == null || (long)endIndex < offset)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException(argument);
				goto IL_3A;
			}
			goto IL_3A;
		}

		// Token: 0x06006342 RID: 25410 RVA: 0x0014C398 File Offset: 0x0014A598
		private void BoundsCheck(in SequencePosition position, bool positionIsNotNull)
		{
			uint index = (uint)ReadOnlySequence<T>.GetIndex(position);
			object startObject = this._startObject;
			object endObject = this._endObject;
			uint index2 = (uint)ReadOnlySequence<T>.GetIndex(this._startInteger);
			uint index3 = (uint)ReadOnlySequence<T>.GetIndex(this._endInteger);
			if (startObject == endObject)
			{
				if (!ReadOnlySequence<T>.InRange(index, index2, index3))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					return;
				}
			}
			else
			{
				ulong start = (ulong)(((ReadOnlySequenceSegment<T>)startObject).RunningIndex + (long)((ulong)index2));
				long num = 0L;
				if (positionIsNotNull)
				{
					num = ((ReadOnlySequenceSegment<T>)position.GetObject()).RunningIndex;
				}
				if (!ReadOnlySequence<T>.InRange((ulong)(num + (long)((ulong)index)), start, (ulong)(((ReadOnlySequenceSegment<T>)endObject).RunningIndex + (long)((ulong)index3))))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
			}
		}

		// Token: 0x06006343 RID: 25411 RVA: 0x0014C434 File Offset: 0x0014A634
		private void BoundsCheck(uint sliceStartIndex, object sliceStartObject, uint sliceEndIndex, object sliceEndObject)
		{
			object startObject = this._startObject;
			object endObject = this._endObject;
			uint index = (uint)ReadOnlySequence<T>.GetIndex(this._startInteger);
			uint index2 = (uint)ReadOnlySequence<T>.GetIndex(this._endInteger);
			if (startObject == endObject)
			{
				if (sliceStartObject != sliceEndObject || sliceStartObject != startObject || sliceStartIndex > sliceEndIndex || sliceStartIndex < index || sliceEndIndex > index2)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
					return;
				}
			}
			else
			{
				ulong num = (ulong)sliceStartIndex;
				ulong num2 = (ulong)sliceEndIndex;
				if (sliceStartObject != null)
				{
					num += (ulong)((ReadOnlySequenceSegment<T>)sliceStartObject).RunningIndex;
				}
				if (sliceEndObject != null)
				{
					num2 += (ulong)((ReadOnlySequenceSegment<T>)sliceEndObject).RunningIndex;
				}
				if (num > num2)
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
				if (num < (ulong)(((ReadOnlySequenceSegment<T>)startObject).RunningIndex + (long)((ulong)index)) || num2 > (ulong)(((ReadOnlySequenceSegment<T>)endObject).RunningIndex + (long)((ulong)index2)))
				{
					ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
				}
			}
		}

		// Token: 0x06006344 RID: 25412 RVA: 0x0014C4EC File Offset: 0x0014A6EC
		private static SequencePosition GetEndPosition(ReadOnlySequenceSegment<T> startSegment, object startObject, int startIndex, object endObject, int endIndex, long length)
		{
			int num = startSegment.Memory.Length - startIndex;
			if ((long)num > length)
			{
				return new SequencePosition(startObject, startIndex + (int)length);
			}
			if (num < 0)
			{
				ThrowHelper.ThrowArgumentOutOfRangeException_PositionOutOfRange();
			}
			return ReadOnlySequence<T>.SeekMultiSegment(startSegment.Next, endObject, endIndex, length - (long)num, ExceptionArgument.length);
		}

		// Token: 0x06006345 RID: 25413 RVA: 0x0014C53A File Offset: 0x0014A73A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlySequence<T>.SequenceType GetSequenceType()
		{
			return (ReadOnlySequence<T>.SequenceType)(-(ReadOnlySequence<T>.SequenceType)(2 * (this._startInteger >> 31) + (this._endInteger >> 31)));
		}

		// Token: 0x06006346 RID: 25414 RVA: 0x0014C552 File Offset: 0x0014A752
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int GetIndex(in SequencePosition position)
		{
			return position.GetInteger() & int.MaxValue;
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x0014C560 File Offset: 0x0014A760
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int GetIndex(int Integer)
		{
			return Integer & int.MaxValue;
		}

		// Token: 0x06006348 RID: 25416 RVA: 0x0014C569 File Offset: 0x0014A769
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlySequence<T> SliceImpl(in SequencePosition start, in SequencePosition end)
		{
			return new ReadOnlySequence<T>(start.GetObject(), ReadOnlySequence<T>.GetIndex(start) | (this._startInteger & int.MinValue), end.GetObject(), ReadOnlySequence<T>.GetIndex(end) | (this._endInteger & int.MinValue));
		}

		// Token: 0x06006349 RID: 25417 RVA: 0x0014C5A2 File Offset: 0x0014A7A2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ReadOnlySequence<T> SliceImpl(in SequencePosition start)
		{
			return new ReadOnlySequence<T>(start.GetObject(), ReadOnlySequence<T>.GetIndex(start) | (this._startInteger & int.MinValue), this._endObject, this._endInteger);
		}

		// Token: 0x0600634A RID: 25418 RVA: 0x0014C5D0 File Offset: 0x0014A7D0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private long GetLength()
		{
			object startObject = this._startObject;
			object endObject = this._endObject;
			int index = ReadOnlySequence<T>.GetIndex(this._startInteger);
			int index2 = ReadOnlySequence<T>.GetIndex(this._endInteger);
			if (startObject != endObject)
			{
				ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)startObject;
				return ((ReadOnlySequenceSegment<T>)endObject).RunningIndex + (long)index2 - (readOnlySequenceSegment.RunningIndex + (long)index);
			}
			return (long)(index2 - index);
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x0014C630 File Offset: 0x0014A830
		internal bool TryGetReadOnlySequenceSegment(out ReadOnlySequenceSegment<T> startSegment, out int startIndex, out ReadOnlySequenceSegment<T> endSegment, out int endIndex)
		{
			object startObject = this._startObject;
			if (startObject == null || this.GetSequenceType() != ReadOnlySequence<T>.SequenceType.MultiSegment)
			{
				startSegment = null;
				startIndex = 0;
				endSegment = null;
				endIndex = 0;
				return false;
			}
			startSegment = (ReadOnlySequenceSegment<T>)startObject;
			startIndex = ReadOnlySequence<T>.GetIndex(this._startInteger);
			endSegment = (ReadOnlySequenceSegment<T>)this._endObject;
			endIndex = ReadOnlySequence<T>.GetIndex(this._endInteger);
			return true;
		}

		// Token: 0x0600634C RID: 25420 RVA: 0x0014C690 File Offset: 0x0014A890
		internal bool TryGetArray(out ArraySegment<T> segment)
		{
			if (this.GetSequenceType() != ReadOnlySequence<T>.SequenceType.Array)
			{
				segment = default(ArraySegment<T>);
				return false;
			}
			int index = ReadOnlySequence<T>.GetIndex(this._startInteger);
			segment = new ArraySegment<T>((T[])this._startObject, index, ReadOnlySequence<T>.GetIndex(this._endInteger) - index);
			return true;
		}

		// Token: 0x0600634D RID: 25421 RVA: 0x0014C6E0 File Offset: 0x0014A8E0
		internal bool TryGetString(out string text, out int start, out int length)
		{
			if (typeof(T) != typeof(char) || this.GetSequenceType() != ReadOnlySequence<T>.SequenceType.String)
			{
				start = 0;
				length = 0;
				text = null;
				return false;
			}
			start = ReadOnlySequence<T>.GetIndex(this._startInteger);
			length = ReadOnlySequence<T>.GetIndex(this._endInteger) - start;
			text = (string)this._startObject;
			return true;
		}

		// Token: 0x0600634E RID: 25422 RVA: 0x0008D099 File Offset: 0x0008B299
		private static bool InRange(uint value, uint start, uint end)
		{
			return value - start <= end - start;
		}

		// Token: 0x0600634F RID: 25423 RVA: 0x0008D099 File Offset: 0x0008B299
		private static bool InRange(ulong value, ulong start, ulong end)
		{
			return value - start <= end - start;
		}

		// Token: 0x06006350 RID: 25424 RVA: 0x0014C748 File Offset: 0x0014A948
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void GetFirstSpan(out ReadOnlySpan<T> first, out SequencePosition next)
		{
			first = default(ReadOnlySpan<T>);
			next = default(SequencePosition);
			object startObject = this._startObject;
			int startInteger = this._startInteger;
			if (startObject != null)
			{
				bool flag = startObject != this._endObject;
				int endInteger = this._endInteger;
				if (startInteger >= 0)
				{
					if (endInteger < 0)
					{
						if (flag)
						{
							ThrowHelper.ThrowInvalidOperationException_EndPositionNotReached();
						}
						first = new ReadOnlySpan<T>((T[])startObject, startInteger, (endInteger & int.MaxValue) - startInteger);
						return;
					}
					ReadOnlySequenceSegment<T> readOnlySequenceSegment = (ReadOnlySequenceSegment<T>)startObject;
					next = new SequencePosition(readOnlySequenceSegment.Next, 0);
					first = readOnlySequenceSegment.Memory.Span;
					if (flag)
					{
						first = first.Slice(startInteger);
						return;
					}
					first = first.Slice(startInteger, endInteger - startInteger);
					return;
				}
				else
				{
					first = ReadOnlySequence<T>.GetFirstSpanSlow(startObject, startInteger, endInteger, flag);
				}
			}
		}

		// Token: 0x06006351 RID: 25425 RVA: 0x0014C81C File Offset: 0x0014AA1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static ReadOnlySpan<T> GetFirstSpanSlow(object startObject, int startIndex, int endIndex, bool hasMultipleSegments)
		{
			if (hasMultipleSegments)
			{
				ThrowHelper.ThrowInvalidOperationException_EndPositionNotReached();
			}
			if (typeof(T) == typeof(char) && endIndex < 0)
			{
				ReadOnlySpan<char> span = ((string)startObject).AsSpan(startIndex & int.MaxValue, endIndex - startIndex);
				return MemoryMarshal.CreateReadOnlySpan<T>(Unsafe.As<char, T>(MemoryMarshal.GetReference<char>(span)), span.Length);
			}
			startIndex &= int.MaxValue;
			return ((MemoryManager<T>)startObject).Memory.Span.Slice(startIndex, endIndex - startIndex);
		}

		// Token: 0x04003A61 RID: 14945
		private readonly object _startObject;

		// Token: 0x04003A62 RID: 14946
		private readonly object _endObject;

		// Token: 0x04003A63 RID: 14947
		private readonly int _startInteger;

		// Token: 0x04003A64 RID: 14948
		private readonly int _endInteger;

		// Token: 0x04003A65 RID: 14949
		public static readonly ReadOnlySequence<T> Empty = new ReadOnlySequence<T>(Array.Empty<T>());

		// Token: 0x02000AE6 RID: 2790
		public struct Enumerator
		{
			// Token: 0x06006353 RID: 25427 RVA: 0x0014C8BC File Offset: 0x0014AABC
			public Enumerator(in ReadOnlySequence<T> sequence)
			{
				this._currentMemory = default(ReadOnlyMemory<T>);
				this._next = sequence.Start;
				this._sequence = sequence;
			}

			// Token: 0x1700118F RID: 4495
			// (get) Token: 0x06006354 RID: 25428 RVA: 0x0014C8E2 File Offset: 0x0014AAE2
			public ReadOnlyMemory<T> Current
			{
				get
				{
					return this._currentMemory;
				}
			}

			// Token: 0x06006355 RID: 25429 RVA: 0x0014C8EA File Offset: 0x0014AAEA
			public bool MoveNext()
			{
				return this._next.GetObject() != null && this._sequence.TryGet(ref this._next, out this._currentMemory, true);
			}

			// Token: 0x04003A66 RID: 14950
			private readonly ReadOnlySequence<T> _sequence;

			// Token: 0x04003A67 RID: 14951
			private SequencePosition _next;

			// Token: 0x04003A68 RID: 14952
			private ReadOnlyMemory<T> _currentMemory;
		}

		// Token: 0x02000AE7 RID: 2791
		private enum SequenceType
		{
			// Token: 0x04003A6A RID: 14954
			MultiSegment,
			// Token: 0x04003A6B RID: 14955
			Array,
			// Token: 0x04003A6C RID: 14956
			MemoryManager,
			// Token: 0x04003A6D RID: 14957
			String,
			// Token: 0x04003A6E RID: 14958
			Empty
		}
	}
}
