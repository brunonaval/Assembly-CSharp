﻿using System;
using System.Runtime.Serialization;

namespace System.Runtime.InteropServices
{
	/// <summary>The exception thrown by the marshaler when it encounters an argument of a variant type that can not be marshaled to managed code.</summary>
	// Token: 0x020006DB RID: 1755
	[Serializable]
	public class InvalidOleVariantTypeException : SystemException
	{
		/// <summary>Initializes a new instance of the <see langword="InvalidOleVariantTypeException" /> class with default values.</summary>
		// Token: 0x0600403D RID: 16445 RVA: 0x000E0CF5 File Offset: 0x000DEEF5
		public InvalidOleVariantTypeException() : base("Specified OLE variant was invalid.")
		{
			base.HResult = -2146233039;
		}

		/// <summary>Initializes a new instance of the <see langword="InvalidOleVariantTypeException" /> class with a specified message.</summary>
		/// <param name="message">The message that indicates the reason for the exception.</param>
		// Token: 0x0600403E RID: 16446 RVA: 0x000E0D0D File Offset: 0x000DEF0D
		public InvalidOleVariantTypeException(string message) : base(message)
		{
			base.HResult = -2146233039;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.InvalidOleVariantTypeException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x0600403F RID: 16447 RVA: 0x000E0D21 File Offset: 0x000DEF21
		public InvalidOleVariantTypeException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233039;
		}

		/// <summary>Initializes a new instance of the <see langword="InvalidOleVariantTypeException" /> class from serialization data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x06004040 RID: 16448 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected InvalidOleVariantTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
