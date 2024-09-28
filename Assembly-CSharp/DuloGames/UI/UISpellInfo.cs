using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005EC RID: 1516
	[Serializable]
	public class UISpellInfo
	{
		// Token: 0x04001B62 RID: 7010
		public int ID;

		// Token: 0x04001B63 RID: 7011
		public string Name;

		// Token: 0x04001B64 RID: 7012
		public Sprite Icon;

		// Token: 0x04001B65 RID: 7013
		public string Description;

		// Token: 0x04001B66 RID: 7014
		public float Range;

		// Token: 0x04001B67 RID: 7015
		public float Cooldown;

		// Token: 0x04001B68 RID: 7016
		public float CastTime;

		// Token: 0x04001B69 RID: 7017
		public float PowerCost;

		// Token: 0x04001B6A RID: 7018
		[BitMask(typeof(UISpellInfo_Flags))]
		public UISpellInfo_Flags Flags;
	}
}
