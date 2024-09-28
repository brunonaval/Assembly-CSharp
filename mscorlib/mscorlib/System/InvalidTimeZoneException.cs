﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when time zone information is invalid.</summary>
	// Token: 0x0200014E RID: 334
	[TypeForwardedFrom("System.Core, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
	[Serializable]
	public class InvalidTimeZoneException : Exception
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidTimeZoneException" /> class with a system-supplied message.</summary>
		// Token: 0x06000C96 RID: 3222 RVA: 0x00004B05 File Offset: 0x00002D05
		public InvalidTimeZoneException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidTimeZoneException" /> class with the specified message string.</summary>
		/// <param name="message">A string that describes the exception.</param>
		// Token: 0x06000C97 RID: 3223 RVA: 0x000328A6 File Offset: 0x00030AA6
		public InvalidTimeZoneException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidTimeZoneException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">A string that describes the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		// Token: 0x06000C98 RID: 3224 RVA: 0x000328AF File Offset: 0x00030AAF
		public InvalidTimeZoneException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.InvalidTimeZoneException" /> class from serialized data.</summary>
		/// <param name="info">The object that contains the serialized data.</param>
		/// <param name="context">The stream that contains the serialized data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="context" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06000C99 RID: 3225 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected InvalidTimeZoneException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
