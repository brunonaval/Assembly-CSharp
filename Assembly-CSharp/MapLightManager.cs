using System;
using Mirror;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class MapLightManager : MonoBehaviour
{
	// Token: 0x0600004B RID: 75 RVA: 0x00002E81 File Offset: 0x00001081
	private void Start()
	{
		if (NetworkServer.active)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
