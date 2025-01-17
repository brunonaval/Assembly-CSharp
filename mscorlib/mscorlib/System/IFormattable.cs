﻿using System;

namespace System
{
	/// <summary>Provides functionality to format the value of an object into a string representation.</summary>
	// Token: 0x02000140 RID: 320
	public interface IFormattable
	{
		/// <summary>Formats the value of the current instance using the specified format.</summary>
		/// <param name="format">The format to use.  
		///  -or-  
		///  A null reference (<see langword="Nothing" /> in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable" /> implementation.</param>
		/// <param name="formatProvider">The provider to use to format the value.  
		///  -or-  
		///  A null reference (<see langword="Nothing" /> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
		/// <returns>The value of the current instance in the specified format.</returns>
		// Token: 0x06000C00 RID: 3072
		string ToString(string format, IFormatProvider formatProvider);
	}
}
