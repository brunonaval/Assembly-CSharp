using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000655 RID: 1621
	[Serializable]
	public class UITooltipLineStyle : IComparable<UITooltipLineStyle>
	{
		// Token: 0x060023DB RID: 9179 RVA: 0x000B00B1 File Offset: 0x000AE2B1
		public UITooltipLineStyle()
		{
			this.Defaults();
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000B00C6 File Offset: 0x000AE2C6
		public UITooltipLineStyle(bool displayName)
		{
			this.Defaults();
			this.DisplayName = displayName;
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x000B00E4 File Offset: 0x000AE2E4
		private void Defaults()
		{
			this.Name = "";
			this.TextFont = FontData.defaultFontData.font;
			this.TextFontStyle = FontData.defaultFontData.fontStyle;
			this.TextFontSize = FontData.defaultFontData.fontSize;
			this.TextFontLineSpacing = FontData.defaultFontData.lineSpacing;
			this.OverrideTextAlignment = OverrideTextAlignment.No;
			this.TextFontColor = Color.white;
			this.TextEffects = new UITooltipTextEffect[0];
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000B015A File Offset: 0x000AE35A
		public int CompareTo(UITooltipLineStyle other)
		{
			return this.Name.CompareTo(other.Name);
		}

		// Token: 0x04001CF7 RID: 7415
		public string Name;

		// Token: 0x04001CF8 RID: 7416
		public Font TextFont;

		// Token: 0x04001CF9 RID: 7417
		public FontStyle TextFontStyle;

		// Token: 0x04001CFA RID: 7418
		public int TextFontSize;

		// Token: 0x04001CFB RID: 7419
		public float TextFontLineSpacing;

		// Token: 0x04001CFC RID: 7420
		public OverrideTextAlignment OverrideTextAlignment;

		// Token: 0x04001CFD RID: 7421
		public Color TextFontColor;

		// Token: 0x04001CFE RID: 7422
		public UITooltipTextEffect[] TextEffects;

		// Token: 0x04001CFF RID: 7423
		public bool DisplayName = true;
	}
}
