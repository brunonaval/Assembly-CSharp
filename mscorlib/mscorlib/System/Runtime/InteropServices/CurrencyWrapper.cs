﻿using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Wraps objects the marshaler should marshal as a <see langword="VT_CY" />.</summary>
	// Token: 0x020006D4 RID: 1748
	public sealed class CurrencyWrapper
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.CurrencyWrapper" /> class with the <see langword="Decimal" /> to be wrapped and marshaled as type <see langword="VT_CY" />.</summary>
		/// <param name="obj">The <see langword="Decimal" /> to be wrapped and marshaled as <see langword="VT_CY" />.</param>
		// Token: 0x0600402B RID: 16427 RVA: 0x000E0C1A File Offset: 0x000DEE1A
		public CurrencyWrapper(decimal obj)
		{
			this.m_WrappedObject = obj;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.CurrencyWrapper" /> class with the object containing the <see langword="Decimal" /> to be wrapped and marshaled as type <see langword="VT_CY" />.</summary>
		/// <param name="obj">The object containing the <see langword="Decimal" /> to be wrapped and marshaled as <see langword="VT_CY" />.</param>
		/// <exception cref="T:System.ArgumentException">The <paramref name="obj" /> parameter is not a <see cref="T:System.Decimal" /> type.</exception>
		// Token: 0x0600402C RID: 16428 RVA: 0x000E0C29 File Offset: 0x000DEE29
		public CurrencyWrapper(object obj)
		{
			if (!(obj is decimal))
			{
				throw new ArgumentException("Object must be of type Decimal.", "obj");
			}
			this.m_WrappedObject = (decimal)obj;
		}

		/// <summary>Gets the wrapped object to be marshaled as type <see langword="VT_CY" />.</summary>
		/// <returns>The wrapped object to be marshaled as type <see langword="VT_CY" />.</returns>
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x000E0C55 File Offset: 0x000DEE55
		public decimal WrappedObject
		{
			get
			{
				return this.m_WrappedObject;
			}
		}

		// Token: 0x04002A1C RID: 10780
		private decimal m_WrappedObject;
	}
}
