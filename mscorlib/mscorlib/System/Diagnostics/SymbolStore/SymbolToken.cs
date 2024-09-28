﻿using System;

namespace System.Diagnostics.SymbolStore
{
	/// <summary>The <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure is an object representation of a token that represents symbolic information.</summary>
	// Token: 0x020009D4 RID: 2516
	public readonly struct SymbolToken
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure when given a value.</summary>
		/// <param name="val">The value to be used for the token.</param>
		// Token: 0x06005A3C RID: 23100 RVA: 0x00134249 File Offset: 0x00132449
		public SymbolToken(int val)
		{
			this._token = val;
		}

		/// <summary>Gets the value of the current token.</summary>
		/// <returns>The value of the current token.</returns>
		// Token: 0x06005A3D RID: 23101 RVA: 0x00134252 File Offset: 0x00132452
		public int GetToken()
		{
			return this._token;
		}

		/// <summary>Generates the hash code for the current token.</summary>
		/// <returns>The hash code for the current token.</returns>
		// Token: 0x06005A3E RID: 23102 RVA: 0x00134252 File Offset: 0x00132452
		public override int GetHashCode()
		{
			return this._token;
		}

		/// <summary>Determines whether <paramref name="obj" /> is an instance of <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> and is equal to this instance.</summary>
		/// <param name="obj">The object to check.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> and is equal to this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A3F RID: 23103 RVA: 0x0013425A File Offset: 0x0013245A
		public override bool Equals(object obj)
		{
			return obj is SymbolToken && this.Equals((SymbolToken)obj);
		}

		/// <summary>Determines whether <paramref name="obj" /> is equal to this instance.</summary>
		/// <param name="obj">The <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> to check.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is equal to this instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A40 RID: 23104 RVA: 0x00134272 File Offset: 0x00132472
		public bool Equals(SymbolToken obj)
		{
			return obj._token == this._token;
		}

		/// <summary>Returns a value indicating whether two <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> objects are equal.</summary>
		/// <param name="a">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		/// <param name="b">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A41 RID: 23105 RVA: 0x00134282 File Offset: 0x00132482
		public static bool operator ==(SymbolToken a, SymbolToken b)
		{
			return a.Equals(b);
		}

		/// <summary>Returns a value indicating whether two <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> objects are not equal.</summary>
		/// <param name="a">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		/// <param name="b">A <see cref="T:System.Diagnostics.SymbolStore.SymbolToken" /> structure.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A42 RID: 23106 RVA: 0x0013428C File Offset: 0x0013248C
		public static bool operator !=(SymbolToken a, SymbolToken b)
		{
			return !(a == b);
		}

		// Token: 0x040037BD RID: 14269
		private readonly int _token;
	}
}
