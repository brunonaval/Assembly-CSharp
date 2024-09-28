using System;
using System.Collections.Generic;

namespace Telepathy
{
	// Token: 0x0200000A RID: 10
	public class MagnificentSendPipe
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002A10 File Offset: 0x00000C10
		public MagnificentSendPipe(int MaxMessageSize)
		{
			this.pool = new Pool<byte[]>(() => new byte[MaxMessageSize]);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002A54 File Offset: 0x00000C54
		public int Count
		{
			get
			{
				int count;
				lock (this)
				{
					count = this.queue.Count;
				}
				return count;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002A98 File Offset: 0x00000C98
		public int PoolCount
		{
			get
			{
				int result;
				lock (this)
				{
					result = this.pool.Count();
				}
				return result;
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002ADC File Offset: 0x00000CDC
		public void Enqueue(ArraySegment<byte> message)
		{
			lock (this)
			{
				byte[] array = this.pool.Take();
				Buffer.BlockCopy(message.Array, message.Offset, array, 0, message.Count);
				ArraySegment<byte> item = new ArraySegment<byte>(array, 0, message.Count);
				this.queue.Enqueue(item);
			}
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B58 File Offset: 0x00000D58
		public bool DequeueAndSerializeAll(ref byte[] payload, out int packetSize)
		{
			bool result;
			lock (this)
			{
				packetSize = 0;
				if (this.queue.Count == 0)
				{
					result = false;
				}
				else
				{
					packetSize = 0;
					foreach (ArraySegment<byte> arraySegment in this.queue)
					{
						packetSize += 4 + arraySegment.Count;
					}
					if (payload == null || payload.Length < packetSize)
					{
						payload = new byte[packetSize];
					}
					int num = 0;
					while (this.queue.Count > 0)
					{
						ArraySegment<byte> arraySegment2 = this.queue.Dequeue();
						Utils.IntToBytesBigEndianNonAlloc(arraySegment2.Count, payload, num);
						num += 4;
						Buffer.BlockCopy(arraySegment2.Array, arraySegment2.Offset, payload, num, arraySegment2.Count);
						num += arraySegment2.Count;
						this.pool.Return(arraySegment2.Array);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002C78 File Offset: 0x00000E78
		public void Clear()
		{
			lock (this)
			{
				while (this.queue.Count > 0)
				{
					this.pool.Return(this.queue.Dequeue().Array);
				}
			}
		}

		// Token: 0x0400001B RID: 27
		private readonly Queue<ArraySegment<byte>> queue = new Queue<ArraySegment<byte>>();

		// Token: 0x0400001C RID: 28
		private Pool<byte[]> pool;
	}
}
