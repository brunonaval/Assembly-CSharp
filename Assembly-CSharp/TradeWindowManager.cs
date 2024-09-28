using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200024A RID: 586
public class TradeWindowManager : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x06000878 RID: 2168 RVA: 0x00028684 File Offset: 0x00026884
	private void OnEnable()
	{
		base.InvokeRepeating("UpdateTradingInformation", 0f, 0.5f);
		this.addGoldWindow.SetActive(false);
		this.addGoldButton.interactable = true;
		this.confirmTradeButton.interactable = true;
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x000286BF File Offset: 0x000268BF
	private void OnDisable()
	{
		this.CancelTrade();
		base.CancelInvoke("UpdateTradingInformation");
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x000286D4 File Offset: 0x000268D4
	private void UpdateTradingInformation()
	{
		if (this.uiSystemModule.TradeModule.NetworkCurrentTrader == null)
		{
			this.uiSystemModule.HideTradeWindow();
			return;
		}
		TradeModule tradeModule;
		if (!this.uiSystemModule.TradeModule.NetworkCurrentTrader.TryGetComponent<TradeModule>(out tradeModule))
		{
			this.uiSystemModule.HideTradeWindow();
			return;
		}
		if (!this.uiSystemModule.TradeModule.ItemsHasChanged & !tradeModule.ItemsHasChanged)
		{
			return;
		}
		this.UpdatePlayerTradingInformation();
		this.UpdateOtherPlayerTradingInformation();
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00028758 File Offset: 0x00026958
	private void UpdatePlayerTradingInformation()
	{
		this.playerNameLabel.text = this.uiSystemModule.CreatureModule.CreatureName.ToLower();
		if (this.uiSystemModule.TradeModule.TradeConfirmed)
		{
			this.confirmTradeButton.interactable = false;
			Text text = this.playerNameLabel;
			text.text = text.text + " <color=green>(" + LanguageManager.Instance.GetText("confirmed") + ")</color>";
		}
		this.playerGoldCoinsLabel.text = GlobalUtils.FormatLongNumber(this.uiSystemModule.TradeModule.TradingGoldCoins, LanguageManager.Instance.GetText("api_culture"));
		if (this.uiSystemModule.TradeModule.TradingGoldCoins != 0L)
		{
			this.addGoldButton.interactable = false;
		}
		for (int i = 0; i < this.playerItemsHolder.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.playerItemsHolder.transform.GetChild(i).gameObject);
		}
		foreach (Item item in this.uiSystemModule.TradeModule.TradingItems)
		{
			this.InstantiateItemSlotOnHolder(this.playerItemsHolder, item);
		}
		this.uiSystemModule.TradeModule.ItemsHasChanged = false;
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x000288BC File Offset: 0x00026ABC
	private void UpdateOtherPlayerTradingInformation()
	{
		CreatureModule creatureModule;
		this.uiSystemModule.TradeModule.NetworkCurrentTrader.TryGetComponent<CreatureModule>(out creatureModule);
		TradeModule tradeModule;
		this.uiSystemModule.TradeModule.NetworkCurrentTrader.TryGetComponent<TradeModule>(out tradeModule);
		this.otherPlayerNameLabel.text = creatureModule.CreatureName.ToLower();
		if (tradeModule.TradeConfirmed)
		{
			Text text = this.otherPlayerNameLabel;
			text.text = text.text + " <color=green>(" + LanguageManager.Instance.GetText("confirmed") + ")</color>";
		}
		this.otherPlayerGoldCoinsLabel.text = tradeModule.TradingGoldCoins.ToString();
		for (int i = 0; i < this.otherPlayerItemsHolder.transform.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.otherPlayerItemsHolder.transform.GetChild(i).gameObject);
		}
		foreach (Item item in tradeModule.TradingItems)
		{
			this.InstantiateItemSlotOnHolder(this.otherPlayerItemsHolder, item);
		}
		tradeModule.ItemsHasChanged = false;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x000289E8 File Offset: 0x00026BE8
	private void InstantiateItemSlotOnHolder(GameObject holder, Item item)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.tradeItemSlotPrefab);
		gameObject.transform.SetParent(holder.transform, false);
		gameObject.transform.position = Vector2.zero;
		TradeItemSlotManager tradeItemSlotManager;
		gameObject.TryGetComponent<TradeItemSlotManager>(out tradeItemSlotManager);
		tradeItemSlotManager.Item = item;
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00028A38 File Offset: 0x00026C38
	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			return;
		}
		InventorySlotManager inventorySlotManager;
		if (eventData.pointerDrag.TryGetComponent<InventorySlotManager>(out inventorySlotManager))
		{
			this.uiSystemModule.TradeModule.CmdAddItem(inventorySlotManager.Item);
			return;
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00028A7E File Offset: 0x00026C7E
	public void OpenAddGoldWindow()
	{
		if (this.uiSystemModule.TradeModule.TradingGoldCoins != 0L)
		{
			return;
		}
		this.addGoldInput.text = "0";
		this.addGoldWindow.SetActive(true);
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00028AAF File Offset: 0x00026CAF
	public void CancelAddGold()
	{
		this.addGoldWindow.SetActive(false);
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00028AC0 File Offset: 0x00026CC0
	public void ConfirmAddGold()
	{
		long amount;
		if (!long.TryParse(this.addGoldInput.text, out amount))
		{
			return;
		}
		this.addGoldWindow.SetActive(false);
		this.uiSystemModule.TradeModule.CmdAddGold(this.ClampGoldAmount(amount));
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00028B08 File Offset: 0x00026D08
	public void IncreaseGoldValue()
	{
		long num;
		long.TryParse(this.addGoldInput.text, out num);
		num += 1000L;
		this.addGoldInput.text = this.ClampGoldAmount(num).ToString();
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00028B4C File Offset: 0x00026D4C
	public void DecreaseGoldValue()
	{
		long num;
		long.TryParse(this.addGoldInput.text, out num);
		num -= 1000L;
		this.addGoldInput.text = this.ClampGoldAmount(num).ToString();
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00028B8F File Offset: 0x00026D8F
	private long ClampGoldAmount(long amount)
	{
		amount = (long)Mathf.Min((float)this.uiSystemModule.WalletModule.GoldCoins, (float)amount);
		amount = (long)Mathf.Max(0f, (float)amount);
		return amount;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00028BBC File Offset: 0x00026DBC
	public void ConfirmTrade()
	{
		this.uiSystemModule.TradeModule.CmdConfirmTrade();
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00028BCE File Offset: 0x00026DCE
	public void CancelTrade()
	{
		this.uiSystemModule.TradeModule.CmdCancelTrade();
	}

	// Token: 0x04000A47 RID: 2631
	[SerializeField]
	private GameObject tradeItemSlotPrefab;

	// Token: 0x04000A48 RID: 2632
	[SerializeField]
	private GameObject playerItemsHolder;

	// Token: 0x04000A49 RID: 2633
	[SerializeField]
	private Text playerNameLabel;

	// Token: 0x04000A4A RID: 2634
	[SerializeField]
	private Text playerGoldCoinsLabel;

	// Token: 0x04000A4B RID: 2635
	[SerializeField]
	private GameObject otherPlayerItemsHolder;

	// Token: 0x04000A4C RID: 2636
	[SerializeField]
	private Text otherPlayerNameLabel;

	// Token: 0x04000A4D RID: 2637
	[SerializeField]
	private Text otherPlayerGoldCoinsLabel;

	// Token: 0x04000A4E RID: 2638
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000A4F RID: 2639
	[SerializeField]
	private GameObject addGoldWindow;

	// Token: 0x04000A50 RID: 2640
	[SerializeField]
	private InputField addGoldInput;

	// Token: 0x04000A51 RID: 2641
	[SerializeField]
	private Button confirmTradeButton;

	// Token: 0x04000A52 RID: 2642
	[SerializeField]
	private Button addGoldButton;
}
