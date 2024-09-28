﻿using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	/// <summary>Provides a fast way to swap method body implementation given a method of a class.</summary>
	// Token: 0x02000936 RID: 2358
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodRental))]
	public sealed class MethodRental : _MethodRental
	{
		// Token: 0x0600516A RID: 20842 RVA: 0x0000259F File Offset: 0x0000079F
		private MethodRental()
		{
		}

		/// <summary>Swaps the body of a method.</summary>
		/// <param name="cls">The class containing the method.</param>
		/// <param name="methodtoken">The token for the method.</param>
		/// <param name="rgIL">A pointer to the method. This should include the method header.</param>
		/// <param name="methodSize">The size of the new method body in bytes.</param>
		/// <param name="flags">Flags that control the swapping. See the definitions of the constants.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="cls" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The type <paramref name="cls" /> is not complete.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="methodSize" /> is less than one or greater than 4128767 (3effff hex).</exception>
		// Token: 0x0600516B RID: 20843 RVA: 0x000FE8D4 File Offset: 0x000FCAD4
		[MonoTODO]
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public static void SwapMethodBody(Type cls, int methodtoken, IntPtr rgIL, int methodSize, int flags)
		{
			if (methodSize <= 0 || methodSize >= 4128768)
			{
				throw new ArgumentException("Data size must be > 0 and < 0x3f0000", "methodSize");
			}
			if (cls == null)
			{
				throw new ArgumentNullException("cls");
			}
			if (cls is TypeBuilder && !((TypeBuilder)cls).is_created)
			{
				throw new NotSupportedException("Type '" + ((cls != null) ? cls.ToString() : null) + "' is not yet created.");
			}
			throw new NotImplementedException();
		}

		/// <summary>Maps a set of names to a corresponding set of dispatch identifiers.</summary>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="rgszNames">Passed-in array of names to be mapped.</param>
		/// <param name="cNames">Count of the names to be mapped.</param>
		/// <param name="lcid">The locale context in which to interpret the names.</param>
		/// <param name="rgDispId">Caller-allocated array which receives the IDs corresponding to the names.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		// Token: 0x0600516C RID: 20844 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the type information for an object, which can then be used to get the type information for an interface.</summary>
		/// <param name="iTInfo">The type information to return.</param>
		/// <param name="lcid">The locale identifier for the type information.</param>
		/// <param name="ppTInfo">Receives a pointer to the requested type information object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		// Token: 0x0600516D RID: 20845 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Retrieves the number of type information interfaces that an object provides (either 0 or 1).</summary>
		/// <param name="pcTInfo">Points to a location that receives the number of type information interfaces provided by the object.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		// Token: 0x0600516E RID: 20846 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Provides access to properties and methods exposed by an object.</summary>
		/// <param name="dispIdMember">Identifies the member.</param>
		/// <param name="riid">Reserved for future use. Must be IID_NULL.</param>
		/// <param name="lcid">The locale context in which to interpret arguments.</param>
		/// <param name="wFlags">Flags describing the context of the call.</param>
		/// <param name="pDispParams">Pointer to a structure containing an array of arguments, an array of argument DISPIDs for named arguments, and counts for the number of elements in the arrays.</param>
		/// <param name="pVarResult">Pointer to the location where the result is to be stored.</param>
		/// <param name="pExcepInfo">Pointer to a structure that contains exception information.</param>
		/// <param name="puArgErr">The index of the first argument that has an error.</param>
		/// <exception cref="T:System.NotImplementedException">The method is called late-bound using the COM IDispatch interface.</exception>
		// Token: 0x0600516F RID: 20847 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodRental.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		/// <summary>Specifies that the method should be just-in-time (JIT) compiled immediately.</summary>
		// Token: 0x040031D6 RID: 12758
		public const int JitImmediate = 1;

		/// <summary>Specifies that the method should be just-in-time (JIT) compiled when needed.</summary>
		// Token: 0x040031D7 RID: 12759
		public const int JitOnDemand = 0;
	}
}
