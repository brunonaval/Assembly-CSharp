using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005F2 RID: 1522
	public class UITalentDatabase : ScriptableObject
	{
		// Token: 0x1700032C RID: 812
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000A783E File Offset: 0x000A5A3E
		public static UITalentDatabase Instance
		{
			get
			{
				if (UITalentDatabase.m_Instance == null)
				{
					UITalentDatabase.m_Instance = (Resources.Load("Databases/TalentDatabase") as UITalentDatabase);
				}
				return UITalentDatabase.m_Instance;
			}
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000A7866 File Offset: 0x000A5A66
		public UITalentInfo Get(int index)
		{
			return this.talents[index];
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000A7870 File Offset: 0x000A5A70
		public UITalentInfo GetByID(int ID)
		{
			for (int i = 0; i < this.talents.Length; i++)
			{
				if (this.talents[i].ID == ID)
				{
					return this.talents[i];
				}
			}
			return null;
		}

		// Token: 0x04001B76 RID: 7030
		private static UITalentDatabase m_Instance;

		// Token: 0x04001B77 RID: 7031
		public UITalentInfo[] talents;
	}
}
