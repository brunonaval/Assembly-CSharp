﻿using System;
using System.Collections;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020005A3 RID: 1443
	internal class AggregateEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x0600380D RID: 14349 RVA: 0x000C9382 File Offset: 0x000C7582
		public AggregateEnumerator(IDictionary[] dics)
		{
			this.dictionaries = dics;
			this.Reset();
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600380E RID: 14350 RVA: 0x000C9397 File Offset: 0x000C7597
		public DictionaryEntry Entry
		{
			get
			{
				return this.currente.Entry;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x0600380F RID: 14351 RVA: 0x000C93A4 File Offset: 0x000C75A4
		public object Key
		{
			get
			{
				return this.currente.Key;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06003810 RID: 14352 RVA: 0x000C93B1 File Offset: 0x000C75B1
		public object Value
		{
			get
			{
				return this.currente.Value;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06003811 RID: 14353 RVA: 0x000C93BE File Offset: 0x000C75BE
		public object Current
		{
			get
			{
				return this.currente.Current;
			}
		}

		// Token: 0x06003812 RID: 14354 RVA: 0x000C93CC File Offset: 0x000C75CC
		public bool MoveNext()
		{
			if (this.pos >= this.dictionaries.Length)
			{
				return false;
			}
			if (this.currente.MoveNext())
			{
				return true;
			}
			this.pos++;
			if (this.pos >= this.dictionaries.Length)
			{
				return false;
			}
			this.currente = this.dictionaries[this.pos].GetEnumerator();
			return this.MoveNext();
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x000C9438 File Offset: 0x000C7638
		public void Reset()
		{
			this.pos = 0;
			if (this.dictionaries.Length != 0)
			{
				this.currente = this.dictionaries[0].GetEnumerator();
			}
		}

		// Token: 0x040025CA RID: 9674
		private IDictionary[] dictionaries;

		// Token: 0x040025CB RID: 9675
		private int pos;

		// Token: 0x040025CC RID: 9676
		private IDictionaryEnumerator currente;
	}
}
