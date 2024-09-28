﻿using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	/// <summary>Instructs the debugger to step through the code instead of stepping into the code. This class cannot be inherited.</summary>
	// Token: 0x020009B4 RID: 2484
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class DebuggerStepThroughAttribute : Attribute
	{
	}
}
