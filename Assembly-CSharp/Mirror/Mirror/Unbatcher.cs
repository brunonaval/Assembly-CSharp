using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000011 RID: 17
	public class Unbatcher
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002253 File Offset: 0x00000453
		public int BatchesCount
		{
			get
			{
				return this.batches.Count;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002260 File Offset: 0x00000460
		private void StartReadingBatch(NetworkWriterPooled batch)
		{
			this.reader.SetBuffer(batch.ToArraySegment());
			this.readerRemoteTimeStamp = this.reader.ReadDouble();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002284 File Offset: 0x00000484
		public bool AddBatch(ArraySegment<byte> batch)
		{
			if (batch.Count < 8)
			{
				return false;
			}
			NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
			networkWriterPooled.WriteBytes(batch.Array, batch.Offset, batch.Count);
			if (this.batches.Count == 0)
			{
				this.StartReadingBatch(networkWriterPooled);
			}
			this.batches.Enqueue(networkWriterPooled);
			return true;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000022E0 File Offset: 0x000004E0
		public bool GetNextMessage(out ArraySegment<byte> message, out double remoteTimeStamp)
		{
			message = default(ArraySegment<byte>);
			remoteTimeStamp = 0.0;
			if (this.batches.Count == 0)
			{
				return false;
			}
			if (this.reader.Capacity == 0)
			{
				return false;
			}
			if (this.reader.Remaining == 0)
			{
				NetworkWriterPool.Return(this.batches.Dequeue());
				if (this.batches.Count <= 0)
				{
					return false;
				}
				NetworkWriterPooled batch = this.batches.Peek();
				this.StartReadingBatch(batch);
			}
			remoteTimeStamp = this.readerRemoteTimeStamp;
			if (this.reader.Remaining == 0)
			{
				return false;
			}
			int num = (int)Compression.DecompressVarUInt(this.reader);
			if (this.reader.Remaining < num)
			{
				return false;
			}
			message = this.reader.ReadBytesSegment(num);
			return true;
		}

		// Token: 0x04000010 RID: 16
		private readonly Queue<NetworkWriterPooled> batches = new Queue<NetworkWriterPooled>();

		// Token: 0x04000011 RID: 17
		private readonly NetworkReader reader = new NetworkReader(new byte[0]);

		// Token: 0x04000012 RID: 18
		private double readerRemoteTimeStamp;
	}
}
