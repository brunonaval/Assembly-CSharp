using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200024B RID: 587
public class BuyGemStoreManager : MonoBehaviour
{
	// Token: 0x06000888 RID: 2184 RVA: 0x00028BE0 File Offset: 0x00026DE0
	private void Update()
	{
		if (Time.time - this.lastUpdateTime > 0.5f)
		{
			this.lastUpdateTime = Time.time;
			this.UpdateGemText();
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00028C06 File Offset: 0x00026E06
	private void OnEnable()
	{
		this.loadingOverlay.SetActive(false);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00028C14 File Offset: 0x00026E14
	private void UpdateGemText()
	{
		this.gemText.text = this.uiSystemModule.WalletModule.Gems.ToString();
	}

	// Token: 0x04000A53 RID: 2643
	[SerializeField]
	private GameObject loadingOverlay;

	// Token: 0x04000A54 RID: 2644
	[SerializeField]
	private Text gemText;

	// Token: 0x04000A55 RID: 2645
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000A56 RID: 2646
	private float lastUpdateTime;
}
