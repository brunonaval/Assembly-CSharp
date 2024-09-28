﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	/// <summary>Provides a set of services that convert a managed assembly to a COM type library and vice versa.</summary>
	// Token: 0x02000752 RID: 1874
	[Guid("f1c3bf79-c3e4-11d3-88e7-00902754c43a")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class TypeLibConverter : ITypeLibConverter
	{
		/// <summary>Converts an assembly to a COM type library.</summary>
		/// <param name="assembly">The assembly to convert.</param>
		/// <param name="strTypeLibName">The file name of the resulting type library.</param>
		/// <param name="flags">A <see cref="T:System.Runtime.InteropServices.TypeLibExporterFlags" /> value indicating any special settings.</param>
		/// <param name="notifySink">The <see cref="T:System.Runtime.InteropServices.ITypeLibExporterNotifySink" /> interface implemented by the caller.</param>
		/// <returns>An object that implements the <see langword="ITypeLib" /> interface.</returns>
		// Token: 0x06004245 RID: 16965 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecuritySafeCritical]
		[MonoTODO("implement")]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public object ConvertAssemblyToTypeLib(Assembly assembly, string strTypeLibName, TypeLibExporterFlags flags, ITypeLibExporterNotifySink notifySink)
		{
			throw new NotImplementedException();
		}

		/// <summary>Converts a COM type library to an assembly.</summary>
		/// <param name="typeLib">The object that implements the <see langword="ITypeLib" /> interface.</param>
		/// <param name="asmFileName">The file name of the resulting assembly.</param>
		/// <param name="flags">A <see cref="T:System.Runtime.InteropServices.TypeLibImporterFlags" /> value indicating any special settings.</param>
		/// <param name="notifySink">
		///   <see cref="T:System.Runtime.InteropServices.ITypeLibImporterNotifySink" /> interface implemented by the caller.</param>
		/// <param name="publicKey">A <see langword="byte" /> array containing the public key.</param>
		/// <param name="keyPair">A <see cref="T:System.Reflection.StrongNameKeyPair" /> object containing the public and private cryptographic key pair.</param>
		/// <param name="unsafeInterfaces">If <see langword="true" />, the interfaces require link time checks for <see cref="F:System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode" /> permission. If <see langword="false" />, the interfaces require run time checks that require a stack walk and are more expensive, but help provide greater protection.</param>
		/// <returns>An <see cref="T:System.Reflection.Emit.AssemblyBuilder" /> object containing the converted type library.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeLib" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="asmFileName" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="notifySink" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asmFileName" /> is an empty string.  
		/// -or-  
		/// <paramref name="asmFileName" /> is longer than the system-defined maximum length. For more information, see <see cref="T:System.IO.PathTooLongException" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="flags" /> is not <see cref="F:System.Runtime.InteropServices.TypeLibImporterFlags.PrimaryInteropAssembly" />.  
		/// -or-  
		/// <paramref name="publicKey" /> and <paramref name="keyPair" /> are <see langword="null" />.</exception>
		/// <exception cref="T:System.Reflection.ReflectionTypeLoadException">The metadata produced has errors preventing any types from loading.</exception>
		// Token: 0x06004246 RID: 16966 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		[SecuritySafeCritical]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, int flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, bool unsafeInterfaces)
		{
			throw new NotImplementedException();
		}

		/// <summary>Converts a COM type library to an assembly.</summary>
		/// <param name="typeLib">The object that implements the <see langword="ITypeLib" /> interface.</param>
		/// <param name="asmFileName">The file name of the resulting assembly.</param>
		/// <param name="flags">A <see cref="T:System.Runtime.InteropServices.TypeLibImporterFlags" /> value indicating any special settings.</param>
		/// <param name="notifySink">
		///   <see cref="T:System.Runtime.InteropServices.ITypeLibImporterNotifySink" /> interface implemented by the caller.</param>
		/// <param name="publicKey">A <see langword="byte" /> array containing the public key.</param>
		/// <param name="keyPair">A <see cref="T:System.Reflection.StrongNameKeyPair" /> object containing the public and private cryptographic key pair.</param>
		/// <param name="asmNamespace">The namespace for the resulting assembly.</param>
		/// <param name="asmVersion">The version of the resulting assembly. If <see langword="null" />, the version of the type library is used.</param>
		/// <returns>An <see cref="T:System.Reflection.Emit.AssemblyBuilder" /> object containing the converted type library.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="typeLib" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="asmFileName" /> is <see langword="null" />.  
		/// -or-  
		/// <paramref name="notifySink" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asmFileName" /> is an empty string.  
		/// -or-  
		/// <paramref name="asmFileName" /> is longer than the system-defined maximum length. For more information, see <see cref="T:System.IO.PathTooLongException" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="flags" /> is not <see cref="F:System.Runtime.InteropServices.TypeLibImporterFlags.PrimaryInteropAssembly" />.  
		/// -or-  
		/// <paramref name="publicKey" /> and <paramref name="keyPair" /> are <see langword="null" />.</exception>
		/// <exception cref="T:System.Reflection.ReflectionTypeLoadException">The metadata produced has errors preventing any types from loading.</exception>
		// Token: 0x06004247 RID: 16967 RVA: 0x000479FC File Offset: 0x00045BFC
		[SecuritySafeCritical]
		[MonoTODO("implement")]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public AssemblyBuilder ConvertTypeLibToAssembly([MarshalAs(UnmanagedType.Interface)] object typeLib, string asmFileName, TypeLibImporterFlags flags, ITypeLibImporterNotifySink notifySink, byte[] publicKey, StrongNameKeyPair keyPair, string asmNamespace, Version asmVersion)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets the name and code base of a primary interop assembly for a specified type library.</summary>
		/// <param name="g">The GUID of the type library.</param>
		/// <param name="major">The major version number of the type library.</param>
		/// <param name="minor">The minor version number of the type library.</param>
		/// <param name="lcid">The LCID of the type library.</param>
		/// <param name="asmName">On successful return, the name of the primary interop assembly associated with <paramref name="g" />.</param>
		/// <param name="asmCodeBase">On successful return, the code base of the primary interop assembly associated with <paramref name="g" />.</param>
		/// <returns>
		///   <see langword="true" /> if the primary interop assembly was found in the registry; otherwise <see langword="false" />.</returns>
		// Token: 0x06004248 RID: 16968 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("implement")]
		public bool GetPrimaryInteropAssembly(Guid g, int major, int minor, int lcid, out string asmName, out string asmCodeBase)
		{
			throw new NotImplementedException();
		}
	}
}
