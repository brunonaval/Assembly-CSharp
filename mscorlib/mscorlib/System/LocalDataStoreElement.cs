﻿using System;

namespace System
{
	// Token: 0x020001E6 RID: 486
	internal sealed class LocalDataStoreElement
	{
		// Token: 0x060014CB RID: 5323 RVA: 0x00051E24 File Offset: 0x00050024
		public LocalDataStoreElement(long cookie)
		{
			this.m_cookie = cookie;
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060014CC RID: 5324 RVA: 0x00051E33 File Offset: 0x00050033
		// (set) Token: 0x060014CD RID: 5325 RVA: 0x00051E3B File Offset: 0x0005003B
		public object Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060014CE RID: 5326 RVA: 0x00051E44 File Offset: 0x00050044
		public long Cookie
		{
			get
			{
				return this.m_cookie;
			}
		}

		// Token: 0x040014E5 RID: 5349
		private object m_value;

		// Token: 0x040014E6 RID: 5350
		private long m_cookie;
	}
}
