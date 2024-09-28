using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020001FC RID: 508
public class ItemBoostWindowManager : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x06000679 RID: 1657 RVA: 0x00020B13 File Offset: 0x0001ED13
	private void OnEnable()
	{
		this.ClearReagentItem();
		this.ClearEquipmentItem();
		this.boostItemButton.interactable = true;
		this.UpdateFailureText();
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00020B33 File Offset: 0x0001ED33
	private void OnDisable()
	{
		this.ClearBoosterItem();
		this.ClearReagentItem();
		this.ClearEquipmentItem();
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00020B48 File Offset: 0x0001ED48
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
			this.SetReagent(inventorySlotManager.Item);
			return;
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x00020B90 File Offset: 0x0001ED90
	private void SetEquipment(Item equipment)
	{
		if (!equipment.Equipable)
		{
			return;
		}
		this.equipment = equipment;
		this.equipmentTooltipManager.SetItem(equipment);
		this.equipmentIcon.sprite = equipment.Icon;
		this.UpdateDifficultyText();
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x00020BC8 File Offset: 0x0001EDC8
	private void UpdateDifficultyText()
	{
		string boostDifficultyDescription = this.GetBoostDifficultyDescription(this.equipment.BoostLevel + 1, this.reagent.IsDefined);
		float num = this.equipment.NextBoostLevelPercentBonus - this.equipment.BoostLevelPercentBonus;
		this.helpMessageText.text = string.Format(LanguageManager.Instance.GetText(boostDifficultyDescription), num * 100f);
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00020C33 File Offset: 0x0001EE33
	private void SetReagent(Item reagent)
	{
		if (reagent.Type != ItemType.Reagent)
		{
			return;
		}
		this.reagent = reagent;
		this.reagentTooltipManager.SetItem(reagent);
		this.reagentIcon.sprite = reagent.Icon;
		this.UpdateDifficultyText();
		this.UpdateFailureText();
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00020C71 File Offset: 0x0001EE71
	public void SetBooster(Item booster)
	{
		if (!booster.IsItemBooster)
		{
			return;
		}
		this.booster = booster;
		this.boosterTooltipManager.SetItem(booster);
		this.boosterIcon.sprite = booster.Icon;
		this.UpdateFailureText();
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00020CA8 File Offset: 0x0001EEA8
	private void UpdateFailureText()
	{
		if (this.booster.Type != ItemType.SacredItemBooster)
		{
			this.failureMessageText.text = LanguageManager.Instance.GetText("item_boost_window_failure_message");
			return;
		}
		if (this.reagent.IsDefined)
		{
			this.failureMessageText.text = LanguageManager.Instance.GetText("item_boost_window_reagent_failure_message");
			return;
		}
		this.failureMessageText.text = LanguageManager.Instance.GetText("item_boost_window_sacred_failure_message");
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00020D21 File Offset: 0x0001EF21
	public void OnBoostItemButtonClicked()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			base.StartCoroutine(this.BoostItem());
		});
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00020D3C File Offset: 0x0001EF3C
	private void ClearEquipmentItem()
	{
		this.equipment = default(Item);
		this.equipmentIcon.sprite = this.defaultEquipmentIcon;
		this.equipmentTooltipManager.SetItem(default(Item));
		this.helpMessageText.text = LanguageManager.Instance.GetText("item_boost_window_help_message");
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00020D94 File Offset: 0x0001EF94
	private void ClearBoosterItem()
	{
		this.booster = default(Item);
		this.boosterIcon.sprite = this.defaultBoosterIcon;
		this.boosterTooltipManager.SetItem(default(Item));
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00020DD4 File Offset: 0x0001EFD4
	private void ClearReagentItem()
	{
		this.reagent = default(Item);
		this.reagentIcon.sprite = this.defaultReagentIcon;
		this.reagentTooltipManager.SetItem(default(Item));
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00020E12 File Offset: 0x0001F012
	private IEnumerator BoostItem()
	{
		this.boostItemButton.interactable = false;
		this.progressFill.fillAmount = 0f;
		do
		{
			yield return new WaitForSeconds(0.1f);
			this.progressFill.fillAmount += 0.1f;
		}
		while (this.progressFill.fillAmount < 1f);
		try
		{
			this.uiSystemModule.CraftModule.CmdBoostEquipment(this.booster.UniqueId, this.equipment.UniqueId, this.reagent.UniqueId);
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
			this.boostItemButton.interactable = true;
		}
		yield break;
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00020E24 File Offset: 0x0001F024
	private string GetBoostDifficultyDescription(int boostLevel, bool usingReagent)
	{
		float num = GlobalUtils.GetItemBoostPercentSuccessChance(boostLevel);
		if (usingReagent)
		{
			num += num * 0.25f;
			num = Mathf.Min(1f, num);
		}
		if (num >= 0.95f)
		{
			return "boost_difficulty_very_easy";
		}
		if (num >= 0.83f)
		{
			return "boost_difficulty_easy";
		}
		if (num >= 0.69f)
		{
			return "boost_difficulty_little_easy";
		}
		if (num >= 0.5f)
		{
			return "boost_difficulty_average";
		}
		if (num >= 0.31f)
		{
			return "boost_difficulty_little_hard";
		}
		if (num >= 0.17f)
		{
			return "boost_difficulty_hard";
		}
		if (num >= 0.05f)
		{
			return "boost_difficulty_very_hard";
		}
		return "boost_difficulty_impossible";
	}

	// Token: 0x040008A8 RID: 2216
	[SerializeField]
	private ItemTooltipManager equipmentTooltipManager;

	// Token: 0x040008A9 RID: 2217
	[SerializeField]
	private ItemTooltipManager boosterTooltipManager;

	// Token: 0x040008AA RID: 2218
	[SerializeField]
	private ItemTooltipManager reagentTooltipManager;

	// Token: 0x040008AB RID: 2219
	[SerializeField]
	private Image equipmentIcon;

	// Token: 0x040008AC RID: 2220
	[SerializeField]
	private Image boosterIcon;

	// Token: 0x040008AD RID: 2221
	[SerializeField]
	private Image reagentIcon;

	// Token: 0x040008AE RID: 2222
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x040008AF RID: 2223
	[SerializeField]
	private Button boostItemButton;

	// Token: 0x040008B0 RID: 2224
	[SerializeField]
	private Image progressFill;

	// Token: 0x040008B1 RID: 2225
	[SerializeField]
	private Sprite defaultBoosterIcon;

	// Token: 0x040008B2 RID: 2226
	[SerializeField]
	private Sprite defaultEquipmentIcon;

	// Token: 0x040008B3 RID: 2227
	[SerializeField]
	private Sprite defaultReagentIcon;

	// Token: 0x040008B4 RID: 2228
	[SerializeField]
	private Text failureMessageText;

	// Token: 0x040008B5 RID: 2229
	[SerializeField]
	private Text helpMessageText;

	// Token: 0x040008B6 RID: 2230
	private Item booster;

	// Token: 0x040008B7 RID: 2231
	private Item equipment;

	// Token: 0x040008B8 RID: 2232
	private Item reagent;
}
