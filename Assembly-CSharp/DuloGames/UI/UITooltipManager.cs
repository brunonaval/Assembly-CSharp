using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000656 RID: 1622
	public class UITooltipManager : ScriptableObject
	{
		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060023DF RID: 9183 RVA: 0x000B016D File Offset: 0x000AE36D
		public static UITooltipManager Instance
		{
			get
			{
				if (UITooltipManager.m_Instance == null)
				{
					UITooltipManager.m_Instance = (Resources.Load("TooltipManager") as UITooltipManager);
				}
				return UITooltipManager.m_Instance;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x000B0195 File Offset: 0x000AE395
		public GameObject prefab
		{
			get
			{
				return this.m_TooltipPrefab;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060023E1 RID: 9185 RVA: 0x000B019D File Offset: 0x000AE39D
		public int spacerHeight
		{
			get
			{
				return this.m_SpacerHeight;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x000B01A5 File Offset: 0x000AE3A5
		public int itemTooltipWidth
		{
			get
			{
				return this.m_ItemTooltipWidth;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060023E3 RID: 9187 RVA: 0x000B01AD File Offset: 0x000AE3AD
		public int spellTooltipWidth
		{
			get
			{
				return this.m_SpellTooltipWidth;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000B01B5 File Offset: 0x000AE3B5
		public UITooltipLineStyle defaultLineStyle
		{
			get
			{
				return this.m_DefaultLineStyle;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060023E5 RID: 9189 RVA: 0x000B01BD File Offset: 0x000AE3BD
		public UITooltipLineStyle titleLineStyle
		{
			get
			{
				return this.m_TitleLineStyle;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000B01C5 File Offset: 0x000AE3C5
		public UITooltipLineStyle descriptionLineStyle
		{
			get
			{
				return this.m_DescriptionLineStyle;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000B01CD File Offset: 0x000AE3CD
		public UITooltipLineStyle[] customStyles
		{
			get
			{
				return this.m_CustomStyles;
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000B01D8 File Offset: 0x000AE3D8
		public UITooltipLineStyle GetCustomStyle(string name)
		{
			if (this.m_CustomStyles.Length != 0)
			{
				foreach (UITooltipLineStyle uitooltipLineStyle in this.m_CustomStyles)
				{
					if (uitooltipLineStyle.Name.Equals(name))
					{
						return uitooltipLineStyle;
					}
				}
			}
			return this.m_DefaultLineStyle;
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000B021D File Offset: 0x000AE41D
		[ContextMenu("Sort Custom Styles")]
		public void SortCustomStyles()
		{
			Array.Sort<UITooltipLineStyle>(this.m_CustomStyles);
		}

		// Token: 0x04001D00 RID: 7424
		private static UITooltipManager m_Instance;

		// Token: 0x04001D01 RID: 7425
		[SerializeField]
		private GameObject m_TooltipPrefab;

		// Token: 0x04001D02 RID: 7426
		[SerializeField]
		private int m_SpacerHeight = 6;

		// Token: 0x04001D03 RID: 7427
		[SerializeField]
		private int m_ItemTooltipWidth = 514;

		// Token: 0x04001D04 RID: 7428
		[SerializeField]
		private int m_SpellTooltipWidth = 514;

		// Token: 0x04001D05 RID: 7429
		[Header("Styles")]
		[SerializeField]
		private UITooltipLineStyle m_DefaultLineStyle = new UITooltipLineStyle(false);

		// Token: 0x04001D06 RID: 7430
		[SerializeField]
		private UITooltipLineStyle m_TitleLineStyle = new UITooltipLineStyle(false);

		// Token: 0x04001D07 RID: 7431
		[SerializeField]
		private UITooltipLineStyle m_DescriptionLineStyle = new UITooltipLineStyle(false);

		// Token: 0x04001D08 RID: 7432
		[SerializeField]
		private UITooltipLineStyle[] m_CustomStyles = new UITooltipLineStyle[0];
	}
}
