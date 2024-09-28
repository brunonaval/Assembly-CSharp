using System;

namespace GooglePlayGames.BasicApi
{
	// Token: 0x020006A1 RID: 1697
	public class CommonTypesUtil
	{
		// Token: 0x0600255F RID: 9567 RVA: 0x000B4BBC File Offset: 0x000B2DBC
		public static bool StatusIsSuccess(ResponseStatus status)
		{
			return status > (ResponseStatus)0;
		}
	}
}
