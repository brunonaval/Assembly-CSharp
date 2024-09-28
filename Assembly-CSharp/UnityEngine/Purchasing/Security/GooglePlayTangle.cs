using System;

namespace UnityEngine.Purchasing.Security
{
	// Token: 0x02000587 RID: 1415
	public class GooglePlayTangle
	{
		// Token: 0x06001F59 RID: 8025 RVA: 0x0009DFF8 File Offset: 0x0009C1F8
		public static byte[] Data()
		{
			if (!GooglePlayTangle.IsPopulated)
			{
				return null;
			}
			return Obfuscator.DeObfuscate(GooglePlayTangle.data, GooglePlayTangle.order, GooglePlayTangle.key);
		}

		// Token: 0x04001950 RID: 6480
		private static byte[] data = Convert.FromBase64String("zH793szx+vXWerR6C/H9/f35/P/QRBQT5E2TC4SqYBWp7yv5FZYZMphvwAmlv/Ss8eTGh4c7GXN67peLo8WPAMd2MrXMOCs5Ff+2u5S5atuH2DFOour4GpUzM/Bdu0TDfbPoYiD/jKb/KyHO96MzKT7Ezr0RKlg4zCEt498n7BQF/ZdmbqhojePcsUEo8bovHNXLypKPfetZqAToGwdF+4kNPDnx4h6hpCkUcR76iLdhcgXyfv3z/Mx+/fb+fv39/AUepBKbcon3hZrPErriCg0xoA70v/I2KOfKAm5ZNh5MpDSn4YiQ6S57fKWvCffojj7fWbrHGnpL161X8Usklg3shqz8Oji16dzPvP5qGminGCGgAZB6LLV4V/EyprsS0/7//fz9");

		// Token: 0x04001951 RID: 6481
		private static int[] order = new int[]
		{
			0,
			10,
			8,
			3,
			4,
			13,
			11,
			11,
			8,
			10,
			12,
			11,
			13,
			13,
			14
		};

		// Token: 0x04001952 RID: 6482
		private static int key = 252;

		// Token: 0x04001953 RID: 6483
		public static readonly bool IsPopulated = true;
	}
}
