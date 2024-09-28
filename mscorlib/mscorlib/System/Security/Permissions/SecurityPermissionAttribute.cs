﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.SecurityPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	// Token: 0x02000458 RID: 1112
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class SecurityPermissionAttribute : CodeAccessSecurityAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.SecurityPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</param>
		// Token: 0x06002D21 RID: 11553 RVA: 0x000A1D83 File Offset: 0x0009FF83
		public SecurityPermissionAttribute(SecurityAction action) : base(action)
		{
			this.m_Flags = SecurityPermissionFlag.NoFlags;
		}

		/// <summary>Gets or sets a value indicating whether permission to assert that all this code's callers have the requisite permission for the operation is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to assert is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000A1D93 File Offset: 0x0009FF93
		// (set) Token: 0x06002D23 RID: 11555 RVA: 0x000A1DA0 File Offset: 0x0009FFA0
		public bool Assertion
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Assertion) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Assertion;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.Assertion;
			}
		}

		/// <summary>Gets or sets a value that indicates whether code has permission to perform binding redirection in the application configuration file.</summary>
		/// <returns>
		///   <see langword="true" /> if code can perform binding redirects; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06002D24 RID: 11556 RVA: 0x000A1DC3 File Offset: 0x0009FFC3
		// (set) Token: 0x06002D25 RID: 11557 RVA: 0x000A1DD4 File Offset: 0x0009FFD4
		public bool BindingRedirects
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.BindingRedirects) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.BindingRedirects;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.BindingRedirects;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to manipulate <see cref="T:System.AppDomain" /> is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to manipulate <see cref="T:System.AppDomain" /> is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06002D26 RID: 11558 RVA: 0x000A1DFE File Offset: 0x0009FFFE
		// (set) Token: 0x06002D27 RID: 11559 RVA: 0x000A1E0F File Offset: 0x000A000F
		public bool ControlAppDomain
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlAppDomain) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlAppDomain;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlAppDomain;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to alter or manipulate domain security policy is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to alter or manipulate security policy in an application domain is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06002D28 RID: 11560 RVA: 0x000A1E39 File Offset: 0x000A0039
		// (set) Token: 0x06002D29 RID: 11561 RVA: 0x000A1E4A File Offset: 0x000A004A
		public bool ControlDomainPolicy
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlDomainPolicy) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlDomainPolicy;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlDomainPolicy;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to alter or manipulate evidence is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if the ability to alter or manipulate evidence is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06002D2A RID: 11562 RVA: 0x000A1E74 File Offset: 0x000A0074
		// (set) Token: 0x06002D2B RID: 11563 RVA: 0x000A1E82 File Offset: 0x000A0082
		public bool ControlEvidence
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlEvidence) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlEvidence;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlEvidence;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to view and manipulate security policy is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to manipulate security policy is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06002D2C RID: 11564 RVA: 0x000A1EA6 File Offset: 0x000A00A6
		// (set) Token: 0x06002D2D RID: 11565 RVA: 0x000A1EB4 File Offset: 0x000A00B4
		public bool ControlPolicy
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlPolicy) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlPolicy;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlPolicy;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to manipulate the current principal is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to manipulate the current principal is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06002D2E RID: 11566 RVA: 0x000A1ED8 File Offset: 0x000A00D8
		// (set) Token: 0x06002D2F RID: 11567 RVA: 0x000A1EE9 File Offset: 0x000A00E9
		public bool ControlPrincipal
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlPrincipal) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlPrincipal;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlPrincipal;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to manipulate threads is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to manipulate threads is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06002D30 RID: 11568 RVA: 0x000A1F13 File Offset: 0x000A0113
		// (set) Token: 0x06002D31 RID: 11569 RVA: 0x000A1F21 File Offset: 0x000A0121
		public bool ControlThread
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlThread) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlThread;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.ControlThread;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to execute code is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to execute code is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06002D32 RID: 11570 RVA: 0x000A1F45 File Offset: 0x000A0145
		// (set) Token: 0x06002D33 RID: 11571 RVA: 0x000A1F52 File Offset: 0x000A0152
		public bool Execution
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Execution) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Execution;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.Execution;
			}
		}

		/// <summary>Gets or sets a value indicating whether code can plug into the common language runtime infrastructure, such as adding Remoting Context Sinks, Envoy Sinks and Dynamic Sinks.</summary>
		/// <returns>
		///   <see langword="true" /> if code can plug into the common language runtime infrastructure; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06002D34 RID: 11572 RVA: 0x000A1F75 File Offset: 0x000A0175
		// (set) Token: 0x06002D35 RID: 11573 RVA: 0x000A1F86 File Offset: 0x000A0186
		[ComVisible(true)]
		public bool Infrastructure
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Infrastructure) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Infrastructure;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.Infrastructure;
			}
		}

		/// <summary>Gets or sets a value indicating whether code can configure remoting types and channels.</summary>
		/// <returns>
		///   <see langword="true" /> if code can configure remoting types and channels; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06002D36 RID: 11574 RVA: 0x000A1FB0 File Offset: 0x000A01B0
		// (set) Token: 0x06002D37 RID: 11575 RVA: 0x000A1FC1 File Offset: 0x000A01C1
		public bool RemotingConfiguration
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.RemotingConfiguration) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.RemotingConfiguration;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.RemotingConfiguration;
			}
		}

		/// <summary>Gets or sets a value indicating whether code can use a serialization formatter to serialize or deserialize an object.</summary>
		/// <returns>
		///   <see langword="true" /> if code can use a serialization formatter to serialize or deserialize an object; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06002D38 RID: 11576 RVA: 0x000A1FEB File Offset: 0x000A01EB
		// (set) Token: 0x06002D39 RID: 11577 RVA: 0x000A1FFC File Offset: 0x000A01FC
		public bool SerializationFormatter
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.SerializationFormatter) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.SerializationFormatter;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.SerializationFormatter;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to bypass code verification is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to bypass code verification is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D7 RID: 1495
		// (get) Token: 0x06002D3A RID: 11578 RVA: 0x000A2026 File Offset: 0x000A0226
		// (set) Token: 0x06002D3B RID: 11579 RVA: 0x000A2033 File Offset: 0x000A0233
		public bool SkipVerification
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.SkipVerification) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.SkipVerification;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.SkipVerification;
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to call unmanaged code is declared.</summary>
		/// <returns>
		///   <see langword="true" /> if permission to call unmanaged code is declared; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06002D3C RID: 11580 RVA: 0x000A2056 File Offset: 0x000A0256
		// (set) Token: 0x06002D3D RID: 11581 RVA: 0x000A2063 File Offset: 0x000A0263
		public bool UnmanagedCode
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.UnmanagedCode) > SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.UnmanagedCode;
					return;
				}
				this.m_Flags &= ~SecurityPermissionFlag.UnmanagedCode;
			}
		}

		/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.SecurityPermission" />.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.SecurityPermission" /> that corresponds to this attribute.</returns>
		// Token: 0x06002D3E RID: 11582 RVA: 0x000A2088 File Offset: 0x000A0288
		public override IPermission CreatePermission()
		{
			SecurityPermission result;
			if (base.Unrestricted)
			{
				result = new SecurityPermission(PermissionState.Unrestricted);
			}
			else
			{
				result = new SecurityPermission(this.m_Flags);
			}
			return result;
		}

		/// <summary>Gets or sets all permission flags comprising the <see cref="T:System.Security.Permissions.SecurityPermission" /> permissions.</summary>
		/// <returns>One or more of the <see cref="T:System.Security.Permissions.SecurityPermissionFlag" /> values combined using a bitwise OR.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to an invalid value. See <see cref="T:System.Security.Permissions.SecurityPermissionFlag" /> for the valid values.</exception>
		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000A20B5 File Offset: 0x000A02B5
		// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000A20BD File Offset: 0x000A02BD
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}

		// Token: 0x04002097 RID: 8343
		private SecurityPermissionFlag m_Flags;
	}
}
