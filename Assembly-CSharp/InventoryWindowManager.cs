using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027E RID: 638
public class InventoryWindowManager : MonoBehaviour
{
	// Token: 0x060009C2 RID: 2498 RVA: 0x0002D540 File Offset: 0x0002B740
	private void Update()
	{
		this.gemsText.text = this.uiSystemModule.WalletModule.Gems.ToString();
		this.goldCoinsText.text = GlobalUtils.FormatLongNumber(this.uiSystemModule.WalletModule.GoldCoins, LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0002D59C File Offset: 0x0002B79C
	public void SortItems()
	{
		this.uiSystemModule.InventoryModule.CmdSortItems();
	}

	// Token: 0x04000B4E RID: 2894
	[SerializeField]
	private Text gemsText;

	// Token: 0x04000B4F RID: 2895
	[SerializeField]
	private Text goldCoinsText;

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private UISystemModule uiSystemModule;
}
