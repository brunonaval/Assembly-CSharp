﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace System.IO.Enumeration
{
	// Token: 0x02000B7A RID: 2938
	public class FileSystemEnumerable<TResult> : IEnumerable<!0>, IEnumerable
	{
		// Token: 0x06006B0C RID: 27404 RVA: 0x0016E24C File Offset: 0x0016C44C
		public FileSystemEnumerable(string directory, FileSystemEnumerable<TResult>.FindTransform transform, EnumerationOptions options = null)
		{
			if (directory == null)
			{
				throw new ArgumentNullException("directory");
			}
			this._directory = directory;
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			this._transform = transform;
			this._options = (options ?? EnumerationOptions.Default);
			this._enumerator = new FileSystemEnumerable<TResult>.DelegateEnumerator(this);
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06006B0D RID: 27405 RVA: 0x0016E2A7 File Offset: 0x0016C4A7
		// (set) Token: 0x06006B0E RID: 27406 RVA: 0x0016E2AF File Offset: 0x0016C4AF
		public FileSystemEnumerable<TResult>.FindPredicate ShouldIncludePredicate { get; set; }

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06006B0F RID: 27407 RVA: 0x0016E2B8 File Offset: 0x0016C4B8
		// (set) Token: 0x06006B10 RID: 27408 RVA: 0x0016E2C0 File Offset: 0x0016C4C0
		public FileSystemEnumerable<TResult>.FindPredicate ShouldRecursePredicate { get; set; }

		// Token: 0x06006B11 RID: 27409 RVA: 0x0016E2C9 File Offset: 0x0016C4C9
		public IEnumerator<TResult> GetEnumerator()
		{
			return Interlocked.Exchange<FileSystemEnumerable<TResult>.DelegateEnumerator>(ref this._enumerator, null) ?? new FileSystemEnumerable<TResult>.DelegateEnumerator(this);
		}

		// Token: 0x06006B12 RID: 27410 RVA: 0x0016E2E1 File Offset: 0x0016C4E1
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04003DAC RID: 15788
		private FileSystemEnumerable<TResult>.DelegateEnumerator _enumerator;

		// Token: 0x04003DAD RID: 15789
		private readonly FileSystemEnumerable<TResult>.FindTransform _transform;

		// Token: 0x04003DAE RID: 15790
		private readonly EnumerationOptions _options;

		// Token: 0x04003DAF RID: 15791
		private readonly string _directory;

		// Token: 0x02000B7B RID: 2939
		// (Invoke) Token: 0x06006B14 RID: 27412
		public delegate bool FindPredicate(ref FileSystemEntry entry);

		// Token: 0x02000B7C RID: 2940
		// (Invoke) Token: 0x06006B18 RID: 27416
		public delegate TResult FindTransform(ref FileSystemEntry entry);

		// Token: 0x02000B7D RID: 2941
		private sealed class DelegateEnumerator : FileSystemEnumerator<TResult>
		{
			// Token: 0x06006B1B RID: 27419 RVA: 0x0016E2E9 File Offset: 0x0016C4E9
			public DelegateEnumerator(FileSystemEnumerable<TResult> enumerable) : base(enumerable._directory, enumerable._options)
			{
				this._enumerable = enumerable;
			}

			// Token: 0x06006B1C RID: 27420 RVA: 0x0016E304 File Offset: 0x0016C504
			protected override TResult TransformEntry(ref FileSystemEntry entry)
			{
				return this._enumerable._transform(ref entry);
			}

			// Token: 0x06006B1D RID: 27421 RVA: 0x0016E317 File Offset: 0x0016C517
			protected override bool ShouldRecurseIntoEntry(ref FileSystemEntry entry)
			{
				FileSystemEnumerable<TResult>.FindPredicate shouldRecursePredicate = this._enumerable.ShouldRecursePredicate;
				return shouldRecursePredicate == null || shouldRecursePredicate(ref entry);
			}

			// Token: 0x06006B1E RID: 27422 RVA: 0x0016E330 File Offset: 0x0016C530
			protected override bool ShouldIncludeEntry(ref FileSystemEntry entry)
			{
				FileSystemEnumerable<TResult>.FindPredicate shouldIncludePredicate = this._enumerable.ShouldIncludePredicate;
				return shouldIncludePredicate == null || shouldIncludePredicate(ref entry);
			}

			// Token: 0x04003DB2 RID: 15794
			private readonly FileSystemEnumerable<TResult> _enumerable;
		}
	}
}
