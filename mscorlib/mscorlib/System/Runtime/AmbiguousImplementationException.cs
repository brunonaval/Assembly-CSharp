using System;
using System.Runtime.Serialization;

namespace System.Runtime
{
	// Token: 0x0200054E RID: 1358
	[Serializable]
	public sealed class AmbiguousImplementationException : Exception
	{
		// Token: 0x060035AB RID: 13739 RVA: 0x000C1F7F File Offset: 0x000C017F
		public AmbiguousImplementationException() : base("Ambiguous implementation found.")
		{
			base.HResult = -2146234262;
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000C1F97 File Offset: 0x000C0197
		public AmbiguousImplementationException(string message) : base(message)
		{
			base.HResult = -2146234262;
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000C1FAB File Offset: 0x000C01AB
		public AmbiguousImplementationException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146234262;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x00020FAB File Offset: 0x0001F1AB
		private AmbiguousImplementationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
