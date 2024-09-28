using System;

namespace DuloGames.UI
{
	// Token: 0x0200064D RID: 1613
	[Serializable]
	public class UITooltipLineContent
	{
		// Token: 0x060023CA RID: 9162 RVA: 0x000AFE3C File Offset: 0x000AE03C
		public UITooltipLineContent()
		{
			this.LineStyle = UITooltipLines.LineStyle.Default;
			this.CustomLineStyle = string.Empty;
			this.Content = string.Empty;
			this.IsSpacer = false;
		}

		// Token: 0x04001CDB RID: 7387
		public UITooltipLines.LineStyle LineStyle;

		// Token: 0x04001CDC RID: 7388
		public string CustomLineStyle;

		// Token: 0x04001CDD RID: 7389
		public string Content;

		// Token: 0x04001CDE RID: 7390
		public bool IsSpacer;
	}
}
