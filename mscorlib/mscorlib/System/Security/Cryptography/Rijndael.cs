﻿using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	/// <summary>Represents the base class from which all implementations of the <see cref="T:System.Security.Cryptography.Rijndael" /> symmetric encryption algorithm must inherit.</summary>
	// Token: 0x020004A3 RID: 1187
	[ComVisible(true)]
	public abstract class Rijndael : SymmetricAlgorithm
	{
		/// <summary>Initializes a new instance of <see cref="T:System.Security.Cryptography.Rijndael" />.</summary>
		// Token: 0x06002F80 RID: 12160 RVA: 0x000A90FF File Offset: 0x000A72FF
		protected Rijndael()
		{
			this.KeySizeValue = 256;
			this.BlockSizeValue = 128;
			this.FeedbackSizeValue = this.BlockSizeValue;
			this.LegalBlockSizesValue = Rijndael.s_legalBlockSizes;
			this.LegalKeySizesValue = Rijndael.s_legalKeySizes;
		}

		/// <summary>Creates a cryptographic object to perform the <see cref="T:System.Security.Cryptography.Rijndael" /> algorithm.</summary>
		/// <returns>A cryptographic object.</returns>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		// Token: 0x06002F81 RID: 12161 RVA: 0x000A913F File Offset: 0x000A733F
		public new static Rijndael Create()
		{
			return Rijndael.Create("System.Security.Cryptography.Rijndael");
		}

		/// <summary>Creates a cryptographic object to perform the specified implementation of the <see cref="T:System.Security.Cryptography.Rijndael" /> algorithm.</summary>
		/// <param name="algName">The name of the specific implementation of <see cref="T:System.Security.Cryptography.Rijndael" /> to create.</param>
		/// <returns>A cryptographic object.</returns>
		/// <exception cref="T:System.Reflection.TargetInvocationException">The algorithm described by the <paramref name="algName" /> parameter was used with Federal Information Processing Standards (FIPS) mode enabled, but is not FIPS compatible.</exception>
		// Token: 0x06002F82 RID: 12162 RVA: 0x000A914B File Offset: 0x000A734B
		public new static Rijndael Create(string algName)
		{
			return (Rijndael)CryptoConfig.CreateFromName(algName);
		}

		// Token: 0x04002197 RID: 8599
		private static KeySizes[] s_legalBlockSizes = new KeySizes[]
		{
			new KeySizes(128, 256, 64)
		};

		// Token: 0x04002198 RID: 8600
		private static KeySizes[] s_legalKeySizes = new KeySizes[]
		{
			new KeySizes(128, 256, 64)
		};
	}
}
