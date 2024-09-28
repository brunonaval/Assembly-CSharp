﻿using System;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x020005E8 RID: 1512
	internal class SoapHelper
	{
		// Token: 0x06003963 RID: 14691 RVA: 0x000CB96B File Offset: 0x000C9B6B
		public static Exception GetException(ISoapXsd type, string msg)
		{
			return new RemotingException("Soap Parse error, xsd:type xsd:" + type.GetXsdType() + " " + msg);
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0000270D File Offset: 0x0000090D
		public static string Normalize(string s)
		{
			return s;
		}
	}
}
