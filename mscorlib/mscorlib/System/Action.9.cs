﻿using System;

namespace System
{
	/// <summary>Encapsulates a method that has eight parameters and does not return a value.</summary>
	/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param>
	/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param>
	/// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T3">The type of the third parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T4">The type of the fourth parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T5">The type of the fifth parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T6">The type of the sixth parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T7">The type of the seventh parameter of the method that this delegate encapsulates.</typeparam>
	/// <typeparam name="T8">The type of the eighth parameter of the method that this delegate encapsulates.</typeparam>
	// Token: 0x020000E4 RID: 228
	// (Invoke) Token: 0x060006B3 RID: 1715
	public delegate void Action<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
}
