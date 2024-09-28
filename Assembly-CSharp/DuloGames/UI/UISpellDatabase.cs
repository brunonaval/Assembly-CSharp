using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005F1 RID: 1521
	public class UISpellDatabase : ScriptableObject
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000A77D2 File Offset: 0x000A59D2
		public static UISpellDatabase Instance
		{
			get
			{
				if (UISpellDatabase.m_Instance == null)
				{
					UISpellDatabase.m_Instance = (Resources.Load("Databases/SpellDatabase") as UISpellDatabase);
				}
				return UISpellDatabase.m_Instance;
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000A77FA File Offset: 0x000A59FA
		public UISpellInfo Get(int index)
		{
			return this.spells[index];
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000A7804 File Offset: 0x000A5A04
		public UISpellInfo GetByID(int ID)
		{
			for (int i = 0; i < this.spells.Length; i++)
			{
				if (this.spells[i].ID == ID)
				{
					return this.spells[i];
				}
			}
			return null;
		}

		// Token: 0x04001B74 RID: 7028
		private static UISpellDatabase m_Instance;

		// Token: 0x04001B75 RID: 7029
		public UISpellInfo[] spells;
	}
}
