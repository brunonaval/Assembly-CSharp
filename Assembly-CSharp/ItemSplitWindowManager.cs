using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200027F RID: 639
public class ItemSplitWindowManager : MonoBehaviour
{
	// Token: 0x060009C5 RID: 2501 RVA: 0x0002D5AE File Offset: 0x0002B7AE
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0002D5C6 File Offset: 0x0002B7C6
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) | Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			this.OnConfirmButtonClicked();
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0002D5E2 File Offset: 0x0002B7E2
	private IEnumerator Start()
	{
		yield return this.EnableInputAfterDelay();
		yield break;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0002D5F1 File Offset: 0x0002B7F1
	private void OnEnable()
	{
		base.StartCoroutine(this.EnableInputAfterDelay());
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0002D600 File Offset: 0x0002B800
	private void OnDisable()
	{
		this.selectedItem = default(Item);
		this.itemIcon.sprite = this.defaultItemIcon;
		this.itemTooltipManager.SetItem(default(Item));
		this.amountInput.text = "1";
		this.amountInput.interactable = false;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0002D65A File Offset: 0x0002B85A
	private int ParseAndClampAmount(int amount)
	{
		amount = Mathf.Min(750, amount);
		amount = Mathf.Min(this.selectedItem.Amount, amount);
		amount = Mathf.Max(1, amount);
		return amount;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0002D688 File Offset: 0x0002B888
	public void OnAmountInputChanged()
	{
		int amount;
		int.TryParse(this.amountInput.text, out amount);
		this.amountInput.text = this.ParseAndClampAmount(amount).ToString();
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0002D6C4 File Offset: 0x0002B8C4
	public void OnIncreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.amountInput.text, out num);
		num++;
		this.amountInput.text = this.ParseAndClampAmount(num).ToString();
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0002D704 File Offset: 0x0002B904
	public void OnDescreaseAmountButtonClick()
	{
		int num;
		int.TryParse(this.amountInput.text, out num);
		num--;
		this.amountInput.text = this.ParseAndClampAmount(num).ToString();
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0002D744 File Offset: 0x0002B944
	public void OnConfirmButtonClicked()
	{
		int num;
		int.TryParse(this.amountInput.text, out num);
		this.amountInput.text = this.ParseAndClampAmount(num).ToString();
		Action<int> action = this.confirmCallback;
		if (action != null)
		{
			action(num);
		}
		DragWindowModule dragWindowModule;
		base.TryGetComponent<DragWindowModule>(out dragWindowModule);
		dragWindowModule.CloseWindow();
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0002D7A0 File Offset: 0x0002B9A0
	private void SetSelectedItem(Item item)
	{
		if (!item.IsDefined)
		{
			return;
		}
		this.selectedItem = item;
		this.itemTooltipManager.SetItem(item);
		this.itemIcon.sprite = item.Icon;
		this.itemAmountText.text = this.selectedItem.Amount.ToString();
		this.amountInput.text = item.Amount.ToString();
		this.amountInput.Select();
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0002D819 File Offset: 0x0002BA19
	public void Initialize(Item item, Action<int> callback)
	{
		this.confirmCallback = callback;
		this.SetSelectedItem(item);
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0002D829 File Offset: 0x0002BA29
	private IEnumerator EnableInputAfterDelay()
	{
		yield return new WaitForSecondsRealtime(0.1f);
		this.amountInput.interactable = true;
		if (!GlobalSettings.IsMobilePlatform)
		{
			this.amountInput.Select();
		}
		yield break;
	}

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private ItemTooltipManager itemTooltipManager;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	private Sprite defaultItemIcon;

	// Token: 0x04000B54 RID: 2900
	[SerializeField]
	private InputField amountInput;

	// Token: 0x04000B55 RID: 2901
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000B56 RID: 2902
	private Item selectedItem;

	// Token: 0x04000B57 RID: 2903
	private Action<int> confirmCallback;

	// Token: 0x04000B58 RID: 2904
	private UISystemModule uiSystemModule;
}
