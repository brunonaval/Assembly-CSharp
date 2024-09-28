﻿using System;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>The exception that is thrown when a path or fully qualified file name is longer than the system-defined maximum length.</summary>
	// Token: 0x02000B0E RID: 2830
	[Serializable]
	public class PathTooLongException : IOException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IO.PathTooLongException" /> class with its HRESULT set to COR_E_PATHTOOLONG.</summary>
		// Token: 0x0600652D RID: 25901 RVA: 0x001584A7 File Offset: 0x001566A7
		public PathTooLongException() : base("The specified file name or path is too long, or a component of the specified path is too long.")
		{
			base.HResult = -2147024690;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.PathTooLongException" /> class with its message string set to <paramref name="message" /> and its HRESULT set to COR_E_PATHTOOLONG.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		// Token: 0x0600652E RID: 25902 RVA: 0x001584BF File Offset: 0x001566BF
		public PathTooLongException(string message) : base(message)
		{
			base.HResult = -2147024690;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.PathTooLongException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x0600652F RID: 25903 RVA: 0x001584D3 File Offset: 0x001566D3
		public PathTooLongException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2147024690;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.PathTooLongException" /> class with the specified serialization and context information.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		// Token: 0x06006530 RID: 25904 RVA: 0x001566F0 File Offset: 0x001548F0
		protected PathTooLongException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
