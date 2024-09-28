using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000654 RID: 1620
	[Serializable]
	public class UITooltipTextEffect
	{
		// Token: 0x060023DA RID: 9178 RVA: 0x000B005C File Offset: 0x000AE25C
		public UITooltipTextEffect()
		{
			this.Effect = UITooltipTextEffectType.Shadow;
			this.EffectColor = new Color(0f, 0f, 0f, 128f);
			this.EffectDistance = new Vector2(1f, -1f);
			this.UseGraphicAlpha = true;
		}

		// Token: 0x04001CF3 RID: 7411
		public UITooltipTextEffectType Effect;

		// Token: 0x04001CF4 RID: 7412
		public Color EffectColor;

		// Token: 0x04001CF5 RID: 7413
		public Vector2 EffectDistance;

		// Token: 0x04001CF6 RID: 7414
		public bool UseGraphicAlpha;
	}
}
