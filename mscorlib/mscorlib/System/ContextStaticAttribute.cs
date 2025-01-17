﻿using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Indicates that the value of a static field is unique for a particular context.</summary>
	// Token: 0x020001EF RID: 495
	[AttributeUsage(AttributeTargets.Field, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public class ContextStaticAttribute : Attribute
	{
	}
}
