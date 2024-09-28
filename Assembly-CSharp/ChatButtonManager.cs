using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001DB RID: 475
public class ChatButtonManager : MonoBehaviour
{
	// Token: 0x06000595 RID: 1429 RVA: 0x0001DAF8 File Offset: 0x0001BCF8
	private void Update()
	{
		if (Time.time - this.updateTime < 1f)
		{
			return;
		}
		this.updateTime = Time.time;
		if (this.uiSystemModule == null)
		{
			return;
		}
		if (this.uiSystemModule.PlatformChatHolderManager == null)
		{
			return;
		}
		if (this.uiSystemModule.PlatformChatHolderManager.HasUnreadMessages())
		{
			this.redExclamationImage.enabled = true;
			return;
		}
		this.redExclamationImage.enabled = false;
	}

	// Token: 0x04000803 RID: 2051
	[SerializeField]
	private Image redExclamationImage;

	// Token: 0x04000804 RID: 2052
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000805 RID: 2053
	private float updateTime;
}
