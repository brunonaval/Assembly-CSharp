﻿using System;

namespace System.Security.Permissions
{
	/// <summary>Allows a permission to expose an unrestricted state.</summary>
	// Token: 0x0200042B RID: 1067
	public interface IUnrestrictedPermission
	{
		/// <summary>Returns a value indicating whether unrestricted access to the resource protected by the permission is allowed.</summary>
		/// <returns>
		///   <see langword="true" /> if unrestricted use of the resource protected by the permission is allowed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002B98 RID: 11160
		bool IsUnrestricted();
	}
}
