﻿using System;

namespace System
{
	/// <summary>Encapsulates a method that has two parameters and does not return a value.</summary>
	/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
	/// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
	// Token: 0x020000DE RID: 222
	// (Invoke) Token: 0x0600069B RID: 1691
	public delegate void Action<in T1, in T2>(T1 arg1, T2 arg2);
}
