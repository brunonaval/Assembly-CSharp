using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000260 RID: 608
public class SellOrderManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600091E RID: 2334 RVA: 0x0002AC9C File Offset: 0x00028E9C
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0002ACC0 File Offset: 0x00028EC0
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0002ACEC File Offset: 0x00028EEC
	private void OnDisable()
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0002AD10 File Offset: 0x00028F10
	private void Update()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.marketOrder.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.marketOrder.Item);
		}
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0002AD60 File Offset: 0x00028F60
	public void SetOrder(MarketOrder order)
	{
		this.marketOrder = order;
		if (this.marketOrder.IsDefined)
		{
			this.itemIconImage.sprite = this.marketOrder.Item.Icon;
			this.itemNameText.text = order.Item.DisplayName;
			this.itemAmountText.text = this.marketOrder.Amount.ToString();
			this.itemIconImage.enabled = true;
		}
		else
		{
			this.itemIconImage.enabled = false;
		}
		this.itemAmountInput.text = "1";
		this.unitValueInput.text = this.marketOrder.UnitValue.ToString();
		int num = this.FormatUnitValueInput() * this.FormatAmountInput();
		this.priceText.text = GlobalUtils.FormatLongNumber((long)num, LanguageManager.Instance.GetText("api_culture"));
		this.feeText.text = GlobalUtils.FormatLongNumber((long)this.CalculateFee(num), LanguageManager.Instance.GetText("api_culture"));
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0002AE6C File Offset: 0x0002906C
	public int FormatAmountInput()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num = Mathf.Max(num, 1);
		return num;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x0002AE98 File Offset: 0x00029098
	public int FormatUnitValueInput()
	{
		int num;
		int.TryParse(this.unitValueInput.text, out num);
		num = Mathf.Max(num, 1);
		return num;
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x0002AEC4 File Offset: 0x000290C4
	public void OnIncreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num++;
		this.itemAmountInput.text = num.ToString();
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0002AEFC File Offset: 0x000290FC
	public void OnDecreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.itemAmountInput.text, out num);
		num--;
		num = Mathf.Max(1, num);
		this.itemAmountInput.text = num.ToString();
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0002AF3C File Offset: 0x0002913C
	public void OnIncreaseUnitValueButtonClick()
	{
		int num;
		int.TryParse(this.unitValueInput.text, out num);
		num++;
		this.unitValueInput.text = num.ToString();
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0002AF74 File Offset: 0x00029174
	public void OnDecreaseUnitValueButtonClick()
	{
		int num;
		int.TryParse(this.unitValueInput.text, out num);
		num--;
		num = Mathf.Max(1, num);
		this.unitValueInput.text = num.ToString();
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0002AFB4 File Offset: 0x000291B4
	public void OnSellButtonClick()
	{
		int num = this.FormatAmountInput();
		int unitValue = this.FormatUnitValueInput();
		if (!this.ValidateOrder(num, unitValue))
		{
			return;
		}
		this.uiSystemModule.PlayerModule.CmdPlaceOrder(this.marketOrder, num, unitValue);
		if (num >= this.marketOrder.Amount)
		{
			UnityEngine.Object.Destroy(this.orderObject);
			return;
		}
		this.marketOrder.Amount = this.marketOrder.Amount - num;
		this.SetOrder(this.marketOrder);
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0002B028 File Offset: 0x00029228
	private bool ValidateOrder(int amount, int unitValue)
	{
		if (this.uiSystemModule.AreaModule.AreaType != AreaType.ProtectedArea)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_invalid_area_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (amount < 1)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_amount_invalid_message", 3, 3.5f, Array.Empty<string>());
			return false;
		}
		if (unitValue < this.marketOrder.Item.Value)
		{
			this.uiSystemModule.EffectModule.ShowScreenMessage("market_value_too_low_message", 0, 3.5f, Array.Empty<string>());
			return false;
		}
		return true;
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0002B0C8 File Offset: 0x000292C8
	public void OnAmountInputValueChanged()
	{
		int price = this.FormatUnitValueInput() * this.FormatAmountInput();
		this.priceText.text = price.ToString();
		this.feeText.text = this.CalculateFee(price).ToString();
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0002B110 File Offset: 0x00029310
	public void OnUnitValueInputValueChanged()
	{
		int price = this.FormatUnitValueInput() * this.FormatAmountInput();
		this.priceText.text = price.ToString();
		this.feeText.text = this.CalculateFee(price).ToString();
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0002B158 File Offset: 0x00029358
	private int CalculateFee(int price)
	{
		int num = Mathf.RoundToInt((float)price * 0.04f);
		if (this.uiSystemModule.PlayerModule.PremiumDays > 0)
		{
			num -= Mathf.RoundToInt((float)num * 0.5f);
		}
		return num;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0002B197 File Offset: 0x00029397
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0002ACEC File Offset: 0x00028EEC
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.marketOrder.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x04000ACF RID: 2767
	[SerializeField]
	private Image itemIconImage;

	// Token: 0x04000AD0 RID: 2768
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000AD1 RID: 2769
	[SerializeField]
	private InputField itemAmountInput;

	// Token: 0x04000AD2 RID: 2770
	[SerializeField]
	private InputField unitValueInput;

	// Token: 0x04000AD3 RID: 2771
	[SerializeField]
	private Text itemNameText;

	// Token: 0x04000AD4 RID: 2772
	[SerializeField]
	private Text priceText;

	// Token: 0x04000AD5 RID: 2773
	[SerializeField]
	private Text feeText;

	// Token: 0x04000AD6 RID: 2774
	[SerializeField]
	private GameObject orderObject;

	// Token: 0x04000AD7 RID: 2775
	private bool isOver;

	// Token: 0x04000AD8 RID: 2776
	private float overTime;

	// Token: 0x04000AD9 RID: 2777
	private MarketOrder marketOrder;

	// Token: 0x04000ADA RID: 2778
	private UISystemModule uiSystemModule;
}
