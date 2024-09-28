﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Represents a membership condition that matches all code. This class cannot be inherited.</summary>
	// Token: 0x020003FE RID: 1022
	[ComVisible(true)]
	[Serializable]
	public sealed class AllMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		/// <summary>Determines whether the specified evidence satisfies the membership condition.</summary>
		/// <param name="evidence">The evidence set against which to make the test.</param>
		/// <returns>Always <see langword="true" />.</returns>
		// Token: 0x060029C7 RID: 10695 RVA: 0x000040F7 File Offset: 0x000022F7
		public bool Check(Evidence evidence)
		{
			return true;
		}

		/// <summary>Creates an equivalent copy of the membership condition.</summary>
		/// <returns>A new, identical copy of the current membership condition.</returns>
		// Token: 0x060029C8 RID: 10696 RVA: 0x00097E37 File Offset: 0x00096037
		public IMembershipCondition Copy()
		{
			return new AllMembershipCondition();
		}

		/// <summary>Determines whether the specified membership condition is an <see cref="T:System.Security.Policy.AllMembershipCondition" />.</summary>
		/// <param name="o">The object to compare to <see cref="T:System.Security.Policy.AllMembershipCondition" />.</param>
		/// <returns>
		///   <see langword="true" /> if the specified membership condition is an <see cref="T:System.Security.Policy.AllMembershipCondition" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060029C9 RID: 10697 RVA: 0x00097E3E File Offset: 0x0009603E
		public override bool Equals(object o)
		{
			return o is AllMembershipCondition;
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object.</param>
		// Token: 0x060029CA RID: 10698 RVA: 0x00097E49 File Offset: 0x00096049
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object.</param>
		/// <param name="level">The policy level context used to resolve named permission set references.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="e" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="e" /> parameter is not a valid membership condition element.</exception>
		// Token: 0x060029CB RID: 10699 RVA: 0x00097E53 File Offset: 0x00096053
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
		}

		/// <summary>Gets the hash code for the current membership condition.</summary>
		/// <returns>The hash code for the current membership condition.</returns>
		// Token: 0x060029CC RID: 10700 RVA: 0x00097E6D File Offset: 0x0009606D
		public override int GetHashCode()
		{
			return typeof(AllMembershipCondition).GetHashCode();
		}

		/// <summary>Creates and returns a string representation of the membership condition.</summary>
		/// <returns>A representation of the membership condition.</returns>
		// Token: 0x060029CD RID: 10701 RVA: 0x00097E7E File Offset: 0x0009607E
		public override string ToString()
		{
			return "All code";
		}

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		// Token: 0x060029CE RID: 10702 RVA: 0x00097E85 File Offset: 0x00096085
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		/// <summary>Creates an XML encoding of the security object and its current state with the specified <see cref="T:System.Security.Policy.PolicyLevel" />.</summary>
		/// <param name="level">The policy level context for resolving named permission set references.</param>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		// Token: 0x060029CF RID: 10703 RVA: 0x00097E8E File Offset: 0x0009608E
		public SecurityElement ToXml(PolicyLevel level)
		{
			return MembershipConditionHelper.Element(typeof(AllMembershipCondition), this.version);
		}

		// Token: 0x04001F52 RID: 8018
		private readonly int version = 1;
	}
}
