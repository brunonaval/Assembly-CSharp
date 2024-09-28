using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200025B RID: 603
public class PlayerOrderManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060008FB RID: 2299 RVA: 0x0002A5DC File Offset: 0x000287DC
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0002A600 File Offset: 0x00028800
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0002A62C File Offset: 0x0002882C
	private void OnDisable()
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002A650 File Offset: 0x00028850
	private void Update()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.marketOrder.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.marketOrder.Item);
		}
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0002A6A0 File Offset: 0x000288A0
	public void SetOrder(MarketOrder order)
	{
		this.marketOrder = order;
		if (this.marketOrder.IsDefined)
		{
			this.itemIconImage.sprite = this.marketOrder.Item.Icon;
			this.itemNameText.text = this.marketOrder.Item.DisplayName;
			this.itemAmountText.text = this.marketOrder.Amount.ToString();
			this.itemIconImage.enabled = true;
		}
		else
		{
			this.itemIconImage.enabled = false;
		}
		this.amountText.text = this.marketOrder.Amount.ToString();
		this.unitValueText.text = GlobalUtils.FormatLongNumber((long)this.marketOrder.UnitValue, LanguageManager.Instance.GetText("api_culture"));
		this.priceText.text = GlobalUtils.FormatLongNumber((long)(this.marketOrder.UnitValue * this.marketOrder.Amount), LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0002A7A9 File Offset: 0x000289A9
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0002A62C File Offset: 0x0002882C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0002A7BD File Offset: 0x000289BD
	public void OnCancelButtonClick()
	{
		if (!this.ValidateOrder())
		{
			return;
		}
		this.uiSystemModule.PlayerModule.CmdCancelOrder(this.marketOrder.Id);
		UnityEngine.Object.Destroy(this.orderObject);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0002A7F0 File Offset: 0x000289F0
	private bool ValidateOrder()
	{
		if (this.marketOrder.PlayerId != this.uiSystemModule.PlayerModule.PlayerId)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_cant_cancel_others_offer_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x04000AB0 RID: 2736
	[SerializeField]
	private Image itemIconImage;

	// Token: 0x04000AB1 RID: 2737
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000AB2 RID: 2738
	[SerializeField]
	private Text itemNameText;

	// Token: 0x04000AB3 RID: 2739
	[SerializeField]
	private Text amountText;

	// Token: 0x04000AB4 RID: 2740
	[SerializeField]
	private Text unitValueText;

	// Token: 0x04000AB5 RID: 2741
	[SerializeField]
	private Text priceText;

	// Token: 0x04000AB6 RID: 2742
	[SerializeField]
	private GameObject orderObject;

	// Token: 0x04000AB7 RID: 2743
	private bool isOver;

	// Token: 0x04000AB8 RID: 2744
	private float overTime;

	// Token: 0x04000AB9 RID: 2745
	private MarketOrder marketOrder;

	// Token: 0x04000ABA RID: 2746
	private UISystemModule uiSystemModule;
}
