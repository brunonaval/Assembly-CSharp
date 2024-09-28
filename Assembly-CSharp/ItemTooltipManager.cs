using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000201 RID: 513
public class ItemTooltipManager : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060006AE RID: 1710 RVA: 0x00021694 File Offset: 0x0001F894
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x000216B8 File Offset: 0x0001F8B8
	private void OnEnable()
	{
		this.isOver = false;
		this.overTime = Time.time;
		if (this.item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x000216E4 File Offset: 0x0001F8E4
	private void OnDisable()
	{
		this.isOver = false;
		if (this.item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x00021705 File Offset: 0x0001F905
	private void LateUpdate()
	{
		if (this.isOver && Time.time - this.overTime > 0.7f && this.item.IsDefined)
		{
			this.uiSystemModule.ShowItemTooltip(Input.mousePosition, this.item);
		}
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x00021745 File Offset: 0x0001F945
	public void SetItem(Item item)
	{
		this.item = item;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0002174E File Offset: 0x0001F94E
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.isOver = true;
		this.overTime = Time.time;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x000216E4 File Offset: 0x0001F8E4
	public void OnPointerExit(PointerEventData eventData)
	{
		this.isOver = false;
		if (this.item.IsDefined)
		{
			this.uiSystemModule.CloseItemTooltip();
		}
	}

	// Token: 0x040008D7 RID: 2263
	private Item item;

	// Token: 0x040008D8 RID: 2264
	private bool isOver;

	// Token: 0x040008D9 RID: 2265
	private float overTime;

	// Token: 0x040008DA RID: 2266
	private UISystemModule uiSystemModule;
}
