using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000018 RID: 24
	public class HistoryBounds
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000027F5 File Offset: 0x000009F5
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000027FD File Offset: 0x000009FD
		public int boundsCount { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002808 File Offset: 0x00000A08
		public Bounds total
		{
			get
			{
				Bounds result = default(Bounds);
				result.SetMinMax(this.totalMinMax.min, this.totalMinMax.max);
				return result;
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000283B File Offset: 0x00000A3B
		public HistoryBounds(int boundsLimit, int boundsPerBucket)
		{
			this.boundsPerBucket = boundsPerBucket;
			this.bucketLimit = boundsLimit / boundsPerBucket;
			this.fullBuckets = new Queue<MinMaxBounds>(this.bucketLimit + 1);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002868 File Offset: 0x00000A68
		public void Insert(Bounds bounds)
		{
			MinMaxBounds minMaxBounds = new MinMaxBounds
			{
				min = bounds.min,
				max = bounds.max
			};
			if (this.boundsCount == 0)
			{
				this.totalMinMax = minMaxBounds;
			}
			if (this.currentBucket == null)
			{
				this.currentBucket = new MinMaxBounds?(minMaxBounds);
			}
			else
			{
				this.currentBucket.Value.Encapsulate(minMaxBounds);
			}
			this.currentBucketSize++;
			this.boundsCount++;
			this.totalMinMax.Encapsulate(minMaxBounds);
			if (this.currentBucketSize == this.boundsPerBucket)
			{
				this.fullBuckets.Enqueue(this.currentBucket.Value);
				this.currentBucket = null;
				this.currentBucketSize = 0;
				if (this.fullBuckets.Count > this.bucketLimit)
				{
					this.fullBuckets.Dequeue();
					this.boundsCount -= this.boundsPerBucket;
					this.totalMinMax = minMaxBounds;
					foreach (MinMaxBounds bounds2 in this.fullBuckets)
					{
						this.totalMinMax.Encapsulate(bounds2);
					}
				}
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000029C0 File Offset: 0x00000BC0
		public void Reset()
		{
			this.fullBuckets.Clear();
			this.currentBucket = null;
			this.currentBucketSize = 0;
			this.boundsCount = 0;
			this.totalMinMax = default(MinMaxBounds);
		}

		// Token: 0x0400001A RID: 26
		private readonly int boundsPerBucket;

		// Token: 0x0400001B RID: 27
		private readonly Queue<MinMaxBounds> fullBuckets;

		// Token: 0x0400001C RID: 28
		private readonly int bucketLimit;

		// Token: 0x0400001D RID: 29
		private MinMaxBounds? currentBucket;

		// Token: 0x0400001E RID: 30
		private int currentBucketSize;

		// Token: 0x04000020 RID: 32
		private MinMaxBounds totalMinMax;
	}
}
