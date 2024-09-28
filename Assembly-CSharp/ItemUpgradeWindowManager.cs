using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001FE RID: 510
public class ItemUpgradeWindowManager : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x0600068F RID: 1679 RVA: 0x00020FE4 File Offset: 0x0001F1E4
	private void OnEnable()
	{
		this.ClearEquipmentItem();
		this.upgradeItemButton.interactable = true;
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00020FF8 File Offset: 0x0001F1F8
	private void OnDisable()
	{
		this.ClearScrollItem();
		this.ClearEquipmentItem();
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00021008 File Offset: 0x0001F208
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

	// Token: 0x06000692 RID: 1682 RVA: 0x00021044 File Offset: 0x0001F244
	public void OnUpgradeItemButtonClicked()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			base.StartCoroutine(this.UpgradeItem());
		});
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00021060 File Offset: 0x0001F260
	private void ClearEquipmentItem()
	{
		this.equipment = default(Item);
		this.equipmentIcon.sprite = this.defaultEquipmentIcon;
		this.equipmentTooltipManager.SetItem(default(Item));
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x000210A0 File Offset: 0x0001F2A0
	private void ClearScrollItem()
	{
		this.scroll = default(Item);
		this.scrollIcon.sprite = this.defaultScrollIcon;
		this.scrollTooltipManager.SetItem(default(Item));
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x000210DE File Offset: 0x0001F2DE
	public void SetScroll(Item scroll)
	{
		if (!scroll.IsItemBooster)
		{
			return;
		}
		this.scroll = scroll;
		this.scrollTooltipManager.SetItem(scroll);
		this.scrollIcon.sprite = scroll.Icon;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0002110F File Offset: 0x0001F30F
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

	// Token: 0x06000697 RID: 1687 RVA: 0x00021140 File Offset: 0x0001F340
	private IEnumerator UpgradeItem()
	{
		this.upgradeItemButton.interactable = false;
		this.progressFill.fillAmount = 0f;
		do
		{
			yield return new WaitForSeconds(0.1f);
			this.progressFill.fillAmount += 0.1f;
		}
		while (this.progressFill.fillAmount < 1f);
		try
		{
			this.uiSystemModule.CraftModule.CmdUpgradeEquipment(this.scroll.UniqueId, this.equipment.UniqueId);
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
			this.upgradeItemButton.interactable = true;
		}
		yield break;
	}

	// Token: 0x040008BC RID: 2236
	[SerializeField]
	private ItemTooltipManager equipmentTooltipManager;

	// Token: 0x040008BD RID: 2237
	[SerializeField]
	private ItemTooltipManager scrollTooltipManager;

	// Token: 0x040008BE RID: 2238
	[SerializeField]
	private Image equipmentIcon;

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	private Image scrollIcon;

	// Token: 0x040008C0 RID: 2240
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x040008C1 RID: 2241
	[SerializeField]
	private Button upgradeItemButton;

	// Token: 0x040008C2 RID: 2242
	[SerializeField]
	private Image progressFill;

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	private Sprite defaultScrollIcon;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	private Sprite defaultEquipmentIcon;

	// Token: 0x040008C5 RID: 2245
	private Item scroll;

	// Token: 0x040008C6 RID: 2246
	private Item equipment;
}
