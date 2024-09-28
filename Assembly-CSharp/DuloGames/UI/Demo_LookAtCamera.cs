using System;
using UnityEngine;

namespace DuloGames.UI
{
	// Token: 0x020005A0 RID: 1440
	public class Demo_LookAtCamera : MonoBehaviour
	{
		// Token: 0x06001FCD RID: 8141 RVA: 0x0009FF18 File Offset: 0x0009E118
		protected void Awake()
		{
			if (this.m_Camera == null)
			{
				this.m_Camera = Camera.main;
			}
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0009FF33 File Offset: 0x0009E133
		private void Update()
		{
			if (this.m_Camera)
			{
				base.transform.rotation = Quaternion.LookRotation(this.m_Camera.transform.forward);
			}
		}

		// Token: 0x040019B0 RID: 6576
		[SerializeField]
		private Camera m_Camera;
	}
}
