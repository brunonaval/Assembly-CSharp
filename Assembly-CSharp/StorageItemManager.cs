using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000261 RID: 609
public class StorageItemManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06000931 RID: 2353 RVA: 0x0002B1AC File Offset: 0x000293AC
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0002B1D0 File Offset: 0x000293D0
	private void Update()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.storageItem.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.storageItem.Item);
		}
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0002B220 File Offset: 0x00029420
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.storageItem.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x0002B24C File Offset: 0x0002944C
	private void OnDisable()
	{
		this.isOver = false;
		if (this.storageItem.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0002B270 File Offset: 0x00029470
	public void SetMarketOrder(MarketStorage storageItem)
	{
		this.storageItem = storageItem;
		if (storageItem.IsDefined)
		{
			this.itemIconImage.sprite = storageItem.Item.Icon;
			this.itemNameText.text = storageItem.Item.DisplayName;
			this.itemAmountText.text = GlobalUtils.FormatLongNumber((long)storageItem.Amount, LanguageManager.Instance.GetText("api_culture"));
			this.itemIconImage.enabled = true;
		}
		else
		{
			this.itemIconImage.enabled = false;
		}
		this.amountText.text = GlobalUtils.FormatLongNumber((long)storageItem.Amount, LanguageManager.Instance.GetText("api_culture"));
		this.unitValueText.text = GlobalUtils.FormatLongNumber((long)storageItem.Item.Value, LanguageManager.Instance.GetText("api_culture"));
		this.priceText.text = GlobalUtils.FormatLongNumber((long)(storageItem.Item.Value * storageItem.Amount), LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0002B380 File Offset: 0x00029580
	public void OnTakeButtonClick()
	{
		if (!this.ValidateBeforeTake())
		{
			return;
		}
		this.uiSystemModule.PlayerModule.CmdTakeFromStorage(this.storageItem.Id);
		UnityEngine.Object.Destroy(this.storageItemObject);
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x0002B3B4 File Offset: 0x000295B4
	private bool ValidateBeforeTake()
	{
		if (this.uiSystemModule.AreaModule.AreaType != AreaType.ProtectedArea)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("storage_invalid_area_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (this.uiSystemModule.InventoryModule.GetTotalFreeSlots(this.storageItem.Item) == 0)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("inventory_full_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x06000938 RID: 2360 RVA: 0x0002B435 File Offset: 0x00029635
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0002B24C File Offset: 0x0002944C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.storageItem.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x04000ADB RID: 2779
	[SerializeField]
	private Image itemIconImage;

	// Token: 0x04000ADC RID: 2780
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000ADD RID: 2781
	[SerializeField]
	private Text itemNameText;

	// Token: 0x04000ADE RID: 2782
	[SerializeField]
	private Text amountText;

	// Token: 0x04000ADF RID: 2783
	[SerializeField]
	private Text unitValueText;

	// Token: 0x04000AE0 RID: 2784
	[SerializeField]
	private Text priceText;

	// Token: 0x04000AE1 RID: 2785
	[SerializeField]
	private GameObject storageItemObject;

	// Token: 0x04000AE2 RID: 2786
	private bool isOver;

	// Token: 0x04000AE3 RID: 2787
	private float overTime;

	// Token: 0x04000AE4 RID: 2788
	private MarketStorage storageItem;

	// Token: 0x04000AE5 RID: 2789
	private UISystemModule uiSystemModule;
}
