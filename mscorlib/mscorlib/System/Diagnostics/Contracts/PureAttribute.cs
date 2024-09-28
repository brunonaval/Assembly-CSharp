﻿using System;

namespace System.Diagnostics.Contracts
{
	/// <summary>Indicates that a type or method is pure, that is, it does not make any visible state changes.</summary>
	// Token: 0x020009C4 RID: 2500
	[Conditional("CONTRACTS_FULL")]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = false, Inherited = true)]
	public sealed class PureAttribute : Attribute
	{
	}
}
