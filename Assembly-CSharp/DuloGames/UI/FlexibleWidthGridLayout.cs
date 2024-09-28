using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x02000617 RID: 1559
	public class FlexibleWidthGridLayout : GridLayoutGroup
	{
		// Token: 0x0600226B RID: 8811 RVA: 0x000AADA5 File Offset: 0x000A8FA5
		public override void SetLayoutHorizontal()
		{
			this.UpdateCellSize();
			base.SetLayoutHorizontal();
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000AADB3 File Offset: 0x000A8FB3
		public override void SetLayoutVertical()
		{
			this.UpdateCellSize();
			base.SetLayoutVertical();
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000AADC4 File Offset: 0x000A8FC4
		private void UpdateCellSize()
		{
			float x = (base.rectTransform.rect.size.x - (float)base.padding.horizontal - base.spacing.x * (float)(base.constraintCount - 1)) / (float)base.constraintCount;
			base.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			base.cellSize = new Vector2(x, base.cellSize.y);
		}
	}
}
