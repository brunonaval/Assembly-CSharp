﻿using System;

namespace System
{
	/// <summary>Defines a method that supports custom formatting of the value of an object.</summary>
	// Token: 0x0200013C RID: 316
	public interface ICustomFormatter
	{
		/// <summary>Converts the value of a specified object to an equivalent string representation using specified format and culture-specific formatting information.</summary>
		/// <param name="format">A format string containing formatting specifications.</param>
		/// <param name="arg">An object to format.</param>
		/// <param name="formatProvider">An object that supplies format information about the current instance.</param>
		/// <returns>The string representation of the value of <paramref name="arg" />, formatted as specified by <paramref name="format" /> and <paramref name="formatProvider" />.</returns>
		// Token: 0x06000BFC RID: 3068
		string Format(string format, object arg, IFormatProvider formatProvider);
	}
}
