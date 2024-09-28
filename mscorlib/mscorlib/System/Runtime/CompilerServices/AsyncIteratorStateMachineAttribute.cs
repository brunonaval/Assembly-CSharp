using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007DA RID: 2010
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class AsyncIteratorStateMachineAttribute : StateMachineAttribute
	{
		// Token: 0x060045C0 RID: 17856 RVA: 0x000E5196 File Offset: 0x000E3396
		public AsyncIteratorStateMachineAttribute(Type stateMachineType) : base(stateMachineType)
		{
		}
	}
}
