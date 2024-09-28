using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F3 RID: 499
public class EquipmentSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IEndDragHandler, IDropHandler, IPointerDownHandler
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001FB48 File Offset: 0x0001DD48
	// (set) Token: 0x0600063A RID: 1594 RVA: 0x0001FB50 File Offset: 0x0001DD50
	public Item Item { get; private set; }

	// Token: 0x0600063B RID: 1595 RVA: 0x0001FB5C File Offset: 0x0001DD5C
	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		gameObject.TryGetComponent<Canvas>(out this.uiSystemCanvas);
		gameObject.TryGetComponent<UISystemModule>(out this.uiSystemModule);
		this.itemIcon.gameObject.SetActive(false);
		this.emptyIcon.gameObject.SetActive(true);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0001FBB0 File Offset: 0x0001DDB0
	private void Update()
	{
		Image image = this.itemIcon;
		Item item = this.Item;
		image.sprite = item.Icon;
		GameObject gameObject = this.itemIcon.gameObject;
		item = this.Item;
		gameObject.SetActive(item.IsDefined);
		GameObject gameObject2 = this.emptyIcon.gameObject;
		item = this.Item;
		gameObject2.SetActive(!item.IsDefined);
		if (this.itemAmountText != null)
		{
			Text text = this.itemAmountText;
			string text2;
			if (this.Item.Amount <= 1)
			{
				text2 = string.Empty;
			}
			else
			{
				item = this.Item;
				text2 = item.Amount.ToString();
			}
			text.text = text2;
		}
		Graphic graphic = this.itemIcon;
		int baseLevel = this.uiSystemModule.AttributeModule.BaseLevel;
		item = this.Item;
		graphic.color = ((baseLevel < item.RequiredLevel) ? Color.red : Color.white);
		if (this.isOver && Time.time - this.overTime > 0.7f && !string.IsNullOrEmpty(this.Item.Name))
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.Item);
		}
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0001FCD0 File Offset: 0x0001DED0
	private void OnDisable()
	{
		this.isOver = false;
		if (this.Item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0001FCFF File Offset: 0x0001DEFF
	public void SetItem(Item item)
	{
		this.Item = item;
		this.frameImage.color = GlobalUtils.RarityToFrameColor(item.Rarity);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0001FD20 File Offset: 0x0001DF20
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Time.time - this.clickTime < 0.3f)
			{
				this.clickTime = 0f;
				if (this.uiSystemModule.InventoryModule.HasFreeSlots(this.Item, this.Item.Amount))
				{
					this.uiSystemModule.EquipmentModule.CmdUnequip(this.SlotType, true);
					this.uiSystemModule.CloseItemTooltip();
					this.isOver = false;
					return;
				}
			}
			else
			{
				this.clickTime = Time.time;
			}
		}
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0001FDAB File Offset: 0x0001DFAB
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001FDC0 File Offset: 0x0001DFC0
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.itemIcon != null)
		{
			this.itemIcon.color = Color.white;
		}
		this.isOver = false;
		if (this.Item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0001FE10 File Offset: 0x0001E010
	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.hovered.Count == 0)
		{
			if (!this.uiSystemCanvas.enabled)
			{
				return;
			}
			Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			this.uiSystemModule.EquipmentModule.CmdUnequipToGround(this.SlotType, position, false, true);
		}
		if (this.itemIcon != null)
		{
			this.itemIcon.color = Color.white;
		}
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x0001FE80 File Offset: 0x0001E080
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			InventorySlotManager component = eventData.pointerDrag.GetComponent<InventorySlotManager>();
			if (component != null && this.SlotType == component.Item.SlotType)
			{
				this.uiSystemModule.EquipmentModule.CmdEquipFromInventory(component.SlotPosition, true);
			}
		}
		if (this.itemIcon != null)
		{
			this.itemIcon.color = Color.white;
		}
	}

	// Token: 0x0400087E RID: 2174
	public SlotType SlotType;

	// Token: 0x0400087F RID: 2175
	private bool isOver;

	// Token: 0x04000880 RID: 2176
	private float overTime;

	// Token: 0x04000881 RID: 2177
	private float clickTime;

	// Token: 0x04000882 RID: 2178
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04000883 RID: 2179
	[SerializeField]
	private Image emptyIcon;

	// Token: 0x04000884 RID: 2180
	[SerializeField]
	private Text itemAmountText;

	// Token: 0x04000885 RID: 2181
	[SerializeField]
	private Image frameImage;

	// Token: 0x04000886 RID: 2182
	private Canvas uiSystemCanvas;

	// Token: 0x04000887 RID: 2183
	private UISystemModule uiSystemModule;
}
