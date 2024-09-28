using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000249 RID: 585
public class TradeItemSlotManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x00028413 File Offset: 0x00026613
	// (set) Token: 0x0600086E RID: 2158 RVA: 0x0002841C File Offset: 0x0002661C
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
				this.itemIcon.sprite = this._item.Icon;
				this.itemAmount.text = this._item.Amount.ToString();
			}
			else
			{
				this.worseItemIcon.SetActive(false);
				this.betterItemIcon.SetActive(false);
				this.itemIcon.enabled = false;
				this.itemAmount.enabled = false;
			}
			this.frameImage.color = GlobalUtils.RarityToFrameColor(this._item.Rarity);
		}
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x000284D3 File Offset: 0x000266D3
	private void Awake()
	{
		GameObject.FindGameObjectWithTag("UISystem").TryGetComponent<UISystemModule>(out this.uiSystemModule);
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x000284EB File Offset: 0x000266EB
	private void Start()
	{
		this.vocationAllowedItemTypes = this.uiSystemModule.VocationModule.AllowedItemTypes();
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00028503 File Offset: 0x00026703
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0002852F File Offset: 0x0002672F
	private void OnDisable()
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x00028550 File Offset: 0x00026750
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

	// Token: 0x06000874 RID: 2164 RVA: 0x000285D0 File Offset: 0x000267D0
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

	// Token: 0x06000875 RID: 2165 RVA: 0x00028670 File Offset: 0x00026870
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0002852F File Offset: 0x0002672F
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this._item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x04000A3D RID: 2621
	[SerializeField]
	private Image itemIcon;

	// Token: 0x04000A3E RID: 2622
	[SerializeField]
	private Text itemAmount;

	// Token: 0x04000A3F RID: 2623
	[SerializeField]
	private GameObject betterItemIcon;

	// Token: 0x04000A40 RID: 2624
	[SerializeField]
	private GameObject worseItemIcon;

	// Token: 0x04000A41 RID: 2625
	[SerializeField]
	private Image frameImage;

	// Token: 0x04000A42 RID: 2626
	private bool isOver;

	// Token: 0x04000A43 RID: 2627
	private float overTime;

	// Token: 0x04000A44 RID: 2628
	private UISystemModule uiSystemModule;

	// Token: 0x04000A45 RID: 2629
	private ItemType[] vocationAllowedItemTypes;

	// Token: 0x04000A46 RID: 2630
	private Item _item;
}
