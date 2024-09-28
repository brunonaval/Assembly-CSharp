using System;
using System.Collections;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x0200006C RID: 108
	public class SyncIDictionary<TKey, TValue> : SyncObject, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000C348 File Offset: 0x0000A548
		public int Count
		{
			get
			{
				return this.objects.Count;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000C355 File Offset: 0x0000A555
		public bool IsReadOnly
		{
			get
			{
				return !this.IsWritable();
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000332 RID: 818 RVA: 0x0000C368 File Offset: 0x0000A568
		// (remove) Token: 0x06000333 RID: 819 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		public event SyncIDictionary<TKey, TValue>.SyncDictionaryChanged Callback;

		// Token: 0x06000334 RID: 820 RVA: 0x0000C3D5 File Offset: 0x0000A5D5
		public override void Reset()
		{
			this.changes.Clear();
			this.changesAhead = 0;
			this.objects.Clear();
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public ICollection<TKey> Keys
		{
			get
			{
				return this.objects.Keys;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000C401 File Offset: 0x0000A601
		public ICollection<TValue> Values
		{
			get
			{
				return this.objects.Values;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		IEnumerable<TKey> IReadOnlyDictionary<!0, !1>.Keys
		{
			get
			{
				return this.objects.Keys;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000C401 File Offset: 0x0000A601
		IEnumerable<TValue> IReadOnlyDictionary<!0, !1>.Values
		{
			get
			{
				return this.objects.Values;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000C40E File Offset: 0x0000A60E
		public override void ClearChanges()
		{
			this.changes.Clear();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000C41B File Offset: 0x0000A61B
		public SyncIDictionary(IDictionary<TKey, TValue> objects)
		{
			this.objects = objects;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000C438 File Offset: 0x0000A638
		private void AddOperation(SyncIDictionary<TKey, TValue>.Operation op, TKey key, TValue item, bool checkAccess)
		{
			if (checkAccess && this.IsReadOnly)
			{
				throw new InvalidOperationException("SyncDictionaries can only be modified by the owner.");
			}
			SyncIDictionary<TKey, TValue>.Change item2 = new SyncIDictionary<TKey, TValue>.Change
			{
				operation = op,
				key = key,
				item = item
			};
			if (this.IsRecording())
			{
				this.changes.Add(item2);
				Action onDirty = this.OnDirty;
				if (onDirty != null)
				{
					onDirty();
				}
			}
			SyncIDictionary<TKey, TValue>.SyncDictionaryChanged callback = this.Callback;
			if (callback == null)
			{
				return;
			}
			callback(op, key, item);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000C4BC File Offset: 0x0000A6BC
		public override void OnSerializeAll(NetworkWriter writer)
		{
			writer.WriteUInt((uint)this.objects.Count);
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.objects)
			{
				writer.Write<TKey>(keyValuePair.Key);
				writer.Write<TValue>(keyValuePair.Value);
			}
			writer.WriteUInt((uint)this.changes.Count);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000C540 File Offset: 0x0000A740
		public override void OnSerializeDelta(NetworkWriter writer)
		{
			writer.WriteUInt((uint)this.changes.Count);
			for (int i = 0; i < this.changes.Count; i++)
			{
				SyncIDictionary<TKey, TValue>.Change change = this.changes[i];
				writer.WriteByte((byte)change.operation);
				switch (change.operation)
				{
				case SyncIDictionary<TKey, TValue>.Operation.OP_ADD:
				case SyncIDictionary<TKey, TValue>.Operation.OP_SET:
					writer.Write<TKey>(change.key);
					writer.Write<TValue>(change.item);
					break;
				case SyncIDictionary<TKey, TValue>.Operation.OP_REMOVE:
					writer.Write<TKey>(change.key);
					break;
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C5D4 File Offset: 0x0000A7D4
		public override void OnDeserializeAll(NetworkReader reader)
		{
			int num = (int)reader.ReadUInt();
			this.objects.Clear();
			this.changes.Clear();
			for (int i = 0; i < num; i++)
			{
				TKey key = reader.Read<TKey>();
				TValue value = reader.Read<TValue>();
				this.objects.Add(key, value);
			}
			this.changesAhead = (int)reader.ReadUInt();
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000C634 File Offset: 0x0000A834
		public override void OnDeserializeDelta(NetworkReader reader)
		{
			int num = (int)reader.ReadUInt();
			for (int i = 0; i < num; i++)
			{
				SyncIDictionary<TKey, TValue>.Operation operation = (SyncIDictionary<TKey, TValue>.Operation)reader.ReadByte();
				bool flag = this.changesAhead == 0;
				TKey key = default(TKey);
				TValue tvalue = default(TValue);
				switch (operation)
				{
				case SyncIDictionary<TKey, TValue>.Operation.OP_ADD:
				case SyncIDictionary<TKey, TValue>.Operation.OP_SET:
					key = reader.Read<TKey>();
					tvalue = reader.Read<TValue>();
					if (flag)
					{
						if (this.ContainsKey(key))
						{
							this.objects[key] = tvalue;
							this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_SET, key, tvalue, false);
						}
						else
						{
							this.objects[key] = tvalue;
							this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_ADD, key, tvalue, false);
						}
					}
					break;
				case SyncIDictionary<TKey, TValue>.Operation.OP_CLEAR:
					if (flag)
					{
						this.objects.Clear();
						this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_CLEAR, default(TKey), default(TValue), false);
					}
					break;
				case SyncIDictionary<TKey, TValue>.Operation.OP_REMOVE:
					key = reader.Read<TKey>();
					if (flag && this.objects.TryGetValue(key, out tvalue))
					{
						this.objects.Remove(key);
						this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_REMOVE, key, tvalue, false);
					}
					break;
				}
				if (!flag)
				{
					this.changesAhead--;
				}
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000C764 File Offset: 0x0000A964
		public void Clear()
		{
			this.objects.Clear();
			this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_CLEAR, default(TKey), default(TValue), true);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000C796 File Offset: 0x0000A996
		public bool ContainsKey(TKey key)
		{
			return this.objects.ContainsKey(key);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000C7A4 File Offset: 0x0000A9A4
		public bool Remove(TKey key)
		{
			TValue item;
			if (this.objects.TryGetValue(key, out item) && this.objects.Remove(key))
			{
				this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_REMOVE, key, item, true);
				return true;
			}
			return false;
		}

		// Token: 0x1700005A RID: 90
		public TValue this[TKey i]
		{
			get
			{
				return this.objects[i];
			}
			set
			{
				if (this.ContainsKey(i))
				{
					this.objects[i] = value;
					this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_SET, i, value, true);
					return;
				}
				this.objects[i] = value;
				this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_ADD, i, value, true);
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000C824 File Offset: 0x0000AA24
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.objects.TryGetValue(key, out value);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000C833 File Offset: 0x0000AA33
		public void Add(TKey key, TValue value)
		{
			this.objects.Add(key, value);
			this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_ADD, key, value, true);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000C84C File Offset: 0x0000AA4C
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000C864 File Offset: 0x0000AA64
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			TValue x;
			return this.TryGetValue(item.Key, out x) && EqualityComparer<TValue>.Default.Equals(x, item.Value);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000C898 File Offset: 0x0000AA98
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (arrayIndex < 0 || arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Array Index Out of Range");
			}
			if (array.Length - arrayIndex < this.Count)
			{
				throw new ArgumentException("The number of items in the SyncDictionary is greater than the available space from arrayIndex to the end of the destination array");
			}
			int num = arrayIndex;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this.objects)
			{
				array[num] = keyValuePair;
				num++;
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000C920 File Offset: 0x0000AB20
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			bool flag = this.objects.Remove(item.Key);
			if (flag)
			{
				this.AddOperation(SyncIDictionary<TKey, TValue>.Operation.OP_REMOVE, item.Key, item.Value, true);
			}
			return flag;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000C94D File Offset: 0x0000AB4D
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.objects.GetEnumerator();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000C94D File Offset: 0x0000AB4D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.objects.GetEnumerator();
		}

		// Token: 0x04000143 RID: 323
		protected readonly IDictionary<TKey, TValue> objects;

		// Token: 0x04000145 RID: 325
		private readonly List<SyncIDictionary<TKey, TValue>.Change> changes = new List<SyncIDictionary<TKey, TValue>.Change>();

		// Token: 0x04000146 RID: 326
		private int changesAhead;

		// Token: 0x0200006D RID: 109
		// (Invoke) Token: 0x0600034E RID: 846
		public delegate void SyncDictionaryChanged(SyncIDictionary<TKey, TValue>.Operation op, TKey key, TValue item);

		// Token: 0x0200006E RID: 110
		public enum Operation : byte
		{
			// Token: 0x04000148 RID: 328
			OP_ADD,
			// Token: 0x04000149 RID: 329
			OP_CLEAR,
			// Token: 0x0400014A RID: 330
			OP_REMOVE,
			// Token: 0x0400014B RID: 331
			OP_SET
		}

		// Token: 0x0200006F RID: 111
		private struct Change
		{
			// Token: 0x0400014C RID: 332
			internal SyncIDictionary<TKey, TValue>.Operation operation;

			// Token: 0x0400014D RID: 333
			internal TKey key;

			// Token: 0x0400014E RID: 334
			internal TValue item;
		}
	}
}
