using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027D RID: 637
public class InventoryBoosterWindowManager : MonoBehaviour
{
	// Token: 0x060009BA RID: 2490 RVA: 0x0002D2DD File Offset: 0x0002B4DD
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0002D2F5 File Offset: 0x0002B4F5
	private void OnEnable()
	{
		this.InitializeForGem();
		this.InitializeForGold();
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0002D304 File Offset: 0x0002B504
	private void InitializeForGem()
	{
		int maxInventorySlots = GlobalUtils.GetMaxInventorySlots(this.uiSystemModule.PlayerModule.PackageType);
		if (this.uiSystemModule.InventoryModule.Items.Count >= maxInventorySlots)
		{
			this.gemIcon.SetActive(true);
			this.goldIcon.SetActive(false);
			this.messageText.text = string.Format(LanguageManager.Instance.GetText("inventory_booster_window_gem_message"), 2);
		}
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0002D37C File Offset: 0x0002B57C
	private void InitializeForGold()
	{
		int maxInventorySlots = GlobalUtils.GetMaxInventorySlots(this.uiSystemModule.PlayerModule.PackageType);
		if (this.uiSystemModule.InventoryModule.Items.Count < maxInventorySlots)
		{
			this.gemIcon.SetActive(false);
			this.goldIcon.SetActive(true);
			this.messageText.text = string.Format(LanguageManager.Instance.GetText("inventory_booster_window_gold_message"), 2000);
		}
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0002D3F8 File Offset: 0x0002B5F8
	public void OnConfirmButtonClicked()
	{
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
		if (this.uiSystemModule.InventoryModule.Items.Count >= 200)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("inventory_limit_reached_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (this.IncreaseInventoryUsingGold())
		{
			return;
		}
		this.IncreaseInventoryUsingGem();
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0002D464 File Offset: 0x0002B664
	private bool IncreaseInventoryUsingGold()
	{
		int maxInventorySlots = GlobalUtils.GetMaxInventorySlots(this.uiSystemModule.PlayerModule.PackageType);
		if (this.uiSystemModule.InventoryModule.Items.Count >= maxInventorySlots)
		{
			return false;
		}
		if (this.uiSystemModule.WalletModule.GoldCoins < 2000L)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_money_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.uiSystemModule.InventoryModule.CmdIncreaseInventory();
		return true;
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0002D4EC File Offset: 0x0002B6EC
	private bool IncreaseInventoryUsingGem()
	{
		if (this.uiSystemModule.WalletModule.Gems < 2)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_gems_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.uiSystemModule.InventoryModule.CmdIncreaseInventory();
		return true;
	}

	// Token: 0x04000B4A RID: 2890
	[SerializeField]
	private GameObject gemIcon;

	// Token: 0x04000B4B RID: 2891
	[SerializeField]
	private GameObject goldIcon;

	// Token: 0x04000B4C RID: 2892
	[SerializeField]
	private Text messageText;

	// Token: 0x04000B4D RID: 2893
	private UISystemModule uiSystemModule;
}
