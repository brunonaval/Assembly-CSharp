﻿using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Lifetime
{
	/// <summary>Indicates that the implementer wants to be a lifetime lease sponsor.</summary>
	// Token: 0x02000584 RID: 1412
	[ComVisible(true)]
	public interface ISponsor
	{
		/// <summary>Requests a sponsoring client to renew the lease for the specified object.</summary>
		/// <param name="lease">The lifetime lease of the object that requires lease renewal.</param>
		/// <returns>The additional lease time for the specified object.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission.</exception>
		// Token: 0x0600375C RID: 14172
		TimeSpan Renewal(ILease lease);
	}
}
