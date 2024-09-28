using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200058F RID: 1423
	public class Demo_CharacterCreateMgr : MonoBehaviour
	{
		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x0009E39B File Offset: 0x0009C59B
		public static Demo_CharacterCreateMgr instance
		{
			get
			{
				return Demo_CharacterCreateMgr.m_Mgr;
			}
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0009E3A2 File Offset: 0x0009C5A2
		protected void Awake()
		{
			Demo_CharacterCreateMgr.m_Mgr = this;
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x0009E3AA File Offset: 0x0009C5AA
		protected void OnDestroy()
		{
			Demo_CharacterCreateMgr.m_Mgr = null;
		}

		// Token: 0x0400196B RID: 6507
		private static Demo_CharacterCreateMgr m_Mgr;
	}
}
