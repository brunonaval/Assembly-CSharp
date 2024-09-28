﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Security.Policy
{
	/// <summary>The exception that is thrown when policy forbids code to run.</summary>
	// Token: 0x0200041B RID: 1051
	[ComVisible(true)]
	[Serializable]
	public class PolicyException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.PolicyException" /> class with default properties.</summary>
		// Token: 0x06002AE4 RID: 10980 RVA: 0x0009AEC0 File Offset: 0x000990C0
		public PolicyException() : base(Locale.GetText("Cannot run because of policy."))
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.PolicyException" /> class with a specified error message.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		// Token: 0x06002AE5 RID: 10981 RVA: 0x0006E699 File Offset: 0x0006C899
		public PolicyException(string message) : base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.PolicyException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		// Token: 0x06002AE6 RID: 10982 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected PolicyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.PolicyException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="exception">The exception that is the cause of the current exception. If the <paramref name="exception" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06002AE7 RID: 10983 RVA: 0x0006E6A2 File Offset: 0x0006C8A2
		public PolicyException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
