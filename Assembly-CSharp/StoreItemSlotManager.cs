using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000244 RID: 580
public class StoreItemSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600084F RID: 2127 RVA: 0x000279A8 File Offset: 0x00025BA8
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x000279CC File Offset: 0x00025BCC
	private void Update()
	{
		if (this.itemDefined && this.itemsUpdateTime != this.uiSystemModule.InventoryModule.ItemsUpdateTime)
		{
			this.itemsUpdateTime = this.uiSystemModule.InventoryModule.ItemsUpdateTime;
			this.itemValue.text = GlobalUtils.FormatLongNumber((long)(this.storeItem.ItemValue * this.GetAmountInputValue()), LanguageManager.Instance.GetText("api_culture"));
			int amount = this.uiSystemModule.InventoryModule.GetAmount(this.item.UniqueId);
			this.itemAmount.text = GlobalUtils.FormatLongNumber((long)amount, LanguageManager.Instance.GetText("api_culture"));
			if (amount == 0 & this.storeAction == StoreAction.Sell)
			{
				UnityEngine.Object.Destroy(base.gameObject.transform.parent.gameObject);
				return;
			}
		}
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00027AB0 File Offset: 0x00025CB0
	private void LateUpdate()
	{
		if (this.item.IsDefined)
		{
			this.itemIcon.enabled = true;
			this.itemAmount.enabled = true;
		}
		else
		{
			this.itemIcon.enabled = false;
			this.itemAmount.enabled = false;
		}
		if (this.isOver && Time.time - this.overTime > 0.7f && this.item.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.item);
		}
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x00027B3A File Offset: 0x00025D3A
	private void OnDisable()
	{
		this.isOver = false;
		if (this.item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00027B5B File Offset: 0x00025D5B
	private void OnEnable()
	{
		this.itemsUpdateTime = 0f;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x00027B68 File Offset: 0x00025D68
	public void SetStoreItem(StoreItemConfig storeItem, StoreAction storeAction)
	{
		this.item = storeItem.Item;
		this.itemIcon.sprite = this.item.Icon;
		this.itemName.text = LanguageManager.Instance.GetText(this.item.Name);
		this.storeAction = storeAction;
		if (storeAction == StoreAction.Repurchase)
		{
			storeItem.ItemValue = ((storeItem.ItemValue >= 0) ? storeItem.ItemValue : this.item.Value);
			this.amountInput.text = storeItem.Item.Amount.ToString();
			this.storeButton.text = LanguageManager.Instance.GetText("repurchase").ToLower();
		}
		else if (storeAction == StoreAction.Buy)
		{
			this.item.Amount = 1;
			storeItem.ItemValue = ((storeItem.ItemValue >= 0) ? storeItem.ItemValue : (this.item.Value * GlobalSettings.StoreItemPriceMultiplier));
			this.amountInput.text = "1";
			this.storeButton.text = LanguageManager.Instance.GetText("buy").ToLower();
		}
		else if (storeAction == StoreAction.Sell)
		{
			storeItem.ItemValue = ((storeItem.ItemValue >= 0) ? storeItem.ItemValue : this.item.Value);
			this.amountInput.text = this.uiSystemModule.InventoryModule.GetAmount(this.item.UniqueId).ToString();
			this.storeButton.text = LanguageManager.Instance.GetText("sell").ToLower();
		}
		this.storeItem = storeItem;
		this.itemDefined = true;
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x00027D18 File Offset: 0x00025F18
	public void ReloadStoreItem()
	{
		this.item = this.storeItem.Item;
		if (this.storeAction == StoreAction.Repurchase)
		{
			this.storeItem.ItemValue = ((this.storeItem.ItemValue >= 0) ? this.storeItem.ItemValue : this.item.Value);
			this.amountInput.text = this.storeItem.Item.Amount.ToString();
			this.storeButton.text = LanguageManager.Instance.GetText("repurchase").ToLower();
		}
		else if (this.storeAction == StoreAction.Buy)
		{
			this.item.Amount = 1;
			this.storeItem.ItemValue = ((this.storeItem.ItemValue >= 0) ? this.storeItem.ItemValue : (this.item.Value * GlobalSettings.StoreItemPriceMultiplier));
			this.amountInput.text = "1";
			this.storeButton.text = LanguageManager.Instance.GetText("buy").ToLower();
		}
		else if (this.storeAction == StoreAction.Sell)
		{
			this.storeItem.ItemValue = ((this.storeItem.ItemValue >= 0) ? this.storeItem.ItemValue : this.item.Value);
			int amount = this.uiSystemModule.InventoryModule.GetAmount(this.item.UniqueId);
			this.amountInput.text = amount.ToString();
			if (amount < 1)
			{
				UnityEngine.Object.Destroy(base.transform.parent.gameObject);
			}
			this.storeButton.text = LanguageManager.Instance.GetText("sell").ToLower();
		}
		this.itemDefined = true;
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x00027EE3 File Offset: 0x000260E3
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x00027B3A File Offset: 0x00025D3A
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x00027EF8 File Offset: 0x000260F8
	public void StoreButtonClick()
	{
		int amountInputValue = this.GetAmountInputValue();
		if (this.storeAction == StoreAction.Repurchase)
		{
			this.uiSystemModule.StoreModule.CmdRepurchaseItem(this.storeItem.NpcId, this.storeItem.Item.UniqueId, amountInputValue);
			this.storeItem.Item.Amount = this.storeItem.Item.Amount - amountInputValue;
		}
		if (this.storeAction == StoreAction.Buy && amountInputValue > 0)
		{
			this.uiSystemModule.StoreModule.CmdBuyItem(this.storeItem.NpcId, this.storeItem.Item.UniqueId, amountInputValue);
			return;
		}
		if (this.storeAction == StoreAction.Sell)
		{
			this.uiSystemModule.StoreModule.CmdSellItem(this.storeItem.Item.UniqueId, amountInputValue);
		}
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x00027FBC File Offset: 0x000261BC
	public void OnAmountInputChanged()
	{
		this.amountInput.text = this.GetAmountInputValue().ToString();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00027FE4 File Offset: 0x000261E4
	public int GetAmountInputValue()
	{
		int num;
		int.TryParse(this.amountInput.text, out num);
		int max = 750;
		if (this.storeAction == StoreAction.Sell)
		{
			max = this.uiSystemModule.InventoryModule.GetAmount(this.item.UniqueId);
		}
		if (this.storeAction == StoreAction.Repurchase)
		{
			max = this.item.Amount;
		}
		num = Mathf.Clamp(num, 0, max);
		return num;
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x00028050 File Offset: 0x00026250
	public void IncreaseAmount()
	{
		int num = this.GetAmountInputValue();
		num++;
		int max = 750;
		if (this.storeAction == StoreAction.Sell)
		{
			max = this.uiSystemModule.InventoryModule.GetAmount(this.item.UniqueId);
		}
		if (this.storeAction == StoreAction.Repurchase)
		{
			max = this.storeItem.Item.Amount;
		}
		num = Mathf.Clamp(num, 0, max);
		this.amountInput.text = num.ToString();
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x000280C8 File Offset: 0x000262C8
	public void DecreaseAmount()
	{
		int num = this.GetAmountInputValue();
		num--;
		int max = 750;
		if (this.storeAction == StoreAction.Sell)
		{
			max = this.uiSystemModule.InventoryModule.GetAmount(this.item.UniqueId);
		}
		if (this.storeAction == StoreAction.Repurchase)
		{
			max = this.storeItem.Item.Amount;
		}
		num = Mathf.Clamp(num, 0, max);
		this.amountInput.text = num.ToString();
	}

	// Token: 0x04000A23 RID: 2595
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04000A24 RID: 2596
	[SerializeField]
	private Text itemAmount;

	// Token: 0x04000A25 RID: 2597
	[SerializeField]
	private InputField amountInput;

	// Token: 0x04000A26 RID: 2598
	[SerializeField]
	private Text itemName;

	// Token: 0x04000A27 RID: 2599
	[SerializeField]
	private Text itemValue;

	// Token: 0x04000A28 RID: 2600
	[SerializeField]
	private Text storeButton;

	// Token: 0x04000A29 RID: 2601
	private Item item;

	// Token: 0x04000A2A RID: 2602
	private bool isOver;

	// Token: 0x04000A2B RID: 2603
	private float overTime;

	// Token: 0x04000A2C RID: 2604
	private StoreAction storeAction;

	// Token: 0x04000A2D RID: 2605
	private bool itemDefined;

	// Token: 0x04000A2E RID: 2606
	private StoreItemConfig storeItem;

	// Token: 0x04000A2F RID: 2607
	private UISystemModule uiSystemModule;

	// Token: 0x04000A30 RID: 2608
	private float itemsUpdateTime;
}
