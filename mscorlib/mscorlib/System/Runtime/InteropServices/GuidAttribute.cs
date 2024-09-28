﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Supplies an explicit <see cref="T:System.Guid" /> when an automatic GUID is undesirable.</summary>
	// Token: 0x020006FF RID: 1791
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
	[ComVisible(true)]
	public sealed class GuidAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.GuidAttribute" /> class with the specified GUID.</summary>
		/// <param name="guid">The <see cref="T:System.Guid" /> to be assigned.</param>
		// Token: 0x0600408E RID: 16526 RVA: 0x000E118E File Offset: 0x000DF38E
		public GuidAttribute(string guid)
		{
			this._val = guid;
		}

		/// <summary>Gets the <see cref="T:System.Guid" /> of the class.</summary>
		/// <returns>The <see cref="T:System.Guid" /> of the class.</returns>
		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x0600408F RID: 16527 RVA: 0x000E119D File Offset: 0x000DF39D
		public string Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002AC7 RID: 10951
		internal string _val;
	}
}
