using System;
using UnityEngine;

namespace Mirror
{
	// Token: 0x0200005E RID: 94
	[DisallowMultipleComponent]
	[AddComponentMenu("Network/Network Start Position")]
	[HelpURL("https://mirror-networking.gitbook.io/docs/components/network-start-position")]
	public class NetworkStartPosition : MonoBehaviour
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000B3F8 File Offset: 0x000095F8
		public void Awake()
		{
			NetworkManager.RegisterStartPosition(base.transform);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000B405 File Offset: 0x00009605
		public void OnDestroy()
		{
			NetworkManager.UnRegisterStartPosition(base.transform);
		}
	}
}
