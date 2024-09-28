using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Field)]
	public class SyncVarAttribute : PropertyAttribute
	{
		// Token: 0x04000006 RID: 6
		public string hook;
	}
}
