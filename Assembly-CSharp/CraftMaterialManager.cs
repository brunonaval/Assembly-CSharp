using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001E9 RID: 489
public class CraftMaterialManager : MonoBehaviour
{
	// Token: 0x060005F2 RID: 1522 RVA: 0x0001EC38 File Offset: 0x0001CE38
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("UISystem");
		this.uiSystemModule = gameObject.GetComponent<UISystemModule>();
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0001EC5C File Offset: 0x0001CE5C
	private void Update()
	{
		if (Time.time - this.lastUIUpdateTime < 0.5f)
		{
			return;
		}
		this.UpdateMaterialAmountUI();
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0001EC78 File Offset: 0x0001CE78
	private void UpdateMaterialAmountUI()
	{
		this.lastUIUpdateTime = Time.time;
		int amount = this.uiSystemModule.InventoryModule.GetAmount(this.material.MaterialItem.Id);
		if (amount >= this.material.MaterialAmount)
		{
			this.materialInventoryAmountText.color = Color.green;
		}
		else
		{
			this.materialInventoryAmountText.color = Color.red;
		}
		this.materialInventoryAmountText.text = amount.ToString();
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0001ECF4 File Offset: 0x0001CEF4
	public void SetMaterial(BlueprintMaterial material)
	{
		this.material = material;
		this.itemTooltipManager.SetItem(this.material.MaterialItem);
		this.materialIcon.sprite = this.material.MaterialItem.Icon;
		this.materialNameText.text = LanguageManager.Instance.GetText(this.material.MaterialItem.Name);
		this.materialAmountNeededText.text = this.material.MaterialAmount.ToString();
	}

	// Token: 0x0400083D RID: 2109
	[SerializeField]
	private Image materialIcon;

	// Token: 0x0400083E RID: 2110
	[SerializeField]
	private Text materialNameText;

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	private Text materialAmountNeededText;

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	private Text materialInventoryAmountText;

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	private ItemTooltipManager itemTooltipManager;

	// Token: 0x04000842 RID: 2114
	private float lastUIUpdateTime;

	// Token: 0x04000843 RID: 2115
	private BlueprintMaterial material;

	// Token: 0x04000844 RID: 2116
	private UISystemModule uiSystemModule;
}
