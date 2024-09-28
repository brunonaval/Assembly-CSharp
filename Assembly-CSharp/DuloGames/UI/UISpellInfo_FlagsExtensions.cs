using System;

namespace DuloGames.UI
{
	// Token: 0x020005EE RID: 1518
	public static class UISpellInfo_FlagsExtensions
	{
		// Token: 0x0600216D RID: 8557 RVA: 0x000A767C File Offset: 0x000A587C
		public static bool Has(this UISpellInfo_Flags type, UISpellInfo_Flags value)
		{
			bool result;
			try
			{
				result = ((type & value) == value);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600216E RID: 8558 RVA: 0x000A76A8 File Offset: 0x000A58A8
		public static bool Is(this UISpellInfo_Flags type, UISpellInfo_Flags value)
		{
			bool result;
			try
			{
				result = (type == value);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600216F RID: 8559 RVA: 0x000A76D4 File Offset: 0x000A58D4
		public static UISpellInfo_Flags Add(this UISpellInfo_Flags type, UISpellInfo_Flags value)
		{
			UISpellInfo_Flags result;
			try
			{
				result = (type | value);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(UISpellInfo_Flags).Name), innerException);
			}
			return result;
		}

		// Token: 0x06002170 RID: 8560 RVA: 0x000A771C File Offset: 0x000A591C
		public static UISpellInfo_Flags Remove(this UISpellInfo_Flags type, UISpellInfo_Flags value)
		{
			UISpellInfo_Flags result;
			try
			{
				result = (type & ~value);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(UISpellInfo_Flags).Name), innerException);
			}
			return result;
		}
	}
}
