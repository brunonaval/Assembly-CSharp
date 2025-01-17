﻿using System;

namespace System.Security.Principal
{
	/// <summary>Defines security impersonation levels. Security impersonation levels govern the degree to which a server process can act on behalf of a client process.</summary>
	// Token: 0x020004E1 RID: 1249
	public enum TokenImpersonationLevel
	{
		/// <summary>An impersonation level is not assigned.</summary>
		// Token: 0x040022B8 RID: 8888
		None,
		/// <summary>The server process cannot obtain identification information about the client, and it cannot impersonate the client.</summary>
		// Token: 0x040022B9 RID: 8889
		Anonymous,
		/// <summary>The server process can obtain information about the client, such as security identifiers and privileges, but it cannot impersonate the client. This is useful for servers that export their own objects, for example, database products that export tables and views. Using the retrieved client-security information, the server can make access-validation decisions without being able to use other services that are using the client's security context.</summary>
		// Token: 0x040022BA RID: 8890
		Identification,
		/// <summary>The server process can impersonate the client's security context on its local system. The server cannot impersonate the client on remote systems.</summary>
		// Token: 0x040022BB RID: 8891
		Impersonation,
		/// <summary>The server process can impersonate the client's security context on remote systems.</summary>
		// Token: 0x040022BC RID: 8892
		Delegation
	}
}
