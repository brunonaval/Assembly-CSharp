﻿using System;
using System.Text;

namespace System
{
	// Token: 0x02000261 RID: 609
	internal class PointerSpec : ModifierSpec
	{
		// Token: 0x06001BD5 RID: 7125 RVA: 0x00067C80 File Offset: 0x00065E80
		internal PointerSpec(int pointer_level)
		{
			this.pointer_level = pointer_level;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x00067C90 File Offset: 0x00065E90
		public Type Resolve(Type type)
		{
			for (int i = 0; i < this.pointer_level; i++)
			{
				type = type.MakePointerType();
			}
			return type;
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x00067CB7 File Offset: 0x00065EB7
		public StringBuilder Append(StringBuilder sb)
		{
			return sb.Append('*', this.pointer_level);
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x00067CC7 File Offset: 0x00065EC7
		public override string ToString()
		{
			return this.Append(new StringBuilder()).ToString();
		}

		// Token: 0x04001995 RID: 6549
		private int pointer_level;
	}
}
