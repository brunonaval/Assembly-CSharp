using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200021B RID: 539
public class MobileItemHolderManager : MonoBehaviour
{
	// Token: 0x0600074F RID: 1871 RVA: 0x000239A0 File Offset: 0x00021BA0
	private void Start()
	{
		if (GlobalSettings.IsMobilePlatform)
		{
			this.holderGridLayoutGroup.cellSize = new Vector2(150f, 150f);
		}
	}

	// Token: 0x04000946 RID: 2374
	[SerializeField]
	private GridLayoutGroup holderGridLayoutGroup;
}
