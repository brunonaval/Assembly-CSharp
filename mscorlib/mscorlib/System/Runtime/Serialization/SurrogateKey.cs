﻿using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000676 RID: 1654
	[Serializable]
	internal class SurrogateKey
	{
		// Token: 0x06003DC2 RID: 15810 RVA: 0x000D58F1 File Offset: 0x000D3AF1
		internal SurrogateKey(Type type, StreamingContext context)
		{
			this.m_type = type;
			this.m_context = context;
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x000D5907 File Offset: 0x000D3B07
		public override int GetHashCode()
		{
			return this.m_type.GetHashCode();
		}

		// Token: 0x040027A3 RID: 10147
		internal Type m_type;

		// Token: 0x040027A4 RID: 10148
		internal StreamingContext m_context;
	}
}
