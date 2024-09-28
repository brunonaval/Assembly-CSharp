using System;
using System.Collections.Generic;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200062F RID: 1583
	public class UIModalBoxManager : ScriptableObject
	{
		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060022D5 RID: 8917 RVA: 0x000AC833 File Offset: 0x000AAA33
		public static UIModalBoxManager Instance
		{
			get
			{
				if (UIModalBoxManager.m_Instance == null)
				{
					UIModalBoxManager.m_Instance = (Resources.Load("ModalBoxManager") as UIModalBoxManager);
				}
				return UIModalBoxManager.m_Instance;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060022D6 RID: 8918 RVA: 0x000AC85B File Offset: 0x000AAA5B
		public GameObject prefab
		{
			get
			{
				return this.m_ModalBoxPrefab;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060022D7 RID: 8919 RVA: 0x000AC863 File Offset: 0x000AAA63
		public UIModalBox[] activeBoxes
		{
			get
			{
				this.m_ActiveBoxes.RemoveAll((UIModalBox item) => item == null);
				return this.m_ActiveBoxes.ToArray();
			}
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000AC89C File Offset: 0x000AAA9C
		public UIModalBox Create(GameObject rel)
		{
			if (this.m_ModalBoxPrefab == null || rel == null)
			{
				return null;
			}
			Canvas canvas = UIUtility.FindInParents<Canvas>(rel);
			if (canvas != null)
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.m_ModalBoxPrefab, canvas.transform, false).GetComponent<UIModalBox>();
			}
			return null;
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000AC8EB File Offset: 0x000AAAEB
		public void RegisterActiveBox(UIModalBox box)
		{
			if (!this.m_ActiveBoxes.Contains(box))
			{
				this.m_ActiveBoxes.Add(box);
			}
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000AC907 File Offset: 0x000AAB07
		public void UnregisterActiveBox(UIModalBox box)
		{
			if (this.m_ActiveBoxes.Contains(box))
			{
				this.m_ActiveBoxes.Remove(box);
			}
		}

		// Token: 0x04001C51 RID: 7249
		private static UIModalBoxManager m_Instance;

		// Token: 0x04001C52 RID: 7250
		[SerializeField]
		private GameObject m_ModalBoxPrefab;

		// Token: 0x04001C53 RID: 7251
		private List<UIModalBox> m_ActiveBoxes = new List<UIModalBox>();
	}
}
