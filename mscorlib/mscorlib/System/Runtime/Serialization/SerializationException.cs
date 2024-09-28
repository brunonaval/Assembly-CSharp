﻿using System;

namespace System.Runtime.Serialization
{
	/// <summary>The exception thrown when an error occurs during serialization or deserialization.</summary>
	// Token: 0x0200064B RID: 1611
	[Serializable]
	public class SerializationException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.SerializationException" /> class with default properties.</summary>
		// Token: 0x06003C4F RID: 15439 RVA: 0x000D1457 File Offset: 0x000CF657
		public SerializationException() : base(SerializationException.s_nullMessage)
		{
			base.HResult = -2146233076;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.SerializationException" /> class with a specified message.</summary>
		/// <param name="message">Indicates the reason why the exception occurred.</param>
		// Token: 0x06003C50 RID: 15440 RVA: 0x000D146F File Offset: 0x000CF66F
		public SerializationException(string message) : base(message)
		{
			base.HResult = -2146233076;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.SerializationException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06003C51 RID: 15441 RVA: 0x000D1483 File Offset: 0x000CF683
		public SerializationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146233076;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.SerializationException" /> class from serialized data.</summary>
		/// <param name="info">The serialization information object holding the serialized object data in the name-value form.</param>
		/// <param name="context">The contextual information about the source or destination of the exception.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06003C52 RID: 15442 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected SerializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x04002715 RID: 10005
		private static string s_nullMessage = "Serialization error.";
	}
}
