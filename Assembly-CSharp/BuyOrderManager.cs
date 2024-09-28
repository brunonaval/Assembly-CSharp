using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000251 RID: 593
public class BuyOrderManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060008AB RID: 2219 RVA: 0x000293F0 File Offset: 0x000275F0
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00029414 File Offset: 0x00027614
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00029440 File Offset: 0x00027640
	private void OnDisable()
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00029464 File Offset: 0x00027664
	private void Update()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.marketOrder.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.marketOrder.Item);
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x000294B4 File Offset: 0x000276B4
	public void SetOrder(MarketOrder order)
	{
		this.marketOrder = order;
		if (this.marketOrder.IsDefined)
		{
			this.itemIconImage.sprite = this.marketOrder.Item.Icon;
			this.itemNameText.text = this.marketOrder.Item.DisplayName;
			this.itemAmountText.text = this.marketOrder.Amount.ToString();
			this.sellerNameText.text = this.marketOrder.SellerName;
			this.itemIconImage.enabled = true;
		}
		else
		{
			this.itemIconImage.enabled = false;
		}
		this.itemAmountInput.text = "1";
		this.unitValueText.text = GlobalUtils.FormatLongNumber((long)this.marketOrder.UnitValue, LanguageManager.Instance.GetText("api_culture"));
		this.priceText.text = GlobalUtils.FormatLongNumber((long)(this.marketOrder.UnitValue * this.FormatAmountInput()), LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x000295C4 File Offset: 0x000277C4
	public int FormatAmountInput()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num = Mathf.Max(num, 1);
		return num;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000295F0 File Offset: 0x000277F0
	public void OnAmountInputValueChanged()
	{
		this.priceText.text = (this.marketOrder.UnitValue * this.FormatAmountInput()).ToString();
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00029624 File Offset: 0x00027824
	public void OnIncreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num++;
		this.itemAmountInput.text = num.ToString();
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0002965C File Offset: 0x0002785C
	public void OnDecreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num--;
		num = Mathf.Max(1, num);
		this.itemAmountInput.text = num.ToString();
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0002969C File Offset: 0x0002789C
	public void OnBuyButtonClick()
	{
		int num = this.FormatAmountInput();
		if (!this.ValidateOrder(num))
		{
			return;
		}
		this.uiSystemModule.PlayerModule.CmdBuyFromMarket(this.marketOrder.Id, num);
		if (num >= this.marketOrder.Amount)
		{
			UnityEngine.Object.Destroy(this.orderObject);
			return;
		}
		this.marketOrder.Amount = this.marketOrder.Amount - num;
		this.SetOrder(this.marketOrder);
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x0002970C File Offset: 0x0002790C
	private bool ValidateOrder(int amount)
	{
		if (amount < 1)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_amount_invalid_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		int num = this.marketOrder.UnitValue * amount;
		if (this.uiSystemModule.WalletModule.GoldCoins < (long)num)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("not_enough_money_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (amount > this.marketOrder.Amount)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_amount_unavailable_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000297B4 File Offset: 0x000279B4
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00029440 File Offset: 0x00027640
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x04000A70 RID: 2672
	[SerializeField]
	private Image itemIconImage;

	// Token: 0x04000A71 RID: 2673
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000A72 RID: 2674
	[SerializeField]
	private Text sellerNameText;

	// Token: 0x04000A73 RID: 2675
	[SerializeField]
	private InputField itemAmountInput;

	// Token: 0x04000A74 RID: 2676
	[SerializeField]
	private Text unitValueText;

	// Token: 0x04000A75 RID: 2677
	[SerializeField]
	private Text itemNameText;

	// Token: 0x04000A76 RID: 2678
	[SerializeField]
	private Text priceText;

	// Token: 0x04000A77 RID: 2679
	[SerializeField]
	private Button buyButton;

	// Token: 0x04000A78 RID: 2680
	[SerializeField]
	private GameObject orderObject;

	// Token: 0x04000A79 RID: 2681
	private bool isOver;

	// Token: 0x04000A7A RID: 2682
	private float overTime;

	// Token: 0x04000A7B RID: 2683
	private MarketOrder marketOrder;

	// Token: 0x04000A7C RID: 2684
	private UISystemModule uiSystemModule;
}
