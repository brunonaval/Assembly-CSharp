﻿using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Represents the <see langword="Token" /> returned by the metadata to represent a signature.</summary>
	// Token: 0x02000945 RID: 2373
	[ComVisible(true)]
	public readonly struct SignatureToken : IEquatable<SignatureToken>
	{
		// Token: 0x06005275 RID: 21109 RVA: 0x00102D97 File Offset: 0x00100F97
		internal SignatureToken(int val)
		{
			this.tokValue = val;
		}

		/// <summary>Checks if the given object is an instance of <see langword="SignatureToken" /> and is equal to this instance.</summary>
		/// <param name="obj">The object to compare with this <see langword="SignatureToken" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="obj" /> is an instance of <see langword="SignatureToken" /> and is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005276 RID: 21110 RVA: 0x00102DA0 File Offset: 0x00100FA0
		public override bool Equals(object obj)
		{
			bool flag = obj is SignatureToken;
			if (flag)
			{
				SignatureToken signatureToken = (SignatureToken)obj;
				flag = (this.tokValue == signatureToken.tokValue);
			}
			return flag;
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Reflection.Emit.SignatureToken" />.</summary>
		/// <param name="obj">The <see cref="T:System.Reflection.Emit.SignatureToken" /> to compare to the current instance.</param>
		/// <returns>
		///   <see langword="true" /> if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005277 RID: 21111 RVA: 0x00102DD1 File Offset: 0x00100FD1
		public bool Equals(SignatureToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.SignatureToken" /> structures are equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.SignatureToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.SignatureToken" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005278 RID: 21112 RVA: 0x00102DE1 File Offset: 0x00100FE1
		public static bool operator ==(SignatureToken a, SignatureToken b)
		{
			return object.Equals(a, b);
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.SignatureToken" /> structures are not equal.</summary>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.SignatureToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.SignatureToken" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///   <see langword="true" /> if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005279 RID: 21113 RVA: 0x00102DF4 File Offset: 0x00100FF4
		public static bool operator !=(SignatureToken a, SignatureToken b)
		{
			return !object.Equals(a, b);
		}

		/// <summary>Generates the hash code for this signature.</summary>
		/// <returns>The hash code for this signature.</returns>
		// Token: 0x0600527A RID: 21114 RVA: 0x00102E0A File Offset: 0x0010100A
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		/// <summary>Retrieves the metadata token for the local variable signature for this method.</summary>
		/// <returns>Read-only. Retrieves the metadata token of this signature.</returns>
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x0600527B RID: 21115 RVA: 0x00102E0A File Offset: 0x0010100A
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		// Token: 0x04003318 RID: 13080
		internal readonly int tokValue;

		/// <summary>The default <see langword="SignatureToken" /> with <see cref="P:System.Reflection.Emit.SignatureToken.Token" /> value 0.</summary>
		// Token: 0x04003319 RID: 13081
		public static readonly SignatureToken Empty;
	}
}
