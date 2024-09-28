using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E8 RID: 488
public class BlueprintSlotManager : MonoBehaviour
{
	// Token: 0x060005ED RID: 1517 RVA: 0x0001EB68 File Offset: 0x0001CD68
	private void Awake()
	{
		this.itemTooltipManager = base.GetComponentInChildren<ItemTooltipManager>();
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0001EB76 File Offset: 0x0001CD76
	private void Start()
	{
		this.blueprintSlotToggle.group = base.GetComponentInParent<ToggleGroup>();
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0001EB8C File Offset: 0x0001CD8C
	public void SetPlayerBlueprint(PlayerBlueprint playerBlueprint)
	{
		this.playerBlueprint = playerBlueprint;
		if (this.playerBlueprint.IsDefined)
		{
			this.itemTooltipManager.SetItem(playerBlueprint.Blueprint.ProducesItem);
			this.itemImage.sprite = playerBlueprint.Blueprint.ProducesItem.Icon;
			int producesAmount = playerBlueprint.Blueprint.ProducesAmount;
			string text = LanguageManager.Instance.GetText(this.playerBlueprint.Blueprint.ItemName);
			this.itemNameText.text = string.Format("{0} (x{1})", text, producesAmount);
		}
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001EC22 File Offset: 0x0001CE22
	public void OnToggleValueChanged()
	{
		base.GetComponentInParent<CraftWindowManager>().SetSelectedBlueprint(this.playerBlueprint);
	}

	// Token: 0x04000838 RID: 2104
	private PlayerBlueprint playerBlueprint;

	// Token: 0x04000839 RID: 2105
	[SerializeField]
	private Image itemImage;

	// Token: 0x0400083A RID: 2106
	[SerializeField]
	private Text itemNameText;

	// Token: 0x0400083B RID: 2107
	[SerializeField]
	private Toggle blueprintSlotToggle;

	// Token: 0x0400083C RID: 2108
	private ItemTooltipManager itemTooltipManager;
}
