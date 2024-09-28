using System;
using System.Linq;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001F8 RID: 504
public class InventorySlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerDownHandler
{
	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x00020339 File Offset: 0x0001E539
	// (set) Token: 0x0600065F RID: 1631 RVA: 0x00020344 File Offset: 0x0001E544
	public Item Item
	{
		get
		{
			return this._item;
		}
		set
		{
			this._item = value;
			if (this._item.IsDefined)
			{
				this.itemIcon.enabled = true;
				this.itemAmount.enabled = true;
				this.slotBase.dragAndDropEnabled = true;
				this.itemIcon.sprite = null;
				this.itemIcon.sprite = this._item.Icon;
				this.itemAmount.text = this._item.Amount.ToString();
			}
			else
			{
				this.worseItemIcon.SetActive(false);
				this.betterItemIcon.SetActive(false);
				this.itemIcon.enabled = false;
				this.itemAmount.enabled = false;
				this.slotBase.dragAndDropEnabled = false;
			}
			this.frameImage.color = GlobalUtils.RarityToFrameColor(this._item.Rarity);
			this._item.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00020430 File Offset: 0x0001E630
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		gameObject.TryGetComponent<Canvas>(out this.uiSystemCanvas);
		gameObject.TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00020455 File Offset: 0x0001E655
	private void Start()
	{
		this.vocationAllowedItemTypes = this.uiSystemModule.VocationModule.AllowedItemTypes();
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x0002046D File Offset: 0x0001E66D
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x00020499 File Offset: 0x0001E699
	private void OnDisable()
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x000204BC File Offset: 0x0001E6BC
	private void Update()
	{
		if (this._item.IsDefined)
		{
			this.itemIcon.enabled = true;
			this.itemIcon.sprite = this._item.Icon;
			this.UpdateBetterWorseItemIcon();
		}
		if (this.isOver && Time.time - this.overTime > 0.7f && this._item.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this._item);
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x0002053C File Offset: 0x0001E73C
	private void UpdateBetterWorseItemIcon()
	{
		if (!this.vocationAllowedItemTypes.Contains(this._item.Type))
		{
			this.betterItemIcon.SetActive(false);
			this.worseItemIcon.SetActive(false);
			return;
		}
		Item item = this.uiSystemModule.EquipmentModule.GetItem(this._item.SlotType);
		int num = (int)(this._item.Attack + this._item.Defense);
		int num2 = (int)(item.Attack + item.Defense);
		this.betterItemIcon.SetActive(num > num2);
		this.worseItemIcon.SetActive(num < num2);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000205DC File Offset: 0x0001E7DC
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00020499 File Offset: 0x0001E699
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x000205F0 File Offset: 0x0001E7F0
	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Time.time - this.clickTime < 0.3f)
			{
				this.clickTime = 0f;
				if (Time.time - this.useTime < 1f)
				{
					return;
				}
				this.useTime = Time.time;
				this.uiSystemModule.ItemModule.CmdUseItem(this._item.SlotPosition);
				this.uiSystemModule.CloseItemTooltip();
				this.isOver = false;
				return;
			}
			else
			{
				this.clickTime = Time.time;
			}
		}
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x0002067B File Offset: 0x0001E87B
	public void OnDrag(PointerEventData eventData)
	{
		if (this._item.IsDefined)
		{
			this._item.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x0002069C File Offset: 0x0001E89C
	public void OnEndDrag(PointerEventData eventData)
	{
		bool flag;
		if (eventData.hovered.Count != 0)
		{
			flag = eventData.hovered.Any((GameObject h) => h.CompareTag("Joystick"));
		}
		else
		{
			flag = true;
		}
		if (flag & eventData.button == PointerEventData.InputButton.Left)
		{
			if (!this.uiSystemCanvas.enabled)
			{
				return;
			}
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (SettingsManager.Instance.DisableDropProtection)
			{
				this.uiSystemModule.InventoryModule.CmdDropItem(this.SlotPosition, mousePosition);
				return;
			}
			string text = LanguageManager.Instance.GetText("drop_item_confirm_message");
			text = string.Format(text, this.Item.DisplayName);
			this.uiSystemModule.ShowConfirmationWindow(this.Item.Icon, text, delegate
			{
				this.uiSystemModule.InventoryModule.CmdDropItem(this.SlotPosition, mousePosition);
			});
		}
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002079B File Offset: 0x0001E99B
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData == null)
		{
			return;
		}
		if (eventData.pointerDrag == null)
		{
			return;
		}
		if (eventData.pointerDrag.GetComponent<UISlotBase>().dropPreformed)
		{
			return;
		}
		this.ProcessInventoryDrop(eventData);
		this.ProcessEquipmentDrop(eventData);
		this.ProcessWarehouseDrop(eventData);
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x000207D8 File Offset: 0x0001E9D8
	private void ProcessWarehouseDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.CompareTag("UIWarehouseSlot"))
		{
			WarehouseSlotManager component = eventData.pointerDrag.GetComponent<WarehouseSlotManager>();
			Item item = component.Item;
			if (this.uiSystemModule.InventoryModule.HasFreeSlots(item, item.Amount))
			{
				this.uiSystemModule.WarehouseModule.CmdSendToInventory(component.SlotPosition, this.SlotPosition);
			}
		}
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00020840 File Offset: 0x0001EA40
	private void ProcessEquipmentDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.CompareTag("UIEquipmentSlot"))
		{
			EquipmentSlotManager component = eventData.pointerDrag.GetComponent<EquipmentSlotManager>();
			Item item = component.Item;
			if (this.uiSystemModule.InventoryModule.HasFreeSlots(item, item.Amount))
			{
				this.uiSystemModule.EquipmentModule.CmdUnequip(component.SlotType, true);
			}
		}
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x000208A4 File Offset: 0x0001EAA4
	private void ProcessInventoryDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			return;
		}
		Item draggingItem = eventData.pointerDrag.GetComponent<InventorySlotManager>().Item;
		if (!draggingItem.IsDefined)
		{
			return;
		}
		if (draggingItem.SlotPosition == this._item.SlotPosition)
		{
			return;
		}
		if (this._item.IsDefined && this._item.UniqueId == draggingItem.UniqueId && this._item.Stackable)
		{
			if ((GlobalSettings.IsMobilePlatform | Input.GetKey(KeyCode.LeftShift)) && this._item.Amount < 750)
			{
				this.uiSystemModule.ShowItemSplitWindow(draggingItem, delegate(int amount)
				{
					this.uiSystemModule.InventoryModule.CmdAddAmountFromSlot(draggingItem.SlotPosition, this.SlotPosition, amount, true);
				});
				return;
			}
			if (this._item.Amount + draggingItem.Amount <= 750)
			{
				this.uiSystemModule.InventoryModule.CmdAddAmountFromSlot(draggingItem.SlotPosition, this.SlotPosition, draggingItem.Amount, true);
				return;
			}
			this.uiSystemModule.InventoryModule.CmdSwapItemPosition(this.SlotPosition, draggingItem.SlotPosition, true);
			return;
		}
		else
		{
			if (!this._item.IsDefined && draggingItem.Amount > 1 && (Input.GetKey(KeyCode.LeftShift) | GlobalSettings.IsMobilePlatform))
			{
				this.uiSystemModule.ShowItemSplitWindow(draggingItem, delegate(int amount)
				{
					this.uiSystemModule.InventoryModule.CmdAddAmountFromSlot(draggingItem.SlotPosition, this.SlotPosition, amount, true);
				});
				return;
			}
			this.uiSystemModule.InventoryModule.CmdSwapItemPosition(this.SlotPosition, draggingItem.SlotPosition, true);
			return;
		}
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00020A6C File Offset: 0x0001EC6C
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (Time.time - this.dragTime < 0.3f)
		{
			eventData.pointerDrag.GetComponent<UISlotBase>().dropPreformed = true;
			return;
		}
		this.dragTime = Time.time;
	}

	// Token: 0x04000892 RID: 2194
	public int SlotPosition;

	// Token: 0x04000893 RID: 2195
	private Item _item;

	// Token: 0x04000894 RID: 2196
	private bool isOver;

	// Token: 0x04000895 RID: 2197
	private float overTime;

	// Token: 0x04000896 RID: 2198
	private float clickTime;

	// Token: 0x04000897 RID: 2199
	private float dragTime;

	// Token: 0x04000898 RID: 2200
	private float useTime;

	// Token: 0x04000899 RID: 2201
	[SerializeField]
	private Image itemIcon;

	// Token: 0x0400089A RID: 2202
	[SerializeField]
	private Text itemAmount;

	// Token: 0x0400089B RID: 2203
	[SerializeField]
	private GameObject betterItemIcon;

	// Token: 0x0400089C RID: 2204
	[SerializeField]
	private GameObject worseItemIcon;

	// Token: 0x0400089D RID: 2205
	[SerializeField]
	private Image frameImage;

	// Token: 0x0400089E RID: 2206
	[SerializeField]
	private UISlotBase slotBase;

	// Token: 0x0400089F RID: 2207
	private Canvas uiSystemCanvas;

	// Token: 0x040008A0 RID: 2208
	private UISystemModule uiSystemModule;

	// Token: 0x040008A1 RID: 2209
	private ItemType[] vocationAllowedItemTypes;
}
