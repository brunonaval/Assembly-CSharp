﻿using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Messaging
{
	/// <summary>Marks a method as one way, without a return value and <see langword="out" /> or <see langword="ref" /> parameters.</summary>
	// Token: 0x02000631 RID: 1585
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method)]
	public class OneWayAttribute : Attribute
	{
	}
}
