using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000645 RID: 1605
	[Serializable]
	public struct SpriteStateExtended
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06002368 RID: 9064 RVA: 0x000AE417 File Offset: 0x000AC617
		// (set) Token: 0x06002369 RID: 9065 RVA: 0x000AE41F File Offset: 0x000AC61F
		public Sprite highlightedSprite
		{
			get
			{
				return this.m_HighlightedSprite;
			}
			set
			{
				this.m_HighlightedSprite = value;
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x000AE428 File Offset: 0x000AC628
		// (set) Token: 0x0600236B RID: 9067 RVA: 0x000AE430 File Offset: 0x000AC630
		public Sprite pressedSprite
		{
			get
			{
				return this.m_PressedSprite;
			}
			set
			{
				this.m_PressedSprite = value;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x000AE439 File Offset: 0x000AC639
		// (set) Token: 0x0600236D RID: 9069 RVA: 0x000AE441 File Offset: 0x000AC641
		public Sprite activeSprite
		{
			get
			{
				return this.m_ActiveSprite;
			}
			set
			{
				this.m_ActiveSprite = value;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600236E RID: 9070 RVA: 0x000AE44A File Offset: 0x000AC64A
		// (set) Token: 0x0600236F RID: 9071 RVA: 0x000AE452 File Offset: 0x000AC652
		public Sprite activeHighlightedSprite
		{
			get
			{
				return this.m_ActiveHighlightedSprite;
			}
			set
			{
				this.m_ActiveHighlightedSprite = value;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x000AE45B File Offset: 0x000AC65B
		// (set) Token: 0x06002371 RID: 9073 RVA: 0x000AE463 File Offset: 0x000AC663
		public Sprite activePressedSprite
		{
			get
			{
				return this.m_ActivePressedSprite;
			}
			set
			{
				this.m_ActivePressedSprite = value;
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06002372 RID: 9074 RVA: 0x000AE46C File Offset: 0x000AC66C
		// (set) Token: 0x06002373 RID: 9075 RVA: 0x000AE474 File Offset: 0x000AC674
		public Sprite disabledSprite
		{
			get
			{
				return this.m_DisabledSprite;
			}
			set
			{
				this.m_DisabledSprite = value;
			}
		}

		// Token: 0x04001CA5 RID: 7333
		[SerializeField]
		private Sprite m_HighlightedSprite;

		// Token: 0x04001CA6 RID: 7334
		[SerializeField]
		private Sprite m_PressedSprite;

		// Token: 0x04001CA7 RID: 7335
		[SerializeField]
		private Sprite m_ActiveSprite;

		// Token: 0x04001CA8 RID: 7336
		[SerializeField]
		private Sprite m_ActiveHighlightedSprite;

		// Token: 0x04001CA9 RID: 7337
		[SerializeField]
		private Sprite m_ActivePressedSprite;

		// Token: 0x04001CAA RID: 7338
		[SerializeField]
		private Sprite m_DisabledSprite;
	}
}
