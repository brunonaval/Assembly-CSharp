﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.IEnumConnectionPoints" /> instead.</summary>
	// Token: 0x02000758 RID: 1880
	[Guid("b196b285-bab4-101a-b69c-00aa00341d07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete]
	[ComImport]
	public interface UCOMIEnumConnectionPoints
	{
		/// <summary>Retrieves a specified number of items in the enumeration sequence.</summary>
		/// <param name="celt">The number of <see langword="IConnectionPoint" /> references to return in <paramref name="rgelt" />.</param>
		/// <param name="rgelt">On successful return, a reference to the enumerated connections.</param>
		/// <param name="pceltFetched">On successful return, a reference to the actual number of connections enumerated in <paramref name="rgelt" />.</param>
		/// <returns>
		///   <see langword="S_OK" /> if the <paramref name="pceltFetched" /> parameter equals the <paramref name="celt" /> parameter; otherwise, <see langword="S_FALSE" />.</returns>
		// Token: 0x0600425A RID: 16986
		[PreserveSig]
		int Next(int celt, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [Out] UCOMIConnectionPoint[] rgelt, out int pceltFetched);

		/// <summary>Skips over a specified number of items in the enumeration sequence.</summary>
		/// <param name="celt">The number of elements to skip in the enumeration.</param>
		/// <returns>
		///   <see langword="S_OK" /> if the number of elements skipped equals the <paramref name="celt" /> parameter; otherwise, <see langword="S_FALSE" />.</returns>
		// Token: 0x0600425B RID: 16987
		[PreserveSig]
		int Skip(int celt);

		/// <summary>Resets the enumeration sequence to the beginning.</summary>
		/// <returns>An HRESULT with the value <see langword="S_OK" />.</returns>
		// Token: 0x0600425C RID: 16988
		[PreserveSig]
		int Reset();

		/// <summary>Creates another enumerator that contains the same enumeration state as the current one.</summary>
		/// <param name="ppenum">On successful return, a reference to the newly created enumerator.</param>
		// Token: 0x0600425D RID: 16989
		void Clone(out UCOMIEnumConnectionPoints ppenum);
	}
}
