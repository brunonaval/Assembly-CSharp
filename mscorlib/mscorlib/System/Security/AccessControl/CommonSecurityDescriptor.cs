﻿using System;
using System.Security.Principal;

namespace System.Security.AccessControl
{
	/// <summary>Represents a security descriptor. A security descriptor includes an owner, a primary group, a Discretionary Access Control List (DACL), and a System Access Control List (SACL).</summary>
	// Token: 0x02000517 RID: 1303
	public sealed class CommonSecurityDescriptor : GenericSecurityDescriptor
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> class from the specified <see cref="T:System.Security.AccessControl.RawSecurityDescriptor" /> object.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new security descriptor is associated with a container object.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new security descriptor is associated with a directory object.</param>
		/// <param name="rawSecurityDescriptor">The <see cref="T:System.Security.AccessControl.RawSecurityDescriptor" /> object from which to create the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		// Token: 0x060033B1 RID: 13233 RVA: 0x000BDCC0 File Offset: 0x000BBEC0
		public CommonSecurityDescriptor(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor)
		{
			this.Init(isContainer, isDS, rawSecurityDescriptor);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> class from the specified Security Descriptor Definition Language (SDDL) string.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new security descriptor is associated with a container object.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new security descriptor is associated with a directory object.</param>
		/// <param name="sddlForm">The SDDL string from which to create the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		// Token: 0x060033B2 RID: 13234 RVA: 0x000BDCD1 File Offset: 0x000BBED1
		public CommonSecurityDescriptor(bool isContainer, bool isDS, string sddlForm)
		{
			this.Init(isContainer, isDS, new RawSecurityDescriptor(sddlForm));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> class from the specified array of byte values.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new security descriptor is associated with a container object.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new security descriptor is associated with a directory object.</param>
		/// <param name="binaryForm">The array of byte values from which to create the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		/// <param name="offset">The offset in the <paramref name="binaryForm" /> array at which to begin copying.</param>
		// Token: 0x060033B3 RID: 13235 RVA: 0x000BDCE7 File Offset: 0x000BBEE7
		public CommonSecurityDescriptor(bool isContainer, bool isDS, byte[] binaryForm, int offset)
		{
			this.Init(isContainer, isDS, new RawSecurityDescriptor(binaryForm, offset));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> class from the specified information.</summary>
		/// <param name="isContainer">
		///   <see langword="true" /> if the new security descriptor is associated with a container object.</param>
		/// <param name="isDS">
		///   <see langword="true" /> if the new security descriptor is associated with a directory object.</param>
		/// <param name="flags">Flags that specify behavior of the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		/// <param name="owner">The owner for the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		/// <param name="group">The primary group for the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		/// <param name="systemAcl">The System Access Control List (SACL) for the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		/// <param name="discretionaryAcl">The Discretionary Access Control List (DACL) for the new <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</param>
		// Token: 0x060033B4 RID: 13236 RVA: 0x000BDCFF File Offset: 0x000BBEFF
		public CommonSecurityDescriptor(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			this.Init(isContainer, isDS, flags, owner, group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060033B5 RID: 13237 RVA: 0x000BDD18 File Offset: 0x000BBF18
		private void Init(bool isContainer, bool isDS, RawSecurityDescriptor rawSecurityDescriptor)
		{
			if (rawSecurityDescriptor == null)
			{
				throw new ArgumentNullException("rawSecurityDescriptor");
			}
			SystemAcl systemAcl = null;
			if (rawSecurityDescriptor.SystemAcl != null)
			{
				systemAcl = new SystemAcl(isContainer, isDS, rawSecurityDescriptor.SystemAcl);
			}
			DiscretionaryAcl discretionaryAcl = null;
			if (rawSecurityDescriptor.DiscretionaryAcl != null)
			{
				discretionaryAcl = new DiscretionaryAcl(isContainer, isDS, rawSecurityDescriptor.DiscretionaryAcl);
			}
			this.Init(isContainer, isDS, rawSecurityDescriptor.ControlFlags, rawSecurityDescriptor.Owner, rawSecurityDescriptor.Group, systemAcl, discretionaryAcl);
		}

		// Token: 0x060033B6 RID: 13238 RVA: 0x000BDD7F File Offset: 0x000BBF7F
		private void Init(bool isContainer, bool isDS, ControlFlags flags, SecurityIdentifier owner, SecurityIdentifier group, SystemAcl systemAcl, DiscretionaryAcl discretionaryAcl)
		{
			this.flags = (flags & ~ControlFlags.SystemAclPresent);
			this.is_container = isContainer;
			this.is_ds = isDS;
			this.Owner = owner;
			this.Group = group;
			this.SystemAcl = systemAcl;
			this.DiscretionaryAcl = discretionaryAcl;
		}

		/// <summary>Gets values that specify behavior of the <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</summary>
		/// <returns>One or more values of the <see cref="T:System.Security.AccessControl.ControlFlags" /> enumeration combined with a logical OR operation.</returns>
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x060033B7 RID: 13239 RVA: 0x000BDDBC File Offset: 0x000BBFBC
		public override ControlFlags ControlFlags
		{
			get
			{
				ControlFlags controlFlags = this.flags;
				controlFlags |= ControlFlags.DiscretionaryAclPresent;
				controlFlags |= ControlFlags.SelfRelative;
				if (this.SystemAcl != null)
				{
					controlFlags |= ControlFlags.SystemAclPresent;
				}
				return controlFlags;
			}
		}

		/// <summary>Gets or sets the discretionary access control list (DACL) for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object. The DACL contains access rules.</summary>
		/// <returns>The DACL for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</returns>
		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x060033B8 RID: 13240 RVA: 0x000BDDEA File Offset: 0x000BBFEA
		// (set) Token: 0x060033B9 RID: 13241 RVA: 0x000BDDF4 File Offset: 0x000BBFF4
		public DiscretionaryAcl DiscretionaryAcl
		{
			get
			{
				return this.discretionary_acl;
			}
			set
			{
				if (value == null)
				{
					value = new DiscretionaryAcl(this.IsContainer, this.IsDS, 1);
					value.AddAccess(AccessControlType.Allow, new SecurityIdentifier("WD"), -1, this.IsContainer ? (InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit) : InheritanceFlags.None, PropagationFlags.None);
					value.IsAefa = true;
				}
				this.CheckAclConsistency(value);
				this.discretionary_acl = value;
			}
		}

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x060033BA RID: 13242 RVA: 0x000BDE4C File Offset: 0x000BC04C
		internal override GenericAcl InternalDacl
		{
			get
			{
				return this.DiscretionaryAcl;
			}
		}

		/// <summary>Gets or sets the primary group for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</summary>
		/// <returns>The primary group for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</returns>
		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060033BB RID: 13243 RVA: 0x000BDE54 File Offset: 0x000BC054
		// (set) Token: 0x060033BC RID: 13244 RVA: 0x000BDE5C File Offset: 0x000BC05C
		public override SecurityIdentifier Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the object associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is a container object.</summary>
		/// <returns>
		///   <see langword="true" /> if the object associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is a container object; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x000BDE65 File Offset: 0x000BC065
		public bool IsContainer
		{
			get
			{
				return this.is_container;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is in canonical order.</summary>
		/// <returns>
		///   <see langword="true" /> if the DACL associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is in canonical order; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x000BDE6D File Offset: 0x000BC06D
		public bool IsDiscretionaryAclCanonical
		{
			get
			{
				return this.DiscretionaryAcl.IsCanonical;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the object associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is a directory object.</summary>
		/// <returns>
		///   <see langword="true" /> if the object associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is a directory object; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x000BDE7A File Offset: 0x000BC07A
		public bool IsDS
		{
			get
			{
				return this.is_ds;
			}
		}

		/// <summary>Gets a Boolean value that specifies whether the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is in canonical order.</summary>
		/// <returns>
		///   <see langword="true" /> if the SACL associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object is in canonical order; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000BDE82 File Offset: 0x000BC082
		public bool IsSystemAclCanonical
		{
			get
			{
				return this.SystemAcl == null || this.SystemAcl.IsCanonical;
			}
		}

		/// <summary>Gets or sets the owner of the object associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</summary>
		/// <returns>The owner of the object associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</returns>
		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000BDE99 File Offset: 0x000BC099
		// (set) Token: 0x060033C2 RID: 13250 RVA: 0x000BDEA1 File Offset: 0x000BC0A1
		public override SecurityIdentifier Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		/// <summary>Gets or sets the System Access Control List (SACL) for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object. The SACL contains audit rules.</summary>
		/// <returns>The SACL for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</returns>
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000BDEAA File Offset: 0x000BC0AA
		// (set) Token: 0x060033C4 RID: 13252 RVA: 0x000BDEB2 File Offset: 0x000BC0B2
		public SystemAcl SystemAcl
		{
			get
			{
				return this.system_acl;
			}
			set
			{
				if (value != null)
				{
					this.CheckAclConsistency(value);
				}
				this.system_acl = value;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000BDEC5 File Offset: 0x000BC0C5
		internal override GenericAcl InternalSacl
		{
			get
			{
				return this.SystemAcl;
			}
		}

		/// <summary>Removes all access rules for the specified security identifier from the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</summary>
		/// <param name="sid">The security identifier for which to remove access rules.</param>
		// Token: 0x060033C6 RID: 13254 RVA: 0x000BDECD File Offset: 0x000BC0CD
		public void PurgeAccessControl(SecurityIdentifier sid)
		{
			this.DiscretionaryAcl.Purge(sid);
		}

		/// <summary>Removes all audit rules for the specified security identifier from the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object.</summary>
		/// <param name="sid">The security identifier for which to remove audit rules.</param>
		// Token: 0x060033C7 RID: 13255 RVA: 0x000BDEDB File Offset: 0x000BC0DB
		public void PurgeAudit(SecurityIdentifier sid)
		{
			if (this.SystemAcl != null)
			{
				this.SystemAcl.Purge(sid);
			}
		}

		/// <summary>Sets the inheritance protection for the Discretionary Access Control List (DACL) associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object. DACLs that are protected do not inherit access rules from parent containers.</summary>
		/// <param name="isProtected">
		///   <see langword="true" /> to protect the DACL from inheritance.</param>
		/// <param name="preserveInheritance">
		///   <see langword="true" /> to keep inherited access rules in the DACL; <see langword="false" /> to remove inherited access rules from the DACL.</param>
		// Token: 0x060033C8 RID: 13256 RVA: 0x000BDEF4 File Offset: 0x000BC0F4
		public void SetDiscretionaryAclProtection(bool isProtected, bool preserveInheritance)
		{
			this.DiscretionaryAcl.IsAefa = false;
			if (!isProtected)
			{
				this.flags &= ~ControlFlags.DiscretionaryAclProtected;
				return;
			}
			this.flags |= ControlFlags.DiscretionaryAclProtected;
			if (!preserveInheritance)
			{
				this.DiscretionaryAcl.RemoveInheritedAces();
			}
		}

		/// <summary>Sets the inheritance protection for the System Access Control List (SACL) associated with this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> object. SACLs that are protected do not inherit audit rules from parent containers.</summary>
		/// <param name="isProtected">
		///   <see langword="true" /> to protect the SACL from inheritance.</param>
		/// <param name="preserveInheritance">
		///   <see langword="true" /> to keep inherited audit rules in the SACL; <see langword="false" /> to remove inherited audit rules from the SACL.</param>
		// Token: 0x060033C9 RID: 13257 RVA: 0x000BDF43 File Offset: 0x000BC143
		public void SetSystemAclProtection(bool isProtected, bool preserveInheritance)
		{
			if (!isProtected)
			{
				this.flags &= ~ControlFlags.SystemAclProtected;
				return;
			}
			this.flags |= ControlFlags.SystemAclProtected;
			if (!preserveInheritance && this.SystemAcl != null)
			{
				this.SystemAcl.RemoveInheritedAces();
			}
		}

		/// <summary>Sets the <see cref="P:System.Security.AccessControl.CommonSecurityDescriptor.DiscretionaryAcl" /> property for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> instance and sets the <see cref="F:System.Security.AccessControl.ControlFlags.DiscretionaryAclPresent" /> flag.</summary>
		/// <param name="revision">The revision level of the new <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object.</param>
		/// <param name="trusted">The number of Access Control Entries (ACEs) this <see cref="T:System.Security.AccessControl.DiscretionaryAcl" /> object can contain. This number is to be used only as a hint.</param>
		// Token: 0x060033CA RID: 13258 RVA: 0x000BDF83 File Offset: 0x000BC183
		public void AddDiscretionaryAcl(byte revision, int trusted)
		{
			this.DiscretionaryAcl = new DiscretionaryAcl(this.IsContainer, this.IsDS, revision, trusted);
			this.flags |= ControlFlags.DiscretionaryAclPresent;
		}

		/// <summary>Sets the <see cref="P:System.Security.AccessControl.CommonSecurityDescriptor.SystemAcl" /> property for this <see cref="T:System.Security.AccessControl.CommonSecurityDescriptor" /> instance and sets the <see cref="F:System.Security.AccessControl.ControlFlags.SystemAclPresent" /> flag.</summary>
		/// <param name="revision">The revision level of the new <see cref="T:System.Security.AccessControl.SystemAcl" /> object.</param>
		/// <param name="trusted">The number of Access Control Entries (ACEs) this <see cref="T:System.Security.AccessControl.SystemAcl" /> object can contain. This number should only be used as a hint.</param>
		// Token: 0x060033CB RID: 13259 RVA: 0x000BDFAC File Offset: 0x000BC1AC
		public void AddSystemAcl(byte revision, int trusted)
		{
			this.SystemAcl = new SystemAcl(this.IsContainer, this.IsDS, revision, trusted);
			this.flags |= ControlFlags.SystemAclPresent;
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000BDFD6 File Offset: 0x000BC1D6
		private void CheckAclConsistency(CommonAcl acl)
		{
			if (this.IsContainer != acl.IsContainer)
			{
				throw new ArgumentException("IsContainer must match between descriptor and ACL.");
			}
			if (this.IsDS != acl.IsDS)
			{
				throw new ArgumentException("IsDS must match between descriptor and ACL.");
			}
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x060033CD RID: 13261 RVA: 0x000BE00A File Offset: 0x000BC20A
		internal override bool DaclIsUnmodifiedAefa
		{
			get
			{
				return this.DiscretionaryAcl.IsAefa;
			}
		}

		// Token: 0x04002456 RID: 9302
		private bool is_container;

		// Token: 0x04002457 RID: 9303
		private bool is_ds;

		// Token: 0x04002458 RID: 9304
		private ControlFlags flags;

		// Token: 0x04002459 RID: 9305
		private SecurityIdentifier owner;

		// Token: 0x0400245A RID: 9306
		private SecurityIdentifier group;

		// Token: 0x0400245B RID: 9307
		private SystemAcl system_acl;

		// Token: 0x0400245C RID: 9308
		private DiscretionaryAcl discretionary_acl;
	}
}
