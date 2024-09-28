using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005F0 RID: 1520
	public class UIItemDatabase : ScriptableObject
	{
		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x000A7764 File Offset: 0x000A5964
		public static UIItemDatabase Instance
		{
			get
			{
				if (UIItemDatabase.m_Instance == null)
				{
					UIItemDatabase.m_Instance = (Resources.Load("Databases/ItemDatabase") as UIItemDatabase);
				}
				return UIItemDatabase.m_Instance;
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000A778C File Offset: 0x000A598C
		public UIItemInfo Get(int index)
		{
			return this.items[index];
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000A7798 File Offset: 0x000A5998
		public UIItemInfo GetByID(int ID)
		{
			for (int i = 0; i < this.items.Length; i++)
			{
				if (this.items[i].ID == ID)
				{
					return this.items[i];
				}
			}
			return null;
		}

		// Token: 0x04001B72 RID: 7026
		private static UIItemDatabase m_Instance;

		// Token: 0x04001B73 RID: 7027
		public UIItemInfo[] items;
	}
}
