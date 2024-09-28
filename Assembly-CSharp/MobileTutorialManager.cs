using System;
using Mirror;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class MobileTutorialManager : MonoBehaviour
{
	// Token: 0x06000955 RID: 2389 RVA: 0x0002BCF0 File Offset: 0x00029EF0
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (!GlobalSettings.IsMobilePlatform)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.gameObject.SetActive(true);
	}
}
