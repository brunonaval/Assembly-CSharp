using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x02000670 RID: 1648
	public class UIBlackOverlayManager : ScriptableObject
	{
		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x000B2E4D File Offset: 0x000B104D
		public static UIBlackOverlayManager Instance
		{
			get
			{
				if (UIBlackOverlayManager.m_Instance == null)
				{
					UIBlackOverlayManager.m_Instance = (Resources.Load("BlackOverlayManager") as UIBlackOverlayManager);
				}
				return UIBlackOverlayManager.m_Instance;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x000B2E75 File Offset: 0x000B1075
		public GameObject prefab
		{
			get
			{
				return this.m_BlackOverlayPrefab;
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x000B2E7D File Offset: 0x000B107D
		public UIBlackOverlay Create(Transform parent)
		{
			if (this.m_BlackOverlayPrefab == null)
			{
				return null;
			}
			return UnityEngine.Object.Instantiate<GameObject>(this.m_BlackOverlayPrefab, parent).GetComponent<UIBlackOverlay>();
		}

		// Token: 0x04001DBD RID: 7613
		private static UIBlackOverlayManager m_Instance;

		// Token: 0x04001DBE RID: 7614
		[SerializeField]
		private GameObject m_BlackOverlayPrefab;
	}
}
