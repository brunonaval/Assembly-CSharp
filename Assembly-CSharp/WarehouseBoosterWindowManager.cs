using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000299 RID: 665
public class WarehouseBoosterWindowManager : MonoBehaviour
{
	// Token: 0x06000A72 RID: 2674 RVA: 0x0002FA19 File Offset: 0x0002DC19
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0002FA31 File Offset: 0x0002DC31
	private void OnEnable()
	{
		this.InitializeForGem();
		this.InitializeForGold();
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0002FA40 File Offset: 0x0002DC40
	private void InitializeForGem()
	{
		if (this.uiSystemModule.WarehouseModule.Items.Count >= 35)
		{
			this.gemIcon.SetActive(true);
			this.goldIcon.SetActive(false);
			this.messageText.text = string.Format(LanguageManager.Instance.GetText("warehouse_booster_window_gem_message"), 4);
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0002FAA4 File Offset: 0x0002DCA4
	private void InitializeForGold()
	{
		if (this.uiSystemModule.WarehouseModule.Items.Count < 35)
		{
			this.gemIcon.SetActive(false);
			this.goldIcon.SetActive(true);
			this.messageText.text = string.Format(LanguageManager.Instance.GetText("warehouse_booster_window_gold_message"), 4000);
		}
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0002FB0C File Offset: 0x0002DD0C
	public void OnConfirmButtonClicked()
	{
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
		if (this.uiSystemModule.WarehouseModule.Items.Count >= 450)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("warehouse_limit_reached_message", 3, 3.5f, Array.Empty<string>());
			return;
		}
		if (this.IncreaseWarehouseUsingGem())
		{
			return;
		}
		this.IncreaseWarehouseUsingGold();
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0002FB78 File Offset: 0x0002DD78
	private bool IncreaseWarehouseUsingGold()
	{
		if (this.uiSystemModule.WarehouseModule.Items.Count >= 35)
		{
			return false;
		}
		if (this.uiSystemModule.WalletModule.GoldCoins < 4000L)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_money_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.uiSystemModule.WarehouseModule.CmdIncreaseWarehouse();
		return true;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0002FBEC File Offset: 0x0002DDEC
	private bool IncreaseWarehouseUsingGem()
	{
		if (this.uiSystemModule.WalletModule.Gems < 4)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_gems_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		this.uiSystemModule.WarehouseModule.CmdIncreaseWarehouse();
		return true;
	}

	// Token: 0x04000BF3 RID: 3059
	[SerializeField]
	private GameObject gemIcon;

	// Token: 0x04000BF4 RID: 3060
	[SerializeField]
	private GameObject goldIcon;

	// Token: 0x04000BF5 RID: 3061
	[SerializeField]
	private Text messageText;

	// Token: 0x04000BF6 RID: 3062
	private UISystemModule uiSystemModule;
}
