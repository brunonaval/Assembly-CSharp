using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200023C RID: 572
public class SkillEnchantWindowManager : MonoBehaviour, IDropHandler, IEventSystemHandler
{
	// Token: 0x06000815 RID: 2069 RVA: 0x00027045 File Offset: 0x00025245
	private void OnEnable()
	{
		this.ClearReagentItem();
		this.ClearSkill();
		this.enchantButton.interactable = true;
		this.UpdateFailureText();
	}

	// Token: 0x06000816 RID: 2070 RVA: 0x00027065 File Offset: 0x00025265
	private void OnDisable()
	{
		this.ClearEnchantItem();
		this.ClearReagentItem();
		this.ClearSkill();
	}

	// Token: 0x06000817 RID: 2071 RVA: 0x00027079 File Offset: 0x00025279
	public void OnDrop(PointerEventData eventData)
	{
		this.ProcessInventoryDrop(eventData);
		this.ProcessSkillBarDrop(eventData);
		this.ProcessSkillBookDrop(eventData);
	}

	// Token: 0x06000818 RID: 2072 RVA: 0x00027090 File Offset: 0x00025290
	private void ProcessInventoryDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UIInventorySlot"))
		{
			return;
		}
		InventorySlotManager inventorySlotManager;
		eventData.pointerDrag.TryGetComponent<InventorySlotManager>(out inventorySlotManager);
		this.SetReagent(inventorySlotManager.Item);
	}

	// Token: 0x06000819 RID: 2073 RVA: 0x000270CC File Offset: 0x000252CC
	private void ProcessSkillBarDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UISkillBarSlot"))
		{
			return;
		}
		SkillBarSlotModule skillBarSlotModule;
		if (eventData.pointerDrag.TryGetComponent<SkillBarSlotModule>(out skillBarSlotModule))
		{
			this.SetSkill(skillBarSlotModule.Skill);
			return;
		}
		MobileSkillButtonManager mobileSkillButtonManager;
		if (eventData.pointerDrag.TryGetComponent<MobileSkillButtonManager>(out mobileSkillButtonManager))
		{
			this.SetSkill(mobileSkillButtonManager.Skill);
			return;
		}
	}

	// Token: 0x0600081A RID: 2074 RVA: 0x00027124 File Offset: 0x00025324
	private void ProcessSkillBookDrop(PointerEventData eventData)
	{
		if (!eventData.pointerDrag.CompareTag("UISkillBookSlot"))
		{
			return;
		}
		SkillBookSlotModule componentInParent = eventData.pointerDrag.GetComponentInParent<SkillBookSlotModule>();
		this.SetSkill(componentInParent.Skill);
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x0002715C File Offset: 0x0002535C
	private void SetSkill(Skill skill)
	{
		if (!skill.IsDefined)
		{
			return;
		}
		this.skill = skill;
		this.skillTooltipManager.SetSkill(skill);
		this.skillIcon.sprite = skill.Icon;
		this.UpdateDifficultyText();
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x00027193 File Offset: 0x00025393
	public void SetEnchant(Item enchant)
	{
		if (enchant.Type != ItemType.SkillEnchant)
		{
			return;
		}
		this.enchant = enchant;
		this.enchantTooltipManager.SetItem(enchant);
		this.enchantIcon.sprite = enchant.Icon;
		this.UpdateFailureText();
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x000271CB File Offset: 0x000253CB
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

	// Token: 0x0600081E RID: 2078 RVA: 0x0002720C File Offset: 0x0002540C
	private void UpdateDifficultyText()
	{
		string enchantDifficultyDescription = this.GetEnchantDifficultyDescription(this.skill.EnchantLevel + 1, this.reagent.IsDefined);
		float num = this.skill.NextEnchantLevelPercentBonus - this.skill.EnchantLevelPercentBonus;
		this.helpMessageText.text = string.Format(LanguageManager.Instance.GetText(enchantDifficultyDescription), num * 100f);
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00027278 File Offset: 0x00025478
	private void UpdateFailureText()
	{
		if (this.reagent.IsDefined)
		{
			this.failureMessageText.text = LanguageManager.Instance.GetText("skill_enchant_window_reagent_failure_message");
			return;
		}
		this.failureMessageText.text = LanguageManager.Instance.GetText("skill_enchant_window_failure_message");
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000272C8 File Offset: 0x000254C8
	private void ClearSkill()
	{
		this.skill = default(Skill);
		this.skillIcon.sprite = this.defaultSkillIcon;
		this.skillTooltipManager.SetSkill(default(Skill));
		this.helpMessageText.text = LanguageManager.Instance.GetText("skill_enchant_window_help_message");
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00027320 File Offset: 0x00025520
	private void ClearEnchantItem()
	{
		this.enchant = default(Item);
		this.enchantIcon.sprite = this.defaultEnchantIcon;
		this.enchantTooltipManager.SetItem(default(Item));
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00027360 File Offset: 0x00025560
	private void ClearReagentItem()
	{
		this.reagent = default(Item);
		this.reagentIcon.sprite = this.defaultReagentIcon;
		this.reagentTooltipManager.SetItem(default(Item));
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x0002739E File Offset: 0x0002559E
	public void OnEnchantButtonClicked()
	{
		this.uiSystemModule.ShowDefaultConfirmationWindow(delegate
		{
			base.StartCoroutine(this.EnchantSkill());
		});
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x000273B7 File Offset: 0x000255B7
	private IEnumerator EnchantSkill()
	{
		this.enchantButton.interactable = false;
		this.progressFill.fillAmount = 0f;
		do
		{
			yield return new WaitForSeconds(0.1f);
			this.progressFill.fillAmount += 0.1f;
		}
		while (this.progressFill.fillAmount < 1f);
		try
		{
			this.uiSystemModule.SkillModule.CmdEnchantSkill(this.enchant.UniqueId, this.reagent.UniqueId, this.skill.Id);
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
			this.enchantButton.interactable = true;
		}
		yield break;
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x000273C8 File Offset: 0x000255C8
	private string GetEnchantDifficultyDescription(int enchantLevel, bool usingReagent)
	{
		float num = GlobalUtils.GetSkillEnchantPercentSuccessChance(enchantLevel);
		if (usingReagent)
		{
			num += num * 0.25f;
			num = Mathf.Min(1f, num);
		}
		if (num >= 0.95f)
		{
			return "enchant_difficulty_very_easy";
		}
		if (num >= 0.83f)
		{
			return "enchant_difficulty_easy";
		}
		if (num >= 0.69f)
		{
			return "enchant_difficulty_little_easy";
		}
		if (num >= 0.5f)
		{
			return "enchant_difficulty_average";
		}
		if (num >= 0.31f)
		{
			return "enchant_difficulty_little_hard";
		}
		if (num >= 0.17f)
		{
			return "enchant_difficulty_hard";
		}
		if (num >= 0.05f)
		{
			return "enchant_difficulty_very_hard";
		}
		return "enchant_difficulty_impossible";
	}

	// Token: 0x040009FD RID: 2557
	[SerializeField]
	private SkillTooltipManager skillTooltipManager;

	// Token: 0x040009FE RID: 2558
	[SerializeField]
	private ItemTooltipManager enchantTooltipManager;

	// Token: 0x040009FF RID: 2559
	[SerializeField]
	private ItemTooltipManager reagentTooltipManager;

	// Token: 0x04000A00 RID: 2560
	[SerializeField]
	private Image skillIcon;

	// Token: 0x04000A01 RID: 2561
	[SerializeField]
	private Image enchantIcon;

	// Token: 0x04000A02 RID: 2562
	[SerializeField]
	private Image reagentIcon;

	// Token: 0x04000A03 RID: 2563
	[SerializeField]
	private UISystemModule uiSystemModule;

	// Token: 0x04000A04 RID: 2564
	[SerializeField]
	private Button enchantButton;

	// Token: 0x04000A05 RID: 2565
	[SerializeField]
	private Image progressFill;

	// Token: 0x04000A06 RID: 2566
	[SerializeField]
	private Sprite defaultEnchantIcon;

	// Token: 0x04000A07 RID: 2567
	[SerializeField]
	private Sprite defaultSkillIcon;

	// Token: 0x04000A08 RID: 2568
	[SerializeField]
	private Sprite defaultReagentIcon;

	// Token: 0x04000A09 RID: 2569
	[SerializeField]
	private Text failureMessageText;

	// Token: 0x04000A0A RID: 2570
	[SerializeField]
	private Text helpMessageText;

	// Token: 0x04000A0B RID: 2571
	private Skill skill;

	// Token: 0x04000A0C RID: 2572
	private Item enchant;

	// Token: 0x04000A0D RID: 2573
	private Item reagent;
}
