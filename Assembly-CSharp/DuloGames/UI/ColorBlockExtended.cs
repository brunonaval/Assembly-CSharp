using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000644 RID: 1604
	[Serializable]
	public struct ColorBlockExtended
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x000AE268 File Offset: 0x000AC468
		public static ColorBlockExtended defaultColorBlock
		{
			get
			{
				return new ColorBlockExtended
				{
					m_NormalColor = new Color32(128, 128, 128, 128),
					m_HighlightedColor = new Color32(128, 128, 128, 178),
					m_PressedColor = new Color32(88, 88, 88, 178),
					m_ActiveColor = new Color32(128, 128, 128, 128),
					m_ActiveHighlightedColor = new Color32(128, 128, 128, 178),
					m_ActivePressedColor = new Color32(88, 88, 88, 178),
					m_DisabledColor = new Color32(64, 64, 64, 128),
					m_ColorMultiplier = 1f,
					m_FadeDuration = 0.1f
				};
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06002356 RID: 9046 RVA: 0x000AE37E File Offset: 0x000AC57E
		// (set) Token: 0x06002357 RID: 9047 RVA: 0x000AE386 File Offset: 0x000AC586
		public Color normalColor
		{
			get
			{
				return this.m_NormalColor;
			}
			set
			{
				this.m_NormalColor = value;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x000AE38F File Offset: 0x000AC58F
		// (set) Token: 0x06002359 RID: 9049 RVA: 0x000AE397 File Offset: 0x000AC597
		public Color highlightedColor
		{
			get
			{
				return this.m_HighlightedColor;
			}
			set
			{
				this.m_HighlightedColor = value;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x000AE3A0 File Offset: 0x000AC5A0
		// (set) Token: 0x0600235B RID: 9051 RVA: 0x000AE3A8 File Offset: 0x000AC5A8
		public Color pressedColor
		{
			get
			{
				return this.m_PressedColor;
			}
			set
			{
				this.m_PressedColor = value;
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000AE3B1 File Offset: 0x000AC5B1
		// (set) Token: 0x0600235D RID: 9053 RVA: 0x000AE3B9 File Offset: 0x000AC5B9
		public Color disabledColor
		{
			get
			{
				return this.m_DisabledColor;
			}
			set
			{
				this.m_DisabledColor = value;
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x000AE3C2 File Offset: 0x000AC5C2
		// (set) Token: 0x0600235F RID: 9055 RVA: 0x000AE3CA File Offset: 0x000AC5CA
		public Color activeColor
		{
			get
			{
				return this.m_ActiveColor;
			}
			set
			{
				this.m_ActiveColor = value;
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000AE3D3 File Offset: 0x000AC5D3
		// (set) Token: 0x06002361 RID: 9057 RVA: 0x000AE3DB File Offset: 0x000AC5DB
		public Color activeHighlightedColor
		{
			get
			{
				return this.m_ActiveHighlightedColor;
			}
			set
			{
				this.m_ActiveHighlightedColor = value;
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000AE3E4 File Offset: 0x000AC5E4
		// (set) Token: 0x06002363 RID: 9059 RVA: 0x000AE3EC File Offset: 0x000AC5EC
		public Color activePressedColor
		{
			get
			{
				return this.m_ActivePressedColor;
			}
			set
			{
				this.m_ActivePressedColor = value;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06002364 RID: 9060 RVA: 0x000AE3F5 File Offset: 0x000AC5F5
		// (set) Token: 0x06002365 RID: 9061 RVA: 0x000AE3FD File Offset: 0x000AC5FD
		public float colorMultiplier
		{
			get
			{
				return this.m_ColorMultiplier;
			}
			set
			{
				this.m_ColorMultiplier = value;
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06002366 RID: 9062 RVA: 0x000AE406 File Offset: 0x000AC606
		// (set) Token: 0x06002367 RID: 9063 RVA: 0x000AE40E File Offset: 0x000AC60E
		public float fadeDuration
		{
			get
			{
				return this.m_FadeDuration;
			}
			set
			{
				this.m_FadeDuration = value;
			}
		}

		// Token: 0x04001C9C RID: 7324
		[SerializeField]
		private Color m_NormalColor;

		// Token: 0x04001C9D RID: 7325
		[SerializeField]
		private Color m_HighlightedColor;

		// Token: 0x04001C9E RID: 7326
		[SerializeField]
		private Color m_PressedColor;

		// Token: 0x04001C9F RID: 7327
		[SerializeField]
		private Color m_ActiveColor;

		// Token: 0x04001CA0 RID: 7328
		[SerializeField]
		private Color m_ActiveHighlightedColor;

		// Token: 0x04001CA1 RID: 7329
		[SerializeField]
		private Color m_ActivePressedColor;

		// Token: 0x04001CA2 RID: 7330
		[SerializeField]
		private Color m_DisabledColor;

		// Token: 0x04001CA3 RID: 7331
		[Range(1f, 5f)]
		[SerializeField]
		private float m_ColorMultiplier;

		// Token: 0x04001CA4 RID: 7332
		[SerializeField]
		private float m_FadeDuration;
	}
}
