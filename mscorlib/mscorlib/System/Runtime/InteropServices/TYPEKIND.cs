﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.TYPEKIND" /> instead.</summary>
	// Token: 0x02000721 RID: 1825
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEKIND instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	public enum TYPEKIND
	{
		/// <summary>A set of enumerators.</summary>
		// Token: 0x04002B0E RID: 11022
		TKIND_ENUM,
		/// <summary>A structure with no methods.</summary>
		// Token: 0x04002B0F RID: 11023
		TKIND_RECORD,
		/// <summary>A module that can only have static functions and data (for example, a DLL).</summary>
		// Token: 0x04002B10 RID: 11024
		TKIND_MODULE,
		/// <summary>A type that has virtual functions, all of which are pure.</summary>
		// Token: 0x04002B11 RID: 11025
		TKIND_INTERFACE,
		/// <summary>A set of methods and properties that are accessible through <see langword="IDispatch::Invoke" />. By default, dual interfaces return <see langword="TKIND_DISPATCH" />.</summary>
		// Token: 0x04002B12 RID: 11026
		TKIND_DISPATCH,
		/// <summary>A set of implemented components interfaces.</summary>
		// Token: 0x04002B13 RID: 11027
		TKIND_COCLASS,
		/// <summary>A type that is an alias for another type.</summary>
		// Token: 0x04002B14 RID: 11028
		TKIND_ALIAS,
		/// <summary>A union of all members that have an offset of zero.</summary>
		// Token: 0x04002B15 RID: 11029
		TKIND_UNION,
		/// <summary>End of enumeration marker.</summary>
		// Token: 0x04002B16 RID: 11030
		TKIND_MAX
	}
}
