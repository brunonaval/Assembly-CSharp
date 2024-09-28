﻿using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Determines whether an assembly belongs to a code group by testing the site from which it originated. This class cannot be inherited.</summary>
	// Token: 0x0200041F RID: 1055
	[ComVisible(true)]
	[Serializable]
	public sealed class SiteMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x0009C45F File Offset: 0x0009A65F
		internal SiteMembershipCondition()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.SiteMembershipCondition" /> class with name of the site that determines membership.</summary>
		/// <param name="site">The site name or wildcard expression.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="site" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="site" /> parameter is not a valid <see cref="T:System.Security.Policy.Site" />.</exception>
		// Token: 0x06002B27 RID: 11047 RVA: 0x0009C46E File Offset: 0x0009A66E
		public SiteMembershipCondition(string site)
		{
			this.Site = site;
		}

		/// <summary>Gets or sets the site for which the membership condition tests.</summary>
		/// <returns>The site for which the membership condition tests.</returns>
		/// <exception cref="T:System.ArgumentNullException">An attempt is made to set <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> to <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> to an invalid <see cref="T:System.Security.Policy.Site" />.</exception>
		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002B28 RID: 11048 RVA: 0x0009C484 File Offset: 0x0009A684
		// (set) Token: 0x06002B29 RID: 11049 RVA: 0x0009C48C File Offset: 0x0009A68C
		public string Site
		{
			get
			{
				return this._site;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("site");
				}
				if (!System.Security.Policy.Site.IsValid(value))
				{
					throw new ArgumentException("invalid site");
				}
				this._site = value;
			}
		}

		/// <summary>Determines whether the specified evidence satisfies the membership condition.</summary>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> against which to make the test.</param>
		/// <returns>
		///   <see langword="true" /> if the specified evidence satisfies the membership condition; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property is <see langword="null" />.</exception>
		// Token: 0x06002B2A RID: 11050 RVA: 0x0009C4B8 File Offset: 0x0009A6B8
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				if (hostEnumerator.Current is Site)
				{
					string[] array = this._site.Split('.', StringSplitOptions.None);
					string[] array2 = (hostEnumerator.Current as Site).origin_site.Split('.', StringSplitOptions.None);
					int i = array.Length - 1;
					int num = array2.Length - 1;
					while (i >= 0)
					{
						if (i == 0)
						{
							return string.Compare(array[0], "*", true, CultureInfo.InvariantCulture) == 0;
						}
						if (string.Compare(array[i], array2[num], true, CultureInfo.InvariantCulture) != 0)
						{
							return false;
						}
						i--;
						num--;
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>Creates an equivalent copy of the membership condition.</summary>
		/// <returns>A new, identical copy of the current membership condition.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property is <see langword="null" />.</exception>
		// Token: 0x06002B2B RID: 11051 RVA: 0x0009C566 File Offset: 0x0009A766
		public IMembershipCondition Copy()
		{
			return new SiteMembershipCondition(this._site);
		}

		/// <summary>Determines whether the site from the specified <see cref="T:System.Security.Policy.SiteMembershipCondition" /> object is equivalent to the site contained in the current <see cref="T:System.Security.Policy.SiteMembershipCondition" />.</summary>
		/// <param name="o">The <see cref="T:System.Security.Policy.SiteMembershipCondition" /> object to compare to the current <see cref="T:System.Security.Policy.SiteMembershipCondition" />.</param>
		/// <returns>
		///   <see langword="true" /> if the site from the specified <see cref="T:System.Security.Policy.SiteMembershipCondition" /> object is equivalent to the site contained in the current <see cref="T:System.Security.Policy.SiteMembershipCondition" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property for the current object or the specified object is <see langword="null" />.</exception>
		// Token: 0x06002B2C RID: 11052 RVA: 0x0009C573 File Offset: 0x0009A773
		public override bool Equals(object o)
		{
			return o != null && o is SiteMembershipCondition && new Site((o as SiteMembershipCondition)._site).Equals(new Site(this._site));
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid membership condition element.</exception>
		// Token: 0x06002B2D RID: 11053 RVA: 0x0009C5A4 File Offset: 0x0009A7A4
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object.</param>
		/// <param name="level">The <see cref="T:System.Security.Policy.PolicyLevel" /> context, used to resolve <see cref="T:System.Security.NamedPermissionSet" /> references.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid membership condition element.</exception>
		// Token: 0x06002B2E RID: 11054 RVA: 0x0009C5AE File Offset: 0x0009A7AE
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
			this._site = e.Attribute("Site");
		}

		/// <summary>Gets the hash code for the current membership condition.</summary>
		/// <returns>The hash code for the current membership condition.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property is <see langword="null" />.</exception>
		// Token: 0x06002B2F RID: 11055 RVA: 0x0009C5D9 File Offset: 0x0009A7D9
		public override int GetHashCode()
		{
			return this._site.GetHashCode();
		}

		/// <summary>Creates and returns a string representation of the membership condition.</summary>
		/// <returns>A string representation of the membership condition.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property is <see langword="null" />.</exception>
		// Token: 0x06002B30 RID: 11056 RVA: 0x0009C5E6 File Offset: 0x0009A7E6
		public override string ToString()
		{
			return "Site - " + this._site;
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property is <see langword="null" />.</exception>
		// Token: 0x06002B31 RID: 11057 RVA: 0x0009C5F8 File Offset: 0x0009A7F8
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		/// <summary>Creates an XML encoding of the security object and its current state with the specified <see cref="T:System.Security.Policy.PolicyLevel" />.</summary>
		/// <param name="level">The <see cref="T:System.Security.Policy.PolicyLevel" /> context, used to resolve <see cref="T:System.Security.NamedPermissionSet" /> references.</param>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Security.Policy.SiteMembershipCondition.Site" /> property is <see langword="null" />.</exception>
		// Token: 0x06002B32 RID: 11058 RVA: 0x0009C601 File Offset: 0x0009A801
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = MembershipConditionHelper.Element(typeof(SiteMembershipCondition), this.version);
			securityElement.AddAttribute("Site", this._site);
			return securityElement;
		}

		// Token: 0x04001FB3 RID: 8115
		private readonly int version = 1;

		// Token: 0x04001FB4 RID: 8116
		private string _site;
	}
}
