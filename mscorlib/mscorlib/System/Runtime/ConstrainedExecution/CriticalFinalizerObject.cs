using System;

namespace System.Runtime.ConstrainedExecution
{
	/// <summary>Ensures that all finalization code in derived classes is marked as critical.</summary>
	// Token: 0x020007D7 RID: 2007
	public abstract class CriticalFinalizerObject
	{
		/// <summary>Releases all the resources used by the <see cref="T:System.Runtime.ConstrainedExecution.CriticalFinalizerObject" /> class.</summary>
		// Token: 0x060045B7 RID: 17847 RVA: 0x000E5100 File Offset: 0x000E3300
		~CriticalFinalizerObject()
		{
		}
	}
}
