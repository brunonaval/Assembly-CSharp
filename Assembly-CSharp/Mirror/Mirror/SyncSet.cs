using System;
using System.Collections;
using System.Collections.Generic;

namespace Mirror
{
	// Token: 0x02000078 RID: 120
	public class SyncSet<T> : SyncObject, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600038E RID: 910 RVA: 0x0000D325 File Offset: 0x0000B525
		public int Count
		{
			get
			{
				return this.objects.Count;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0000C355 File Offset: 0x0000A555
		public bool IsReadOnly
		{
			get
			{
				return !this.IsWritable();
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000390 RID: 912 RVA: 0x0000D334 File Offset: 0x0000B534
		// (remove) Token: 0x06000391 RID: 913 RVA: 0x0000D36C File Offset: 0x0000B56C
		public event SyncSet<T>.SyncSetChanged Callback;

		// Token: 0x06000392 RID: 914 RVA: 0x0000D3A1 File Offset: 0x0000B5A1
		public SyncSet(ISet<T> objects)
		{
			this.objects = objects;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000D3BB File Offset: 0x0000B5BB
		public override void Reset()
		{
			this.changes.Clear();
			this.changesAhead = 0;
			this.objects.Clear();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000D3DA File Offset: 0x0000B5DA
		public override void ClearChanges()
		{
			this.changes.Clear();
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0000D3E8 File Offset: 0x0000B5E8
		private void AddOperation(SyncSet<T>.Operation op, T item, bool checkAccess)
		{
			if (checkAccess && this.IsReadOnly)
			{
				throw new InvalidOperationException("SyncSets can only be modified by the owner.");
			}
			SyncSet<T>.Change item2 = new SyncSet<T>.Change
			{
				operation = op,
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
			SyncSet<T>.SyncSetChanged callback = this.Callback;
			if (callback == null)
			{
				return;
			}
			callback(op, item);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0000D464 File Offset: 0x0000B664
		private void AddOperation(SyncSet<T>.Operation op, bool checkAccess)
		{
			this.AddOperation(op, default(T), checkAccess);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000D484 File Offset: 0x0000B684
		public override void OnSerializeAll(NetworkWriter writer)
		{
			writer.WriteUInt((uint)this.objects.Count);
			foreach (T value in this.objects)
			{
				writer.Write<T>(value);
			}
			writer.WriteUInt((uint)this.changes.Count);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public override void OnSerializeDelta(NetworkWriter writer)
		{
			writer.WriteUInt((uint)this.changes.Count);
			for (int i = 0; i < this.changes.Count; i++)
			{
				SyncSet<T>.Change change = this.changes[i];
				writer.WriteByte((byte)change.operation);
				switch (change.operation)
				{
				case SyncSet<T>.Operation.OP_ADD:
					writer.Write<T>(change.item);
					break;
				case SyncSet<T>.Operation.OP_REMOVE:
					writer.Write<T>(change.item);
					break;
				}
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000D578 File Offset: 0x0000B778
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

		// Token: 0x0600039A RID: 922 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		public override void OnDeserializeDelta(NetworkReader reader)
		{
			int num = (int)reader.ReadUInt();
			for (int i = 0; i < num; i++)
			{
				SyncSet<T>.Operation operation = (SyncSet<T>.Operation)reader.ReadByte();
				bool flag = this.changesAhead == 0;
				T item = default(T);
				switch (operation)
				{
				case SyncSet<T>.Operation.OP_ADD:
					item = reader.Read<T>();
					if (flag)
					{
						this.objects.Add(item);
						this.AddOperation(SyncSet<T>.Operation.OP_ADD, item, false);
					}
					break;
				case SyncSet<T>.Operation.OP_CLEAR:
					if (flag)
					{
						this.objects.Clear();
						this.AddOperation(SyncSet<T>.Operation.OP_CLEAR, false);
					}
					break;
				case SyncSet<T>.Operation.OP_REMOVE:
					item = reader.Read<T>();
					if (flag)
					{
						this.objects.Remove(item);
						this.AddOperation(SyncSet<T>.Operation.OP_REMOVE, item, false);
					}
					break;
				}
				if (!flag)
				{
					this.changesAhead--;
				}
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000D694 File Offset: 0x0000B894
		public bool Add(T item)
		{
			if (this.objects.Add(item))
			{
				this.AddOperation(SyncSet<T>.Operation.OP_ADD, item, true);
				return true;
			}
			return false;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000D6B0 File Offset: 0x0000B8B0
		void ICollection<!0>.Add(T item)
		{
			if (this.objects.Add(item))
			{
				this.AddOperation(SyncSet<T>.Operation.OP_ADD, item, true);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000D6C9 File Offset: 0x0000B8C9
		public void Clear()
		{
			this.objects.Clear();
			this.AddOperation(SyncSet<T>.Operation.OP_CLEAR, true);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000D6DE File Offset: 0x0000B8DE
		public bool Contains(T item)
		{
			return this.objects.Contains(item);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000D6EC File Offset: 0x0000B8EC
		public void CopyTo(T[] array, int index)
		{
			this.objects.CopyTo(array, index);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000D6FB File Offset: 0x0000B8FB
		public bool Remove(T item)
		{
			if (this.objects.Remove(item))
			{
				this.AddOperation(SyncSet<T>.Operation.OP_REMOVE, item, true);
				return true;
			}
			return false;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000D717 File Offset: 0x0000B917
		public IEnumerator<T> GetEnumerator()
		{
			return this.objects.GetEnumerator();
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000D724 File Offset: 0x0000B924
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D72C File Offset: 0x0000B92C
		public void ExceptWith(IEnumerable<T> other)
		{
			if (other == this)
			{
				this.Clear();
				return;
			}
			foreach (T item in other)
			{
				this.Remove(item);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000D780 File Offset: 0x0000B980
		public void IntersectWith(IEnumerable<T> other)
		{
			ISet<T> set = other as ISet<T>;
			if (set != null)
			{
				this.IntersectWithSet(set);
				return;
			}
			HashSet<T> otherSet = new HashSet<T>(other);
			this.IntersectWithSet(otherSet);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		private void IntersectWithSet(ISet<T> otherSet)
		{
			foreach (T item in new List<T>(this.objects))
			{
				if (!otherSet.Contains(item))
				{
					this.Remove(item);
				}
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D814 File Offset: 0x0000BA14
		public bool IsProperSubsetOf(IEnumerable<T> other)
		{
			return this.objects.IsProperSubsetOf(other);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000D822 File Offset: 0x0000BA22
		public bool IsProperSupersetOf(IEnumerable<T> other)
		{
			return this.objects.IsProperSupersetOf(other);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D830 File Offset: 0x0000BA30
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			return this.objects.IsSubsetOf(other);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D83E File Offset: 0x0000BA3E
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			return this.objects.IsSupersetOf(other);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D84C File Offset: 0x0000BA4C
		public bool Overlaps(IEnumerable<T> other)
		{
			return this.objects.Overlaps(other);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000D85A File Offset: 0x0000BA5A
		public bool SetEquals(IEnumerable<T> other)
		{
			return this.objects.SetEquals(other);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000D868 File Offset: 0x0000BA68
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other == this)
			{
				this.Clear();
				return;
			}
			foreach (T item in other)
			{
				if (!this.Remove(item))
				{
					this.Add(item);
				}
			}
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000D8C8 File Offset: 0x0000BAC8
		public void UnionWith(IEnumerable<T> other)
		{
			if (other != this)
			{
				foreach (T item in other)
				{
					this.Add(item);
				}
			}
		}

		// Token: 0x04000166 RID: 358
		protected readonly ISet<T> objects;

		// Token: 0x04000168 RID: 360
		private readonly List<SyncSet<T>.Change> changes = new List<SyncSet<T>.Change>();

		// Token: 0x04000169 RID: 361
		private int changesAhead;

		// Token: 0x02000079 RID: 121
		// (Invoke) Token: 0x060003AF RID: 943
		public delegate void SyncSetChanged(SyncSet<T>.Operation op, T item);

		// Token: 0x0200007A RID: 122
		public enum Operation : byte
		{
			// Token: 0x0400016B RID: 363
			OP_ADD,
			// Token: 0x0400016C RID: 364
			OP_CLEAR,
			// Token: 0x0400016D RID: 365
			OP_REMOVE
		}

		// Token: 0x0200007B RID: 123
		private struct Change
		{
			// Token: 0x0400016E RID: 366
			internal SyncSet<T>.Operation operation;

			// Token: 0x0400016F RID: 367
			internal T item;
		}
	}
}
