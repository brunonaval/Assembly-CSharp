using System;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class MobileItemSlotManager : MonoBehaviour
{
	// Token: 0x06000751 RID: 1873 RVA: 0x000239C3 File Offset: 0x00021BC3
	private void Start()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			this.iconRectTransform.sizeDelta = new Vector2(140f, 140f);
		}
	}

	// Token: 0x04000947 RID: 2375
	[SerializeField]
	private RectTransform iconRectTransform;
}
