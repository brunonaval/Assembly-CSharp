﻿using System;
using System.Diagnostics;

namespace System.Buffers
{
	// Token: 0x02000AEA RID: 2794
	internal sealed class ReadOnlySequenceDebugView<T>
	{
		// Token: 0x06006361 RID: 25441 RVA: 0x0014C938 File Offset: 0x0014AB38
		public ReadOnlySequenceDebugView(ReadOnlySequence<T> sequence)
		{
			this._array = sequence.ToArray<T>();
			int num = 0;
			foreach (ReadOnlyMemory<T> readOnlyMemory in sequence)
			{
				num++;
			}
			ReadOnlyMemory<T>[] array = new ReadOnlyMemory<T>[num];
			int num2 = 0;
			foreach (ReadOnlyMemory<T> readOnlyMemory2 in sequence)
			{
				array[num2] = readOnlyMemory2;
				num2++;
			}
			this._segments = new ReadOnlySequenceDebugView<T>.ReadOnlySequenceDebugViewSegments
			{
				Segments = array
			};
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06006362 RID: 25442 RVA: 0x0014C9C3 File Offset: 0x0014ABC3
		public ReadOnlySequenceDebugView<T>.ReadOnlySequenceDebugViewSegments BufferSegments
		{
			get
			{
				return this._segments;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06006363 RID: 25443 RVA: 0x0014C9CB File Offset: 0x0014ABCB
		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public T[] Items
		{
			get
			{
				return this._array;
			}
		}

		// Token: 0x04003A7B RID: 14971
		private readonly T[] _array;

		// Token: 0x04003A7C RID: 14972
		private readonly ReadOnlySequenceDebugView<T>.ReadOnlySequenceDebugViewSegments _segments;

		// Token: 0x02000AEB RID: 2795
		[DebuggerDisplay("Count: {Segments.Length}", Name = "Segments")]
		public struct ReadOnlySequenceDebugViewSegments
		{
			// Token: 0x17001192 RID: 4498
			// (get) Token: 0x06006364 RID: 25444 RVA: 0x0014C9D3 File Offset: 0x0014ABD3
			// (set) Token: 0x06006365 RID: 25445 RVA: 0x0014C9DB File Offset: 0x0014ABDB
			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public ReadOnlyMemory<T>[] Segments { readonly get; set; }
		}
	}
}
