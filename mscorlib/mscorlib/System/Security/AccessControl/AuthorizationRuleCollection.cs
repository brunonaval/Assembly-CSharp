﻿using System;
using System.Collections;

namespace System.Security.AccessControl
{
	/// <summary>Represents a collection of <see cref="T:System.Security.AccessControl.AuthorizationRule" /> objects.</summary>
	// Token: 0x0200050D RID: 1293
	public sealed class AuthorizationRuleCollection : ReadOnlyCollectionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.AccessControl.AuthorizationRuleCollection" /> class.</summary>
		// Token: 0x06003358 RID: 13144 RVA: 0x000BCA0F File Offset: 0x000BAC0F
		public AuthorizationRuleCollection()
		{
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000BCA17 File Offset: 0x000BAC17
		internal AuthorizationRuleCollection(AuthorizationRule[] rules)
		{
			base.InnerList.AddRange(rules);
		}

		/// <summary>Adds an <see cref="T:System.Security.AccessControl.AuthorizationRule" /> object to the collection.</summary>
		/// <param name="rule">The <see cref="T:System.Security.AccessControl.AuthorizationRule" /> object to add to the collection.</param>
		// Token: 0x0600335A RID: 13146 RVA: 0x000BCA2B File Offset: 0x000BAC2B
		public void AddRule(AuthorizationRule rule)
		{
			base.InnerList.Add(rule);
		}

		/// <summary>Gets the <see cref="T:System.Security.AccessControl.AuthorizationRule" /> object at the specified index of the collection.</summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Security.AccessControl.AuthorizationRule" /> object to get.</param>
		/// <returns>The <see cref="T:System.Security.AccessControl.AuthorizationRule" /> object at the specified index.</returns>
		// Token: 0x170006F8 RID: 1784
		public AuthorizationRule this[int index]
		{
			get
			{
				return (AuthorizationRule)base.InnerList[index];
			}
		}

		/// <summary>Copies the contents of the collection to an array.</summary>
		/// <param name="rules">An array to which to copy the contents of the collection.</param>
		/// <param name="index">The zero-based index from which to begin copying.</param>
		// Token: 0x0600335C RID: 13148 RVA: 0x000BCA4D File Offset: 0x000BAC4D
		public void CopyTo(AuthorizationRule[] rules, int index)
		{
			base.InnerList.CopyTo(rules, index);
		}
	}
}
