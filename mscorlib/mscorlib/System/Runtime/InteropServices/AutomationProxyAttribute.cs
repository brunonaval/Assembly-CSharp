﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Specifies whether the type should be marshaled using the Automation marshaler or a custom proxy and stub.</summary>
	// Token: 0x0200070A RID: 1802
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
	[ComVisible(true)]
	public sealed class AutomationProxyAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.AutomationProxyAttribute" /> class.</summary>
		/// <param name="val">
		///   <see langword="true" /> if the class should be marshaled using the Automation Marshaler; <see langword="false" /> if a proxy stub marshaler should be used.</param>
		// Token: 0x060040AF RID: 16559 RVA: 0x000E1517 File Offset: 0x000DF717
		public AutomationProxyAttribute(bool val)
		{
			this._val = val;
		}

		/// <summary>Gets a value indicating the type of marshaler to use.</summary>
		/// <returns>
		///   <see langword="true" /> if the class should be marshaled using the Automation Marshaler; <see langword="false" /> if a proxy stub marshaler should be used.</returns>
		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060040B0 RID: 16560 RVA: 0x000E1526 File Offset: 0x000DF726
		public bool Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04002AE1 RID: 10977
		internal bool _val;
	}
}
