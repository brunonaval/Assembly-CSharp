﻿using System;
using System.Runtime.Serialization;

namespace System.Threading
{
	/// <summary>The exception that is thrown when one thread acquires a <see cref="T:System.Threading.Mutex" /> object that another thread has abandoned by exiting without releasing it.</summary>
	// Token: 0x0200027F RID: 639
	[Serializable]
	public class AbandonedMutexException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with default values.</summary>
		// Token: 0x06001D50 RID: 7504 RVA: 0x0006D98F File Offset: 0x0006BB8F
		public AbandonedMutexException() : base("The wait completed due to an abandoned mutex.")
		{
			base.HResult = -2146233043;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message.</summary>
		/// <param name="message">An error message that explains the reason for the exception.</param>
		// Token: 0x06001D51 RID: 7505 RVA: 0x0006D9AE File Offset: 0x0006BBAE
		public AbandonedMutexException(string message) : base(message)
		{
			base.HResult = -2146233043;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message and inner exception.</summary>
		/// <param name="message">An error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06001D52 RID: 7506 RVA: 0x0006D9C9 File Offset: 0x0006BBC9
		public AbandonedMutexException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233043;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified index for the abandoned mutex, if applicable, and a <see cref="T:System.Threading.Mutex" /> object that represents the mutex.</summary>
		/// <param name="location">The index of the abandoned mutex in the array of wait handles if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, or -1 if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitOne" /> or <see cref="Overload:System.Threading.WaitHandle.WaitAll" /> methods.</param>
		/// <param name="handle">A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex.</param>
		// Token: 0x06001D53 RID: 7507 RVA: 0x0006D9E5 File Offset: 0x0006BBE5
		public AbandonedMutexException(int location, WaitHandle handle) : base("The wait completed due to an abandoned mutex.")
		{
			base.HResult = -2146233043;
			this.SetupException(location, handle);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message, the index of the abandoned mutex, if applicable, and the abandoned mutex.</summary>
		/// <param name="message">An error message that explains the reason for the exception.</param>
		/// <param name="location">The index of the abandoned mutex in the array of wait handles if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, or -1 if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitOne" /> or <see cref="Overload:System.Threading.WaitHandle.WaitAll" /> methods.</param>
		/// <param name="handle">A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex.</param>
		// Token: 0x06001D54 RID: 7508 RVA: 0x0006DA0C File Offset: 0x0006BC0C
		public AbandonedMutexException(string message, int location, WaitHandle handle) : base(message)
		{
			base.HResult = -2146233043;
			this.SetupException(location, handle);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with a specified error message, the inner exception, the index for the abandoned mutex, if applicable, and a <see cref="T:System.Threading.Mutex" /> object that represents the mutex.</summary>
		/// <param name="message">An error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		/// <param name="location">The index of the abandoned mutex in the array of wait handles if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, or -1 if the exception is thrown for the <see cref="Overload:System.Threading.WaitHandle.WaitOne" /> or <see cref="Overload:System.Threading.WaitHandle.WaitAll" /> methods.</param>
		/// <param name="handle">A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex.</param>
		// Token: 0x06001D55 RID: 7509 RVA: 0x0006DA2F File Offset: 0x0006BC2F
		public AbandonedMutexException(string message, Exception inner, int location, WaitHandle handle) : base(message, inner)
		{
			base.HResult = -2146233043;
			this.SetupException(location, handle);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.AbandonedMutexException" /> class with serialized data.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains contextual information about the source or destination.</param>
		// Token: 0x06001D56 RID: 7510 RVA: 0x0006DA54 File Offset: 0x0006BC54
		protected AbandonedMutexException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x0006DA65 File Offset: 0x0006BC65
		private void SetupException(int location, WaitHandle handle)
		{
			this._mutexIndex = location;
			if (handle != null)
			{
				this._mutex = (handle as Mutex);
			}
		}

		/// <summary>Gets the abandoned mutex that caused the exception, if known.</summary>
		/// <returns>A <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex, or <see langword="null" /> if the abandoned mutex could not be identified.</returns>
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001D58 RID: 7512 RVA: 0x0006DA7D File Offset: 0x0006BC7D
		public Mutex Mutex
		{
			get
			{
				return this._mutex;
			}
		}

		/// <summary>Gets the index of the abandoned mutex that caused the exception, if known.</summary>
		/// <returns>The index, in the array of wait handles passed to the <see cref="Overload:System.Threading.WaitHandle.WaitAny" /> method, of the <see cref="T:System.Threading.Mutex" /> object that represents the abandoned mutex, or -1 if the index of the abandoned mutex could not be determined.</returns>
		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x0006DA85 File Offset: 0x0006BC85
		public int MutexIndex
		{
			get
			{
				return this._mutexIndex;
			}
		}

		// Token: 0x04001A1E RID: 6686
		private int _mutexIndex = -1;

		// Token: 0x04001A1F RID: 6687
		private Mutex _mutex;
	}
}
