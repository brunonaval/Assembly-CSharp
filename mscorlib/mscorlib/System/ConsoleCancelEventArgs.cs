﻿using System;
using Unity;

namespace System
{
	/// <summary>Provides data for the <see cref="E:System.Console.CancelKeyPress" /> event. This class cannot be inherited.</summary>
	// Token: 0x020001BD RID: 445
	[Serializable]
	public sealed class ConsoleCancelEventArgs : EventArgs
	{
		// Token: 0x06001334 RID: 4916 RVA: 0x0004D864 File Offset: 0x0004BA64
		internal ConsoleCancelEventArgs(ConsoleSpecialKey type)
		{
			this._type = type;
		}

		/// <summary>Gets or sets a value that indicates whether simultaneously pressing the <see cref="F:System.ConsoleModifiers.Control" /> modifier key and the <see cref="F:System.ConsoleKey.C" /> console key (Ctrl+C) or the Ctrl+Break keys terminates the current process. The default is <see langword="false" />, which terminates the current process.</summary>
		/// <returns>
		///   <see langword="true" /> if the current process should resume when the event handler concludes; <see langword="false" /> if the current process should terminate. The default value is <see langword="false" />; the current process terminates when the event handler returns. If <see langword="true" />, the current process continues.</returns>
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06001335 RID: 4917 RVA: 0x0004D873 File Offset: 0x0004BA73
		// (set) Token: 0x06001336 RID: 4918 RVA: 0x0004D87B File Offset: 0x0004BA7B
		public bool Cancel { get; set; }

		/// <summary>Gets the combination of modifier and console keys that interrupted the current process.</summary>
		/// <returns>One of the enumeration values that specifies the key combination that interrupted the current process. There is no default value.</returns>
		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06001337 RID: 4919 RVA: 0x0004D884 File Offset: 0x0004BA84
		public ConsoleSpecialKey SpecialKey
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x06001338 RID: 4920 RVA: 0x000173AD File Offset: 0x000155AD
		internal ConsoleCancelEventArgs()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001395 RID: 5013
		private readonly ConsoleSpecialKey _type;
	}
}
