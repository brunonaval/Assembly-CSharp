using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public class LagCompensationSettings
	{
		// Token: 0x04000021 RID: 33
		[Header("Buffering")]
		[Tooltip("Keep this many past snapshots in the buffer. The larger this is, the further we can rewind into the past.\nMaximum rewind time := historyAmount * captureInterval")]
		public int historyLimit = 6;

		// Token: 0x04000022 RID: 34
		[Tooltip("Capture state every 'captureInterval' seconds. Larger values will space out the captures more, which gives a longer history but with possible gaps inbetween.\nSmaller values will have fewer gaps, with shorter history.")]
		public float captureInterval = 0.1f;
	}
}
