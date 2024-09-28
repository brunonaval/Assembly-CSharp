using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000243 RID: 579
public class StorageButtonManager : MonoBehaviour
{
	// Token: 0x0600084D RID: 2125 RVA: 0x0002794C File Offset: 0x00025B4C
	private void LateUpdate()
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		if (this.uiSystemModule.PlayerModule == null)
		{
			return;
		}
		if (this.uiSystemModule.PlayerModule.HasItemOnStorage)
		{
			this.redExclamationImage.enabled = true;
			return;
		}
		this.redExclamationImage.enabled = false;
	}

	// Token: 0x04000A21 RID: 2593
	[SerializeField]
	private Image redExclamationImage;

	// Token: 0x04000A22 RID: 2594
	[SerializeField]
	private UISystemModule uiSystemModule;
}
