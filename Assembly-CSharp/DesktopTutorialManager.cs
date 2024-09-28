using System;
using Mirror;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class DesktopTutorialManager : MonoBehaviour
{
	// Token: 0x06000953 RID: 2387 RVA: 0x0002BCBC File Offset: 0x00029EBC
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (GlobalSettings.IsMobilePlatform)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.gameObject.SetActive(true);
	}
}
