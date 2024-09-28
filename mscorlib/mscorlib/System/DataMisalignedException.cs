﻿using System;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when a unit of data is read from or written to an address that is not a multiple of the data size. This class cannot be inherited.</summary>
	// Token: 0x0200010B RID: 267
	[Serializable]
	public sealed class DataMisalignedException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.DataMisalignedException" /> class.</summary>
		// Token: 0x060009A3 RID: 2467 RVA: 0x000258FE File Offset: 0x00023AFE
		public DataMisalignedException() : base("A datatype misalignment was detected in a load or store instruction.")
		{
			base.HResult = -2146233023;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DataMisalignedException" /> class using the specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> object that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		// Token: 0x060009A4 RID: 2468 RVA: 0x00025916 File Offset: 0x00023B16
		public DataMisalignedException(string message) : base(message)
		{
			base.HResult = -2146233023;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.DataMisalignedException" /> class using the specified error message and underlying exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> object that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		/// <param name="innerException">The exception that is the cause of the current <see cref="T:System.DataMisalignedException" />. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x060009A5 RID: 2469 RVA: 0x0002592A File Offset: 0x00023B2A
		public DataMisalignedException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233023;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal DataMisalignedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
