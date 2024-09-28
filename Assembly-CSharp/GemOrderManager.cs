using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000252 RID: 594
public class GemOrderManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060008B9 RID: 2233 RVA: 0x000297C8 File Offset: 0x000279C8
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060008BA RID: 2234 RVA: 0x000297EC File Offset: 0x000279EC
	private void Update()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.marketOrder.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.marketOrder.Item);
		}
	}

	// Token: 0x060008BB RID: 2235 RVA: 0x0002983C File Offset: 0x00027A3C
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00029868 File Offset: 0x00027A68
	private void OnDisable()
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0002988C File Offset: 0x00027A8C
	public void SetMarketOrder(MarketOrder order)
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
		this.unitValueText.text = order.UnitValue.ToString();
		this.priceText.text = (order.UnitValue * this.FormatAmountInput()).ToString();
		this.itemAmountInput.text = "1";
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00029960 File Offset: 0x00027B60
	public int FormatAmountInput()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num = Mathf.Max(num, 1);
		return num;
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0002998C File Offset: 0x00027B8C
	public void OnAmountChanged()
	{
		this.priceText.text = (this.marketOrder.UnitValue * this.FormatAmountInput()).ToString();
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x000299C0 File Offset: 0x00027BC0
	public void OnStoreButtonClick()
	{
		int amount = this.FormatAmountInput();
		if (!this.ValidateOrder(amount))
		{
			return;
		}
		this.uiSystemModule.PlayerModule.CmdBuyFromGemMarket(this.marketOrder.Id, amount);
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x000299FC File Offset: 0x00027BFC
	private bool ValidateOrder(int amount)
	{
		if (amount < 1)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_amount_invalid_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		int num = this.marketOrder.UnitValue * amount;
		if (this.uiSystemModule.WalletModule.Gems < num)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_gems_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00029A74 File Offset: 0x00027C74
	public void OnIncreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num++;
		this.itemAmountInput.text = num.ToString();
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00029AAC File Offset: 0x00027CAC
	public void OnDecreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num--;
		num = Mathf.Max(1, num);
		this.itemAmountInput.text = num.ToString();
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x00029AEA File Offset: 0x00027CEA
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x00029868 File Offset: 0x00027A68
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x04000A7D RID: 2685
	[SerializeField]
	private Image itemIconImage;

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000A7F RID: 2687
	[SerializeField]
	private InputField itemAmountInput;

	// Token: 0x04000A80 RID: 2688
	[SerializeField]
	private Text itemNameText;

	// Token: 0x04000A81 RID: 2689
	[SerializeField]
	private Text priceText;

	// Token: 0x04000A82 RID: 2690
	[SerializeField]
	private Text unitValueText;

	// Token: 0x04000A83 RID: 2691
	private bool isOver;

	// Token: 0x04000A84 RID: 2692
	private float overTime;

	// Token: 0x04000A85 RID: 2693
	private MarketOrder marketOrder;

	// Token: 0x04000A86 RID: 2694
	private UISystemModule uiSystemModule;
}
