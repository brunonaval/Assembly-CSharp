﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.IEnumString" /> instead.</summary>
	// Token: 0x0200075A RID: 1882
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000101-0000-0000-c000-000000000046")]
	[Obsolete]
	[ComImport]
	public interface UCOMIEnumString
	{
		/// <summary>Retrieves a specified number of items in the enumeration sequence.</summary>
		/// <param name="celt">The number of strings to return in <paramref name="rgelt" />.</param>
		/// <param name="rgelt">On successful return, a reference to the enumerated strings.</param>
		/// <param name="pceltFetched">On successful return, a reference to the actual number of strings enumerated in <paramref name="rgelt" />.</param>
		/// <returns>
		///   <see langword="S_OK" /> if the <paramref name="pceltFetched" /> parameter equals the <paramref name="celt" /> parameter; otherwise, <see langword="S_FALSE" />.</returns>
		// Token: 0x06004262 RID: 16994
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr, SizeParamIndex = 0)] [Out] string[] rgelt, out int pceltFetched);

		/// <summary>Skips over a specified number of items in the enumeration sequence.</summary>
		/// <param name="celt">The number of elements to skip in the enumeration.</param>
		/// <returns>
		///   <see langword="S_OK" /> if the number of elements skipped equals the <paramref name="celt" /> parameter; otherwise, <see langword="S_FALSE" />.</returns>
		// Token: 0x06004263 RID: 16995
		[PreserveSig]
		int Skip(int celt);

		/// <summary>Resets the enumeration sequence to the beginning.</summary>
		/// <returns>An HRESULT with the value <see langword="S_OK" />.</returns>
		// Token: 0x06004264 RID: 16996
		[PreserveSig]
		int Reset();

		/// <summary>Creates another enumerator that contains the same enumeration state as the current one.</summary>
		/// <param name="ppenum">On successful return, a reference to the newly created enumerator.</param>
		// Token: 0x06004265 RID: 16997
		void Clone(out UCOMIEnumString ppenum);
	}
}