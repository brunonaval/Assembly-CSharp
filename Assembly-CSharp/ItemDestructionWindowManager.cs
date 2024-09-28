using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001ED RID: 493
public class ItemDestructionWindowManager : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x06000615 RID: 1557 RVA: 0x0001F560 File Offset: 0x0001D760
	private void OnEnable()
	{
		this.ClearEquipmentItem();
		this.destructItemButton.interactable = true;
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0001F574 File Offset: 0x0001D774
	private void OnDisable()
	{
		this.ClearToolkitItem();
		this.ClearEquipmentItem();
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x0001F584 File Offset: 0x0001D784
	public void OnDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			return;
		}
		InventorySlotManager inventorySlotManager;
		if (eventData.pointerDrag.TryGetComponent<InventorySlotManager>(out inventorySlotManager))
		{
			this.SetEquipment(inventorySlotManager.Item);
			return;
		}
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x0001F5C0 File Offset: 0x0001D7C0
	public void OnDestructItemButtonClicked()
	{
		base.StartCoroutine(this.DestructItem());
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x0001F5D0 File Offset: 0x0001D7D0
	private void ClearEquipmentItem()
	{
		this.equipment = default(Item);
		this.equipmentIcon.sprite = this.defaultEquipmentIcon;
		this.equipmentTooltipManager.SetItem(default(Item));
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x0001F610 File Offset: 0x0001D810
	private void ClearToolkitItem()
	{
		this.toolkit = default(Item);
		this.toolkitIcon.sprite = this.defaultToolkitIcon;
		this.toolkitTooltipManager.SetItem(default(Item));
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0001F64E File Offset: 0x0001D84E
	public void SetToolkit(Item toolkit)
	{
		if (!toolkit.IsToolkit)
		{
			return;
		}
		this.toolkit = toolkit;
		this.toolkitTooltipManager.SetItem(toolkit);
		this.toolkitIcon.sprite = toolkit.Icon;
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0001F67F File Offset: 0x0001D87F
	private void SetEquipment(Item equipment)
	{
		if (!equipment.Equipable)
		{
			return;
		}
		this.equipment = equipment;
		this.equipmentTooltipManager.SetItem(equipment);
		this.equipmentIcon.sprite = equipment.Icon;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0001F6B0 File Offset: 0x0001D8B0
	private IEnumerator DestructItem()
	{
		this.destructItemButton.interactable = false;
		this.progressFill.fillAmount = 0f;
		do
		{
			yield return new WaitForSeconds(0.1f);
			this.progressFill.fillAmount += 0.1f;
		}
		while (this.progressFill.fillAmount < 1f);
		try
		{
			this.uiSystemModule.CraftModule.CmdDestructEquipment(this.toolkit.UniqueId, this.equipment.UniqueId);
			DragWindowModule dragWindowModule;
			base.TryGetComponent<DragWindowModule>(out dragWindowModule);
			dragWindowModule.CloseWindow();
			yield break;
		}
		catch (Exception message)
		{
			Debug.LogError(message);
			yield break;
		}
		finally
		{
			this.destructItemButton.interactable = true;
		}
		yield break;
	}

	// Token: 0x0400085D RID: 2141
	[SerializeField]
	private ItemTooltipManager equipmentTooltipManager;

	// Token: 0x0400085E RID: 2142
	[SerializeField]
	private ItemTooltipManager toolkitTooltipManager;

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	private Image equipmentIcon;

	// Token: 0x04000860 RID: 2144
	[SerializeField]
	private Image toolkitIcon;

	// Token: 0x04000861 RID: 2145
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	private Button destructItemButton;

	// Token: 0x04000863 RID: 2147
	[SerializeField]
	private Image progressFill;

	// Token: 0x04000864 RID: 2148
	[SerializeField]
	private Sprite defaultToolkitIcon;

	// Token: 0x04000865 RID: 2149
	[SerializeField]
	private Sprite defaultEquipmentIcon;

	// Token: 0x04000866 RID: 2150
	private Item toolkit;

	// Token: 0x04000867 RID: 2151
	private Item equipment;
}
