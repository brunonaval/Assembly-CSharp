using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200058D RID: 1421
	public class ColorSchemeManager : ScriptableObject
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001F79 RID: 8057 RVA: 0x0009E362 File Offset: 0x0009C562
		public static ColorSchemeManager Instance
		{
			get
			{
				if (ColorSchemeManager.m_Instance == null)
				{
					ColorSchemeManager.m_Instance = (Resources.Load("ColorSchemeManager") as ColorSchemeManager);
				}
				return ColorSchemeManager.m_Instance;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001F7A RID: 8058 RVA: 0x0009E38A File Offset: 0x0009C58A
		// (set) Token: 0x06001F7B RID: 8059 RVA: 0x0009E392 File Offset: 0x0009C592
		public ColorScheme activeColorScheme
		{
			get
			{
				return this.m_ActiveColorScheme;
			}
			set
			{
				this.m_ActiveColorScheme = value;
			}
		}

		// Token: 0x04001963 RID: 6499
		private static ColorSchemeManager m_Instance;

		// Token: 0x04001964 RID: 6500
		[SerializeField]
		private ColorScheme m_ActiveColorScheme;
	}
}
