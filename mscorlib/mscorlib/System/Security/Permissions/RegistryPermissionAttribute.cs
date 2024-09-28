﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.RegistryPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	// Token: 0x02000454 RID: 1108
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class RegistryPermissionAttribute : CodeAccessSecurityAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.RegistryPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="action" /> parameter is not a valid <see cref="T:System.Security.Permissions.SecurityAction" />.</exception>
		// Token: 0x06002CFD RID: 11517 RVA: 0x0009DCD0 File Offset: 0x0009BED0
		public RegistryPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		/// <summary>Gets or sets full access for the specified registry keys.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for full access.</returns>
		/// <exception cref="T:System.NotSupportedException">The get accessor is called; it is only provided for C# compiler compatibility.</exception>
		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x0009DCD9 File Offset: 0x0009BED9
		// (set) Token: 0x06002CFF RID: 11519 RVA: 0x000A1A10 File Offset: 0x0009FC10
		[Obsolete("use newer properties")]
		public string All
		{
			get
			{
				throw new NotSupportedException("All");
			}
			set
			{
				this.create = value;
				this.read = value;
				this.write = value;
			}
		}

		/// <summary>Gets or sets create-level access for the specified registry keys.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for create-level access.</returns>
		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06002D00 RID: 11520 RVA: 0x000A1A27 File Offset: 0x0009FC27
		// (set) Token: 0x06002D01 RID: 11521 RVA: 0x000A1A2F File Offset: 0x0009FC2F
		public string Create
		{
			get
			{
				return this.create;
			}
			set
			{
				this.create = value;
			}
		}

		/// <summary>Gets or sets read access for the specified registry keys.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for read access.</returns>
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06002D02 RID: 11522 RVA: 0x000A1A38 File Offset: 0x0009FC38
		// (set) Token: 0x06002D03 RID: 11523 RVA: 0x000A1A40 File Offset: 0x0009FC40
		public string Read
		{
			get
			{
				return this.read;
			}
			set
			{
				this.read = value;
			}
		}

		/// <summary>Gets or sets write access for the specified registry keys.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for write access.</returns>
		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x000A1A49 File Offset: 0x0009FC49
		// (set) Token: 0x06002D05 RID: 11525 RVA: 0x000A1A51 File Offset: 0x0009FC51
		public string Write
		{
			get
			{
				return this.write;
			}
			set
			{
				this.write = value;
			}
		}

		/// <summary>Gets or sets change access control for the specified registry keys.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for change access control. .</returns>
		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06002D06 RID: 11526 RVA: 0x000A1A5A File Offset: 0x0009FC5A
		// (set) Token: 0x06002D07 RID: 11527 RVA: 0x000A1A62 File Offset: 0x0009FC62
		public string ChangeAccessControl
		{
			get
			{
				return this.changeAccessControl;
			}
			set
			{
				this.changeAccessControl = value;
			}
		}

		/// <summary>Gets or sets view access control for the specified registry keys.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for view access control.</returns>
		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002D08 RID: 11528 RVA: 0x000A1A6B File Offset: 0x0009FC6B
		// (set) Token: 0x06002D09 RID: 11529 RVA: 0x000A1A73 File Offset: 0x0009FC73
		public string ViewAccessControl
		{
			get
			{
				return this.viewAccessControl;
			}
			set
			{
				this.viewAccessControl = value;
			}
		}

		/// <summary>Gets or sets a specified set of registry keys that can be viewed and modified.</summary>
		/// <returns>A semicolon-separated list of registry key paths, for create, read, and write access.</returns>
		/// <exception cref="T:System.NotSupportedException">The get accessor is called; it is only provided for C# compiler compatibility.</exception>
		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06002D0A RID: 11530 RVA: 0x000472CC File Offset: 0x000454CC
		// (set) Token: 0x06002D0B RID: 11531 RVA: 0x000A1A10 File Offset: 0x0009FC10
		public string ViewAndModify
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				this.create = value;
				this.read = value;
				this.write = value;
			}
		}

		/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.RegistryPermission" />.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.RegistryPermission" /> that corresponds to this attribute.</returns>
		// Token: 0x06002D0C RID: 11532 RVA: 0x000A1A7C File Offset: 0x0009FC7C
		public override IPermission CreatePermission()
		{
			RegistryPermission registryPermission;
			if (base.Unrestricted)
			{
				registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			}
			else
			{
				registryPermission = new RegistryPermission(PermissionState.None);
				if (this.create != null)
				{
					registryPermission.AddPathList(RegistryPermissionAccess.Create, this.create);
				}
				if (this.read != null)
				{
					registryPermission.AddPathList(RegistryPermissionAccess.Read, this.read);
				}
				if (this.write != null)
				{
					registryPermission.AddPathList(RegistryPermissionAccess.Write, this.write);
				}
			}
			return registryPermission;
		}

		// Token: 0x04002084 RID: 8324
		private string create;

		// Token: 0x04002085 RID: 8325
		private string read;

		// Token: 0x04002086 RID: 8326
		private string write;

		// Token: 0x04002087 RID: 8327
		private string changeAccessControl;

		// Token: 0x04002088 RID: 8328
		private string viewAccessControl;
	}
}
