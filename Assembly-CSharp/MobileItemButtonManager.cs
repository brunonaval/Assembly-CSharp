using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000216 RID: 534
public class MobileItemButtonManager : MonoBehaviour, IDropHandler, IEventSystemHandler, IEndDragHandler, IDragHandler
{
	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x06000736 RID: 1846 RVA: 0x0002357E File Offset: 0x0002177E
	public Item Item
	{
		get
		{
			return this.item;
		}
	}

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000737 RID: 1847 RVA: 0x00023586 File Offset: 0x00021786
	private string PrefKey
	{
		get
		{
			return string.Format("{0}_{1}", this.uiSystemModule.PlayerModule.PlayerId, base.gameObject.name);
		}
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x000235B2 File Offset: 0x000217B2
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => this.uiSystemModule.PlayerModule.PlayerId != 0);
		if (PlayerPrefs.HasKey(this.PrefKey))
		{
			string itemUniqueId = PlayerPrefs.GetString(this.PrefKey);
			Item item = this.uiSystemModule.InventoryModule.Items.FirstOrDefault((Item i) => i.UniqueId == itemUniqueId);
			this.SetItem(item);
		}
		yield break;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x000235C4 File Offset: 0x000217C4
	public void OnClick()
	{
		if (!this.item.IsDefined)
		{
			return;
		}
		if (Time.time - this.lastClickTime < this.cooldown)
		{
			return;
		}
		this.lastClickTime = Time.time;
		this.uiSystemModule.ItemModule.CmdUseItemFromItemBar(this.item);
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00023618 File Offset: 0x00021818
	private void Update()
	{
		if (!this.item.IsDefined)
		{
			return;
		}
		float num = Time.time - this.lastClickTime;
		if (this.lastClickTime > Time.time)
		{
			num = 0f;
		}
		this.overlayImage.fillAmount = this.cooldown - num / this.cooldown;
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x00023670 File Offset: 0x00021870
	public void SetItem(Item item)
	{
		Item item2 = this.uiSystemModule.InventoryModule.Items.FirstOrDefault((Item i) => i.UniqueId == item.UniqueId);
		if (!item2.IsDefined)
		{
			return;
		}
		this.item = item2;
		this.iconImage.sprite = this.item.Icon;
		this.overlayImage.sprite = this.item.Icon;
		this.overlayImage.fillAmount = 0f;
		PlayerPrefs.SetString(this.PrefKey, item.UniqueId);
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x00023710 File Offset: 0x00021910
	public void RemoveItem()
	{
		this.item = default(Item);
		Sprite sprite = Resources.Load<Sprite>("Icons/Slots/default_item_slot");
		this.iconImage.sprite = sprite;
		this.overlayImage.sprite = sprite;
		this.overlayImage.fillAmount = 1f;
		PlayerPrefs.DeleteKey(this.PrefKey);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x00023768 File Offset: 0x00021968
	public void OnEndDrag(PointerEventData eventData)
	{
		if (Time.time - this.dragTime <= 0.2f)
		{
			return;
		}
		if (eventData.hovered.Count != 0)
		{
			if (!eventData.hovered.Any((GameObject h) => h.CompareTag("Joystick")))
			{
				return;
			}
		}
		this.RemoveItem();
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x000237C8 File Offset: 0x000219C8
	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			if (eventData.pointerDrag.CompareTag("UIItemBarSlot"))
			{
				MobileItemButtonManager component = eventData.pointerDrag.GetComponent<MobileItemButtonManager>();
				Item item = this.item;
				Item item2 = component.Item;
				this.RemoveItem();
				component.RemoveItem();
				if (item2.IsDefined)
				{
					this.SetItem(item2);
					if (item.IsDefined)
					{
						component.SetItem(item);
					}
				}
				if (!item2.IsDefined && item.IsDefined)
				{
					component.SetItem(item);
				}
			}
			return;
		}
		InventorySlotManager component2 = eventData.pointerDrag.GetComponent<InventorySlotManager>();
		if (component2 == null)
		{
			return;
		}
		this.SetItem(component2.Item);
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0002387B File Offset: 0x00021A7B
	public void OnDrag(PointerEventData eventData)
	{
		this.dragTime = Time.time;
	}

	// Token: 0x04000938 RID: 2360
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000939 RID: 2361
	[SerializeField]
	private Image iconImage;

	// Token: 0x0400093A RID: 2362
	[SerializeField]
	private Image overlayImage;

	// Token: 0x0400093B RID: 2363
	private Item item;

	// Token: 0x0400093C RID: 2364
	private float dragTime;

	// Token: 0x0400093D RID: 2365
	private float lastClickTime;

	// Token: 0x0400093E RID: 2366
	private readonly float cooldown = 1f;
}
