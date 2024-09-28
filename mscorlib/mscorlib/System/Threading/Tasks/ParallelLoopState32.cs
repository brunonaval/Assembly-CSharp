using System;

namespace System.Threading.Tasks
{
	// Token: 0x02000329 RID: 809
	internal class ParallelLoopState32 : ParallelLoopState
	{
		// Token: 0x06002244 RID: 8772 RVA: 0x0007BA3C File Offset: 0x00079C3C
		internal ParallelLoopState32(ParallelLoopStateFlags32 sharedParallelStateFlags) : base(sharedParallelStateFlags)
		{
			this._sharedParallelStateFlags = sharedParallelStateFlags;
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x0007BA4C File Offset: 0x00079C4C
		// (set) Token: 0x06002246 RID: 8774 RVA: 0x0007BA54 File Offset: 0x00079C54
		internal int CurrentIteration
		{
			get
			{
				return this._currentIteration;
			}
			set
			{
				this._currentIteration = value;
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x0007BA5D File Offset: 0x00079C5D
		internal override bool InternalShouldExitCurrentIteration
		{
			get
			{
				return this._sharedParallelStateFlags.ShouldExitLoop(this.CurrentIteration);
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06002248 RID: 8776 RVA: 0x0007BA70 File Offset: 0x00079C70
		internal override long? InternalLowestBreakIteration
		{
			get
			{
				return this._sharedParallelStateFlags.NullableLowestBreakIteration;
			}
		}

		// Token: 0x06002249 RID: 8777 RVA: 0x0007BA7D File Offset: 0x00079C7D
		internal override void InternalBreak()
		{
			ParallelLoopState.Break(this.CurrentIteration, this._sharedParallelStateFlags);
		}

		// Token: 0x04001C34 RID: 7220
		private readonly ParallelLoopStateFlags32 _sharedParallelStateFlags;

		// Token: 0x04001C35 RID: 7221
		private int _currentIteration;
	}
}
