﻿using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Collections.Concurrent
{
	/// <summary>Provides common partitioning strategies for arrays, lists, and enumerables.</summary>
	// Token: 0x02000A68 RID: 2664
	public static class Partitioner
	{
		/// <summary>Creates an orderable partitioner from an <see cref="T:System.Collections.Generic.IList`1" /> instance.</summary>
		/// <param name="list">The list to be partitioned.</param>
		/// <param name="loadBalance">A Boolean value that indicates whether the created partitioner should dynamically load balance between partitions rather than statically partition.</param>
		/// <typeparam name="TSource">Type of the elements in source list.</typeparam>
		/// <returns>An orderable partitioner based on the input list.</returns>
		// Token: 0x06005F94 RID: 24468 RVA: 0x00141103 File Offset: 0x0013F303
		public static OrderablePartitioner<TSource> Create<TSource>(IList<TSource> list, bool loadBalance)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			if (loadBalance)
			{
				return new Partitioner.DynamicPartitionerForIList<TSource>(list);
			}
			return new Partitioner.StaticIndexRangePartitionerForIList<TSource>(list);
		}

		/// <summary>Creates an orderable partitioner from a <see cref="T:System.Array" /> instance.</summary>
		/// <param name="array">The array to be partitioned.</param>
		/// <param name="loadBalance">A Boolean value that indicates whether the created partitioner should dynamically load balance between partitions rather than statically partition.</param>
		/// <typeparam name="TSource">Type of the elements in source array.</typeparam>
		/// <returns>An orderable partitioner based on the input array.</returns>
		// Token: 0x06005F95 RID: 24469 RVA: 0x00141123 File Offset: 0x0013F323
		public static OrderablePartitioner<TSource> Create<TSource>(TSource[] array, bool loadBalance)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (loadBalance)
			{
				return new Partitioner.DynamicPartitionerForArray<TSource>(array);
			}
			return new Partitioner.StaticIndexRangePartitionerForArray<TSource>(array);
		}

		/// <summary>Creates an orderable partitioner from a <see cref="T:System.Collections.Generic.IEnumerable`1" /> instance.</summary>
		/// <param name="source">The enumerable to be partitioned.</param>
		/// <typeparam name="TSource">Type of the elements in source enumerable.</typeparam>
		/// <returns>An orderable partitioner based on the input array.</returns>
		// Token: 0x06005F96 RID: 24470 RVA: 0x00141143 File Offset: 0x0013F343
		public static OrderablePartitioner<TSource> Create<TSource>(IEnumerable<TSource> source)
		{
			return Partitioner.Create<TSource>(source, EnumerablePartitionerOptions.None);
		}

		/// <summary>Creates an orderable partitioner from a <see cref="T:System.Collections.Generic.IEnumerable`1" /> instance.</summary>
		/// <param name="source">The enumerable to be partitioned.</param>
		/// <param name="partitionerOptions">Options to control the buffering behavior of the partitioner.</param>
		/// <typeparam name="TSource">Type of the elements in source enumerable.</typeparam>
		/// <returns>An orderable partitioner based on the input array.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="partitionerOptions" /> argument specifies an invalid value for <see cref="T:System.Collections.Concurrent.EnumerablePartitionerOptions" />.</exception>
		// Token: 0x06005F97 RID: 24471 RVA: 0x0014114C File Offset: 0x0013F34C
		public static OrderablePartitioner<TSource> Create<TSource>(IEnumerable<TSource> source, EnumerablePartitionerOptions partitionerOptions)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if ((partitionerOptions & ~EnumerablePartitionerOptions.NoBuffering) != EnumerablePartitionerOptions.None)
			{
				throw new ArgumentOutOfRangeException("partitionerOptions");
			}
			return new Partitioner.DynamicPartitionerForIEnumerable<TSource>(source, partitionerOptions);
		}

		/// <summary>Creates a partitioner that chunks the user-specified range.</summary>
		/// <param name="fromInclusive">The lower, inclusive bound of the range.</param>
		/// <param name="toExclusive">The upper, exclusive bound of the range.</param>
		/// <returns>A partitioner.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="toExclusive" /> argument is less than or equal to the <paramref name="fromInclusive" /> argument.</exception>
		// Token: 0x06005F98 RID: 24472 RVA: 0x00141174 File Offset: 0x0013F374
		public static OrderablePartitioner<Tuple<long, long>> Create(long fromInclusive, long toExclusive)
		{
			int num = 3;
			if (toExclusive <= fromInclusive)
			{
				throw new ArgumentOutOfRangeException("toExclusive");
			}
			long num2 = (toExclusive - fromInclusive) / (long)(PlatformHelper.ProcessorCount * num);
			if (num2 == 0L)
			{
				num2 = 1L;
			}
			return Partitioner.Create<Tuple<long, long>>(Partitioner.CreateRanges(fromInclusive, toExclusive, num2), EnumerablePartitionerOptions.NoBuffering);
		}

		/// <summary>Creates a partitioner that chunks the user-specified range.</summary>
		/// <param name="fromInclusive">The lower, inclusive bound of the range.</param>
		/// <param name="toExclusive">The upper, exclusive bound of the range.</param>
		/// <param name="rangeSize">The size of each subrange.</param>
		/// <returns>A partitioner.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="toExclusive" /> argument is less than or equal to the <paramref name="fromInclusive" /> argument.  
		///  -or-  
		///  The <paramref name="rangeSize" /> argument is less than or equal to 0.</exception>
		// Token: 0x06005F99 RID: 24473 RVA: 0x001411B3 File Offset: 0x0013F3B3
		public static OrderablePartitioner<Tuple<long, long>> Create(long fromInclusive, long toExclusive, long rangeSize)
		{
			if (toExclusive <= fromInclusive)
			{
				throw new ArgumentOutOfRangeException("toExclusive");
			}
			if (rangeSize <= 0L)
			{
				throw new ArgumentOutOfRangeException("rangeSize");
			}
			return Partitioner.Create<Tuple<long, long>>(Partitioner.CreateRanges(fromInclusive, toExclusive, rangeSize), EnumerablePartitionerOptions.NoBuffering);
		}

		// Token: 0x06005F9A RID: 24474 RVA: 0x001411E2 File Offset: 0x0013F3E2
		private static IEnumerable<Tuple<long, long>> CreateRanges(long fromInclusive, long toExclusive, long rangeSize)
		{
			bool shouldQuit = false;
			long i = fromInclusive;
			while (i < toExclusive && !shouldQuit)
			{
				long item = i;
				long num;
				try
				{
					num = checked(i + rangeSize);
				}
				catch (OverflowException)
				{
					num = toExclusive;
					shouldQuit = true;
				}
				if (num > toExclusive)
				{
					num = toExclusive;
				}
				yield return new Tuple<long, long>(item, num);
				i += rangeSize;
			}
			yield break;
		}

		/// <summary>Creates a partitioner that chunks the user-specified range.</summary>
		/// <param name="fromInclusive">The lower, inclusive bound of the range.</param>
		/// <param name="toExclusive">The upper, exclusive bound of the range.</param>
		/// <returns>A partitioner.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="toExclusive" /> argument is less than or equal to the <paramref name="fromInclusive" /> argument.</exception>
		// Token: 0x06005F9B RID: 24475 RVA: 0x00141200 File Offset: 0x0013F400
		public static OrderablePartitioner<Tuple<int, int>> Create(int fromInclusive, int toExclusive)
		{
			int num = 3;
			if (toExclusive <= fromInclusive)
			{
				throw new ArgumentOutOfRangeException("toExclusive");
			}
			int num2 = (toExclusive - fromInclusive) / (PlatformHelper.ProcessorCount * num);
			if (num2 == 0)
			{
				num2 = 1;
			}
			return Partitioner.Create<Tuple<int, int>>(Partitioner.CreateRanges(fromInclusive, toExclusive, num2), EnumerablePartitionerOptions.NoBuffering);
		}

		/// <summary>Creates a partitioner that chunks the user-specified range.</summary>
		/// <param name="fromInclusive">The lower, inclusive bound of the range.</param>
		/// <param name="toExclusive">The upper, exclusive bound of the range.</param>
		/// <param name="rangeSize">The size of each subrange.</param>
		/// <returns>A partitioner.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="toExclusive" /> argument is less than or equal to the <paramref name="fromInclusive" /> argument.  
		///  -or-  
		///  The <paramref name="rangeSize" /> argument is less than or equal to 0.</exception>
		// Token: 0x06005F9C RID: 24476 RVA: 0x0014123D File Offset: 0x0013F43D
		public static OrderablePartitioner<Tuple<int, int>> Create(int fromInclusive, int toExclusive, int rangeSize)
		{
			if (toExclusive <= fromInclusive)
			{
				throw new ArgumentOutOfRangeException("toExclusive");
			}
			if (rangeSize <= 0)
			{
				throw new ArgumentOutOfRangeException("rangeSize");
			}
			return Partitioner.Create<Tuple<int, int>>(Partitioner.CreateRanges(fromInclusive, toExclusive, rangeSize), EnumerablePartitionerOptions.NoBuffering);
		}

		// Token: 0x06005F9D RID: 24477 RVA: 0x0014126B File Offset: 0x0013F46B
		private static IEnumerable<Tuple<int, int>> CreateRanges(int fromInclusive, int toExclusive, int rangeSize)
		{
			bool shouldQuit = false;
			int i = fromInclusive;
			while (i < toExclusive && !shouldQuit)
			{
				int item = i;
				int num;
				try
				{
					num = checked(i + rangeSize);
				}
				catch (OverflowException)
				{
					num = toExclusive;
					shouldQuit = true;
				}
				if (num > toExclusive)
				{
					num = toExclusive;
				}
				yield return new Tuple<int, int>(item, num);
				i += rangeSize;
			}
			yield break;
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x0014128C File Offset: 0x0013F48C
		private static int GetDefaultChunkSize<TSource>()
		{
			int result;
			if (default(TSource) != null || Nullable.GetUnderlyingType(typeof(TSource)) != null)
			{
				result = 128;
			}
			else
			{
				result = 512 / IntPtr.Size;
			}
			return result;
		}

		// Token: 0x04003967 RID: 14695
		private const int DEFAULT_BYTES_PER_UNIT = 128;

		// Token: 0x04003968 RID: 14696
		private const int DEFAULT_BYTES_PER_CHUNK = 512;

		// Token: 0x02000A69 RID: 2665
		private abstract class DynamicPartitionEnumerator_Abstract<TSource, TSourceReader> : IEnumerator<KeyValuePair<long, TSource>>, IDisposable, IEnumerator
		{
			// Token: 0x06005F9F RID: 24479 RVA: 0x001412D5 File Offset: 0x0013F4D5
			protected DynamicPartitionEnumerator_Abstract(TSourceReader sharedReader, Partitioner.SharedLong sharedIndex) : this(sharedReader, sharedIndex, false)
			{
			}

			// Token: 0x06005FA0 RID: 24480 RVA: 0x001412E0 File Offset: 0x0013F4E0
			protected DynamicPartitionEnumerator_Abstract(TSourceReader sharedReader, Partitioner.SharedLong sharedIndex, bool useSingleChunking)
			{
				this._sharedReader = sharedReader;
				this._sharedIndex = sharedIndex;
				this._maxChunkSize = (useSingleChunking ? 1 : Partitioner.DynamicPartitionEnumerator_Abstract<TSource, TSourceReader>.s_defaultMaxChunkSize);
			}

			// Token: 0x06005FA1 RID: 24481
			protected abstract bool GrabNextChunk(int requestedChunkSize);

			// Token: 0x170010CC RID: 4300
			// (get) Token: 0x06005FA2 RID: 24482
			// (set) Token: 0x06005FA3 RID: 24483
			protected abstract bool HasNoElementsLeft { get; set; }

			// Token: 0x170010CD RID: 4301
			// (get) Token: 0x06005FA4 RID: 24484
			public abstract KeyValuePair<long, TSource> Current { get; }

			// Token: 0x06005FA5 RID: 24485
			public abstract void Dispose();

			// Token: 0x06005FA6 RID: 24486 RVA: 0x000472CC File Offset: 0x000454CC
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x170010CE RID: 4302
			// (get) Token: 0x06005FA7 RID: 24487 RVA: 0x00141307 File Offset: 0x0013F507
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06005FA8 RID: 24488 RVA: 0x00141314 File Offset: 0x0013F514
			public bool MoveNext()
			{
				if (this._localOffset == null)
				{
					this._localOffset = new Partitioner.SharedInt(-1);
					this._currentChunkSize = new Partitioner.SharedInt(0);
					this._doublingCountdown = 3;
				}
				if (this._localOffset.Value < this._currentChunkSize.Value - 1)
				{
					this._localOffset.Value++;
					return true;
				}
				int requestedChunkSize;
				if (this._currentChunkSize.Value == 0)
				{
					requestedChunkSize = 1;
				}
				else if (this._doublingCountdown > 0)
				{
					requestedChunkSize = this._currentChunkSize.Value;
				}
				else
				{
					requestedChunkSize = Math.Min(this._currentChunkSize.Value * 2, this._maxChunkSize);
					this._doublingCountdown = 3;
				}
				this._doublingCountdown--;
				if (this.GrabNextChunk(requestedChunkSize))
				{
					this._localOffset.Value = 0;
					return true;
				}
				return false;
			}

			// Token: 0x04003969 RID: 14697
			protected readonly TSourceReader _sharedReader;

			// Token: 0x0400396A RID: 14698
			protected static int s_defaultMaxChunkSize = Partitioner.GetDefaultChunkSize<TSource>();

			// Token: 0x0400396B RID: 14699
			protected Partitioner.SharedInt _currentChunkSize;

			// Token: 0x0400396C RID: 14700
			protected Partitioner.SharedInt _localOffset;

			// Token: 0x0400396D RID: 14701
			private const int CHUNK_DOUBLING_RATE = 3;

			// Token: 0x0400396E RID: 14702
			private int _doublingCountdown;

			// Token: 0x0400396F RID: 14703
			protected readonly int _maxChunkSize;

			// Token: 0x04003970 RID: 14704
			protected readonly Partitioner.SharedLong _sharedIndex;
		}

		// Token: 0x02000A6A RID: 2666
		private class DynamicPartitionerForIEnumerable<TSource> : OrderablePartitioner<TSource>
		{
			// Token: 0x06005FAA RID: 24490 RVA: 0x00141401 File Offset: 0x0013F601
			internal DynamicPartitionerForIEnumerable(IEnumerable<TSource> source, EnumerablePartitionerOptions partitionerOptions) : base(true, false, true)
			{
				this._source = source;
				this._useSingleChunking = ((partitionerOptions & EnumerablePartitionerOptions.NoBuffering) > EnumerablePartitionerOptions.None);
			}

			// Token: 0x06005FAB RID: 24491 RVA: 0x00141420 File Offset: 0x0013F620
			public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
			{
				if (partitionCount <= 0)
				{
					throw new ArgumentOutOfRangeException("partitionCount");
				}
				IEnumerator<KeyValuePair<long, TSource>>[] array = new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
				IEnumerable<KeyValuePair<long, TSource>> enumerable = new Partitioner.DynamicPartitionerForIEnumerable<TSource>.InternalPartitionEnumerable(this._source.GetEnumerator(), this._useSingleChunking, true);
				for (int i = 0; i < partitionCount; i++)
				{
					array[i] = enumerable.GetEnumerator();
				}
				return array;
			}

			// Token: 0x06005FAC RID: 24492 RVA: 0x00141471 File Offset: 0x0013F671
			public override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
			{
				return new Partitioner.DynamicPartitionerForIEnumerable<TSource>.InternalPartitionEnumerable(this._source.GetEnumerator(), this._useSingleChunking, false);
			}

			// Token: 0x170010CF RID: 4303
			// (get) Token: 0x06005FAD RID: 24493 RVA: 0x000040F7 File Offset: 0x000022F7
			public override bool SupportsDynamicPartitions
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04003971 RID: 14705
			private IEnumerable<TSource> _source;

			// Token: 0x04003972 RID: 14706
			private readonly bool _useSingleChunking;

			// Token: 0x02000A6B RID: 2667
			private class InternalPartitionEnumerable : IEnumerable<KeyValuePair<long, TSource>>, IEnumerable, IDisposable
			{
				// Token: 0x06005FAE RID: 24494 RVA: 0x0014148C File Offset: 0x0013F68C
				internal InternalPartitionEnumerable(IEnumerator<TSource> sharedReader, bool useSingleChunking, bool isStaticPartitioning)
				{
					this._sharedReader = sharedReader;
					this._sharedIndex = new Partitioner.SharedLong(-1L);
					this._hasNoElementsLeft = new Partitioner.SharedBool(false);
					this._sourceDepleted = new Partitioner.SharedBool(false);
					this._sharedLock = new object();
					this._useSingleChunking = useSingleChunking;
					if (!this._useSingleChunking)
					{
						int num = (PlatformHelper.ProcessorCount > 4) ? 4 : 1;
						this._fillBuffer = new KeyValuePair<long, TSource>[num * Partitioner.GetDefaultChunkSize<TSource>()];
					}
					if (isStaticPartitioning)
					{
						this._activePartitionCount = new Partitioner.SharedInt(0);
						return;
					}
					this._activePartitionCount = null;
				}

				// Token: 0x06005FAF RID: 24495 RVA: 0x0014151D File Offset: 0x0013F71D
				public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
				{
					if (this._disposed)
					{
						throw new ObjectDisposedException("Can not call GetEnumerator on partitions after the source enumerable is disposed");
					}
					return new Partitioner.DynamicPartitionerForIEnumerable<TSource>.InternalPartitionEnumerator(this._sharedReader, this._sharedIndex, this._hasNoElementsLeft, this._activePartitionCount, this, this._useSingleChunking);
				}

				// Token: 0x06005FB0 RID: 24496 RVA: 0x00141556 File Offset: 0x0013F756
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				// Token: 0x06005FB1 RID: 24497 RVA: 0x00141560 File Offset: 0x0013F760
				private void TryCopyFromFillBuffer(KeyValuePair<long, TSource>[] destArray, int requestedChunkSize, ref int actualNumElementsGrabbed)
				{
					actualNumElementsGrabbed = 0;
					KeyValuePair<long, TSource>[] fillBuffer = this._fillBuffer;
					if (fillBuffer == null)
					{
						return;
					}
					if (this._fillBufferCurrentPosition >= this._fillBufferSize)
					{
						return;
					}
					Interlocked.Increment(ref this._activeCopiers);
					int num = Interlocked.Add(ref this._fillBufferCurrentPosition, requestedChunkSize);
					int num2 = num - requestedChunkSize;
					if (num2 < this._fillBufferSize)
					{
						actualNumElementsGrabbed = ((num < this._fillBufferSize) ? num : (this._fillBufferSize - num2));
						Array.Copy(fillBuffer, num2, destArray, 0, actualNumElementsGrabbed);
					}
					Interlocked.Decrement(ref this._activeCopiers);
				}

				// Token: 0x06005FB2 RID: 24498 RVA: 0x001415E9 File Offset: 0x0013F7E9
				internal bool GrabChunk(KeyValuePair<long, TSource>[] destArray, int requestedChunkSize, ref int actualNumElementsGrabbed)
				{
					actualNumElementsGrabbed = 0;
					if (this._hasNoElementsLeft.Value)
					{
						return false;
					}
					if (this._useSingleChunking)
					{
						return this.GrabChunk_Single(destArray, requestedChunkSize, ref actualNumElementsGrabbed);
					}
					return this.GrabChunk_Buffered(destArray, requestedChunkSize, ref actualNumElementsGrabbed);
				}

				// Token: 0x06005FB3 RID: 24499 RVA: 0x0014161C File Offset: 0x0013F81C
				internal bool GrabChunk_Single(KeyValuePair<long, TSource>[] destArray, int requestedChunkSize, ref int actualNumElementsGrabbed)
				{
					object sharedLock = this._sharedLock;
					bool result;
					lock (sharedLock)
					{
						if (this._hasNoElementsLeft.Value)
						{
							result = false;
						}
						else
						{
							try
							{
								if (this._sharedReader.MoveNext())
								{
									this._sharedIndex.Value = checked(this._sharedIndex.Value + 1L);
									destArray[0] = new KeyValuePair<long, TSource>(this._sharedIndex.Value, this._sharedReader.Current);
									actualNumElementsGrabbed = 1;
									result = true;
								}
								else
								{
									this._sourceDepleted.Value = true;
									this._hasNoElementsLeft.Value = true;
									result = false;
								}
							}
							catch
							{
								this._sourceDepleted.Value = true;
								this._hasNoElementsLeft.Value = true;
								throw;
							}
						}
					}
					return result;
				}

				// Token: 0x06005FB4 RID: 24500 RVA: 0x00141708 File Offset: 0x0013F908
				internal bool GrabChunk_Buffered(KeyValuePair<long, TSource>[] destArray, int requestedChunkSize, ref int actualNumElementsGrabbed)
				{
					this.TryCopyFromFillBuffer(destArray, requestedChunkSize, ref actualNumElementsGrabbed);
					if (actualNumElementsGrabbed == requestedChunkSize)
					{
						return true;
					}
					if (this._sourceDepleted.Value)
					{
						this._hasNoElementsLeft.Value = true;
						this._fillBuffer = null;
						return actualNumElementsGrabbed > 0;
					}
					object sharedLock = this._sharedLock;
					lock (sharedLock)
					{
						if (this._sourceDepleted.Value)
						{
							return actualNumElementsGrabbed > 0;
						}
						try
						{
							if (this._activeCopiers > 0)
							{
								SpinWait spinWait = default(SpinWait);
								while (this._activeCopiers > 0)
								{
									spinWait.SpinOnce();
								}
							}
							while (actualNumElementsGrabbed < requestedChunkSize)
							{
								if (!this._sharedReader.MoveNext())
								{
									this._sourceDepleted.Value = true;
									break;
								}
								this._sharedIndex.Value = checked(this._sharedIndex.Value + 1L);
								destArray[actualNumElementsGrabbed] = new KeyValuePair<long, TSource>(this._sharedIndex.Value, this._sharedReader.Current);
								actualNumElementsGrabbed++;
							}
							KeyValuePair<long, TSource>[] fillBuffer = this._fillBuffer;
							if (!this._sourceDepleted.Value && fillBuffer != null && this._fillBufferCurrentPosition >= fillBuffer.Length)
							{
								for (int i = 0; i < fillBuffer.Length; i++)
								{
									if (!this._sharedReader.MoveNext())
									{
										this._sourceDepleted.Value = true;
										this._fillBufferSize = i;
										break;
									}
									this._sharedIndex.Value = checked(this._sharedIndex.Value + 1L);
									fillBuffer[i] = new KeyValuePair<long, TSource>(this._sharedIndex.Value, this._sharedReader.Current);
								}
								this._fillBufferCurrentPosition = 0;
							}
						}
						catch
						{
							this._sourceDepleted.Value = true;
							this._hasNoElementsLeft.Value = true;
							throw;
						}
					}
					return actualNumElementsGrabbed > 0;
				}

				// Token: 0x06005FB5 RID: 24501 RVA: 0x00141924 File Offset: 0x0013FB24
				public void Dispose()
				{
					if (!this._disposed)
					{
						this._disposed = true;
						this._sharedReader.Dispose();
					}
				}

				// Token: 0x04003973 RID: 14707
				private readonly IEnumerator<TSource> _sharedReader;

				// Token: 0x04003974 RID: 14708
				private Partitioner.SharedLong _sharedIndex;

				// Token: 0x04003975 RID: 14709
				private volatile KeyValuePair<long, TSource>[] _fillBuffer;

				// Token: 0x04003976 RID: 14710
				private volatile int _fillBufferSize;

				// Token: 0x04003977 RID: 14711
				private volatile int _fillBufferCurrentPosition;

				// Token: 0x04003978 RID: 14712
				private volatile int _activeCopiers;

				// Token: 0x04003979 RID: 14713
				private Partitioner.SharedBool _hasNoElementsLeft;

				// Token: 0x0400397A RID: 14714
				private Partitioner.SharedBool _sourceDepleted;

				// Token: 0x0400397B RID: 14715
				private object _sharedLock;

				// Token: 0x0400397C RID: 14716
				private bool _disposed;

				// Token: 0x0400397D RID: 14717
				private Partitioner.SharedInt _activePartitionCount;

				// Token: 0x0400397E RID: 14718
				private readonly bool _useSingleChunking;
			}

			// Token: 0x02000A6C RID: 2668
			private class InternalPartitionEnumerator : Partitioner.DynamicPartitionEnumerator_Abstract<TSource, IEnumerator<TSource>>
			{
				// Token: 0x06005FB6 RID: 24502 RVA: 0x00141940 File Offset: 0x0013FB40
				internal InternalPartitionEnumerator(IEnumerator<TSource> sharedReader, Partitioner.SharedLong sharedIndex, Partitioner.SharedBool hasNoElementsLeft, Partitioner.SharedInt activePartitionCount, Partitioner.DynamicPartitionerForIEnumerable<TSource>.InternalPartitionEnumerable enumerable, bool useSingleChunking) : base(sharedReader, sharedIndex, useSingleChunking)
				{
					this._hasNoElementsLeft = hasNoElementsLeft;
					this._enumerable = enumerable;
					this._activePartitionCount = activePartitionCount;
					if (this._activePartitionCount != null)
					{
						Interlocked.Increment(ref this._activePartitionCount.Value);
					}
				}

				// Token: 0x06005FB7 RID: 24503 RVA: 0x0014197C File Offset: 0x0013FB7C
				protected override bool GrabNextChunk(int requestedChunkSize)
				{
					if (this.HasNoElementsLeft)
					{
						return false;
					}
					if (this._localList == null)
					{
						this._localList = new KeyValuePair<long, TSource>[this._maxChunkSize];
					}
					return this._enumerable.GrabChunk(this._localList, requestedChunkSize, ref this._currentChunkSize.Value);
				}

				// Token: 0x170010D0 RID: 4304
				// (get) Token: 0x06005FB8 RID: 24504 RVA: 0x001419C9 File Offset: 0x0013FBC9
				// (set) Token: 0x06005FB9 RID: 24505 RVA: 0x001419D8 File Offset: 0x0013FBD8
				protected override bool HasNoElementsLeft
				{
					get
					{
						return this._hasNoElementsLeft.Value;
					}
					set
					{
						this._hasNoElementsLeft.Value = true;
					}
				}

				// Token: 0x170010D1 RID: 4305
				// (get) Token: 0x06005FBA RID: 24506 RVA: 0x001419E8 File Offset: 0x0013FBE8
				public override KeyValuePair<long, TSource> Current
				{
					get
					{
						if (this._currentChunkSize == null)
						{
							throw new InvalidOperationException("MoveNext must be called at least once before calling Current.");
						}
						return this._localList[this._localOffset.Value];
					}
				}

				// Token: 0x06005FBB RID: 24507 RVA: 0x00141A15 File Offset: 0x0013FC15
				public override void Dispose()
				{
					if (this._activePartitionCount != null && Interlocked.Decrement(ref this._activePartitionCount.Value) == 0)
					{
						this._enumerable.Dispose();
					}
				}

				// Token: 0x0400397F RID: 14719
				private KeyValuePair<long, TSource>[] _localList;

				// Token: 0x04003980 RID: 14720
				private readonly Partitioner.SharedBool _hasNoElementsLeft;

				// Token: 0x04003981 RID: 14721
				private readonly Partitioner.SharedInt _activePartitionCount;

				// Token: 0x04003982 RID: 14722
				private Partitioner.DynamicPartitionerForIEnumerable<TSource>.InternalPartitionEnumerable _enumerable;
			}
		}

		// Token: 0x02000A6D RID: 2669
		private abstract class DynamicPartitionerForIndexRange_Abstract<TSource, TCollection> : OrderablePartitioner<TSource>
		{
			// Token: 0x06005FBC RID: 24508 RVA: 0x00141A3C File Offset: 0x0013FC3C
			protected DynamicPartitionerForIndexRange_Abstract(TCollection data) : base(true, false, true)
			{
				this._data = data;
			}

			// Token: 0x06005FBD RID: 24509
			protected abstract IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions_Factory(TCollection data);

			// Token: 0x06005FBE RID: 24510 RVA: 0x00141A50 File Offset: 0x0013FC50
			public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
			{
				if (partitionCount <= 0)
				{
					throw new ArgumentOutOfRangeException("partitionCount");
				}
				IEnumerator<KeyValuePair<long, TSource>>[] array = new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
				IEnumerable<KeyValuePair<long, TSource>> orderableDynamicPartitions_Factory = this.GetOrderableDynamicPartitions_Factory(this._data);
				for (int i = 0; i < partitionCount; i++)
				{
					array[i] = orderableDynamicPartitions_Factory.GetEnumerator();
				}
				return array;
			}

			// Token: 0x06005FBF RID: 24511 RVA: 0x00141A96 File Offset: 0x0013FC96
			public override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions()
			{
				return this.GetOrderableDynamicPartitions_Factory(this._data);
			}

			// Token: 0x170010D2 RID: 4306
			// (get) Token: 0x06005FC0 RID: 24512 RVA: 0x000040F7 File Offset: 0x000022F7
			public override bool SupportsDynamicPartitions
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04003983 RID: 14723
			private TCollection _data;
		}

		// Token: 0x02000A6E RID: 2670
		private abstract class DynamicPartitionEnumeratorForIndexRange_Abstract<TSource, TSourceReader> : Partitioner.DynamicPartitionEnumerator_Abstract<TSource, TSourceReader>
		{
			// Token: 0x06005FC1 RID: 24513 RVA: 0x00141AA4 File Offset: 0x0013FCA4
			protected DynamicPartitionEnumeratorForIndexRange_Abstract(TSourceReader sharedReader, Partitioner.SharedLong sharedIndex) : base(sharedReader, sharedIndex)
			{
			}

			// Token: 0x170010D3 RID: 4307
			// (get) Token: 0x06005FC2 RID: 24514
			protected abstract int SourceCount { get; }

			// Token: 0x06005FC3 RID: 24515 RVA: 0x00141AB0 File Offset: 0x0013FCB0
			protected override bool GrabNextChunk(int requestedChunkSize)
			{
				while (!this.HasNoElementsLeft)
				{
					long num = Volatile.Read(ref this._sharedIndex.Value);
					if (this.HasNoElementsLeft)
					{
						return false;
					}
					long num2 = Math.Min((long)(this.SourceCount - 1), num + (long)requestedChunkSize);
					if (Interlocked.CompareExchange(ref this._sharedIndex.Value, num2, num) == num)
					{
						this._currentChunkSize.Value = (int)(num2 - num);
						this._localOffset.Value = -1;
						this._startIndex = (int)(num + 1L);
						return true;
					}
				}
				return false;
			}

			// Token: 0x170010D4 RID: 4308
			// (get) Token: 0x06005FC4 RID: 24516 RVA: 0x00141B37 File Offset: 0x0013FD37
			// (set) Token: 0x06005FC5 RID: 24517 RVA: 0x00004BF9 File Offset: 0x00002DF9
			protected override bool HasNoElementsLeft
			{
				get
				{
					return Volatile.Read(ref this._sharedIndex.Value) >= (long)(this.SourceCount - 1);
				}
				set
				{
				}
			}

			// Token: 0x06005FC6 RID: 24518 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public override void Dispose()
			{
			}

			// Token: 0x04003984 RID: 14724
			protected int _startIndex;
		}

		// Token: 0x02000A6F RID: 2671
		private class DynamicPartitionerForIList<TSource> : Partitioner.DynamicPartitionerForIndexRange_Abstract<TSource, IList<TSource>>
		{
			// Token: 0x06005FC7 RID: 24519 RVA: 0x00141B57 File Offset: 0x0013FD57
			internal DynamicPartitionerForIList(IList<TSource> source) : base(source)
			{
			}

			// Token: 0x06005FC8 RID: 24520 RVA: 0x00141B60 File Offset: 0x0013FD60
			protected override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions_Factory(IList<TSource> _data)
			{
				return new Partitioner.DynamicPartitionerForIList<TSource>.InternalPartitionEnumerable(_data);
			}

			// Token: 0x02000A70 RID: 2672
			private class InternalPartitionEnumerable : IEnumerable<KeyValuePair<long, TSource>>, IEnumerable
			{
				// Token: 0x06005FC9 RID: 24521 RVA: 0x00141B68 File Offset: 0x0013FD68
				internal InternalPartitionEnumerable(IList<TSource> sharedReader)
				{
					this._sharedReader = sharedReader;
					this._sharedIndex = new Partitioner.SharedLong(-1L);
				}

				// Token: 0x06005FCA RID: 24522 RVA: 0x00141B84 File Offset: 0x0013FD84
				public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
				{
					return new Partitioner.DynamicPartitionerForIList<TSource>.InternalPartitionEnumerator(this._sharedReader, this._sharedIndex);
				}

				// Token: 0x06005FCB RID: 24523 RVA: 0x00141B97 File Offset: 0x0013FD97
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				// Token: 0x04003985 RID: 14725
				private readonly IList<TSource> _sharedReader;

				// Token: 0x04003986 RID: 14726
				private Partitioner.SharedLong _sharedIndex;
			}

			// Token: 0x02000A71 RID: 2673
			private class InternalPartitionEnumerator : Partitioner.DynamicPartitionEnumeratorForIndexRange_Abstract<TSource, IList<TSource>>
			{
				// Token: 0x06005FCC RID: 24524 RVA: 0x00141B9F File Offset: 0x0013FD9F
				internal InternalPartitionEnumerator(IList<TSource> sharedReader, Partitioner.SharedLong sharedIndex) : base(sharedReader, sharedIndex)
				{
				}

				// Token: 0x170010D5 RID: 4309
				// (get) Token: 0x06005FCD RID: 24525 RVA: 0x00141BA9 File Offset: 0x0013FDA9
				protected override int SourceCount
				{
					get
					{
						return this._sharedReader.Count;
					}
				}

				// Token: 0x170010D6 RID: 4310
				// (get) Token: 0x06005FCE RID: 24526 RVA: 0x00141BB8 File Offset: 0x0013FDB8
				public override KeyValuePair<long, TSource> Current
				{
					get
					{
						if (this._currentChunkSize == null)
						{
							throw new InvalidOperationException("MoveNext must be called at least once before calling Current.");
						}
						return new KeyValuePair<long, TSource>((long)(this._startIndex + this._localOffset.Value), this._sharedReader[this._startIndex + this._localOffset.Value]);
					}
				}
			}
		}

		// Token: 0x02000A72 RID: 2674
		private class DynamicPartitionerForArray<TSource> : Partitioner.DynamicPartitionerForIndexRange_Abstract<TSource, TSource[]>
		{
			// Token: 0x06005FCF RID: 24527 RVA: 0x00141C11 File Offset: 0x0013FE11
			internal DynamicPartitionerForArray(TSource[] source) : base(source)
			{
			}

			// Token: 0x06005FD0 RID: 24528 RVA: 0x00141C1A File Offset: 0x0013FE1A
			protected override IEnumerable<KeyValuePair<long, TSource>> GetOrderableDynamicPartitions_Factory(TSource[] _data)
			{
				return new Partitioner.DynamicPartitionerForArray<TSource>.InternalPartitionEnumerable(_data);
			}

			// Token: 0x02000A73 RID: 2675
			private class InternalPartitionEnumerable : IEnumerable<KeyValuePair<long, TSource>>, IEnumerable
			{
				// Token: 0x06005FD1 RID: 24529 RVA: 0x00141C22 File Offset: 0x0013FE22
				internal InternalPartitionEnumerable(TSource[] sharedReader)
				{
					this._sharedReader = sharedReader;
					this._sharedIndex = new Partitioner.SharedLong(-1L);
				}

				// Token: 0x06005FD2 RID: 24530 RVA: 0x00141C3E File Offset: 0x0013FE3E
				IEnumerator IEnumerable.GetEnumerator()
				{
					return this.GetEnumerator();
				}

				// Token: 0x06005FD3 RID: 24531 RVA: 0x00141C46 File Offset: 0x0013FE46
				public IEnumerator<KeyValuePair<long, TSource>> GetEnumerator()
				{
					return new Partitioner.DynamicPartitionerForArray<TSource>.InternalPartitionEnumerator(this._sharedReader, this._sharedIndex);
				}

				// Token: 0x04003987 RID: 14727
				private readonly TSource[] _sharedReader;

				// Token: 0x04003988 RID: 14728
				private Partitioner.SharedLong _sharedIndex;
			}

			// Token: 0x02000A74 RID: 2676
			private class InternalPartitionEnumerator : Partitioner.DynamicPartitionEnumeratorForIndexRange_Abstract<TSource, TSource[]>
			{
				// Token: 0x06005FD4 RID: 24532 RVA: 0x00141C59 File Offset: 0x0013FE59
				internal InternalPartitionEnumerator(TSource[] sharedReader, Partitioner.SharedLong sharedIndex) : base(sharedReader, sharedIndex)
				{
				}

				// Token: 0x170010D7 RID: 4311
				// (get) Token: 0x06005FD5 RID: 24533 RVA: 0x00141C63 File Offset: 0x0013FE63
				protected override int SourceCount
				{
					get
					{
						return this._sharedReader.Length;
					}
				}

				// Token: 0x170010D8 RID: 4312
				// (get) Token: 0x06005FD6 RID: 24534 RVA: 0x00141C70 File Offset: 0x0013FE70
				public override KeyValuePair<long, TSource> Current
				{
					get
					{
						if (this._currentChunkSize == null)
						{
							throw new InvalidOperationException("MoveNext must be called at least once before calling Current.");
						}
						return new KeyValuePair<long, TSource>((long)(this._startIndex + this._localOffset.Value), this._sharedReader[this._startIndex + this._localOffset.Value]);
					}
				}
			}
		}

		// Token: 0x02000A75 RID: 2677
		private abstract class StaticIndexRangePartitioner<TSource, TCollection> : OrderablePartitioner<TSource>
		{
			// Token: 0x06005FD7 RID: 24535 RVA: 0x00141CC9 File Offset: 0x0013FEC9
			protected StaticIndexRangePartitioner() : base(true, true, true)
			{
			}

			// Token: 0x170010D9 RID: 4313
			// (get) Token: 0x06005FD8 RID: 24536
			protected abstract int SourceCount { get; }

			// Token: 0x06005FD9 RID: 24537
			protected abstract IEnumerator<KeyValuePair<long, TSource>> CreatePartition(int startIndex, int endIndex);

			// Token: 0x06005FDA RID: 24538 RVA: 0x00141CD4 File Offset: 0x0013FED4
			public override IList<IEnumerator<KeyValuePair<long, TSource>>> GetOrderablePartitions(int partitionCount)
			{
				if (partitionCount <= 0)
				{
					throw new ArgumentOutOfRangeException("partitionCount");
				}
				int num = this.SourceCount / partitionCount;
				int num2 = this.SourceCount % partitionCount;
				IEnumerator<KeyValuePair<long, TSource>>[] array = new IEnumerator<KeyValuePair<long, TSource>>[partitionCount];
				int num3 = -1;
				for (int i = 0; i < partitionCount; i++)
				{
					int num4 = num3 + 1;
					if (i < num2)
					{
						num3 = num4 + num;
					}
					else
					{
						num3 = num4 + num - 1;
					}
					array[i] = this.CreatePartition(num4, num3);
				}
				return array;
			}
		}

		// Token: 0x02000A76 RID: 2678
		private abstract class StaticIndexRangePartition<TSource> : IEnumerator<KeyValuePair<long, TSource>>, IDisposable, IEnumerator
		{
			// Token: 0x06005FDB RID: 24539 RVA: 0x00141D41 File Offset: 0x0013FF41
			protected StaticIndexRangePartition(int startIndex, int endIndex)
			{
				this._startIndex = startIndex;
				this._endIndex = endIndex;
				this._offset = startIndex - 1;
			}

			// Token: 0x170010DA RID: 4314
			// (get) Token: 0x06005FDC RID: 24540
			public abstract KeyValuePair<long, TSource> Current { get; }

			// Token: 0x06005FDD RID: 24541 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x06005FDE RID: 24542 RVA: 0x000472CC File Offset: 0x000454CC
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06005FDF RID: 24543 RVA: 0x00141D62 File Offset: 0x0013FF62
			public bool MoveNext()
			{
				if (this._offset < this._endIndex)
				{
					this._offset++;
					return true;
				}
				this._offset = this._endIndex + 1;
				return false;
			}

			// Token: 0x170010DB RID: 4315
			// (get) Token: 0x06005FE0 RID: 24544 RVA: 0x00141D99 File Offset: 0x0013FF99
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04003989 RID: 14729
			protected readonly int _startIndex;

			// Token: 0x0400398A RID: 14730
			protected readonly int _endIndex;

			// Token: 0x0400398B RID: 14731
			protected volatile int _offset;
		}

		// Token: 0x02000A77 RID: 2679
		private class StaticIndexRangePartitionerForIList<TSource> : Partitioner.StaticIndexRangePartitioner<TSource, IList<TSource>>
		{
			// Token: 0x06005FE1 RID: 24545 RVA: 0x00141DA6 File Offset: 0x0013FFA6
			internal StaticIndexRangePartitionerForIList(IList<TSource> list)
			{
				this._list = list;
			}

			// Token: 0x170010DC RID: 4316
			// (get) Token: 0x06005FE2 RID: 24546 RVA: 0x00141DB5 File Offset: 0x0013FFB5
			protected override int SourceCount
			{
				get
				{
					return this._list.Count;
				}
			}

			// Token: 0x06005FE3 RID: 24547 RVA: 0x00141DC2 File Offset: 0x0013FFC2
			protected override IEnumerator<KeyValuePair<long, TSource>> CreatePartition(int startIndex, int endIndex)
			{
				return new Partitioner.StaticIndexRangePartitionForIList<TSource>(this._list, startIndex, endIndex);
			}

			// Token: 0x0400398C RID: 14732
			private IList<TSource> _list;
		}

		// Token: 0x02000A78 RID: 2680
		private class StaticIndexRangePartitionForIList<TSource> : Partitioner.StaticIndexRangePartition<TSource>
		{
			// Token: 0x06005FE4 RID: 24548 RVA: 0x00141DD1 File Offset: 0x0013FFD1
			internal StaticIndexRangePartitionForIList(IList<TSource> list, int startIndex, int endIndex) : base(startIndex, endIndex)
			{
				this._list = list;
			}

			// Token: 0x170010DD RID: 4317
			// (get) Token: 0x06005FE5 RID: 24549 RVA: 0x00141DE4 File Offset: 0x0013FFE4
			public override KeyValuePair<long, TSource> Current
			{
				get
				{
					if (this._offset < this._startIndex)
					{
						throw new InvalidOperationException("MoveNext must be called at least once before calling Current.");
					}
					return new KeyValuePair<long, TSource>((long)this._offset, this._list[this._offset]);
				}
			}

			// Token: 0x0400398D RID: 14733
			private volatile IList<TSource> _list;
		}

		// Token: 0x02000A79 RID: 2681
		private class StaticIndexRangePartitionerForArray<TSource> : Partitioner.StaticIndexRangePartitioner<TSource, TSource[]>
		{
			// Token: 0x06005FE6 RID: 24550 RVA: 0x00141E24 File Offset: 0x00140024
			internal StaticIndexRangePartitionerForArray(TSource[] array)
			{
				this._array = array;
			}

			// Token: 0x170010DE RID: 4318
			// (get) Token: 0x06005FE7 RID: 24551 RVA: 0x00141E33 File Offset: 0x00140033
			protected override int SourceCount
			{
				get
				{
					return this._array.Length;
				}
			}

			// Token: 0x06005FE8 RID: 24552 RVA: 0x00141E3D File Offset: 0x0014003D
			protected override IEnumerator<KeyValuePair<long, TSource>> CreatePartition(int startIndex, int endIndex)
			{
				return new Partitioner.StaticIndexRangePartitionForArray<TSource>(this._array, startIndex, endIndex);
			}

			// Token: 0x0400398E RID: 14734
			private TSource[] _array;
		}

		// Token: 0x02000A7A RID: 2682
		private class StaticIndexRangePartitionForArray<TSource> : Partitioner.StaticIndexRangePartition<TSource>
		{
			// Token: 0x06005FE9 RID: 24553 RVA: 0x00141E4C File Offset: 0x0014004C
			internal StaticIndexRangePartitionForArray(TSource[] array, int startIndex, int endIndex) : base(startIndex, endIndex)
			{
				this._array = array;
			}

			// Token: 0x170010DF RID: 4319
			// (get) Token: 0x06005FEA RID: 24554 RVA: 0x00141E5F File Offset: 0x0014005F
			public override KeyValuePair<long, TSource> Current
			{
				get
				{
					if (this._offset < this._startIndex)
					{
						throw new InvalidOperationException("MoveNext must be called at least once before calling Current.");
					}
					return new KeyValuePair<long, TSource>((long)this._offset, this._array[this._offset]);
				}
			}

			// Token: 0x0400398F RID: 14735
			private volatile TSource[] _array;
		}

		// Token: 0x02000A7B RID: 2683
		private class SharedInt
		{
			// Token: 0x06005FEB RID: 24555 RVA: 0x00141E9F File Offset: 0x0014009F
			internal SharedInt(int value)
			{
				this.Value = value;
			}

			// Token: 0x04003990 RID: 14736
			internal volatile int Value;
		}

		// Token: 0x02000A7C RID: 2684
		private class SharedBool
		{
			// Token: 0x06005FEC RID: 24556 RVA: 0x00141EB0 File Offset: 0x001400B0
			internal SharedBool(bool value)
			{
				this.Value = value;
			}

			// Token: 0x04003991 RID: 14737
			internal volatile bool Value;
		}

		// Token: 0x02000A7D RID: 2685
		private class SharedLong
		{
			// Token: 0x06005FED RID: 24557 RVA: 0x00141EC1 File Offset: 0x001400C1
			internal SharedLong(long value)
			{
				this.Value = value;
			}

			// Token: 0x04003992 RID: 14738
			internal long Value;
		}
	}
}
