using System;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000010 RID: 16
	public class Batcher
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000020EE File Offset: 0x000002EE
		public static int MessageHeaderSize(int messageSize)
		{
			return Compression.VarUIntSize((ulong)((long)messageSize));
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000020F7 File Offset: 0x000002F7
		public static int MaxMessageOverhead(int messageSize)
		{
			return 8 + Batcher.MessageHeaderSize(messageSize);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002101 File Offset: 0x00000301
		public Batcher(int threshold)
		{
			this.threshold = threshold;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000211C File Offset: 0x0000031C
		public void AddMessage(ArraySegment<byte> message, double timeStamp)
		{
			int num = Compression.VarUIntSize((ulong)((long)message.Count)) + message.Count;
			if (this.batch != null && this.batch.Position + num > this.threshold)
			{
				this.batches.Enqueue(this.batch);
				this.batch = null;
			}
			if (this.batch == null)
			{
				this.batch = NetworkWriterPool.Get();
				this.batch.WriteDouble(timeStamp);
			}
			Compression.CompressVarUInt(this.batch, (ulong)((long)message.Count));
			this.batch.WriteBytes(message.Array, message.Offset, message.Count);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000021C8 File Offset: 0x000003C8
		private static void CopyAndReturn(NetworkWriterPooled batch, NetworkWriter writer)
		{
			if (writer.Position != 0)
			{
				throw new ArgumentException("GetBatch needs a fresh writer!");
			}
			ArraySegment<byte> arraySegment = batch.ToArraySegment();
			writer.WriteBytes(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
			NetworkWriterPool.Return(batch);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002210 File Offset: 0x00000410
		public bool GetBatch(NetworkWriter writer)
		{
			NetworkWriterPooled networkWriterPooled;
			if (this.batches.TryDequeue(out networkWriterPooled))
			{
				Batcher.CopyAndReturn(networkWriterPooled, writer);
				return true;
			}
			if (this.batch != null)
			{
				Batcher.CopyAndReturn(this.batch, writer);
				this.batch = null;
				return true;
			}
			return false;
		}

		// Token: 0x0400000C RID: 12
		private readonly int threshold;

		// Token: 0x0400000D RID: 13
		public const int TimestampSize = 8;

		// Token: 0x0400000E RID: 14
		private readonly Queue<NetworkWriterPooled> batches = new Queue<NetworkWriterPooled>();

		// Token: 0x0400000F RID: 15
		private NetworkWriterPooled batch;
	}
}
