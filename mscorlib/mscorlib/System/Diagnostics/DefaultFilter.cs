﻿using System;
using System.Security;

namespace System.Diagnostics
{
	// Token: 0x020009B2 RID: 2482
	internal class DefaultFilter : AssertFilter
	{
		// Token: 0x06005997 RID: 22935 RVA: 0x00132F4F File Offset: 0x0013114F
		internal DefaultFilter()
		{
		}

		// Token: 0x06005998 RID: 22936 RVA: 0x00132F57 File Offset: 0x00131157
		[SecuritySafeCritical]
		public override AssertFilters AssertFailure(string condition, string message, StackTrace location, StackTrace.TraceFormat stackTraceFormat, string windowTitle)
		{
			return (AssertFilters)Assert.ShowDefaultAssertDialog(condition, message, location.ToString(stackTraceFormat), windowTitle);
		}
	}
}
