using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F0 RID: 496
public class DailyRewardButtonManager : MonoBehaviour
{
	// Token: 0x06000627 RID: 1575 RVA: 0x0001F808 File Offset: 0x0001DA08
	private void LateUpdate()
	{
		if (this.uiSystemModule == null)
		{
			return;
		}
		if (this.uiSystemModule.DailyRewardModule == null)
		{
			return;
		}
		if (this.uiSystemModule.DailyRewardModule.CanGetReward)
		{
			this.redExclamationImage.enabled = true;
			return;
		}
		this.redExclamationImage.enabled = false;
	}

	// Token: 0x0400086E RID: 2158
	[SerializeField]
	private Image redExclamationImage;

	// Token: 0x0400086F RID: 2159
	[SerializeField]
	private UISystemModule uiSystemModule;
}
