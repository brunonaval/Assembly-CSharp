﻿using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Provides the managed definition of the <see langword="IBindCtx" /> interface.</summary>
	// Token: 0x0200079F RID: 1951
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0000000e-0000-0000-C000-000000000046")]
	[ComImport]
	public interface IBindCtx
	{
		/// <summary>Registers the passed object as one of the objects that has been bound during a moniker operation and that should be released when the operation is complete.</summary>
		/// <param name="punk">The object to register for release.</param>
		// Token: 0x060044FB RID: 17659
		void RegisterObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		/// <summary>Removes the object from the set of registered objects that need to be released.</summary>
		/// <param name="punk">The object to unregister for release.</param>
		// Token: 0x060044FC RID: 17660
		void RevokeObjectBound([MarshalAs(UnmanagedType.Interface)] object punk);

		/// <summary>Releases all the objects currently registered with the bind context by using the <see cref="M:System.Runtime.InteropServices.ComTypes.IBindCtx.RegisterObjectBound(System.Object)" /> method.</summary>
		// Token: 0x060044FD RID: 17661
		void ReleaseBoundObjects();

		/// <summary>Stores a block of parameters in the bind context. These parameters will apply to later <see langword="UCOMIMoniker" /> operations that use this bind context.</summary>
		/// <param name="pbindopts">The structure containing the binding options to set.</param>
		// Token: 0x060044FE RID: 17662
		void SetBindOptions([In] ref BIND_OPTS pbindopts);

		/// <summary>Returns the current binding options stored in the current bind context.</summary>
		/// <param name="pbindopts">A pointer to the structure to receive the binding options.</param>
		// Token: 0x060044FF RID: 17663
		void GetBindOptions(ref BIND_OPTS pbindopts);

		/// <summary>Returns access to the Running Object Table (ROT) relevant to this binding process.</summary>
		/// <param name="pprot">When this method returns, contains a reference to the Running Object Table (ROT). This parameter is passed uninitialized.</param>
		// Token: 0x06004500 RID: 17664
		void GetRunningObjectTable(out IRunningObjectTable pprot);

		/// <summary>Registers the specified object pointer under the specified name in the internally maintained table of object pointers.</summary>
		/// <param name="pszKey">The name to register <paramref name="punk" /> with.</param>
		/// <param name="punk">The object to register.</param>
		// Token: 0x06004501 RID: 17665
		void RegisterObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] object punk);

		/// <summary>Looks up the given key in the internally maintained table of contextual object parameters and returns the corresponding object, if one exists.</summary>
		/// <param name="pszKey">The name of the object to search for.</param>
		/// <param name="ppunk">When this method returns, contains the object interface pointer. This parameter is passed uninitialized.</param>
		// Token: 0x06004502 RID: 17666
		void GetObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey, [MarshalAs(UnmanagedType.Interface)] out object ppunk);

		/// <summary>Enumerates the strings that are the keys of the internally maintained table of contextual object parameters.</summary>
		/// <param name="ppenum">When this method returns, contains a reference to the object parameter enumerator. This parameter is passed uninitialized.</param>
		// Token: 0x06004503 RID: 17667
		void EnumObjectParam(out IEnumString ppenum);

		/// <summary>Revokes the registration of the object currently found under the specified key in the internally maintained table of contextual object parameters, if that key is currently registered.</summary>
		/// <param name="pszKey">The key to unregister.</param>
		/// <returns>An <see langword="S_OK" /><see langword="HRESULT" /> value if the specified key was successfully removed from the table; otherwise, an <see langword="S_FALSE" /><see langword="HRESULT" /> value.</returns>
		// Token: 0x06004504 RID: 17668
		[PreserveSig]
		int RevokeObjectParam([MarshalAs(UnmanagedType.LPWStr)] string pszKey);
	}
}