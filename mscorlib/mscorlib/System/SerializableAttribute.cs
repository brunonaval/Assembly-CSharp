﻿using System;

namespace System
{
	/// <summary>Indicates that a class can be serialized. This class cannot be inherited.</summary>
	// Token: 0x0200017B RID: 379
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate, Inherited = false)]
	public sealed class SerializableAttribute : Attribute
	{
	}
}
