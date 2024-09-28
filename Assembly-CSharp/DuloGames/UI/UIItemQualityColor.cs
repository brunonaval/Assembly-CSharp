using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005EB RID: 1515
	public class UIItemQualityColor
	{
		// Token: 0x06002169 RID: 8553 RVA: 0x000A7614 File Offset: 0x000A5814
		public static string GetHexColor(UIItemQuality quality)
		{
			switch (quality)
			{
			case UIItemQuality.Poor:
				return "9d9d9dff";
			case UIItemQuality.Common:
				return "ffffffff";
			case UIItemQuality.Uncommon:
				return "1eff00ff";
			case UIItemQuality.Rare:
				return "0070ffff";
			case UIItemQuality.Epic:
				return "a335eeff";
			case UIItemQuality.Legendary:
				return "ff8000ff";
			default:
				return "9d9d9dff";
			}
		}

		// Token: 0x0600216A RID: 8554 RVA: 0x000A766A File Offset: 0x000A586A
		public static Color GetColor(UIItemQuality quality)
		{
			return CommonColorBuffer.StringToColor(UIItemQualityColor.GetHexColor(quality));
		}

		// Token: 0x04001B5C RID: 7004
		public const string Poor = "9d9d9dff";

		// Token: 0x04001B5D RID: 7005
		public const string Common = "ffffffff";

		// Token: 0x04001B5E RID: 7006
		public const string Uncommon = "1eff00ff";

		// Token: 0x04001B5F RID: 7007
		public const string Rare = "0070ffff";

		// Token: 0x04001B60 RID: 7008
		public const string Epic = "a335eeff";

		// Token: 0x04001B61 RID: 7009
		public const string Legendary = "ff8000ff";
	}
}
