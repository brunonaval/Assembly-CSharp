using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200064E RID: 1614
	public class UITooltipLines
	{
		// Token: 0x060023CB RID: 9163 RVA: 0x000AFE68 File Offset: 0x000AE068
		public void AddLine(string leftContent, string rightContent)
		{
			this.lineList.Add(new UITooltipLines.Line(leftContent, rightContent, true, new RectOffset(), UITooltipLines.LineStyle.Default, ""));
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000AFE88 File Offset: 0x000AE088
		public void AddLine(string leftContent, string rightContent, RectOffset padding)
		{
			this.lineList.Add(new UITooltipLines.Line(leftContent, rightContent, true, padding, UITooltipLines.LineStyle.Default, ""));
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000AFEA4 File Offset: 0x000AE0A4
		public void AddLine(string content)
		{
			this.lineList.Add(new UITooltipLines.Line(content, string.Empty, true, new RectOffset(), UITooltipLines.LineStyle.Default, ""));
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000AFEC8 File Offset: 0x000AE0C8
		public void AddLine(string content, RectOffset padding)
		{
			this.lineList.Add(new UITooltipLines.Line(content, string.Empty, true, padding, UITooltipLines.LineStyle.Default, ""));
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000AFEE8 File Offset: 0x000AE0E8
		public void AddLine(string content, RectOffset padding, UITooltipLines.LineStyle style)
		{
			this.lineList.Add(new UITooltipLines.Line(content, string.Empty, true, padding, style, ""));
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000AFF08 File Offset: 0x000AE108
		public void AddLine(string content, RectOffset padding, string customStyle)
		{
			this.lineList.Add(new UITooltipLines.Line(content, string.Empty, true, padding, UITooltipLines.LineStyle.Custom, customStyle));
		}

		// Token: 0x060023D1 RID: 9169 RVA: 0x000AFF24 File Offset: 0x000AE124
		public void AddLine(string leftContent, string rightContent, RectOffset padding, UITooltipLines.LineStyle style)
		{
			this.lineList.Add(new UITooltipLines.Line(leftContent, rightContent, true, padding, style, ""));
		}

		// Token: 0x060023D2 RID: 9170 RVA: 0x000AFF41 File Offset: 0x000AE141
		public void AddLine(string leftContent, string rightContent, RectOffset padding, string customStyle)
		{
			this.lineList.Add(new UITooltipLines.Line(leftContent, rightContent, true, padding, UITooltipLines.LineStyle.Custom, customStyle));
		}

		// Token: 0x060023D3 RID: 9171 RVA: 0x000AFF5A File Offset: 0x000AE15A
		public void AddColumn(string content)
		{
			this.AddColumn(content, UITooltipLines.LineStyle.Default, "");
		}

		// Token: 0x060023D4 RID: 9172 RVA: 0x000AFF69 File Offset: 0x000AE169
		public void AddColumn(string content, UITooltipLines.LineStyle style)
		{
			this.AddColumn(content, style, "");
		}

		// Token: 0x060023D5 RID: 9173 RVA: 0x000AFF78 File Offset: 0x000AE178
		public void AddColumn(string content, string customStyle)
		{
			this.AddColumn(content, UITooltipLines.LineStyle.Custom, customStyle);
		}

		// Token: 0x060023D6 RID: 9174 RVA: 0x000AFF84 File Offset: 0x000AE184
		public void AddColumn(string content, UITooltipLines.LineStyle style, string customStyle)
		{
			if (this.lineList.Count == 0)
			{
				this.lineList.Add(new UITooltipLines.Line(content, string.Empty, false, new RectOffset(), style, customStyle));
				return;
			}
			UITooltipLines.Line line = this.lineList[this.lineList.Count - 1];
			if (!line.isComplete)
			{
				line.right = content;
				line.isComplete = true;
				return;
			}
			this.lineList.Add(new UITooltipLines.Line(content, string.Empty, false, new RectOffset(), style, customStyle));
		}

		// Token: 0x04001CDF RID: 7391
		public UITooltipLines.Lines lineList = new UITooltipLines.Lines();

		// Token: 0x0200064F RID: 1615
		public enum LineStyle
		{
			// Token: 0x04001CE1 RID: 7393
			Default,
			// Token: 0x04001CE2 RID: 7394
			Title,
			// Token: 0x04001CE3 RID: 7395
			Description,
			// Token: 0x04001CE4 RID: 7396
			Custom
		}

		// Token: 0x02000650 RID: 1616
		[Serializable]
		public class Line
		{
			// Token: 0x060023D8 RID: 9176 RVA: 0x000B001E File Offset: 0x000AE21E
			public Line(string left, string right, bool isComplete, RectOffset padding, UITooltipLines.LineStyle style, string customStyle)
			{
				this.left = left;
				this.right = right;
				this.isComplete = isComplete;
				this.padding = padding;
				this.style = style;
				this.customStyle = customStyle;
			}

			// Token: 0x04001CE5 RID: 7397
			public string left;

			// Token: 0x04001CE6 RID: 7398
			public string right;

			// Token: 0x04001CE7 RID: 7399
			public bool isComplete;

			// Token: 0x04001CE8 RID: 7400
			public RectOffset padding;

			// Token: 0x04001CE9 RID: 7401
			public UITooltipLines.LineStyle style;

			// Token: 0x04001CEA RID: 7402
			public string customStyle;
		}

		// Token: 0x02000651 RID: 1617
		public class Lines : List<UITooltipLines.Line>
		{
		}
	}
}
