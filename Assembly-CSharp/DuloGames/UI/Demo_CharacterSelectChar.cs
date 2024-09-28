using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DuloGames.UI
{
	// Token: 0x02000590 RID: 1424
	public class Demo_CharacterSelectChar : MonoBehaviour
	{
		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001F81 RID: 8065 RVA: 0x0009E3B2 File Offset: 0x0009C5B2
		// (set) Token: 0x06001F82 RID: 8066 RVA: 0x0009E3BA File Offset: 0x0009C5BA
		public Demo_CharacterInfo info
		{
			get
			{
				return this.m_Info;
			}
			set
			{
				this.m_Info = value;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001F83 RID: 8067 RVA: 0x0009E3C3 File Offset: 0x0009C5C3
		// (set) Token: 0x06001F84 RID: 8068 RVA: 0x0009E3CB File Offset: 0x0009C5CB
		public int index
		{
			get
			{
				return this.m_Index;
			}
			set
			{
				this.m_Index = value;
			}
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x0009E3D4 File Offset: 0x0009C5D4
		private void OnMouseDown()
		{
			if (this.m_Info == null)
			{
				return;
			}
			if (EventSystem.current.IsPointerOverGameObject())
			{
				return;
			}
			if (Demo_CharacterSelectMgr.instance != null)
			{
				Demo_CharacterSelectMgr.instance.SelectCharacter(this);
			}
		}

		// Token: 0x0400196C RID: 6508
		private Demo_CharacterInfo m_Info;

		// Token: 0x0400196D RID: 6509
		private int m_Index;
	}
}
