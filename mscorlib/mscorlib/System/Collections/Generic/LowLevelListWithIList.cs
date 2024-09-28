using System;

namespace System.Collections.Generic
{
	// Token: 0x02000AB9 RID: 2745
	internal sealed class LowLevelListWithIList<T> : LowLevelList<T>, IList<!0>, ICollection<!0>, IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06006234 RID: 25140 RVA: 0x00148773 File Offset: 0x00146973
		public LowLevelListWithIList()
		{
		}

		// Token: 0x06006235 RID: 25141 RVA: 0x0014877B File Offset: 0x0014697B
		public LowLevelListWithIList(int capacity) : base(capacity)
		{
		}

		// Token: 0x06006236 RID: 25142 RVA: 0x00148784 File Offset: 0x00146984
		public LowLevelListWithIList(IEnumerable<T> collection) : base(collection)
		{
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x06006237 RID: 25143 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x0014878D File Offset: 0x0014698D
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return new LowLevelListWithIList<T>.Enumerator(this);
		}

		// Token: 0x06006239 RID: 25145 RVA: 0x0014878D File Offset: 0x0014698D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new LowLevelListWithIList<T>.Enumerator(this);
		}

		// Token: 0x02000ABA RID: 2746
		private struct Enumerator : IEnumerator<!0>, IDisposable, IEnumerator
		{
			// Token: 0x0600623A RID: 25146 RVA: 0x0014879A File Offset: 0x0014699A
			internal Enumerator(LowLevelListWithIList<T> list)
			{
				this._list = list;
				this._index = 0;
				this._version = list._version;
				this._current = default(T);
			}

			// Token: 0x0600623B RID: 25147 RVA: 0x00004BF9 File Offset: 0x00002DF9
			public void Dispose()
			{
			}

			// Token: 0x0600623C RID: 25148 RVA: 0x001487C4 File Offset: 0x001469C4
			public bool MoveNext()
			{
				LowLevelListWithIList<T> list = this._list;
				if (this._version == list._version && this._index < list._size)
				{
					this._current = list._items[this._index];
					this._index++;
					return true;
				}
				return this.MoveNextRare();
			}

			// Token: 0x0600623D RID: 25149 RVA: 0x00148821 File Offset: 0x00146A21
			private bool MoveNextRare()
			{
				if (this._version != this._list._version)
				{
					throw new InvalidOperationException();
				}
				this._index = this._list._size + 1;
				this._current = default(T);
				return false;
			}

			// Token: 0x17001177 RID: 4471
			// (get) Token: 0x0600623E RID: 25150 RVA: 0x0014885C File Offset: 0x00146A5C
			public T Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x17001178 RID: 4472
			// (get) Token: 0x0600623F RID: 25151 RVA: 0x00148864 File Offset: 0x00146A64
			object IEnumerator.Current
			{
				get
				{
					if (this._index == 0 || this._index == this._list._size + 1)
					{
						throw new InvalidOperationException();
					}
					return this.Current;
				}
			}

			// Token: 0x06006240 RID: 25152 RVA: 0x00148894 File Offset: 0x00146A94
			void IEnumerator.Reset()
			{
				if (this._version != this._list._version)
				{
					throw new InvalidOperationException();
				}
				this._index = 0;
				this._current = default(T);
			}

			// Token: 0x04003A2D RID: 14893
			private LowLevelListWithIList<T> _list;

			// Token: 0x04003A2E RID: 14894
			private int _index;

			// Token: 0x04003A2F RID: 14895
			private int _version;

			// Token: 0x04003A30 RID: 14896
			private T _current;
		}
	}
}
