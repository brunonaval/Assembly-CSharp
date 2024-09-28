﻿using System;

namespace System.Threading
{
	/// <summary>The class that provides data change information to <see cref="T:System.Threading.AsyncLocal`1" /> instances that register for change notifications.</summary>
	/// <typeparam name="T">The type of the data.</typeparam>
	// Token: 0x02000283 RID: 643
	public readonly struct AsyncLocalValueChangedArgs<T>
	{
		/// <summary>Gets the data's previous value.</summary>
		/// <returns>The data's previous value.</returns>
		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001D60 RID: 7520 RVA: 0x0006DB26 File Offset: 0x0006BD26
		public T PreviousValue { get; }

		/// <summary>Gets the data's current value.</summary>
		/// <returns>The data's current value.</returns>
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0006DB2E File Offset: 0x0006BD2E
		public T CurrentValue { get; }

		/// <summary>Returns a value that indicates whether the value changes because of a change of execution context.</summary>
		/// <returns>
		///   <see langword="true" /> if the value changed because of a change of execution context; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x0006DB36 File Offset: 0x0006BD36
		public bool ThreadContextChanged { get; }

		// Token: 0x06001D63 RID: 7523 RVA: 0x0006DB3E File Offset: 0x0006BD3E
		internal AsyncLocalValueChangedArgs(T previousValue, T currentValue, bool contextChanged)
		{
			this = default(AsyncLocalValueChangedArgs<T>);
			this.PreviousValue = previousValue;
			this.CurrentValue = currentValue;
			this.ThreadContextChanged = contextChanged;
		}
	}
}
