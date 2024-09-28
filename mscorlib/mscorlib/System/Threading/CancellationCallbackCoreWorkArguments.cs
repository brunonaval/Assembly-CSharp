﻿using System;

namespace System.Threading
{
	// Token: 0x020002AD RID: 685
	internal struct CancellationCallbackCoreWorkArguments
	{
		// Token: 0x06001E3E RID: 7742 RVA: 0x0007019C File Offset: 0x0006E39C
		public CancellationCallbackCoreWorkArguments(SparselyPopulatedArrayFragment<CancellationCallbackInfo> currArrayFragment, int currArrayIndex)
		{
			this._currArrayFragment = currArrayFragment;
			this._currArrayIndex = currArrayIndex;
		}

		// Token: 0x04001A8E RID: 6798
		internal SparselyPopulatedArrayFragment<CancellationCallbackInfo> _currArrayFragment;

		// Token: 0x04001A8F RID: 6799
		internal int _currArrayIndex;
	}
}
