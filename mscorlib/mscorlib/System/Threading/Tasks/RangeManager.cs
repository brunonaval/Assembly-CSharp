﻿using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000331 RID: 817
	internal class RangeManager
	{
		// Token: 0x06002267 RID: 8807 RVA: 0x0007BF30 File Offset: 0x0007A130
		internal RangeManager(long nFromInclusive, long nToExclusive, long nStep, int nNumExpectedWorkers)
		{
			this._nCurrentIndexRangeToAssign = 0;
			this._nStep = nStep;
			if (nNumExpectedWorkers == 1)
			{
				nNumExpectedWorkers = 2;
			}
			long num = nToExclusive - nFromInclusive;
			ulong num2 = (ulong)(num / (long)nNumExpectedWorkers);
			num2 -= num2 % (ulong)nStep;
			if (num2 == 0UL)
			{
				num2 = (ulong)nStep;
			}
			int num3 = (int)(num / (long)num2);
			if (num % (long)num2 != 0L)
			{
				num3++;
			}
			long num4 = (long)num2;
			this._use32BitCurrentIndex = (IntPtr.Size == 4 && num4 <= 2147483647L);
			this._indexRanges = new IndexRange[num3];
			long num5 = nFromInclusive;
			for (int i = 0; i < num3; i++)
			{
				this._indexRanges[i]._nFromInclusive = num5;
				this._indexRanges[i]._nSharedCurrentIndexOffset = null;
				this._indexRanges[i]._bRangeFinished = 0;
				num5 += num4;
				if (num5 < num5 - num4 || num5 > nToExclusive)
				{
					num5 = nToExclusive;
				}
				this._indexRanges[i]._nToExclusive = num5;
			}
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x0007C018 File Offset: 0x0007A218
		internal RangeWorker RegisterNewWorker()
		{
			int nInitialRange = (Interlocked.Increment(ref this._nCurrentIndexRangeToAssign) - 1) % this._indexRanges.Length;
			return new RangeWorker(this._indexRanges, nInitialRange, this._nStep, this._use32BitCurrentIndex);
		}

		// Token: 0x04001C4C RID: 7244
		internal readonly IndexRange[] _indexRanges;

		// Token: 0x04001C4D RID: 7245
		internal readonly bool _use32BitCurrentIndex;

		// Token: 0x04001C4E RID: 7246
		internal int _nCurrentIndexRangeToAssign;

		// Token: 0x04001C4F RID: 7247
		internal long _nStep;
	}
}
