using System;
using System.Globalization;
using UnityEngine;

// Token: 0x02000020 RID: 32
public static class CommonColorBuffer
{
	// Token: 0x06000075 RID: 117 RVA: 0x00003490 File Offset: 0x00001690
	public static string ColorToString(Color32 color)
	{
		return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000034E8 File Offset: 0x000016E8
	public static Color32 StringToColor(string colorString)
	{
		int num = int.Parse(colorString, NumberStyles.HexNumber);
		Color32 result;
		if (colorString.Length == 8)
		{
			result = new Color32((byte)(num >> 24 & 255), (byte)(num >> 16 & 255), (byte)(num >> 8 & 255), (byte)(num & 255));
		}
		else if (colorString.Length == 6)
		{
			result = new Color32((byte)(num >> 16 & 255), (byte)(num >> 8 & 255), (byte)(num & 255), byte.MaxValue);
		}
		else if (colorString.Length == 4)
		{
			result = new Color32((byte)((num >> 12 & 15) * 17), (byte)((num >> 8 & 15) * 17), (byte)((num >> 4 & 15) * 17), (byte)((num & 15) * 17));
		}
		else
		{
			if (colorString.Length != 3)
			{
				throw new FormatException("Support only RRGGBBAA, RRGGBB, RGBA, RGB formats");
			}
			result = new Color32((byte)((num >> 8 & 15) * 17), (byte)((num >> 4 & 15) * 17), (byte)((num & 15) * 17), byte.MaxValue);
		}
		return result;
	}

	// Token: 0x0400004D RID: 77
	public static Color value = Color.white;
}
