﻿using System;

namespace System.Reflection
{
	/// <summary>Identifies kinds of exception-handling clauses.</summary>
	// Token: 0x0200089D RID: 2205
	[Flags]
	public enum ExceptionHandlingClauseOptions
	{
		/// <summary>The clause accepts all exceptions that derive from a specified type.</summary>
		// Token: 0x04002E8D RID: 11917
		Clause = 0,
		/// <summary>The clause contains user-specified instructions that determine whether the exception should be ignored (that is, whether normal execution should resume), be handled by the associated handler, or be passed on to the next clause.</summary>
		// Token: 0x04002E8E RID: 11918
		Filter = 1,
		/// <summary>The clause is executed whenever the try block exits, whether through normal control flow or because of an unhandled exception.</summary>
		// Token: 0x04002E8F RID: 11919
		Finally = 2,
		/// <summary>The clause is executed if an exception occurs, but not on completion of normal control flow.</summary>
		// Token: 0x04002E90 RID: 11920
		Fault = 4
	}
}
