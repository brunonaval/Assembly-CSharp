using System;
using DuloGames.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000200 RID: 512
public class ItemBarSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00021274 File Offset: 0x0001F474
	// (set) Token: 0x060006A1 RID: 1697 RVA: 0x0002127C File Offset: 0x0001F47C
	public Item Item
	{
		get
		{
			return this._item;
		}
		set
		{
			this._item = value;
			this.shortcut.text = string.Format("F{0}", this.SlotPosition + 1);
			if (this._item.IsDefined)
			{
				this.NeedsUpdate = true;
				this.itemIcon.color = Color.white;
				this.itemIcon.sprite = this._item.Icon;
				if (this._item.Stackable)
				{
					this.itemAmount.enabled = true;
					this.itemAmount.text = this._item.Amount.ToString();
				}
				else
				{
					this.itemAmount.enabled = false;
				}
			}
			else
			{
				this.itemIcon.color = Color.gray;
				this.itemIcon.sprite = this.GetDefaultIcon();
				this.itemAmount.enabled = false;
			}
			this._item.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00021370 File Offset: 0x0001F570
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00021394 File Offset: 0x0001F594
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000213C0 File Offset: 0x0001F5C0
	private void OnDisable()
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000213E4 File Offset: 0x0001F5E4
	private void Update()
	{
		if (!this._item.IsDefined)
		{
			this.itemIcon.sprite = this.defaultIcon;
			this.slotBase.dragAndDropEnabled = false;
			return;
		}
		this.itemIcon.sprite = this._item.Icon;
		this.slotBase.dragAndDropEnabled = true;
		if (this.isOver && Time.time - this.overTime > 0.7f)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this._item);
		}
		if (this._item.Stackable && Time.time - this.amountTime > 0.5f)
		{
			if (!this.NeedsUpdate)
			{
				return;
			}
			int amount = this.uiSystemModule.InventoryModule.GetAmount(this._item.UniqueId);
			this.itemAmount.text = amount.ToString();
			this.NeedsUpdate = false;
			this.amountTime = Time.time;
		}
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x000214D8 File Offset: 0x0001F6D8
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000213C0 File Offset: 0x0001F5C0
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x000214EC File Offset: 0x0001F6EC
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Time.time - this.useTime < 1f)
			{
				return;
			}
			this.useTime = Time.time;
			this.uiSystemModule.ItemModule.CmdUseItemFromItemBar(this._item);
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00021541 File Offset: 0x0001F741
	public void OnDrag(PointerEventData eventData)
	{
		if (this._item.IsDefined)
		{
			this._item.SlotPosition = this.SlotPosition;
		}
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00021561 File Offset: 0x0001F761
	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.hovered.Count == 0)
		{
			this.uiSystemModule.InventoryModule.CmdRemoveFromItemBar(this.SlotPosition, true);
		}
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00021588 File Offset: 0x0001F788
	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag.CompareTag("UIEquipmentSlot"))
		{
			EquipmentSlotManager component = eventData.pointerDrag.GetComponent<EquipmentSlotManager>();
			if (component != null)
			{
				this.uiSystemModule.InventoryModule.CmdAddEquippedItemToItemBar(component.SlotType, this.SlotPosition, true);
				return;
			}
		}
		else if (eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			InventorySlotManager component2 = eventData.pointerDrag.GetComponent<InventorySlotManager>();
			if (component2 != null)
			{
				this.uiSystemModule.InventoryModule.CmdAddToItemBar(component2.SlotPosition, this.SlotPosition, true);
				return;
			}
		}
		else if (eventData.pointerDrag.CompareTag("UIItemBarSlot"))
		{
			ItemBarSlotManager component3 = eventData.pointerDrag.GetComponent<ItemBarSlotManager>();
			if (component3 != null)
			{
				this.uiSystemModule.InventoryModule.CmdSwapItemBarSlot(this.SlotPosition, component3.SlotPosition, true);
			}
		}
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x00021666 File Offset: 0x0001F866
	private Sprite GetDefaultIcon()
	{
		if (this.defaultIcon != null)
		{
			return this.defaultIcon;
		}
		this.defaultIcon = Resources.Load<Sprite>("Icons/Slots/default_item_slot");
		return this.defaultIcon;
	}

	// Token: 0x040008CA RID: 2250
	[SerializeField]
	private Image itemIcon;

	// Token: 0x040008CB RID: 2251
	[SerializeField]
	private Text itemAmount;

	// Token: 0x040008CC RID: 2252
	[SerializeField]
	private Text shortcut;

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private UISlotBase slotBase;

	// Token: 0x040008CE RID: 2254
	private bool isOver;

	// Token: 0x040008CF RID: 2255
	private float useTime;

	// Token: 0x040008D0 RID: 2256
	private float overTime;

	// Token: 0x040008D1 RID: 2257
	private float amountTime;

	// Token: 0x040008D2 RID: 2258
	private Sprite defaultIcon;

	// Token: 0x040008D3 RID: 2259
	private UISystemModule uiSystemModule;

	// Token: 0x040008D4 RID: 2260
	public int SlotPosition;

	// Token: 0x040008D5 RID: 2261
	public bool NeedsUpdate;

	// Token: 0x040008D6 RID: 2262
	private Item _item;
}
