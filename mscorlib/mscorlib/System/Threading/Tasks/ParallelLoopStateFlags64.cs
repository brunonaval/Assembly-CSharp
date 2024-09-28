﻿using System;

namespace System.Threading.Tasks
{
	// Token: 0x0200032D RID: 813
	internal class ParallelLoopStateFlags64 : ParallelLoopStateFlags
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x0600225C RID: 8796 RVA: 0x0007BC3D File Offset: 0x00079E3D
		internal long LowestBreakIteration
		{
			get
			{
				if (IntPtr.Size >= 8)
				{
					return this._lowestBreakIteration;
				}
				return Interlocked.Read(ref this._lowestBreakIteration);
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x0007BC5C File Offset: 0x00079E5C
		internal long? NullableLowestBreakIteration
		{
			get
			{
				if (this._lowestBreakIteration == 9223372036854775807L)
				{
					return null;
				}
				if (IntPtr.Size >= 8)
				{
					return new long?(this._lowestBreakIteration);
				}
				return new long?(Interlocked.Read(ref this._lowestBreakIteration));
			}
		}

		// Token: 0x0600225E RID: 8798 RVA: 0x0007BCA8 File Offset: 0x00079EA8
		internal bool ShouldExitLoop(long CallerIteration)
		{
			int loopStateFlags = base.LoopStateFlags;
			return loopStateFlags != 0 && ((loopStateFlags & 13) != 0 || ((loopStateFlags & 2) != 0 && CallerIteration > this.LowestBreakIteration));
		}

		// Token: 0x0600225F RID: 8799 RVA: 0x0007BCDC File Offset: 0x00079EDC
		internal bool ShouldExitLoop()
		{
			int loopStateFlags = base.LoopStateFlags;
			return loopStateFlags != 0 && (loopStateFlags & 9) != 0;
		}

		// Token: 0x04001C3F RID: 7231
		internal long _lowestBreakIteration = long.MaxValue;
	}
}
