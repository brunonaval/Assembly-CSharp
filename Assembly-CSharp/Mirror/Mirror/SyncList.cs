using System;
using System.Collections;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000071 RID: 113
	public class SyncList<T> : SyncObject, IList<T>, ICollection<!0>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000357 RID: 855 RVA: 0x0000C9B9 File Offset: 0x0000ABB9
		public int Count
		{
			get
			{
				return this.objects.Count;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000C355 File Offset: 0x0000A555
		public bool IsReadOnly
		{
			get
			{
				return !this.IsWritable();
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000359 RID: 857 RVA: 0x0000C9C8 File Offset: 0x0000ABC8
		// (remove) Token: 0x0600035A RID: 858 RVA: 0x0000CA00 File Offset: 0x0000AC00
		public event SyncList<T>.SyncListChanged Callback;

		// Token: 0x0600035B RID: 859 RVA: 0x0000CA35 File Offset: 0x0000AC35
		public SyncList() : this(EqualityComparer<T>.Default)
		{
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000CA42 File Offset: 0x0000AC42
		public SyncList(IEqualityComparer<T> comparer)
		{
			this.changes = new List<SyncList<T>.Change>();
			base..ctor();
			this.comparer = (comparer ?? EqualityComparer<T>.Default);
			this.objects = new List<T>();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000CA70 File Offset: 0x0000AC70
		public SyncList(IList<T> objects, IEqualityComparer<T> comparer = null)
		{
			this.changes = new List<SyncList<T>.Change>();
			base..ctor();
			this.comparer = (comparer ?? EqualityComparer<T>.Default);
			this.objects = objects;
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000CA9A File Offset: 0x0000AC9A
		public override void ClearChanges()
		{
			this.changes.Clear();
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0000CAA7 File Offset: 0x0000ACA7
		public override void Reset()
		{
			this.changes.Clear();
			this.changesAhead = 0;
			this.objects.Clear();
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000CAC8 File Offset: 0x0000ACC8
		private void AddOperation(SyncList<T>.Operation op, int itemIndex, T oldItem, T newItem, bool checkAccess)
		{
			if (checkAccess && this.IsReadOnly)
			{
				throw new InvalidOperationException("Synclists can only be modified by the owner.");
			}
			SyncList<T>.Change item = new SyncList<T>.Change
			{
				operation = op,
				index = itemIndex,
				item = newItem
			};
			if (this.IsRecording())
			{
				this.changes.Add(item);
				Action onDirty = this.OnDirty;
				if (onDirty != null)
				{
					onDirty();
				}
			}
			SyncList<T>.SyncListChanged callback = this.Callback;
			if (callback == null)
			{
				return;
			}
			callback(op, itemIndex, oldItem, newItem);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0000CB50 File Offset: 0x0000AD50
		public override void OnSerializeAll(NetworkWriter writer)
		{
			writer.WriteUInt((uint)this.objects.Count);
			for (int i = 0; i < this.objects.Count; i++)
			{
				T value = this.objects[i];
				writer.Write<T>(value);
			}
			writer.WriteUInt((uint)this.changes.Count);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public override void OnSerializeDelta(NetworkWriter writer)
		{
			writer.WriteUInt((uint)this.changes.Count);
			for (int i = 0; i < this.changes.Count; i++)
			{
				SyncList<T>.Change change = this.changes[i];
				writer.WriteByte((byte)change.operation);
				switch (change.operation)
				{
				case SyncList<T>.Operation.OP_ADD:
					writer.Write<T>(change.item);
					break;
				case SyncList<T>.Operation.OP_INSERT:
				case SyncList<T>.Operation.OP_SET:
					writer.WriteUInt((uint)change.index);
					writer.Write<T>(change.item);
					break;
				case SyncList<T>.Operation.OP_REMOVEAT:
					writer.WriteUInt((uint)change.index);
					break;
				}
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000CC54 File Offset: 0x0000AE54
		public override void OnDeserializeAll(NetworkReader reader)
		{
			int num = (int)reader.ReadUInt();
			this.objects.Clear();
			this.changes.Clear();
			for (int i = 0; i < num; i++)
			{
				T item = reader.Read<T>();
				this.objects.Add(item);
			}
			this.changesAhead = (int)reader.ReadUInt();
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000CCAC File Offset: 0x0000AEAC
		public override void OnDeserializeDelta(NetworkReader reader)
		{
			int num = (int)reader.ReadUInt();
			for (int i = 0; i < num; i++)
			{
				SyncList<T>.Operation operation = (SyncList<T>.Operation)reader.ReadByte();
				bool flag = this.changesAhead == 0;
				T oldItem = default(T);
				T t = default(T);
				switch (operation)
				{
				case SyncList<T>.Operation.OP_ADD:
					t = reader.Read<T>();
					if (flag)
					{
						int num2 = this.objects.Count;
						this.objects.Add(t);
						this.AddOperation(SyncList<T>.Operation.OP_ADD, this.objects.Count - 1, default(T), t, false);
					}
					break;
				case SyncList<T>.Operation.OP_CLEAR:
					if (flag)
					{
						this.objects.Clear();
						this.AddOperation(SyncList<T>.Operation.OP_CLEAR, 0, default(T), default(T), false);
					}
					break;
				case SyncList<T>.Operation.OP_INSERT:
				{
					int num2 = (int)reader.ReadUInt();
					t = reader.Read<T>();
					if (flag)
					{
						this.objects.Insert(num2, t);
						this.AddOperation(SyncList<T>.Operation.OP_INSERT, num2, default(T), t, false);
					}
					break;
				}
				case SyncList<T>.Operation.OP_REMOVEAT:
				{
					int num2 = (int)reader.ReadUInt();
					if (flag)
					{
						oldItem = this.objects[num2];
						this.objects.RemoveAt(num2);
						this.AddOperation(SyncList<T>.Operation.OP_REMOVEAT, num2, oldItem, default(T), false);
					}
					break;
				}
				case SyncList<T>.Operation.OP_SET:
				{
					int num2 = (int)reader.ReadUInt();
					t = reader.Read<T>();
					if (flag)
					{
						oldItem = this.objects[num2];
						this.objects[num2] = t;
						this.AddOperation(SyncList<T>.Operation.OP_SET, num2, oldItem, t, false);
					}
					break;
				}
				}
				if (!flag)
				{
					this.changesAhead--;
				}
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000CE64 File Offset: 0x0000B064
		public void Add(T item)
		{
			this.objects.Add(item);
			this.AddOperation(SyncList<T>.Operation.OP_ADD, this.objects.Count - 1, default(T), item, true);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000CE9C File Offset: 0x0000B09C
		public void AddRange(IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				this.Add(item);
			}
		}

		// Token: 0x06000367 RID: 871 RVA: 0x0000CEE4 File Offset: 0x0000B0E4
		public void Clear()
		{
			this.objects.Clear();
			this.AddOperation(SyncList<T>.Operation.OP_CLEAR, 0, default(T), default(T), true);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000CF17 File Offset: 0x0000B117
		public bool Contains(T item)
		{
			return this.IndexOf(item) >= 0;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0000CF26 File Offset: 0x0000B126
		public void CopyTo(T[] array, int index)
		{
			this.objects.CopyTo(array, index);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000CF38 File Offset: 0x0000B138
		public int IndexOf(T item)
		{
			for (int i = 0; i < this.objects.Count; i++)
			{
				if (this.comparer.Equals(item, this.objects[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000CF78 File Offset: 0x0000B178
		public int FindIndex(Predicate<T> match)
		{
			for (int i = 0; i < this.objects.Count; i++)
			{
				if (match(this.objects[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		public T Find(Predicate<T> match)
		{
			int num = this.FindIndex(match);
			if (num == -1)
			{
				return default(T);
			}
			return this.objects[num];
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		public List<T> FindAll(Predicate<T> match)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.objects.Count; i++)
			{
				if (match(this.objects[i]))
				{
					list.Add(this.objects[i]);
				}
			}
			return list;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000D034 File Offset: 0x0000B234
		public void Insert(int index, T item)
		{
			this.objects.Insert(index, item);
			this.AddOperation(SyncList<T>.Operation.OP_INSERT, index, default(T), item, true);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000D064 File Offset: 0x0000B264
		public void InsertRange(int index, IEnumerable<T> range)
		{
			foreach (T item in range)
			{
				this.Insert(index, item);
				index++;
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000D0B4 File Offset: 0x0000B2B4
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			bool flag = num >= 0;
			if (flag)
			{
				this.RemoveAt(num);
			}
			return flag;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000D0DC File Offset: 0x0000B2DC
		public void RemoveAt(int index)
		{
			T oldItem = this.objects[index];
			this.objects.RemoveAt(index);
			this.AddOperation(SyncList<T>.Operation.OP_REMOVEAT, index, oldItem, default(T), true);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000D118 File Offset: 0x0000B318
		public int RemoveAll(Predicate<T> match)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.objects.Count; i++)
			{
				if (match(this.objects[i]))
				{
					list.Add(this.objects[i]);
				}
			}
			foreach (T item in list)
			{
				this.Remove(item);
			}
			return list.Count;
		}

		// Token: 0x1700005F RID: 95
		public T this[int i]
		{
			get
			{
				return this.objects[i];
			}
			set
			{
				if (!this.comparer.Equals(this.objects[i], value))
				{
					T oldItem = this.objects[i];
					this.objects[i] = value;
					this.AddOperation(SyncList<T>.Operation.OP_SET, i, oldItem, value, true);
				}
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000D20C File Offset: 0x0000B40C
		public SyncList<T>.Enumerator GetEnumerator()
		{
			return new SyncList<T>.Enumerator(this);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000D214 File Offset: 0x0000B414
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return new SyncList<T>.Enumerator(this);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0000D214 File Offset: 0x0000B414
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new SyncList<T>.Enumerator(this);
		}

		// Token: 0x0400014F RID: 335
		private readonly IList<T> objects;

		// Token: 0x04000150 RID: 336
		private readonly IEqualityComparer<T> comparer;

		// Token: 0x04000152 RID: 338
		private readonly List<SyncList<T>.Change> changes;

		// Token: 0x04000153 RID: 339
		private int changesAhead;

		// Token: 0x02000072 RID: 114
		// (Invoke) Token: 0x06000379 RID: 889
		public delegate void SyncListChanged(SyncList<T>.Operation op, int itemIndex, T oldItem, T newItem);

		// Token: 0x02000073 RID: 115
		public enum Operation : byte
		{
			// Token: 0x04000155 RID: 341
			OP_ADD,
			// Token: 0x04000156 RID: 342
			OP_CLEAR,
			// Token: 0x04000157 RID: 343
			OP_INSERT,
			// Token: 0x04000158 RID: 344
			OP_REMOVEAT,
			// Token: 0x04000159 RID: 345
			OP_SET
		}

		// Token: 0x02000074 RID: 116
		private struct Change
		{
			// Token: 0x0400015A RID: 346
			internal SyncList<T>.Operation operation;

			// Token: 0x0400015B RID: 347
			internal int index;

			// Token: 0x0400015C RID: 348
			internal T item;
		}

		// Token: 0x02000075 RID: 117
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x17000060 RID: 96
			// (get) Token: 0x0600037C RID: 892 RVA: 0x0000D221 File Offset: 0x0000B421
			// (set) Token: 0x0600037D RID: 893 RVA: 0x0000D229 File Offset: 0x0000B429
			public T Current { readonly get; private set; }

			// Token: 0x0600037E RID: 894 RVA: 0x0000D234 File Offset: 0x0000B434
			public Enumerator(SyncList<T> list)
			{
				this.list = list;
				this.index = -1;
				this.Current = default(T);
			}

			// Token: 0x0600037F RID: 895 RVA: 0x0000D260 File Offset: 0x0000B460
			public bool MoveNext()
			{
				int num = this.index + 1;
				this.index = num;
				if (num >= this.list.Count)
				{
					return false;
				}
				this.Current = this.list[this.index];
				return true;
			}

			// Token: 0x06000380 RID: 896 RVA: 0x0000D2A5 File Offset: 0x0000B4A5
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x17000061 RID: 97
			// (get) Token: 0x06000381 RID: 897 RVA: 0x0000D2AE File Offset: 0x0000B4AE
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000382 RID: 898 RVA: 0x000024F9 File Offset: 0x000006F9
			public void Dispose()
			{
			}

			// Token: 0x0400015D RID: 349
			private readonly SyncList<T> list;

			// Token: 0x0400015E RID: 350
			private int index;
		}
	}
}
