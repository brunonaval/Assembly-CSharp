using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuloGames.UI
{
	// Token: 0x0200058A RID: 1418
	public class ColorScheme : ScriptableObject
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x0009E096 File Offset: 0x0009C296
		// (set) Token: 0x06001F61 RID: 8033 RVA: 0x0009E09E File Offset: 0x0009C29E
		public Color imageLight
		{
			get
			{
				return this.m_ImageLight;
			}
			set
			{
				this.m_ImageLight = value;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x0009E0A7 File Offset: 0x0009C2A7
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x0009E0AF File Offset: 0x0009C2AF
		public Color imageLightest
		{
			get
			{
				return this.m_ImageLightest;
			}
			set
			{
				this.m_ImageLightest = value;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x0009E0B8 File Offset: 0x0009C2B8
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x0009E0C0 File Offset: 0x0009C2C0
		public Color imageDark
		{
			get
			{
				return this.m_ImageDark;
			}
			set
			{
				this.m_ImageDark = value;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x0009E0C9 File Offset: 0x0009C2C9
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x0009E0D1 File Offset: 0x0009C2D1
		public Color imageDarkest
		{
			get
			{
				return this.m_ImageDarkest;
			}
			set
			{
				this.m_ImageDarkest = value;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001F68 RID: 8040 RVA: 0x0009E0DA File Offset: 0x0009C2DA
		// (set) Token: 0x06001F69 RID: 8041 RVA: 0x0009E0E2 File Offset: 0x0009C2E2
		public ColorBlock buttonColors
		{
			get
			{
				return this.m_ButtonColors;
			}
			set
			{
				this.m_ButtonColors = value;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x0009E0EB File Offset: 0x0009C2EB
		// (set) Token: 0x06001F6B RID: 8043 RVA: 0x0009E0F3 File Offset: 0x0009C2F3
		public Color windowHeader
		{
			get
			{
				return this.m_WindowHeader;
			}
			set
			{
				this.m_WindowHeader = value;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x0009E0FC File Offset: 0x0009C2FC
		// (set) Token: 0x06001F6D RID: 8045 RVA: 0x0009E104 File Offset: 0x0009C304
		public ColorBlock linkColors
		{
			get
			{
				return this.m_LinkColors;
			}
			set
			{
				this.m_LinkColors = value;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x0009E10D File Offset: 0x0009C30D
		// (set) Token: 0x06001F6F RID: 8047 RVA: 0x0009E115 File Offset: 0x0009C315
		public ColorBlockExtended tabTextColors
		{
			get
			{
				return this.m_TabTextColors;
			}
			set
			{
				this.m_TabTextColors = value;
			}
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x0009E120 File Offset: 0x0009C320
		public void ApplyColorScheme()
		{
			foreach (ColorSchemeElement element in UnityEngine.Object.FindObjectsOfType<ColorSchemeElement>())
			{
				this.ApplyToElement(element);
			}
			if (ColorSchemeManager.Instance != null)
			{
				ColorSchemeManager.Instance.activeColorScheme = this;
			}
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0009E164 File Offset: 0x0009C364
		public void ApplyToElement(ColorSchemeElement element)
		{
			if (element == null)
			{
				return;
			}
			if (element.elementType == ColorSchemeElementType.Image)
			{
				Image component = element.gameObject.GetComponent<Image>();
				Color color = Color.white;
				if (component != null)
				{
					switch (element.shade)
					{
					case ColorSchemeShade.Light:
						color = this.m_ImageLight;
						break;
					case ColorSchemeShade.Dark:
						color = this.m_ImageDark;
						break;
					case ColorSchemeShade.Lightest:
						color = this.m_ImageLightest;
						break;
					case ColorSchemeShade.Darkest:
						color = this.m_ImageDarkest;
						break;
					case ColorSchemeShade.WindowHeader:
						color = this.m_WindowHeader;
						break;
					}
					component.color = new Color(color.r, color.g, color.b, component.color.a);
					return;
				}
			}
			else if (element.elementType == ColorSchemeElementType.Button)
			{
				Button component2 = element.gameObject.GetComponent<Button>();
				if (component2 != null)
				{
					component2.colors = this.m_ButtonColors;
					return;
				}
			}
			else if (element.elementType == ColorSchemeElementType.Link)
			{
				Button component3 = element.gameObject.GetComponent<Button>();
				if (component3 != null)
				{
					component3.colors = this.m_LinkColors;
					return;
				}
			}
			else if (element.elementType == ColorSchemeElementType.TabText)
			{
				UITab component4 = element.gameObject.GetComponent<UITab>();
				if (component4 != null)
				{
					component4.textColors = this.m_TabTextColors;
				}
			}
		}

		// Token: 0x04001954 RID: 6484
		[Header("Image Colors")]
		[SerializeField]
		private Color m_ImageLight = Color.white;

		// Token: 0x04001955 RID: 6485
		[SerializeField]
		private Color m_ImageLightest = Color.white;

		// Token: 0x04001956 RID: 6486
		[SerializeField]
		private Color m_ImageDark = Color.white;

		// Token: 0x04001957 RID: 6487
		[SerializeField]
		private Color m_ImageDarkest = Color.white;

		// Token: 0x04001958 RID: 6488
		[Header("Button Colors")]
		[SerializeField]
		private ColorBlock m_ButtonColors = ColorBlock.defaultColorBlock;

		// Token: 0x04001959 RID: 6489
		[Header("Window Colors")]
		[SerializeField]
		private Color m_WindowHeader = Color.white;

		// Token: 0x0400195A RID: 6490
		[Header("Link Colors")]
		[SerializeField]
		private ColorBlock m_LinkColors = ColorBlock.defaultColorBlock;

		// Token: 0x0400195B RID: 6491
		[Header("Tab Text Colors")]
		[SerializeField]
		private ColorBlockExtended m_TabTextColors = ColorBlockExtended.defaultColorBlock;
	}
}
