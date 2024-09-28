using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x0200061A RID: 1562
	public class UILoadingOverlayManager : ScriptableObject
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x000AB1D4 File Offset: 0x000A93D4
		public static UILoadingOverlayManager Instance
		{
			get
			{
				if (UILoadingOverlayManager.m_Instance == null)
				{
					UILoadingOverlayManager.m_Instance = (Resources.Load("LoadingOverlayManager") as UILoadingOverlayManager);
				}
				return UILoadingOverlayManager.m_Instance;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06002281 RID: 8833 RVA: 0x000AB1FC File Offset: 0x000A93FC
		public GameObject prefab
		{
			get
			{
				return this.m_LoadingOverlayPrefab;
			}
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000AB204 File Offset: 0x000A9404
		public UILoadingOverlay Create()
		{
			if (this.m_LoadingOverlayPrefab == null)
			{
				return null;
			}
			return UnityEngine.Object.Instantiate<GameObject>(this.m_LoadingOverlayPrefab).GetComponent<UILoadingOverlay>();
		}

		// Token: 0x04001C04 RID: 7172
		private static UILoadingOverlayManager m_Instance;

		// Token: 0x04001C05 RID: 7173
		[SerializeField]
		private GameObject m_LoadingOverlayPrefab;
	}
}
