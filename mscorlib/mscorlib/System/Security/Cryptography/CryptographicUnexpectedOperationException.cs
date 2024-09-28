﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Cryptography
{
	/// <summary>The exception that is thrown when an unexpected operation occurs during a cryptographic operation.</summary>
	// Token: 0x02000484 RID: 1156
	[ComVisible(true)]
	[Serializable]
	public class CryptographicUnexpectedOperationException : CryptographicException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException" /> class with default properties.</summary>
		// Token: 0x06002E9C RID: 11932 RVA: 0x000A68F9 File Offset: 0x000A4AF9
		public CryptographicUnexpectedOperationException()
		{
			base.SetErrorCode(-2146233295);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException" /> class with a specified error message.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		// Token: 0x06002E9D RID: 11933 RVA: 0x000A690C File Offset: 0x000A4B0C
		public CryptographicUnexpectedOperationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233295);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException" /> class with a specified error message in the specified format.</summary>
		/// <param name="format">The format used to output the error message.</param>
		/// <param name="insert">The error message that explains the reason for the exception.</param>
		// Token: 0x06002E9E RID: 11934 RVA: 0x000A6920 File Offset: 0x000A4B20
		public CryptographicUnexpectedOperationException(string format, string insert) : base(string.Format(CultureInfo.CurrentCulture, format, insert))
		{
			base.SetErrorCode(-2146233295);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06002E9F RID: 11935 RVA: 0x000A693F File Offset: 0x000A4B3F
		public CryptographicUnexpectedOperationException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233295);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.CryptographicUnexpectedOperationException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		// Token: 0x06002EA0 RID: 11936 RVA: 0x000A6954 File Offset: 0x000A4B54
		protected CryptographicUnexpectedOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
