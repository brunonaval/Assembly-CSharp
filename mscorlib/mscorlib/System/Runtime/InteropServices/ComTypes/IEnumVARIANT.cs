﻿using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Manages the definition of the <see langword="IEnumVARIANT" /> interface.</summary>
	// Token: 0x020007A7 RID: 1959
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00020404-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IEnumVARIANT
	{
		/// <summary>Retrieves a specified number of items in the enumeration sequence.</summary>
		/// <param name="celt">The number of elements to return in rgelt.</param>
		/// <param name="rgVar">When this method returns, contains a reference to the enumerated elements. This parameter is passed uninitialized.</param>
		/// <param name="pceltFetched">When this method returns, contains a reference to the actual number of elements enumerated in rgelt.</param>
		/// <returns>
		///   <see langword="S_OK" /> if the <paramref name="pceltFetched" /> parameter equals the <paramref name="celt" /> parameter; otherwise, <see langword="S_FALSE" />.</returns>
		// Token: 0x06004519 RID: 17689
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] object[] rgVar, IntPtr pceltFetched);

		/// <summary>Skips a specified number of items in the enumeration sequence.</summary>
		/// <param name="celt">The number of elements to skip in the enumeration.</param>
		/// <returns>
		///   <see langword="S_OK" /> if the number of elements skipped equals <paramref name="celt" /> parameter; otherwise, <see langword="S_FALSE" />.</returns>
		// Token: 0x0600451A RID: 17690
		[PreserveSig]
		int Skip(int celt);

		/// <summary>Resets the enumeration sequence to the beginning.</summary>
		/// <returns>An HRESULT with the value <see langword="S_OK" />.</returns>
		// Token: 0x0600451B RID: 17691
		[PreserveSig]
		int Reset();

		/// <summary>Creates a new enumerator that contains the same enumeration state as the current one.</summary>
		/// <returns>An <see cref="T:System.Runtime.InteropServices.ComTypes.IEnumVARIANT" /> reference to the newly created enumerator.</returns>
		// Token: 0x0600451C RID: 17692
		IEnumVARIANT Clone();
	}
}
