﻿using System;

namespace System.Threading
{
	/// <summary>Encapsulates and propagates the host execution context across threads.</summary>
	// Token: 0x020002EE RID: 750
	[MonoTODO("Useless until the runtime supports it")]
	public class HostExecutionContext : IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.HostExecutionContext" /> class.</summary>
		// Token: 0x060020A8 RID: 8360 RVA: 0x00076971 File Offset: 0x00074B71
		public HostExecutionContext()
		{
			this._state = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.HostExecutionContext" /> class using the specified state.</summary>
		/// <param name="state">An object representing the host execution context state.</param>
		// Token: 0x060020A9 RID: 8361 RVA: 0x00076980 File Offset: 0x00074B80
		public HostExecutionContext(object state)
		{
			this._state = state;
		}

		/// <summary>Creates a copy of the current host execution context.</summary>
		/// <returns>A <see cref="T:System.Threading.HostExecutionContext" /> object representing the host context for the current thread.</returns>
		// Token: 0x060020AA RID: 8362 RVA: 0x0007698F File Offset: 0x00074B8F
		public virtual HostExecutionContext CreateCopy()
		{
			return new HostExecutionContext(this._state);
		}

		/// <summary>Gets or sets the state of the host execution context.</summary>
		/// <returns>An object representing the host execution context state.</returns>
		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060020AB RID: 8363 RVA: 0x0007699C File Offset: 0x00074B9C
		// (set) Token: 0x060020AC RID: 8364 RVA: 0x000769A4 File Offset: 0x00074BA4
		protected internal object State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		/// <summary>Releases all resources used by the current instance of the <see cref="T:System.Threading.HostExecutionContext" /> class.</summary>
		// Token: 0x060020AD RID: 8365 RVA: 0x000769AD File Offset: 0x00074BAD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>When overridden in a derived class, releases the unmanaged resources used by the <see cref="T:System.Threading.WaitHandle" />, and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///   <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
		// Token: 0x060020AE RID: 8366 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x04001B6C RID: 7020
		private object _state;
	}
}
