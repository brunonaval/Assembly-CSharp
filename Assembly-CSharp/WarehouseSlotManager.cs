using System;
using System.Linq;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200026A RID: 618
public class WarehouseSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	// Token: 0x170000F4 RID: 244
	// (get) Token: 0x0600095A RID: 2394 RVA: 0x0002BD5F File Offset: 0x00029F5F
	// (set) Token: 0x0600095B RID: 2395 RVA: 0x0002BD68 File Offset: 0x00029F68
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

	// Token: 0x0600095C RID: 2396 RVA: 0x0002BE54 File Offset: 0x0002A054
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		gameObject.TryGetComponent<Canvas>(out this.uiSystemCanvas);
		gameObject.TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0002BE79 File Offset: 0x0002A079
	private void Start()
	{
		this.vocationAllowedItemTypes = this.uiSystemModule.VocationModule.AllowedItemTypes();
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0002BE94 File Offset: 0x0002A094
	private void Update()
	{
		if (this._item.IsDefined)
		{
			this.itemIcon.enabled = true;
			this.itemIcon.sprite = this._item.Icon;
			this.UpdateBetterWorseItemIcon();
		}
		if (this.isOver && this._item.IsDefined && Time.time - this.overTime > 0.7f)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this._item);
		}
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0002BF14 File Offset: 0x0002A114
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0002BF40 File Offset: 0x0002A140
	private void OnDisable()
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0002BF64 File Offset: 0x0002A164
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

	// Token: 0x06000962 RID: 2402 RVA: 0x0002C004 File Offset: 0x0002A204
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0002BF40 File Offset: 0x0002A140
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0002C018 File Offset: 0x0002A218
	public void OnDrag(PointerEventData eventData)
	{
		if (this._item.IsDefined)
		{
			this._item.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0002C038 File Offset: 0x0002A238
	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.hovered.Count == 0 & eventData.button == PointerEventData.InputButton.Left)
		{
			if (!this.uiSystemCanvas.enabled)
			{
				return;
			}
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (SettingsManager.Instance.DisableDropProtection)
			{
				this.uiSystemModule.WarehouseModule.CmdDropItem(this.SlotPosition, mousePosition);
				return;
			}
			string text = LanguageManager.Instance.GetText("drop_item_confirm_message");
			text = string.Format(text, this.Item.DisplayName);
			this.uiSystemModule.ShowConfirmationWindow(this.Item.Icon, text, delegate
			{
				this.uiSystemModule.WarehouseModule.CmdDropItem(this.SlotPosition, mousePosition);
			});
		}
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0002C10B File Offset: 0x0002A30B
	public void OnDrop(PointerEventData eventData)
	{
		this.ProcessWarehouseDrop(eventData);
		this.ProcessInventoryDrop(eventData);
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0002C11C File Offset: 0x0002A31C
	private void ProcessWarehouseDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UIWarehouseSlot"))
		{
			return;
		}
		Item draggingItem = eventData.pointerDrag.GetComponent<WarehouseSlotManager>().Item;
		if (!draggingItem.IsDefined)
		{
			return;
		}
		if (draggingItem.SlotPosition == this.Item.SlotPosition)
		{
			return;
		}
		if (this.Item.IsDefined && this.Item.UniqueId == draggingItem.UniqueId && (!this.Item.Soulbind | this.Item.OwnerId == draggingItem.OwnerId) && this.Item.Stackable)
		{
			if ((GlobalSettings.IsMobilePlatform | Input.GetKey(KeyCode.LeftShift)) && this.Item.Amount < 750)
			{
				this.uiSystemModule.ShowItemSplitWindow(draggingItem, delegate(int amount)
				{
					this.uiSystemModule.WarehouseModule.CmdAddAmountFromSlot(draggingItem.SlotPosition, this.SlotPosition, amount, true);
				});
				return;
			}
			if (this.Item.Amount + draggingItem.Amount <= 750)
			{
				this.uiSystemModule.WarehouseModule.CmdAddAmountFromSlot(draggingItem.SlotPosition, this.SlotPosition, draggingItem.Amount, true);
				return;
			}
			this.uiSystemModule.WarehouseModule.CmdSwapPosition(this.SlotPosition, draggingItem.SlotPosition);
		}
		if (!this.Item.IsDefined && draggingItem.Amount > 1 && (GlobalSettings.IsMobilePlatform | Input.GetKey(KeyCode.LeftShift)))
		{
			this.uiSystemModule.ShowItemSplitWindow(draggingItem, delegate(int amount)
			{
				this.uiSystemModule.WarehouseModule.CmdAddAmountFromSlot(draggingItem.SlotPosition, this.SlotPosition, amount, true);
			});
			return;
		}
		this.uiSystemModule.WarehouseModule.CmdSwapPosition(this.SlotPosition, draggingItem.SlotPosition);
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0002C318 File Offset: 0x0002A518
	private void ProcessInventoryDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			InventorySlotManager component = eventData.pointerDrag.GetComponent<InventorySlotManager>();
			if (component != null && this.uiSystemModule.WarehouseModule.HasFreeSlots)
			{
				this.uiSystemModule.InventoryModule.CmdSendToWarehouse(component.SlotPosition, this.SlotPosition);
			}
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0002C37A File Offset: 0x0002A57A
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (Time.time - this.dragTime < 0.3f)
		{
			eventData.pointerDrag.GetComponent<UISlotBase>().dropPreformed = true;
			return;
		}
		this.dragTime = Time.time;
	}

	// Token: 0x04000B00 RID: 2816
	private Item _item;

	// Token: 0x04000B01 RID: 2817
	public int SlotPosition;

	// Token: 0x04000B02 RID: 2818
	private bool isOver;

	// Token: 0x04000B03 RID: 2819
	private float overTime;

	// Token: 0x04000B04 RID: 2820
	private float dragTime;

	// Token: 0x04000B05 RID: 2821
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04000B06 RID: 2822
	[SerializeField]
	private Text itemAmount;

	// Token: 0x04000B07 RID: 2823
	[SerializeField]
	private UISlotBase slotBase;

	// Token: 0x04000B08 RID: 2824
	[SerializeField]
	private GameObject betterItemIcon;

	// Token: 0x04000B09 RID: 2825
	[SerializeField]
	private GameObject worseItemIcon;

	// Token: 0x04000B0A RID: 2826
	[SerializeField]
	private Image frameImage;

	// Token: 0x04000B0B RID: 2827
	private Canvas uiSystemCanvas;

	// Token: 0x04000B0C RID: 2828
	private UISystemModule uiSystemModule;

	// Token: 0x04000B0D RID: 2829
	private ItemType[] vocationAllowedItemTypes;
}
