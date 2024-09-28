﻿using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200091B RID: 2331
	[StructLayout(LayoutKind.Sequential)]
	internal class PointerType : SymbolType
	{
		// Token: 0x06004F5F RID: 20319 RVA: 0x000F99C5 File Offset: 0x000F7BC5
		internal PointerType(Type elementType) : base(elementType)
		{
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x000F9A16 File Offset: 0x000F7C16
		internal override Type InternalResolve()
		{
			return this.m_baseType.InternalResolve().MakePointerType();
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x000040F7 File Offset: 0x000022F7
		protected override bool IsPointerImpl()
		{
			return true;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x000F9A28 File Offset: 0x000F7C28
		internal override string FormatName(string elementName)
		{
			if (elementName == null)
			{
				return null;
			}
			return elementName + "*";
		}
	}
}
