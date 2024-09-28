using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200058B RID: 1419
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Color Scheme Element", 48)]
	public class ColorSchemeElement : MonoBehaviour
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x0009E30F File Offset: 0x0009C50F
		// (set) Token: 0x06001F74 RID: 8052 RVA: 0x0009E317 File Offset: 0x0009C517
		public ColorSchemeElementType elementType
		{
			get
			{
				return this.m_ElementType;
			}
			set
			{
				this.m_ElementType = value;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x0009E320 File Offset: 0x0009C520
		// (set) Token: 0x06001F76 RID: 8054 RVA: 0x0009E328 File Offset: 0x0009C528
		public ColorSchemeShade shade
		{
			get
			{
				return this.m_Shade;
			}
			set
			{
				this.m_Shade = value;
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0009E331 File Offset: 0x0009C531
		protected void Awake()
		{
			if (ColorSchemeManager.Instance != null && ColorSchemeManager.Instance.activeColorScheme != null)
			{
				ColorSchemeManager.Instance.activeColorScheme.ApplyToElement(this);
			}
		}

		// Token: 0x0400195C RID: 6492
		[SerializeField]
		private ColorSchemeElementType m_ElementType;

		// Token: 0x0400195D RID: 6493
		[SerializeField]
		private ColorSchemeShade m_Shade;
	}
}
