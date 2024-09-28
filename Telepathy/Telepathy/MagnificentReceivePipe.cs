using System;
using System.Collections.Generic;

namespace Telepathy
{
	// Token: 0x02000009 RID: 9
	public class MagnificentReceivePipe
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002650 File Offset: 0x00000850
		public MagnificentReceivePipe(int MaxMessageSize)
		{
			this.pool = new Pool<byte[]>(() => new byte[MaxMessageSize]);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026A0 File Offset: 0x000008A0
		public int Count(int connectionId)
		{
			int result;
			lock (this)
			{
				int num;
				result = (this.queueCounter.TryGetValue(connectionId, out num) ? num : 0);
			}
			return result;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000014 RID: 20 RVA: 0x000026EC File Offset: 0x000008EC
		public int TotalCount
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002730 File Offset: 0x00000930
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

		// Token: 0x06000016 RID: 22 RVA: 0x00002774 File Offset: 0x00000974
		public void Enqueue(int connectionId, EventType eventType, ArraySegment<byte> message)
		{
			lock (this)
			{
				ArraySegment<byte> data = default(ArraySegment<byte>);
				if (message != default(ArraySegment<byte>))
				{
					byte[] array = this.pool.Take();
					Buffer.BlockCopy(message.Array, message.Offset, array, 0, message.Count);
					data = new ArraySegment<byte>(array, 0, message.Count);
				}
				MagnificentReceivePipe.Entry item = new MagnificentReceivePipe.Entry(connectionId, eventType, data);
				this.queue.Enqueue(item);
				int num = this.Count(connectionId);
				this.queueCounter[connectionId] = num + 1;
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002830 File Offset: 0x00000A30
		public bool TryPeek(out int connectionId, out EventType eventType, out ArraySegment<byte> data)
		{
			connectionId = 0;
			eventType = EventType.Disconnected;
			data = default(ArraySegment<byte>);
			bool result;
			lock (this)
			{
				if (this.queue.Count > 0)
				{
					MagnificentReceivePipe.Entry entry = this.queue.Peek();
					connectionId = entry.connectionId;
					eventType = entry.eventType;
					data = entry.data;
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028B0 File Offset: 0x00000AB0
		public bool TryDequeue()
		{
			bool result;
			lock (this)
			{
				if (this.queue.Count > 0)
				{
					MagnificentReceivePipe.Entry entry = this.queue.Dequeue();
					if (entry.data != default(ArraySegment<byte>))
					{
						this.pool.Return(entry.data.Array);
					}
					Dictionary<int, int> dictionary = this.queueCounter;
					int connectionId = entry.connectionId;
					int num = dictionary[connectionId];
					dictionary[connectionId] = num - 1;
					if (this.queueCounter[entry.connectionId] == 0)
					{
						this.queueCounter.Remove(entry.connectionId);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002984 File Offset: 0x00000B84
		public void Clear()
		{
			lock (this)
			{
				while (this.queue.Count > 0)
				{
					MagnificentReceivePipe.Entry entry = this.queue.Dequeue();
					if (entry.data != default(ArraySegment<byte>))
					{
						this.pool.Return(entry.data.Array);
					}
				}
				this.queueCounter.Clear();
			}
		}

		// Token: 0x04000018 RID: 24
		private readonly Queue<MagnificentReceivePipe.Entry> queue = new Queue<MagnificentReceivePipe.Entry>();

		// Token: 0x04000019 RID: 25
		private Pool<byte[]> pool;

		// Token: 0x0400001A RID: 26
		private Dictionary<int, int> queueCounter = new Dictionary<int, int>();

		// Token: 0x02000014 RID: 20
		private struct Entry
		{
			// Token: 0x0600003C RID: 60 RVA: 0x00003700 File Offset: 0x00001900
			public Entry(int connectionId, EventType eventType, ArraySegment<byte> data)
			{
				this.connectionId = connectionId;
				this.eventType = eventType;
				this.data = data;
			}

			// Token: 0x04000034 RID: 52
			public int connectionId;

			// Token: 0x04000035 RID: 53
			public EventType eventType;

			// Token: 0x04000036 RID: 54
			public ArraySegment<byte> data;
		}
	}
}
