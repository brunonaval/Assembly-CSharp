﻿using System;

namespace System.Runtime.InteropServices.ComTypes
{
	/// <summary>Provides the managed definition of the <see langword="IConnectionPoint" /> interface.</summary>
	// Token: 0x0200079C RID: 1948
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B196B286-BAB4-101A-B69C-00AA00341D07")]
	[ComImport]
	public interface IConnectionPoint
	{
		/// <summary>Returns the IID of the outgoing interface managed by this connection point.</summary>
		/// <param name="pIID">When this parameter returns, contains the IID of the outgoing interface managed by this connection point. This parameter is passed uninitialized.</param>
		// Token: 0x060044F4 RID: 17652
		void GetConnectionInterface(out Guid pIID);

		/// <summary>Retrieves the <see langword="IConnectionPointContainer" /> interface pointer to the connectable object that conceptually owns this connection point.</summary>
		/// <param name="ppCPC">When this parameter returns, contains the connectable object's <see langword="IConnectionPointContainer" /> interface. This parameter is passed uninitialized.</param>
		// Token: 0x060044F5 RID: 17653
		void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);

		/// <summary>Establishes an advisory connection between the connection point and the caller's sink object.</summary>
		/// <param name="pUnkSink">A reference to the sink to receive calls for the outgoing interface managed by this connection point.</param>
		/// <param name="pdwCookie">When this method returns, contains the connection cookie. This parameter is passed uninitialized.</param>
		// Token: 0x060044F6 RID: 17654
		void Advise([MarshalAs(UnmanagedType.Interface)] object pUnkSink, out int pdwCookie);

		/// <summary>Terminates an advisory connection previously established through the <see cref="M:System.Runtime.InteropServices.ComTypes.IConnectionPoint.Advise(System.Object,System.Int32@)" /> method.</summary>
		/// <param name="dwCookie">The connection cookie previously returned from the <see cref="M:System.Runtime.InteropServices.ComTypes.IConnectionPoint.Advise(System.Object,System.Int32@)" /> method.</param>
		// Token: 0x060044F7 RID: 17655
		void Unadvise(int dwCookie);

		/// <summary>Creates an enumerator object for iteration through the connections that exist to this connection point.</summary>
		/// <param name="ppEnum">When this method returns, contains the newly created enumerator. This parameter is passed uninitialized.</param>
		// Token: 0x060044F8 RID: 17656
		void EnumConnections(out IEnumConnections ppEnum);
	}
}
