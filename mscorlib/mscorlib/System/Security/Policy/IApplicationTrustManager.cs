﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Determines whether an application should be executed and which set of permissions should be granted to it.</summary>
	// Token: 0x02000414 RID: 1044
	[ComVisible(true)]
	public interface IApplicationTrustManager : ISecurityEncodable
	{
		/// <summary>Determines whether an application should be executed and which set of permissions should be granted to it.</summary>
		/// <param name="activationContext">The activation context for the application.</param>
		/// <param name="context">The trust manager context for the application.</param>
		/// <returns>An object that contains security decisions about the application.</returns>
		// Token: 0x06002ABE RID: 10942
		ApplicationTrust DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context);
	}
}
