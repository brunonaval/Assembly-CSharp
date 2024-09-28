﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.IsolatedStoragePermission" /> to be applied to code using declarative security.</summary>
	// Token: 0x02000444 RID: 1092
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class IsolatedStoragePermissionAttribute : CodeAccessSecurityAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.IsolatedStoragePermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</param>
		// Token: 0x06002C45 RID: 11333 RVA: 0x0009DCD0 File Offset: 0x0009BED0
		protected IsolatedStoragePermissionAttribute(SecurityAction action) : base(action)
		{
		}

		/// <summary>Gets or sets the level of isolated storage that should be declared.</summary>
		/// <returns>One of the <see cref="T:System.Security.Permissions.IsolatedStorageContainment" /> values.</returns>
		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002C46 RID: 11334 RVA: 0x0009F5AC File Offset: 0x0009D7AC
		// (set) Token: 0x06002C47 RID: 11335 RVA: 0x0009F5B4 File Offset: 0x0009D7B4
		public IsolatedStorageContainment UsageAllowed
		{
			get
			{
				return this.usage_allowed;
			}
			set
			{
				this.usage_allowed = value;
			}
		}

		/// <summary>Gets or sets the maximum user storage quota size.</summary>
		/// <returns>The maximum user storage quota size in bytes.</returns>
		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002C48 RID: 11336 RVA: 0x0009F5BD File Offset: 0x0009D7BD
		// (set) Token: 0x06002C49 RID: 11337 RVA: 0x0009F5C5 File Offset: 0x0009D7C5
		public long UserQuota
		{
			get
			{
				return this.user_quota;
			}
			set
			{
				this.user_quota = value;
			}
		}

		// Token: 0x04002048 RID: 8264
		private IsolatedStorageContainment usage_allowed;

		// Token: 0x04002049 RID: 8265
		private long user_quota;
	}
}
